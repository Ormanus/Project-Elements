using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

class Door
{
    public int width;
    public int startingPoint;
    public byte direction;

    public override bool Equals(object obj)
    {
        if(obj == null)
        {
            return false;
        }
        Door d = obj as Door;
        if((object)d == null)
        {
            return false;
        }
        return d.width == width && d.startingPoint == startingPoint && d.direction == direction;
    }

    public static bool operator ==(Door d1, Door d2)
    {
        if((object)d1 == null && (object)d2 == null)
        {
            return true;
        }
        else if((object)d1 == null || (object)d2 == null)
        {
            return false;
        }
        else
        {
            return d1.width == d2.width && d1.startingPoint == d2.startingPoint && d1.direction == d2.direction;
        }
    }

    public static bool operator !=(Door d1, Door d2)
    {
        return !(d1 == d2);
    }
};

struct PartData
{
    public int w, h, d;
    public short[] tilemap;
    public bool[] collisionMap;
    public List<Door> doors;
    public Rect[] enemyAreas;
    public short[] enemyTypes;
    public Vector2[] enemyRanges;
    public string[] texturePaths;
    public string[] textureData;
}

struct Part
{
    public GameObject go;
    public int x;
    public int y;
    public int w;
    public int h;
    public List<Door> doors;
};

struct Rectangle
{
    public float x, y, z, w, h;
    public short type;
};

class TempPart
{
    public Rect rect;
    public PartData data;
    public Door parent;
    public List<TempPart> children;
    public void removeChildren()
    {
        while (children.Count > 0)
        {
            children[0].removeChildren();
            children.RemoveAt(0);
        }
    }
    public void getChildren(ref List<TempPart> list)
    {
        list.AddRange(children);
        for(int i = 0; i < children.Count; i++)
        {
            children[i].getChildren(ref list);
        }
    }

    public static bool operator ==(TempPart p1, TempPart p2)
    {
        return p1.rect == p2.rect;
    }

    public static bool operator !=(TempPart p1, TempPart p2)
    {
        return !(p1 == p2);
    }
};

struct Animation
{
    public Texture2D texture;
    public int rows;
    public int columns;
    public Rect getRect(int index)
    {
        float w0 = 1.0f / columns;
        float h0 = 1.0f / rows;
        float x0 = (index % columns) * w0;
        float y0 = 1.0f - (Mathf.Floor(index / columns)) * h0;
        return new Rect(x0, y0, w0, h0);
    }
}

struct AnimatedMesh
{
    public GameObject go;
    public Animation animation;
    public MeshRenderer renderer;
    public MeshFilter filter;
    public Mesh mesh;
}

public class GameSceneLevelLoading : MonoBehaviour
{
    public int nRooms; //number of rooms

    public GameObject[] enemies;
    public string[] parts;
    public Material material;

    private Texture2D texture;
    private Rect[] uvs;
    private List<AnimatedMesh>[] aniMesh;
    private GameObject[] level;
    private PartData[] partData;

    private int[] tileIndex;

    private Animation[] animations;
    private int nAnimations;

    private float time;
    private int animationIndex;

    void Start()
    {
        //TODO: make part enemies parent transform
        //  and free them in the end of the generation (?)

        aniMesh = new List<AnimatedMesh>[parts.Length];
        partData = new PartData[parts.Length]; //array of level parts

        for (int j = 0; j < parts.Length; j++)
        {
            partData[j] = loadPart(parts[j]);

            for(int k = 0; k < partData[j].doors.Count; k++)
            {
                print("door [" + j + "][" + k + "]");
            }
        }

        //------------------Generate map------------------//

        print("generate");
        List<TempPart> lev = new List<TempPart>();
        TempPart start = new TempPart();
        start.parent = null;
        start.data = partData[0];
        start.rect = new Rect(0, 0, start.data.w, start.data.h);
        start.children = new List<TempPart>();
        lev.Add(start);

        List<Door> unusedDoors = new List<Door>();
        List<TempPart> doorOwners = new List<TempPart>();

        for(int i = 0; i < start.data.doors.Count; i++)
        {
            unusedDoors.Add(start.data.doors[i]);
            doorOwners.Add(start);
        }

        int messageCount = 0;
        while(unusedDoors.Count > 0)
        {
            print(messageCount++ + " - Unused doors: " + unusedDoors.Count);
            print(messageCount++ + " - Lev.Count: " + lev.Count + "/" + nRooms);
            int r = Random.Range(0, unusedDoors.Count - 1);

            print(messageCount++ + " - r = " + r);

            Door[] newDoors;
            TempPart[] newParts;

            if(expand(doorOwners[r], unusedDoors[r], ref lev, out newDoors, out newParts, (lev.Count >= nRooms)))
            {
                print("Success!");
                unusedDoors.AddRange(newDoors);
                doorOwners.AddRange(newParts);

                unusedDoors.RemoveAt(r);
                doorOwners.RemoveAt(r);
            }
            else
            {
                //remove all
                print("Failure!");
                List<TempPart> list = new List<TempPart>();
                doorOwners[r].getChildren(ref list);

                for(int i = 0; i < doorOwners.Count; i++)
                {
                    for(int j = 0; j < list.Count; j++)
                    {
                        if(doorOwners[i] == list[j])
                        {
                            unusedDoors.RemoveAt(i);
                            doorOwners.RemoveAt(i);
                        }
                    }
                }
            }
        }
        //build level
        for(int i = 0; i < lev.Count; i++)
        {
            placePart((int)lev[i].rect.x, (int)lev[i].rect.y, lev[i].data);
        }
    }

