using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
   
    
    public static float Playerhealth = 1;

    Image HealthImage;

    

    
    void Start()
    {
   
        HealthImage = GameObject.Find("Main Camera").transform.FindChild("Canvas").FindChild("HealthBar").FindChild("Health").GetComponent<Image>();
   
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

        HealthImage.transform.localScale = new Vector3(Playerhealth, 1, 1);

    }

}
