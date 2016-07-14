using UnityEngine;
using UnityEngine.UI;

public class GoldDisplay : MonoBehaviour {

    public Transform gold;
    public Transform health;

    private Text goldText;
    private Text healthText;

    private float hp;
    // Use this for initialization
    void Start () {
        goldText = gold.gameObject.GetComponent<Text>();
        healthText = health.gameObject.GetComponent<Text>();
        hp = PlayerHealth.Playerhealth;
    }

	// Update is called once per frame
	void Update () {
        goldText.text = " Gold: " + Inventory.money;
        healthText.text = " Health: " + hp + "/" + Inventory.maxHealth;
    }
}