    private bool expand(TempPart part, Door door0, ref List<TempPart> lev, out Door[] newDoors, out TempPart[] newParts, bool forceOneDoor = false)
    {
        print("expanding...");
        //make sure that all doors will be used!
        bool[] usedDoors = new bool[part.data.doors.Count];
        int unused;
        if (part.parent == null)
        {
            for (int i = 0; i < part.data.doors.Count; i++)
            {
                usedDoors[i] = false;
            }
            unused = part.data.doors.Count;
        }
        else
        { 
            for (int i = 0; i < part.data.doors.Count; i++)
            {
                if (part.data.doors[i] == part.parent)
                {
                    usedDoors[i] = true;
                    break;
                }
                else
                {
                    usedDoors[i] = false;
                }
            }
            unused = part.data.doors.Count - 1;
        }
        List<Door> newDoorsList = new List<Door>();
        List<TempPart> newPartsList = new List<TempPart>();
        int newCount = 0;

        //find matching doors
        List<KeyValuePair<Door, Door>> doors = new List<KeyValuePair<Door, Door>>();
        List<PartData> doorOwners = new List<PartData>();

        //compare doors
        for(int j = 0; j < part.data.doors.Count; j++)
        {
            if(usedDoors[j] == true)
            {
                continue;
            }
            for (int i = 0; i < partData.Length; i++)
            {
                if(forceOneDoor && partData[i].doors.Count > 1)
                {
                    continue;
                }
                for (int k = 0; k < partData[i].doors.Count; k++)
                {
                    if(part.data.doors[j].width == partData[i].doors[k].width)
                    {
                        doors.Add(new KeyValuePair<Door, Door>(part.data.doors[j], partData[i].doors[k]));
                        doorOwners.Add(partData[i]);
                    }
                }
            }
        }
        while (unused > 0 && doors.Count > 0)
        {
            print("doors = " + doors.Count);
            print("unused = " + unused);
            int[] selection = new int[doors.Count];

            int n = 0;
            for(int i = 0; i < doors.Count; i++)
            {
                int index = -1;
                for (int j = 0; j < part.data.doors.Count; j++)
                {
                    if (part.data.doors[j] == doors[i].Key)
                    {
                        index = j;
                    }
                }
                if(index == -1)
                {
                    print("Generator logic error: couldn't find unused door index");
                    newDoors = null;
                    newParts = null;
                    return false;
                }
                if (!usedDoors[index]) //unnecessary double check? nope.
                {
                    selection[n++] = i;
                }
            }

            int r = Random.Range(0, n - 1);
            print("r = " + r);

            Door d1 = doors[selection[r]].Key;
            Door d2 = doors[selection[r]].Value;
            PartData p = doorOwners[selection[r]];

            Rect rect = new Rect(part.rect.x, part.rect.y, p.w, p.h);

            int deltaW = d1.startingPoint - d2.startingPoint;

            int start2 = d2.startingPoint;
            int wallWidth2 = (d2.direction % 2 == 0 ? p.w : p.h);

            switch (d1.direction)
            {
                case 0:
                    switch (d2.direction)
                    {
                        case 0:
                            deltaW = wallWidth2 - deltaW;
                            break;
                        case 1:
                            rect = rotate(rect, 1);
                            deltaW = wallWidth2 - deltaW;
                            break;
                        case 2:
                            break;
                        case 3:
                            rect = rotate(rect, 1);
                            break;
                    }
                    rect.y += deltaW;
                    rect.x += part.rect.width;
                    break;
                case 1:
                    switch (d2.direction)
                    {
                        case 0:
                            rect = rotate(rect, 1);
                            deltaW = wallWidth2 - deltaW;
                            break;
                        case 1:
                            deltaW = wallWidth2 - deltaW;
                            break;
                        case 2:
                            rect = rotate(rect, 1);
                            break;
                        case 3:
                            break;
                    }
                    rect.x += deltaW;
                    rect.y -= rect.height;
                    break;
                case 2:
                    switch (d2.direction)
                    {
                        case 0:
                            break;
                        case 1:
                            rect = rotate(rect, 1);
                            break;
                        case 2:
                            deltaW = wallWidth2 - deltaW;
                            break;
                        case 3:
                            rect = rotate(rect, 1);
                            deltaW = wallWidth2 - deltaW;
                            break;
                    }
                    rect.y += deltaW;
                    rect.x -= rect.width;
                    break;
                case 3:
                    switch (d2.direction)
                    {
                        case 0:
                            rect = rotate(rect, 1);
                            break;
                        case 1:
                            break;
                        case 2:
                            rect = rotate(rect, 1);
                            deltaW = wallWidth2 - deltaW;
                            break;
                        case 3:
                            deltaW = wallWidth2 - deltaW;
                            break;
                    }
                    rect.x += deltaW;
                    rect.y += part.rect.height;
                    break;
                default: print("Generator error: unknown direction!"); break;
            }
            //check if colliding with all
            bool b = false;
            for(int i = 0; i < lev.Count; i++)
            {
                if(isColliding(lev[i], rect))
                {
                    b = true;
                    break;
                }
            }
            if(!b)
            {
                int index = -1;
                for (int j = 0; j < part.data.doors.Count; j++)
                {
                    if (part.data.doors[j] == d1)
                    {
                        index = j;
                    }
                }
                usedDoors[selection[index]] = true; //--------------------------------------------index out of range
                unused--;
                print("Match found! unused = " + unused);
                for(int i = 0; i < doors.Count; i++)
                {
                    if(doors[i].Key == d1)
                    {
                        doors.RemoveAt(i);
                    } 
                }
                TempPart newPart = new TempPart();
                newPart.parent = d2;
                newPart.rect = rect;
                newPart.data = p;
                newPart.children = new List<TempPart>();
                part.children.Add(newPart);
                for(int i = 0; i < newPart.data.doors.Count; i++)
                {
                    print("NC: " + newCount++);
                    newPartsList.Add(newPart);
                    newDoorsList.Add(newPart.data.doors[i]);
                }
                
                if(unused == 0)
                {
                    newDoors = newDoorsList.ToArray();
                    newParts = newPartsList.ToArray();
                    return true;
                }
            }
            else
            {
                doors.RemoveAt(selection[r]);
            }
        }
        newDoors = null;
        newParts = null;
        return false;
    }

