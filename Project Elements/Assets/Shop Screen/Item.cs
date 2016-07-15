using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public string itemName;
    public string description;
    public int price;
    public Vector2 size;
    public Texture2D background;
    public bool instantEffect;

    private GUIContent content = new GUIContent();
    private int itemNumber = 0;
    // Use this for initialization
    void Start () {
        if(Inventory.inventory == null)
        {
            print("Inventory created.");
            Inventory.inventory = new System.Collections.Generic.List<string>();
        }
        for(int i = 0; i < Inventory.inventory.Count; i++)
        {
            if(Inventory.inventory[i] == itemName)
            {
                itemNumber++;
            }
        }
        updateText();
    }
	
	void OnGUI ()
    {
	    //if(Input.GetMouseButtonDown(0))
        {
            //Vector2 center = size / 2;
            //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GUI.skin.button.normal.background = background;
            GUI.skin.button.hover.background = background;
            GUI.skin.button.active.background = background;

            Vector2 mousePos = Input.mousePosition;
            Vector2 delta = mousePos - (Vector2)transform.position;
            Vector2 pos = transform.position;
            pos.y -= size.y / 2;
            if (mousePos.x > pos.x && mousePos.y > pos.y && mousePos.x < pos.x + size.x && mousePos.y < pos.y + size.y)
            {
                updateText();
                GUI.contentColor = Color.yellow;
                Rect rect = new Rect(new Vector2(mousePos.x - 8,  Display.main.renderingHeight - mousePos.y - 8), new Vector2(256, 80));
                if(GUI.Button(rect, content))
                {
                    if (Inventory.money >= price)
                    {
                        Inventory.money -= price;
                        if (instantEffect)
                        {
                            effect();
                            print(itemName + " used!");
                        }
                        else
                        {
                            Inventory.inventory.Add(itemName);
                            itemNumber++;
                            print(itemName + " bought!");
                        }
                    }
                }
            }
        }
	}

    void Update()
    {

    }

    private void updateText()
    {
        string color;
        if(price > Inventory.money)
        {
            color = "<color=#FF0000>";
        }
        else
        {
            color = "<color=#00FF00>";
        }
        content.text = "Item: " + itemName + "\n" + description + "\nCost: " + color + price + "</color> GP\nYou have " + itemNumber + " " + itemName + "s";
    }

    private void effect()
    {
        if(itemName == "Armor")
        {
            Inventory.maxHealth += 10;
        }
        else if(itemName == "Water Book")
        {
            Inventory.maxMana += 10;
        }
        else if (itemName == "Fire Book")
        {
            Inventory.manaRegen += 0.5f;
        }
        else if (itemName == "Potion")
        {
            PlayerHealth.Playerhealth += 10f;
            if(PlayerHealth.Playerhealth > Inventory.maxHealth)
            {
                PlayerHealth.Playerhealth = Inventory.maxHealth;
            }
        }
    }
}
