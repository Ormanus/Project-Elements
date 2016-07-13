﻿using UnityEngine;
using System.Collections.Generic;

public enum Weapon
{
    WoodenStaff,
    DragonTooth,
    RedManaInfusedCrowbar,
    ObsidianDagger,
    DiamondSword,
    ThunderfuryBlessedBladeOfTheWindseeker
};

public class Inventory : MonoBehaviour {

    public static List<string> inventory;
    public static int money = 500;
    public static Weapon weapon = Weapon.WoodenStaff;
    public static float maxHealth = 1.0f;
    public static float maxMana = 1.0f;

    // Use this for initialization
    void Start () {
        inventory = new List<string>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