    private Rect rotate(Rect r, int degrees90)
    {
        if(degrees90 % 2 != 0)
        {
            float temp = r.width;
            r.width = r.height;
            r.height = temp;
        }
        return r;
    }

    private Rect mirror(Rect r, int point, bool axis)
    {
        return r;
    }

    private bool isColliding(TempPart p1, Rect p2)
    {
        return p1.rect.x + p1.rect.width > p2.x && p1.rect.y + p1.rect.height > p1.rect.y && p1.rect.x < p2.x + p2.width && p1.rect.y < p2.y + p2.height;
    }

    private short[] toShort(byte[] bytes)
    {
        short[] result = new short[bytes.Length / 2];
        for (int i = 0; i < bytes.Length / 2; i++)
        {
            result[i] = (short)(bytes[i * 2] + bytes[i * 2 + 1] * 256);
        }
        return result;
    }

    void Update()
    {
        //Update animated tiles
        float dt = Time.deltaTime;
        time += dt;
        if(time > 1.0f)
        {
            time -= 1.0f;
            animationIndex++;

            for (int i = 0; i < aniMesh.Length; i++)
            {
                for (int k = 0; k < aniMesh[i].Count; k++)
                {
                    Animation ani = aniMesh[i][k].animation;
                    int index = animationIndex % (ani.columns * ani.rows);

                    Vector2[] uv = new Vector2[aniMesh[i][k].mesh.uv.Length];
                    for (int j = 0; j < uv.Length / 4; j++)
                    {
                        Rect uvRect = ani.getRect(index);

                        uv[j * 4 + 0] = new Vector2(uvRect.x, uvRect.y);
                        uv[j * 4 + 1] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
                        uv[j * 4 + 2] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
                        uv[j * 4 + 3] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
                    }
                    aniMesh[i][k].mesh.uv = uv;
                    aniMesh[i][k].filter.mesh = aniMesh[i][k].mesh;
                }
            }
        }
    }

