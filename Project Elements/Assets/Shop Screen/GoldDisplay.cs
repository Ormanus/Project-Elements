using UnityEngine;
using UnityEngine.UI;

public class GoldDisplay : MonoBehaviour {

    public Transform gold;
    public Transform health;
    public Transform mana;
    public Transform speed;

    private Text goldText;
    private Text healthText;
    private Text manaText;
    private Text speedText;

    // Use this for initialization
    void Start () {
        goldText = gold.gameObject.GetComponent<Text>();
        healthText = health.gameObject.GetComponent<Text>();
        manaText = mana.gameObject.GetComponent<Text>();
        speedText = speed.gameObject.GetComponent<Text>();
    }

	// Update is called once per frame
	void Update () {
        goldText.text = " Gold: " + Inventory.money;
        healthText.text = " Health: " + PlayerHealth.Playerhealth + "/" + Inventory.maxHealth;
        manaText.text = " Max. Mana: " + Inventory.maxMana + ", Regen: " + Inventory.manaRegen;
        speedText.text = " Speed: " + Inventory.nopeus;
    }
}
