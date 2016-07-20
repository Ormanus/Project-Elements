using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public string itemName;
    public string description;
    public int price;
    public Texture2D background;
    public bool instantEffect;

    private GUIContent content = new GUIContent();
    private int itemNumber = 0;

    private Rect area;
    // Use this for initialization
    void Start () {
        if(Inventory.inventory == null)
        {
            print("Inventory created.");
            Inventory.inventory = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>();
        }
        for(int i = 0; i < Inventory.inventory.Count; i++)
        {
            if(Inventory.inventory[i].Key == itemName)
            {
                itemNumber = Inventory.inventory[i].Value;
                break;
            }
        }

        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        print("extents: " + collider.bounds.extents.x + "x" + collider.bounds.extents.y);
        area = new Rect(collider.bounds.center - collider.bounds.extents, collider.bounds.size);
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
            //Vector2 delta = mousePos - (Vector2)transform.position;
            Vector2 pos = area.position;
            Vector2 size = area.size;
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
                            if(itemNumber == 0)
                            {
                                Inventory.inventory.Add(new System.Collections.Generic.KeyValuePair<string, int>(itemName, 1));
                            }
                            else
                            {
                                for(int i = 0; i < Inventory.inventory.Count; i++)
                                {
                                    if (Inventory.inventory[i].Key == itemName)
                                    {
                                        Inventory.inventory[i] = new System.Collections.Generic.KeyValuePair<string, int>(itemName, itemNumber + 1);
                                        break;
                                    }
                                }
                            }
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
        string youHave;
        if(instantEffect)
        {
            youHave = "Instant effect";
        }
        else
        {
            youHave = "You have " + itemNumber + " " + itemName + "s";
        }
        content.text = "Item: " + itemName + "\n" + description + "\nCost: " + color + price + "</color> GP\n" + youHave;
    }

    private void effect()
    {
        if(itemName == "Life Stone") //rock
        {
            PlayerHealth.Playerhealth = Inventory.maxHealth;
            price = 0;
        }
        else if(itemName == "'Of Energy Conservation'") //book
        {
            Inventory.maxMana += 5.0f;
        }
        else if (itemName == "'Inner Peace'") //book
        {
            Inventory.manaRegen += 0.1f;
        }
        else if (itemName == "'How to Run'") //book
        {
            Inventory.nopeus += 5.0f;
        }
        else if (itemName == "Ironskin Salve") //potion?
        {
            Inventory.maxHealth += 5.0f;
        }
    }
}
