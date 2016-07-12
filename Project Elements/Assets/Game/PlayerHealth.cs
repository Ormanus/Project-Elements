using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
   
    
    public static float Playerhealth = 1;
    public static float Playermana = 1;

    Image HealthImage;

    

    
    void Start()
    {
        Playerhealth = 1;
        Playermana = 1;
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

        if(Playermana < 1)
        {
            Playermana += Time.deltaTime / 5.0f;
        }
    }
}
