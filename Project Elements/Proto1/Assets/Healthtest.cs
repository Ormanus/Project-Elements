﻿using UnityEngine;
using System.Collections;

public class Healthtest : MonoBehaviour
{

    //RectTransform testi;
    // Use this for initialization
    RectTransform testi;
    public float maxhealt = 1;
    public static float Playerhealth;

    
    void Start()
    {
        Playerhealth = maxhealt;
        testi = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Playerhealth < 0)
        {
            Playerhealth = 0;
        }

        testi.transform.localScale = new Vector3(Playerhealth, 1, 1);
    }

}
