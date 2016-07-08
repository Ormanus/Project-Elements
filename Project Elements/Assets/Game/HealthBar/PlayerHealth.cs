using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{


    RectTransform testi;

    public float maxhealt = 1;
    public static float Playerhealth;

    
    void Start()
    {

        Playerhealth = maxhealt;

        testi = GetComponent<RectTransform>();
    }

    void Update()
    {

        if (Playerhealth < 0)
        {
            Playerhealth = 0;
        }

        if (Playerhealth > 1)
        {
            Playerhealth = 1;

        }

        testi.transform.localScale = new Vector3(Playerhealth, 1, 1);
    }

}
