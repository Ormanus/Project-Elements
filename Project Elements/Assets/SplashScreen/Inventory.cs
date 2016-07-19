using UnityEngine;
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

    public static List<KeyValuePair<string, int>> inventory;
    public static int money = 500;
    public static Weapon weapon = Weapon.WoodenStaff;
    public static float maxHealth = 1.0f; //sliderissa lisää
    public static float maxMana = 1.0f;

    public static float manaRegen = 1.0f;

	public static float nopeus = 1.0f;
	public static float vaikeustas;//vaikeustason valinta
	public static Color varihahmolle; //väri
    // Use this for initialization
    void Start () {
        inventory = new List<KeyValuePair<string, int>>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
