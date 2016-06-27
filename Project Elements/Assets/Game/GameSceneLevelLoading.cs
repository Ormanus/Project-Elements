using UnityEngine;

public class LevelLoading : MonoBehaviour
{

    public int width;
    public int height;

    // Use this for initialization
    void Start()
    {
        GameObject[] level; //array of level parts

        //load part data
        TextAsset ta = Resources.Load<TextAsset>("part0");
        short[] data = toShort(ta.bytes);
        short[] part;

        short w = data[0]; //widht
        short h = data[1]; //height
        // 2 & 3 are needed only in editor
        short num_layers = data[4]; //number of layers

        part = new short[w * h * num_layers];
        bool[] collisionMap = new bool[w * h];
        for(int i = 0; i < w * h; i++)
        {
            collisionMap[i] = false;
        }

        //load collision map
        for (int i = 0; i < num_layers; i++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    short type = (short)(data[x + y * w + i * w * h + 5] - 1);
                    part[x + y * w + i * w * h] = type;
                    if(type != -1)
                    {
                        collisionMap[x + y * w] = true;
                    }
                }
            }
        }

        //find doors
        bool empty = false;
        int doorWidth = 0;
        for (int x = 0; x < w; x++) //top
        {
            if (collisionMap[x + 0 * w])
            {
                if(empty)
                {
                    doorWidth++;
                }
                else
                {
                    doorWidth = 0;
                    empty = true;
                }
            }
            else
            {
                empty = false;
            }
        }

        for (int x = 0; x < w; x++) //bottom
        {
            if (collisionMap[x + (h - 1) * w])
            {

            }
        }

        for (int y = 0; y < h; y++) //right
        {
            if (collisionMap[w - 1 + y * w])
            {

            }
        }

        for (int y = 0; y < h; y++)
        {
            if (collisionMap[0 + y * width])
            {

            }
        }

        //divide to boxes

        //divide to collision boxes
        // and create colliders

        //create buffers

        //assign mesh & collider to a gameobject

        //attach part to the level


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
}
