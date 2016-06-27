using UnityEngine;

public class LevelLoading : MonoBehaviour
{

    public int width;
    public int height;

    // Use this for initialization
    void Start()
    {
        short[] map = new short[width * height];

        TextAsset ta = Resources.Load<TextAsset>("part0");
        short[] data = toShort(ta.bytes);
        short[] part;

        short w = data[0]; //widht
        short h = data[1]; //height
        // 2 & 3 are needed only in editor
        short num_layers = data[4]; //number of layers

        part = new short[w * h * num_layers];

        for (int i = 0; i < num_layers; i++)
        {
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    short type = data[x + y * w + i * w * h];

                }
            }
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

    // Update is called once per frame
    void Update()
    {

    }
}
