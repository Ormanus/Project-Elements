﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
   
    
    public static float Playerhealth = Inventory.maxHealth;
    public static float Playermana = 1;

    Image HealthImage;

    

    
    void Start()
    {

        Playerhealth = Inventory.maxHealth;
        Playermana = Inventory.maxMana;

    }

    void Update()
    {

        if (Playerhealth < 0)
        {
            Playerhealth = 0;
        }

        if (Playerhealth > Inventory.maxHealth)
        {
			Playerhealth = Inventory.maxHealth;

        }

        if(Playermana < 1)
        {
            Playermana += Time.deltaTime / 5.0f;
        }
    }
}