    private Vector2 rectangleFill1(int n, int x, int y, int w0, int h0, short value, short[] list)
    {
        short old = list[x + y * w0 + n * w0 * h0];
        list[x + y * w0 + n * w0 * h0] = value;

        bool hd = false;
        bool vd = false;

        int w = 1;
        int h = 1;
        while (!vd || !hd)
        {
            if (x + w < w0 && !hd)
            {
                for (int i = 0; i < h; i++)
                {
                    if (list[x + w + (y + i) * w0 + n * w0 * h0] != old)
                    {
                        hd = true;
                        break;
                    }
                }
                if (!hd)
                {
                    for (int i = 0; i < h; i++)
                    {
                        list[x + w + (y + i) * w0 + n * w0 * h0] = value;
                    }
                    w++;
                }
            }
            else
            {
                hd = true;
            }
            if (y + h < h0 && !vd)
            {
                for (int i = 0; i < w; i++)
                {
                    if (list[x + i + (y + h) * w0 + n * w0 * h0] != old)
                    {
                        vd = true;
                        break;
                    }
                }
                if (!vd)
                {
                    for (int i = 0; i < w; i++)
                    {
                        list[x + i + (y + h) * w0 + n * w0 * h0] = value;
                    }
                    h++;
                }
            }
            else
            {
                vd = true;
            }
        }
        return new Vector2(w, h);
    }

