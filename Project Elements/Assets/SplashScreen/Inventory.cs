using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public static List<KeyValuePair<string, int>> inventory;
    public static int money = 500;
    public static float maxHealth = 1.0f; //sliderissa lisää
    public static float maxMana = 1.0f;

    public static float manaRegen = 0.5f;

	public static float nopeus = 1.0f;
	public static float vaikeustas;//vaikeustason valinta
	public static Color varihahmolle; //väri

    public static int fireLevel = 1;
    public static int airLevel = 1;
    public static int iceLevel = 1;

    public static bool key = false;

    public static int tradeTokens = 3;


    // Use this for initialization
    void Start () {
        inventory = new List<KeyValuePair<string, int>>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
