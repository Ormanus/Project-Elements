//
// no-generator version
//

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

struct PartData
{
    public int w, h, d;
    public short[] tilemap;
    public bool[] collisionMap;

    public Rect[] enemyAreas;
    public short[] enemyTypes;
    public Vector2[] enemyRanges;

    public Vector2[] thingPos;
    public short[] thingTypes;

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
};

struct Rectangle
{
    public float x, y, z, w, h;
    public short type;
};

struct LevelAnimation
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
    public LevelAnimation animation;
    public MeshRenderer renderer;
    public MeshFilter filter;
    public Mesh mesh;
}

public class GameSceneLevelLoading : MonoBehaviour
{
    public static int levelNumber = 0;
    public static int maxLevel;

    public GameObject[] enemies;
    public GameObject goal;
    public GameObject gate;
    public GameObject boss;
    public string[] levels;
    public Material material;
    //public GameObject AStar;

    public bool isBossLevel;

    private Texture2D texture;
    private Rect[] uvs;
    private List<List<AnimatedMesh>> aniMesh;
    private GameObject[] level;
    private PartData partData;

    private int[] tileIndex;

    private LevelAnimation[] animations;
    private int nAnimations;

    private float time;
    private int animationIndex;

    private Transform player;

    void Start()
    {
        maxLevel = levels.Length;

        if(levelNumber >= levels.Length)
        {
            levelNumber = 0;
            SceneManager.LoadScene("EndScreenScene");
        }
        else
        {
            isBossLevel = ((levelNumber + 1) % 3 == 0);
            player = GameObject.Find("Player").transform;
            aniMesh = new List<List<AnimatedMesh>>();
            placePart(0, 0, loadPart(levels[levelNumber]));
        }
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

            print(aniMesh);
            for (int i = 0; i < aniMesh.Count; i++)
            {
                print("aniMesh " + i + "/" + aniMesh.Count);
                print("aniMesh " + i + " = " + aniMesh[i]);
                for (int k = 0; k < aniMesh[i].Count; k++)
                {
                    LevelAnimation ani = aniMesh[i][k].animation;
                    int index = animationIndex % (ani.columns * ani.rows);

                    Vector2[] uv = new Vector2[aniMesh[i][k].mesh.uv.Length];
                    for (int j = 0; j < uv.Length / 4; j++)
                    {
                        //get ui coordinates
                        float qpx = 0.25f / texture.width; //Quarter pixel, add this to avoid texture bleeding
                        float qpy = 0.25f / texture.height;

                        Rect uvRect = ani.getRect(index);

                        uv[j * 4 + 0] = new Vector2(uvRect.x + qpx, uvRect.y + qpy);
                        uv[j * 4 + 1] = new Vector2(uvRect.x + uvRect.width - qpx, uvRect.y + qpy);
                        uv[j * 4 + 2] = new Vector2(uvRect.x + qpx, uvRect.y + uvRect.height - qpy);
                        uv[j * 4 + 3] = new Vector2(uvRect.x + uvRect.width - qpx, uvRect.y + uvRect.height - qpy);
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
        short version = data[2]; //version number
        short num_things = data[3]; //temp backwards compatibility thing
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
                    if (type > -1 && i != num_layers - 1)
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

            print("Area loaded, type: " + type);

            part.enemyAreas[i] = new Rect(x0, y0, w0, h0);
            part.enemyRanges[i] = new Vector2(spawnMin, spawnMax);
            part.enemyTypes[i] = type;
        }

        print("load things");
        //if you're wondering what "things" are loaded here, google "DOOM thing"
        if(version > 0)
            num_things = data[seek];
        seek++;

        part.thingPos = new Vector2[num_things];
        part.thingTypes = new short[num_things];

        for(int i = 0; i < num_things; i++)
        {
            short x0 = data[seek++];
            short y0 = data[seek++];
            short type = data[seek++];

            part.thingPos[i] = new Vector2(x0, y0);
            part.thingTypes[i] = type;
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

            if (partData.enemyTypes[j] == 0)
            {
                print("creating collider");
                float x1 = partData.enemyAreas[j].x;
                float y1 = partData.enemyAreas[j].y;
                float w1 = partData.enemyAreas[j].width + 1;
                float h1 = partData.enemyAreas[j].height + 1;

                PolygonCollider2D collider = part.go.AddComponent<PolygonCollider2D>();
                Vector2[] points = new Vector2[4];
                points[0] = new Vector2(x1, -y1);
                points[1] = new Vector2(x1 + w1, -y1);
                points[2] = new Vector2(x1 + w1, -y1 - h1);
                points[3] = new Vector2(x1, -y1 - h1);
                collider.points = points;
                continue;
            }

            int amount = 0;
			if (Inventory.vaikeustas < 5) {
				amount = (int)partData.enemyRanges[j].x; //minimum amount of enemies
            } else if (Inventory.vaikeustas >= 5) {
				amount = Random.Range ((int)partData.enemyRanges[j].x, (int)partData.enemyRanges[j].y);
			}
			for (int k = 0; k < amount; k++)
            {
                print("area type: " + partData.enemyTypes[j]);

                float x = x0 + partData.enemyAreas[j].x + Random.Range(0, partData.enemyAreas[j].width);
                float y = y0 + partData.enemyAreas[j].y + Random.Range(0, partData.enemyAreas[j].height);
                if (partData.enemyTypes[j] == 0)
                {
                    continue;
                }
                else if (partData.enemyTypes[j] == 1)
                {
                    GameObject o = Instantiate(gate);
                    BoxCollider2D collider = o.GetComponent<BoxCollider2D>();
                    float x1 = partData.enemyAreas[j].x;
                    float y1 = partData.enemyAreas[j].y;
                    float w1 = partData.enemyAreas[j].width;
                    float h1 = partData.enemyAreas[j].height;
                    collider.size = new Vector2(w1, h1);
                    collider.offset = new Vector2(-w1 / 2, -h1 / 2);
                    o.transform.position = new Vector3(x, -y, 0);
                }
                else if (partData.enemyTypes[j] == 3) //slime monster
                {
                    GameObject o = Instantiate(enemies[partData.enemyTypes[j] - 2]);
                    o.transform.position = new Vector3(x, -y, 0);

                    RandomMovingEnemy component = o.GetComponent<RandomMovingEnemy>();

                    print("creating moving area");
                    float x1 = partData.enemyAreas[j].x;
                    float y1 = partData.enemyAreas[j].y;
                    float w1 = partData.enemyAreas[j].width;
                    float h1 = partData.enemyAreas[j].height;

                    PolygonCollider2D collider = part.go.AddComponent<PolygonCollider2D>();

                    Vector2[] points = new Vector2[4];
                    points[0] = new Vector2(x1, -y1);
                    points[1] = new Vector2(x1 + w1, -y1);
                    points[2] = new Vector2(x1 + w1, -y1 - h1);
                    points[3] = new Vector2(x1, -y1 - h1);
                    collider.points = points;

                    component.MoveArea = collider;
                    component.movespeed = 5.0f;
                }
                else if (partData.enemyTypes[j] == 7) //boss
                {
                    GameObject o = Instantiate(boss);
                    float x1 = partData.enemyAreas[j].x;
                    float y1 = partData.enemyAreas[j].y;
                    float w1 = partData.enemyAreas[j].width + 1;
                    float h1 = partData.enemyAreas[j].height + 1;
                    o.transform.position = new Vector3(x1 + w1 / 2.0f, -(y1 + h1), 0.0f);

                    o.transform.position += new Vector3(0, o.GetComponent<SpriteRenderer>().bounds.size.y, 0);
                }
                else
                {
                    GameObject o = Instantiate(enemies[partData.enemyTypes[j] - 2]);
                    o.transform.position = new Vector3(x, -y, 0);
                }
            }
        }

        print("place things");
        for(int i = 0; i < partData.thingTypes.Length; i++)
        {
            float x = partData.thingPos[i].x;
            float y = partData.thingPos[i].y;
            switch (partData.thingTypes[i])
            {
                case 0:
                    player.position = new Vector3(x + 0.5f, -(y + 0.5f), 0);
                    print("Player position set.");
                    break;
                case 1:
                    GameObject o;
                    o = Instantiate(goal);
                    o.transform.position = new Vector3(x, -(y + 0.5f), 0);
                    break;
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
        texture = new Texture2D(4096, 4096);
        texture.filterMode = FilterMode.Point;
        Texture2D[] textures = new Texture2D[numTextures];
        animations = new LevelAnimation[numAnimated];
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
                LevelAnimation a = new LevelAnimation();
                a.columns = columns[i];
                a.rows = rows[i];
                a.texture = tex;
                tileIndex[tileIndexIndex++] = nAnimations;
                animations[nAnimations++] = a;
            }
            else //divide into multiple textures
            {
                for (int y = 0; y < rows[i]; y++)
                {
                    for (int x = 0; x < columns[i]; x++)
                    {
                        Color[] pixels = tex.GetPixels(x * w1, h0 - (y + 1) * h1, w1, h1);
                        Texture2D t2 = new Texture2D(w1, h1, TextureFormat.ARGB32, false);
                        t2.filterMode = FilterMode.Point;
                        t2.SetPixels(pixels);
                        t2.Apply();
                        tileIndex[tileIndexIndex++] = numTextures;
                        textures[numTextures++] = t2;
                    }
                }
            }
        }
        //create texture atlas and add uv coordinates to a rectangle array
        uvs = texture.PackTextures(textures, 2, 4096, false);

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
                        rect.x = x;// - 0.01f;
                        rect.y = y;// - 0.01f;
                        rect.z = n;
                        rect.w = 1;// .02f;
                        rect.h = 1;// .02f;
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
                float qpx = 0.25f / texture.width; //Quarter pixel, add this to avoid texture bleeding
                float qpy = 0.25f / texture.height;
                Rect uvRect = uvs[tileIndex[staticRects[i].type]];

                uv[i * 4 + 0] = new Vector2(uvRect.x + qpx, uvRect.y + qpy);
                uv[i * 4 + 1] = new Vector2(uvRect.x + uvRect.width - qpx, uvRect.y + qpy);
                uv[i * 4 + 2] = new Vector2(uvRect.x + qpx, uvRect.y + uvRect.height - qpy);
                uv[i * 4 + 3] = new Vector2(uvRect.x + uvRect.width - qpx, uvRect.y + uvRect.height - qpy);
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
        aniMesh.Add( new List<AnimatedMesh>());
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

                float qpx = 0.25f / texture.width; //Quarter pixel, add this to avoid texture bleeding
                float qpy = 0.25f / texture.height;

                uv[k * 4 + 0] = new Vector2(uvRect.x + qpx, uvRect.y + qpy);
                uv[k * 4 + 1] = new Vector2(uvRect.x + uvRect.width - qpx, uvRect.y + qpy);
                uv[k * 4 + 2] = new Vector2(uvRect.x + qpx, uvRect.y + uvRect.height - qpy);
                uv[k * 4 + 3] = new Vector2(uvRect.x + uvRect.width - qpx, uvRect.y + uvRect.height - qpy);
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
            aniMesh[aniMesh.Count - 1].Add(am);
        }
        print("aniMesh.Count = " + aniMesh.Count);

        //create short copy of collision map
        short[] temp2 = new short[w * h];
        for (int i = 0; i < w * h; i++)
        {
            temp2[i] = 0;//(partData.collisionMap[i] ? (short)(1) : (short)(0));
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
        part.go.layer = 8;
        //AstarPath path = AStar.GetComponent<AstarPath>();
        //var graph = AstarPath.active.astarData.gridGraph;
        //graph.width = part.w * 4;
        //graph.depth = part.h * 4;
        //graph.center.x = part.w / 2;
        //graph.center.y = -part.h / 2;
        //graph.UpdateSizeFromWidthDepth();
        //graph.nodeSize = 0.25f;
        //path.Scan();
        return part;
    }
}
