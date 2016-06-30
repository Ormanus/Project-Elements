using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

struct Part
{
    public GameObject go;
    public int width;
    public int height;
    public int[] doorStartingPoints;
    public int[] doorWidths;
};

struct Rectangle
{
    public float x, y, w, h;
    public short type;
};

public class GameSceneLevelLoading : MonoBehaviour
{
    public int width;
    public int height;

    public GameObject[] enemies;
    public string[] parts;
    public Material material;

    private Texture2D texture;
    private Rect[] uvs;
    private Part[] level;

    void Start()
    {
        level = new Part[10]; //array of level parts

        for (int j = 0; j < 1; j++)
        {
            //load part data
            Part part = new Part();
            part.doorStartingPoints = new int[4];
            part.doorWidths = new int[4];
            part.go = new GameObject();

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
                collisionMap[i] = true;
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
                        if (type != -1)
                        {
                            collisionMap[x + y * w] = false;
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

            string[] paths = new string[lines.Length / 2];

            int numTextures = 0;

            int pathnum = 0;
            for (int i = startLine; i < lines.Length; i++)
            {
                paths[pathnum] = lines[i].Substring(0, lines[i].Length - 1);
                i++;
                string dims = lines[i];

                rows[pathnum] = short.Parse(dims.Substring(0, 5));
                columns[pathnum] = short.Parse(dims.Substring(5, 5));

                numTextures += rows[pathnum] * columns[pathnum];
                pathnum++;
            }

            texture = new Texture2D(1024, 1024);
            Texture2D[] textures = new Texture2D[numTextures];

            numTextures = 0;
            for (int i = 0; i < pathnum; i++)
            {
                //load texture file
                Texture2D tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                print("loading texture " + i + " @ '" + Application.dataPath + "/Resources/" + paths[i] + "'");
                byte[] texData = File.ReadAllBytes(Application.dataPath + "/Resources/" + paths[i]);
                tex.LoadImage(texData);
                if(tex == null)
                {
                    print("file failure");
                    return;
                }
                int w0 = tex.width;
                int h0 = tex.height;

                print("texdims: " + w0 + "x" + h0);

                //divide et impera
                int w1 = w0 / columns[i];
                int h1 = h0 / rows[i];

                for (int y = 0; y < rows[i]; y++)
                {
                    for (int x = 0; x < columns[i]; x++)
                    {
                        Color[] pixels = tex.GetPixels(x * w1, h0 - (y + 1) * h1, w1, h1);
                        Texture2D t2 = new Texture2D(w1, h1, TextureFormat.ARGB32, false);
                        t2.SetPixels(pixels);
                        t2.Apply();
                        textures[numTextures++] = t2;
                    }
                }
            }

            //create texture atlas and add uv coordinates to a rectangle array
            //texture = textures[0];
            uvs = texture.PackTextures(textures, 4, 1024, false);

            //find doors
            int first = -1;
            int doorWidth = 0;
            for (int x = 0; x < w; x++) //top
            {
                if (collisionMap[x + 0 * w])
                {
                    if(first == -1)
                    {
                        first = x;
                    }
                    doorWidth=x-first;
                }
            }
            part.doorStartingPoints[1] = first;
            part.doorWidths[1] = doorWidth;

            for (int x = 0; x < w; x++) //bottom
            {
                if (collisionMap[x + (h - 1) * w])
                {
                    if (first == -1)
                    {
                        first = x;
                    }
                    doorWidth = x - first;
                }
            }
            part.doorStartingPoints[3] = first;
            part.doorWidths[3] = doorWidth;

            for (int y = 0; y < h; y++) //right
            {
                if (collisionMap[w - 1 + y * w])
                {
                    if (first == -1)
                    {
                        first = y;
                    }
                    doorWidth = y - first;
                }
            }
            part.doorStartingPoints[0] = first;
            part.doorWidths[0] = doorWidth;

            for (int y = 0; y < h; y++) //left
            {
                if (collisionMap[0 + y * w])
                {
                    if (first == -1)
                    {
                        first = y;
                    }
                    doorWidth = y - first;
                }
            }
            part.doorStartingPoints[2] = first;
            part.doorWidths[2] = doorWidth;

            //divide tilemap to rectangles and create drawable mesh
            List<Rectangle> rects = new List<Rectangle>();

            for (int n = num_layers - 1; n >= 0; n--)
            {
                for (int x = 0; x < w; x++)
                {
                    for (int y = 0; y < h; y++)
                    {
                        //rectangle fill
                        if (tilemap[x + y * w + n * w * h] >= 0)
                        {
                            Rectangle rect = new Rectangle();
                            rect.x = x;
                            rect.y = y;
                            rect.w = 1;
                            rect.h = 1;
                            rect.type = tilemap[x + y * w + n * w * h];
                            rects.Add(rect);
                        }
                    }
                }
            }
            //create mesh
            Vector3[] vertices = new Vector3[rects.Count * 4];
            Vector2[] uv = new Vector2[rects.Count * 4];
            int[] triangles = new int[rects.Count * 6];

            for(int i = 0; i < rects.Count; i++)
            {
                float x = rects[i].x;
                float y = rects[i].y;
                float w0 = rects[i].w;
                float h0 = rects[i].h;

                vertices[i * 4 + 0] = new Vector3(x, -y, 0.0f);
                vertices[i * 4 + 1] = new Vector3(x + w0, -y, 0.0f);
                vertices[i * 4 + 2] = new Vector3(x, -y + h0, 0.0f);
                vertices[i * 4 + 3] = new Vector3(x + w0, -y + h0, 0.0f);

                triangles[i * 6 + 0] = i * 4 + 2;
                triangles[i * 6 + 1] = i * 4 + 0;
                triangles[i * 6 + 2] = i * 4 + 1;
                triangles[i * 6 + 3] = i * 4 + 1;
                triangles[i * 6 + 4] = i * 4 + 2;
                triangles[i * 6 + 5] = i * 4 + 3;

                //get ui coordinates
                int type = rects[i].type;

                uv[i * 4 + 0] = new Vector2(uvs[type].x, uvs[type].y);
                uv[i * 4 + 1] = new Vector2(uvs[type].x + uvs[type].width, uvs[type].y);
                uv[i * 4 + 2] = new Vector2(uvs[type].x, uvs[type].y + uvs[type].height);
                uv[i * 4 + 3] = new Vector2(uvs[type].x + uvs[type].width, uvs[type].y + uvs[type].height);
            }
            Mesh mesh = new Mesh();
            mesh.name = "PartMesh";
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;

            part.go.AddComponent<MeshFilter>().mesh = mesh;
            //Material mat = new Material(Shader.Find("Sprites/Default"));
            Material mat = new Material(material);
            mat.mainTexture = texture;
            //mat.SetTexture("_MainTex", texture);
            //mat.SetTexture("Albedo", texture);
            part.go.AddComponent<MeshRenderer>().material = mat;

            //create short copy of collision map
            short[] temp2 = new short[w * h];
            for(int i = 0; i < w * h; i++)
            {
                temp2[i] = (collisionMap[i] ? (short)(1) : (short)(0));
            }

            //divide to collision boxes and create colliders
            short area = 0;
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    //rectangle fill
                    if (temp2[x + y * w] > 0)
                    {
                        Vector2 size = rectangleFill1(0, x, y, w, h, area, temp2);
                        PolygonCollider2D collider = part.go.AddComponent<PolygonCollider2D>();
                        Vector2[] points = new Vector2[4];
                        points[0] = new Vector2(x, y);
                        points[1] = new Vector2(x + size.x, y);
                        points[2] = new Vector2(x + size.x, y + size.y);
                        points[3] = new Vector2(x, y + size.y);
                        collider.points = points;

                        area++;
                    }
                }
            }

            //assign mesh & collider to a gameobject

            level[j] = part;

            //attach part to the level
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

    // Update is called once per frame
    void Update()
    {

    }

    private Vector2 rectangleFill1(int n, int x, int y, int w0, int h0, short value, short[] list)
    {
        short old = list[x + y * w0 + n*w0*h0];
        list[x + y * width] = value;

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
            if (y + h < height && !vd)
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
