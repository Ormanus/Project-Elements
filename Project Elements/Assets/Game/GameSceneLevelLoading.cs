using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


struct Door
{
    public int width;
    public int startingPoint;
    public byte direction;
};

struct Part
{
    public GameObject go;
    public int w;
    public int h;
    // remove
    public int[] doorStartingPoints;
    public int[] doorWidths;
    // end remove
    public Door[] doors;
};

struct Rectangle
{
    public float x, y, z, w, h;
    public short type;
};

struct Animation
{
    public Texture2D texture;
    public int rows;
    public int columns;
    public Rect getRect(int index)
    {
        //var x0 = (number % GC.tiles[index].columns) * width;
        //var y0 = (floor(number / GC.tiles[index].columns)) * height;
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
    //remove
    public int width;
    public int height;
    //end remove

    //public int nRooms; //number of rooms

    public GameObject[] enemies;
    public string[] parts;
    public Material material;

    private Texture2D texture;
    private Rect[] uvs;
    private AnimatedMesh[] aniMesh;
    private Part[] partData;

    private int[] tileIndex;

    private Animation[] animations;
    private int nAnimations;

    private float time;
    private int animationIndex;

    //private Area[] level;
    //private bool[] levelSpaceMap = new bool[width*height];

    void Start()
    {
        partData = new Part[10]; //array of level parts

        for (int j = 0; j < 1; j++) // TODO: for each parts[]'s value
        {
            //load part data
            Part part = new Part();
            part.doorStartingPoints = new int[4];
            part.doorWidths = new int[4];
            part.go = new GameObject();
            part.go.transform.parent = gameObject.transform;

            //read binary data
            short[] data = toShort(File.ReadAllBytes(Application.dataPath + "/Resources/" + parts[0]));
            short[] tilemap;

            short w = data[0]; //widht
            short h = data[1]; //height
                               // 2 & 3 are needed only in editor
            short num_layers = data[4]; //number of layers

            print(data[0] + "x" + data[1] + "x" + data[4]);

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

            //load enemies
            int seek = num_layers * w * h + 5;
            short num_spawners = data[seek++];
            print("spawns: " + num_spawners);
            for(int i = 0; i < num_spawners; i++)
            {
                short x0 = data[seek++];
                short y0 = data[seek++];
                short w0 = data[seek++];
                short h0 = data[seek++];
                short spawnMax = data[seek++];
                short spawnMin = data[seek++];
                short type = data[seek++];
                int amount = Random.Range(spawnMin, spawnMax);
                print("enemies; max: "+ spawnMax + ", min: " + spawnMin + ", r: " + amount + ", type: " + type);
                for(int k = 0; k < amount; k++)
                {
                    GameObject o = Instantiate(enemies[0]);
                    float x = (float)x0 + Random.Range(0, w0);
                    float y = (float)y0 + Random.Range(0, h0);
                    o.transform.position = new Vector3(x, -y, 0);
                }
            }

            //load textures
            string filename = parts[0];
            string text = File.ReadAllText(Application.dataPath + "/Resources/" + filename);
            string[] lines = text.Split("\n"[0]);
            int startLine = -1;
            for(int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("texturepaths"))
                {
                    startLine = i + 1;
                    break;
                }
            }
            if(startLine == -1)
            {
                print("File error: Texture paths not found");
            }

            short[] rows = new short[lines.Length];
            short[] columns = new short[lines.Length];
            bool[] animated = new bool[lines.Length];

            string[] paths = new string[lines.Length / 2];

            int numTextures = 0;
            int numAnimated = 0;

            int pathnum = 0;
            for (int i = startLine; i < lines.Length; i++)
            {
                paths[pathnum] = lines[i].Substring(0, lines[i].Length - 1);
                i++;
                string dims = lines[i];

                rows[pathnum] = short.Parse(dims.Substring(0, 5));
                columns[pathnum] = short.Parse(dims.Substring(5, 5));
                animated[pathnum] = (byte.Parse(dims.Substring(10, 1)) == 1);

                if(!animated[pathnum])
                {
                    numTextures += rows[pathnum] * columns[pathnum];
                }
                else
                {
                    numAnimated++;
                }
                pathnum++;
            }

            texture = new Texture2D(2048, 2048);
            Texture2D[] textures = new Texture2D[numTextures];
            animations = new Animation[numAnimated];
            tileIndex = new int[numTextures + numAnimated];
            numTextures = 0;
            nAnimations = 0;
            int tileIndexIndex = 0;
            for (int i = 0; i < pathnum; i++)
            {
                //load texture file
                Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                print("loading texture " + i + " @ '" + Application.dataPath + "/Resources/" + paths[i] + "'");
                byte[] texData = File.ReadAllBytes(Application.dataPath + "/Resources/" + paths[i]);
                tex.LoadImage(texData);
                if (tex == null)
                {
                    print("file failure");
                    return;
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
            //find doors

            //-------- TODO: redo door finding! --------
            //int first = -1;
            //int doorWidth = 0;
            //for (int x = 0; x < w; x++) //top
            //{
            //    if (collisionMap[x + 0 * w])
            //    {
            //        if(first == -1)
            //        {
            //            first = x;
            //        }
            //        doorWidth=x-first;
            //    }
            //}
            //part.doorStartingPoints[1] = first;
            //part.doorWidths[1] = doorWidth;

            //for (int x = 0; x < w; x++) //bottom
            //{
            //    if (collisionMap[x + (h - 1) * w])
            //    {
            //        if (first == -1)
            //        {
            //            first = x;
            //        }
            //        doorWidth = x - first;
            //    }
            //}
            //part.doorStartingPoints[3] = first;
            //part.doorWidths[3] = doorWidth;

            //for (int y = 0; y < h; y++) //right
            //{
            //    if (collisionMap[w - 1 + y * w])
            //    {
            //        if (first == -1)
            //        {
            //            first = y;
            //        }
            //        doorWidth = y - first;
            //    }
            //}
            //part.doorStartingPoints[0] = first;
            //part.doorWidths[0] = doorWidth;

            //for (int y = 0; y < h; y++) //left
            //{
            //    if (collisionMap[0 + y * w])
            //    {
            //        if (first == -1)
            //        {
            //            first = y;
            //        }
            //        doorWidth = y - first;
            //    }
            //}
            //part.doorStartingPoints[2] = first;
            //part.doorWidths[2] = doorWidth;

            //divide tilemap to rectangles and create drawable mesh
            List<Rectangle> staticRects = new List<Rectangle>();
            List<Rectangle> animatedRects = new List<Rectangle>();

            for (int n = num_layers - 1; n >= 0; n--)
            {
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        if (tilemap[x + y * w + n * w * h] >= 0)
                        {
                            Rectangle rect = new Rectangle();
                            rect.x = x;
                            rect.y = y;
                            rect.z = n;
                            rect.w = 1;
                            rect.h = 1;
                            rect.type = tilemap[x + y * w + n * w * h];

                            //chech if current tile is animated
                            bool isAnimated = false;
                            int tot = 0;
                            for(int i = 0; i < pathnum; i++)
                            {
                                if(animated[i])
                                {
                                    tot++;
                                }
                                else
                                {
                                    tot += rows[i] * columns[i];
                                }
                                if(tot > rect.type)
                                {
                                    isAnimated = animated[i];
                                    break;
                                }
                            }

                            //add to a list accordingly
                            if (isAnimated)
                            {
                                print("type " + rect.type + " to animated");
                                animatedRects.Add(rect);
                            }
                            else
                            {
                                print("type " + rect.type + " to static");
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

                //print("index = " + index + "/" + animations.Length + "; ");
            }

            print("Collections: " + animationCollection.Length);
            print("Animations:  " + animations.Length);

            //build meshes
            int messageNum = 0;
            aniMesh = new AnimatedMesh[animations.Length];
            for (int i = 0; i < animations.Length; i++)
            {
                print("Meshing things... [" + i + "]");
                AnimatedMesh am = new AnimatedMesh();
                am.go = new GameObject();
                am.go.transform.parent = part.go.transform;
                am.animation = animations[i];

                //mesh
                Vector3[] vertices = new Vector3[animationCollection[i].Count * 4];
                Vector2[] uv = new Vector2[animationCollection[i].Count * 4];
                int[] triangles = new int[animationCollection[i].Count * 6];

                print(messageNum++ + "Count: " + animationCollection[i].Count);

                for (int k = 0; k < animationCollection[i].Count; k++)
                {
                    //print(messageNum++ + "tile [" + k + "]");

                    //int index = tileIndex[animationCollection[i][k].type];

                    //print(messageNum++ + "index [" + index + "]");

                    float x  = animationCollection[i][k].x;
                    float y  = animationCollection[i][k].y;
                    float z  = animationCollection[i][k].z;
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

                aniMesh[i] = am;
            }

            //create short copy of collision map
            short[] temp2 = new short[w * h];
            for(int i = 0; i < w * h; i++)
            {
                temp2[i] = (collisionMap[i] ? (short)(1) : (short)(0));
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
            partData[j] = part;
        }
        //find doors and check for available space
        //place part
        //do until done
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
        }

        for (int i = 0; i < aniMesh.Length; i++)
        {
            Animation ani = aniMesh[i].animation;
            int index = animationIndex % (ani.columns * ani.rows);

            Vector2[] uv = new Vector2[aniMesh[i].mesh.uv.Length];
            for (int j = 0; j < uv.Length / 4; j++)
            {
                Rect uvRect = animations[i].getRect(index);

                uv[j * 4 + 0] = new Vector2(uvRect.x, uvRect.y);
                uv[j * 4 + 1] = new Vector2(uvRect.x + uvRect.width, uvRect.y);
                uv[j * 4 + 2] = new Vector2(uvRect.x, uvRect.y + uvRect.height);
                uv[j * 4 + 3] = new Vector2(uvRect.x + uvRect.width, uvRect.y + uvRect.height);
            }
            aniMesh[i].mesh.uv = uv;
            aniMesh[i].filter.mesh = aniMesh[i].mesh;
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
}
 