    private PartData loadPart(string path)
    {
        //load part data
        PartData part = new PartData();

        //read binary data
        short[] data = toShort(File.ReadAllBytes(Application.dataPath + "/Resources/" + path));
        short[] tilemap;

        short w = data[0]; //widht
        short h = data[1]; //height
                           // 2 & 3 are needed only in editor
        short num_layers = data[4]; //number of layers

        part.w = w;
        part.h = h;
        part.d = num_layers;

        print("load maps");

        tilemap = new short[w * h * num_layers];
        bool[] collisionMap = new bool[w * h];
        for (int i = 0; i < w * h; i++)
        {
            collisionMap[i] = false;
        }
        
        //load maps
        for (int i = 0; i < num_layers; i++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    short type = (short)(data[x + y * w + i * w * h + 5] - 1);
                    tilemap[x + y * w + i * w * h] = type;
                    if (type > -1)
                    {
                        collisionMap[x + y * w] = true;
                    }
                }
            }
        }
        part.tilemap = tilemap;
        part.collisionMap = collisionMap;

        print("load enemies");
        //load enemies
        int seek = num_layers * w * h + 5;
        short num_spawners = data[seek++];

        part.enemyAreas = new Rect[num_spawners];
        part.enemyRanges = new Vector2[num_spawners];
        part.enemyTypes = new short[num_spawners];

        for (int i = 0; i < num_spawners; i++)
        {
            short x0 = data[seek++];
            short y0 = data[seek++];
            short w0 = data[seek++];
            short h0 = data[seek++];
            short spawnMax = data[seek++];
            short spawnMin = data[seek++];
            short type = data[seek++];

            part.enemyAreas[i] = new Rect(x0, y0, w0, h0);
            part.enemyRanges[i] = new Vector2(spawnMin, spawnMax);
            part.enemyTypes[i] = type;
        }

        print("load textuers");
        //load textures
        string filename = path;
        string text = File.ReadAllText(Application.dataPath + "/Resources/" + filename);
        string[] lines = text.Split("\n"[0]);
        int startLine = -1;
        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("texturepaths"))
            {
                startLine = i + 1;
                break;
            }
        }
        if (startLine == -1)
        {
            print("File error: Texture paths not found");
        }

        int textureCount = (lines.Length - startLine + 1) / 2;

        print("TextureCount = " + textureCount);

        part.texturePaths = new string[textureCount];
        part.textureData = new string[textureCount];

        for (int i = 0; i < textureCount; i++)
        {
            part.texturePaths[i] = lines[startLine + i * 2 + 0];
            part.textureData[i] = lines[startLine + i * 2 + 1];
        }

        print("find doors");
        //find doors
        int first = -1;
        int doorWidth = 0;
        bool door = false;

        part.doors = new List<Door>();

        for (int x = 0; x < w; x++) //top
        {
            if (!collisionMap[x + 0 * w])
            {
                door = true;
                if (first == -1)
                {
                    first = x;
                }
                doorWidth++;
            }
            else
            {
                if (door)
                {
                    Door d = new Door();
                    d.direction = 1;
                    d.width = doorWidth;
                    d.startingPoint = first;
                    part.doors.Add(d);
                    first = -1;
                    doorWidth = 0;
                }
                door = false;
            }
        }
        for (int x = 0; x < w; x++) //bottom
        {
            if (collisionMap[x + (h - 1) * w])
            {
                door = true;
                if (first == -1)
                {
                    first = x;
                }
                doorWidth++;
            }
            else
            {
                if (door)
                {
                    Door d = new Door();
                    d.direction = 1;
                    d.width = doorWidth;
                    d.startingPoint = first;
                    part.doors.Add(d);
                    first = -1;
                    doorWidth = 0;
                }
                door = false;
            }
        }
        for (int y = 0; y < h; y++) //right
        {
            if (collisionMap[w - 1 + y * w])
            {
                door = true;
                if (first == -1)
                {
                    first = y;
                }
                doorWidth++;
            }
            else
            {
                if (door)
                {
                    Door d = new Door();
                    d.direction = 1;
                    d.width = doorWidth;
                    d.startingPoint = first;
                    part.doors.Add(d);
                    first = -1;
                    doorWidth = 0;
                }
                door = false;
            }
        }
        for (int y = 0; y < h; y++) //left
        {
            if (collisionMap[0 + y * w])
            {
                door = true;
                if (first == -1)
                {
                    first = y;
                }
                doorWidth++;
            }
            else
            {
                if (door)
                {
                    Door d = new Door();
                    d.direction = 1;
                    d.width = doorWidth;
                    d.startingPoint = first;
                    part.doors.Add(d);
                    first = -1;
                    doorWidth = 0;
                }
                door = false;
            }
        }
        return part;
    }

    private Part placePart(int x0, int y0, PartData partData)
    {
        print("place part at " + x0 + ", " + y0);
        int w = partData.w;
        int h = partData.h;

        Part part = new Part();
        part.go = new GameObject();
        part.go.transform.parent = gameObject.transform;

        print("place enemies");
        for (int j = 0; j < partData.enemyAreas.Length; j++)
        {
            Rect r = partData.enemyAreas[j];
            int amount = Random.Range((int)partData.enemyRanges[j].x, (int)partData.enemyRanges[j].y);
            for (int k = 0; k < amount; k++)
            {
                GameObject o = Instantiate(enemies[partData.enemyTypes[j]]);
                float x = x0 + Random.Range(0, partData.enemyAreas[j].width);
                float y = y0 + Random.Range(0, partData.enemyAreas[j].height);
                o.transform.position = new Vector3(x, -y, 0);
            }
        }

        //------------textures------------
        print("parse texture data");
        short[] rows = new short[partData.texturePaths.Length];
        short[] columns = new short[partData.texturePaths.Length];
        bool[] animated = new bool[partData.texturePaths.Length];

        int numTextures = 0;
        int numAnimated = 0;

        string[] paths = new string[partData.texturePaths.Length];
        for (int i = 0; i < partData.texturePaths.Length; i++)
        {
            paths[i] = partData.texturePaths[i];
            string dims = partData.textureData[i];

            rows[i] = short.Parse(dims.Substring(0, 5));
            columns[i] = short.Parse(dims.Substring(5, 5));
            animated[i] = (byte.Parse(dims.Substring(10, 1)) == 1);
            if (!animated[i])
            {
                numTextures += rows[i] * columns[i];
            }
            else
            {
                numAnimated++;
            }
        }

        int nTotal = numTextures + numAnimated;

        print("load " + nTotal + " textures");
        texture = new Texture2D(2048, 2048);
        Texture2D[] textures = new Texture2D[numTextures];
        animations = new Animation[numAnimated];
        tileIndex = new int[nTotal];
        numTextures = 0;
        nAnimations = 0;
        int tileIndexIndex = 0;
        for (int i = 0; i < partData.texturePaths.Length; i++)
        {
            //load texture file
            Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
            string path0 = paths[i];
            int l = path0.Length;
            string path1 = path0.Substring(0, l-1);
            print("loading texture " + i + " @ '" + Application.dataPath + "/Resources/" + path1 + "'");
            byte[] texData = File.ReadAllBytes(Application.dataPath + "/Resources/" + path1);
            tex.LoadImage(texData);
            if (tex == null)
            {
                print("file failure");
            }
            int w0 = tex.width;
            int h0 = tex.height;

            print("texdims: " + w0 + "x" + h0);

            int w1 = w0 / columns[i];
            int h1 = h0 / rows[i];

            if (animated[i]) //save one texture animation
            {
                Animation a = new Animation();
                a.columns = columns[i];
                a.rows = rows[i];
                a.texture = tex;
                tileIndex[tileIndexIndex++] = nAnimations;
                animations[nAnimations++] = a;
            }
            else //divide into multiple static textures
            {
                for (int y = 0; y < rows[i]; y++)
                {
                    for (int x = 0; x < columns[i]; x++)
                    {
                        Color[] pixels = tex.GetPixels(x * w1, h0 - (y + 1) * h1, w1, h1);
                        Texture2D t2 = new Texture2D(w1, h1, TextureFormat.ARGB32, false);
                        t2.SetPixels(pixels);
                        t2.Apply();
                        tileIndex[tileIndexIndex++] = numTextures;
                        textures[numTextures++] = t2;
                    }
                }
            }
        }
        //create texture atlas and add uv coordinates to a rectangle array
        uvs = texture.PackTextures(textures, 2, 2048, false);

        //divide tilemap to rectangles and create drawable mesh
        List<Rectangle> staticRects = new List<Rectangle>();
        List<Rectangle> animatedRects = new List<Rectangle>();

        for (int n = partData.d - 1; n >= 0; n--)
        {
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    if (partData.tilemap[x + y * w + n * w * h] >= 0)
                    {
                        Rectangle rect = new Rectangle();
                        rect.x = x;
                        rect.y = y;
                        rect.z = n;
                        rect.w = 1;
                        rect.h = 1;
                        rect.type = partData.tilemap[x + y * w + n * w * h];

                        //chech if current tile is animated
                        bool isAnimated = false;
                        int tot = 0;
                        for (int i = 0; i < nTotal; i++)
                        {
                            if (animated[i])
                            {
                                tot++;
                            }
                            else
                            {
                                tot += rows[i] * columns[i];
                            }
                            if (tot > rect.type)
                            {
                                isAnimated = animated[i];
                                break;
                            }
                        }

                        //add to a list accordingly
                        if (isAnimated)
                        {
                            animatedRects.Add(rect);
                        }
                        else
                        {
                            staticRects.Add(rect);
                        }
                    }
                }
            }
        }

        print("animated tot: " + animatedRects.Count);

        {
            //create static mesh
            Vector3[] vertices = new Vector3[staticRects.Count * 4];
            Vector2[] uv = new Vector2[staticRects.Count * 4];
            int[] triangles = new int[staticRects.Count * 6];

            for (int i = 0; i < staticRects.Count; i++)
            {
                float x = staticRects[i].x;
                float y = staticRects[i].y;
                float z = staticRects[i].z;
                float w0 = staticRects[i].w;
                float h0 = staticRects[i].h;

                vertices[i * 4 + 0] = new Vector3(x, -y - 1, z);
                vertices[i * 4 + 1] = new Vector3(x + w0, -y - 1, z);
                vertices[i * 4 + 2] = new Vector3(x, -y + h0 - 1, z);
                vertices[i * 4 + 3] = new Vector3(x + w0, -y + h0 - 1, z);

                triangles[i * 6 + 0] = i * 4 + 2;
                triangles[i * 6 + 1] = i * 4 + 0;
                triangles[i * 6 + 2] = i * 4 + 1;
                triangles[i * 6 + 3] = i * 4 + 1;
                triangles[i * 6 + 4] = i * 4 + 2;
                triangles[i * 6 + 5] = i * 4 + 3;

                //get ui coordinates
                Rect uvRect = uvs[tileIndex[staticRects[i].type]];

                uv[i * 4 + 0] = new Vector2(uvRect.x, uvRect.y);
                uv[i * 4 + 1] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
                uv[i * 4 + 2] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
                uv[i * 4 + 3] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
            }

            Mesh mesh = new Mesh();
            mesh.name = "PartMesh";
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            part.go.AddComponent<MeshFilter>().mesh = mesh;
            Material mat = new Material(material);
            mat.mainTexture = texture;
            part.go.AddComponent<MeshRenderer>().material = mat;
            part.w = w;
            part.h = h;
        }
        print("animated meshes");
        //create animated meshes

        //collect animations of the same texture together
        List<Rectangle>[] animationCollection = new List<Rectangle>[animations.Length];
        for (int i = 0; i < animations.Length; i++) //init
        {
            animationCollection[i] = new List<Rectangle>();
        }
        for (int i = 0; i < animatedRects.Count; i++)
        {
            int index = tileIndex[animatedRects[i].type];
            animationCollection[index].Add(animatedRects[i]);
        }

        //build meshes
        aniMesh[aniMesh.Length - 1] = new List<AnimatedMesh>();
        for (int i = 0; i < animations.Length; i++)
        {
            print("animated meshing");
            AnimatedMesh am = new AnimatedMesh();
            am.go = new GameObject();
            am.go.transform.parent = part.go.transform;
            am.animation = animations[i];

            //mesh
            Vector3[] vertices = new Vector3[animationCollection[i].Count * 4];
            Vector2[] uv = new Vector2[animationCollection[i].Count * 4];
            int[] triangles = new int[animationCollection[i].Count * 6];

            for (int k = 0; k < animationCollection[i].Count; k++)
            {
                float x = animationCollection[i][k].x;
                float y = animationCollection[i][k].y;
                float z = animationCollection[i][k].z;
                float w0 = animationCollection[i][k].w;
                float h0 = animationCollection[i][k].h;

                vertices[k * 4 + 0] = new Vector3(x, -y - 1, z);
                vertices[k * 4 + 1] = new Vector3(x + w0, -y - 1, z);
                vertices[k * 4 + 2] = new Vector3(x, -y + h0 - 1, z);
                vertices[k * 4 + 3] = new Vector3(x + w0, -y + h0 - 1, z);

                triangles[k * 6 + 0] = k * 4 + 2;
                triangles[k * 6 + 1] = k * 4 + 0;
                triangles[k * 6 + 2] = k * 4 + 1;
                triangles[k * 6 + 3] = k * 4 + 1;
                triangles[k * 6 + 4] = k * 4 + 2;
                triangles[k * 6 + 5] = k * 4 + 3;

                //get ui coordinates
                Rect uvRect = animations[i].getRect(0);

                uv[k * 4 + 0] = new Vector2(uvRect.x, uvRect.y);
                uv[k * 4 + 1] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
                uv[k * 4 + 2] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
                uv[k * 4 + 3] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
            }
            Mesh m = new Mesh();
            m.name = "AnimationMesh";
            m.vertices = vertices;
            m.uv = uv;
            m.triangles = triangles;

            am.mesh = m;
            am.filter = am.go.AddComponent<MeshFilter>();
            am.filter.mesh = m;

            Material mat = new Material(material);
            mat.mainTexture = animations[i].texture;
            am.go.AddComponent<MeshRenderer>().material = mat;

            print("animated meshed");
            aniMesh[aniMesh.Length - 1].Add(am);
        }
        print("aniMesh.Length = " + aniMesh.Length);

        //create short copy of collision map
        short[] temp2 = new short[w * h];
        for (int i = 0; i < w * h; i++)
        {
            temp2[i] = (partData.collisionMap[i] ? (short)(1) : (short)(0));
        }

        //divide to collision boxes and create colliders
        short area = 2;
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                //rectangle fill
                if (temp2[x + y * w] == 1)
                {
                    print("creating collider");

                    Vector2 size = rectangleFill1(0, x, y, w, h, area, temp2);
                    PolygonCollider2D collider = part.go.AddComponent<PolygonCollider2D>();
                    Vector2[] points = new Vector2[4];
                    points[0] = new Vector2(x, -y);
                    points[1] = new Vector2(x + size.x, -y);
                    points[2] = new Vector2(x + size.x, -y - size.y);
                    points[3] = new Vector2(x, -y - size.y);
                    collider.points = points;

                    area++;
                }
            }
        }
        part.go.transform.position = new Vector3(x0, y0);

        return part;
    }
}
