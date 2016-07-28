using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemBar : MonoBehaviour {

    public string[] itemNames;
    public Texture2D[] itemTextures;
    public Texture2D defaultTexture;
    public Texture2D selection;
    public Font font;

    public GameObject orb;

    private List<GameObject> numbers;
    private int theChosenOne;
    private Player player;

	void Start () {
        player = GameObject.Find("Player").GetComponent<Player>();

        if(Inventory.inventory == null)
        {
            Inventory.inventory = new List<KeyValuePair<string, int>>();
        }
        numbers = new List<GameObject>(Inventory.inventory.Count);
        print("Inv. count: " + Inventory.inventory.Count);
        for(int i = 0; i < Inventory.inventory.Count; i++)
        {
            print("text " + i);
            numbers.Add(new GameObject());
            numbers[i].transform.parent = gameObject.transform;
            numbers[i].transform.position = new Vector2(0, 0);
            RectTransform rect = numbers[i].AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(1, 1);
            rect.anchorMax = new Vector2(1, 1);
            rect.transform.parent = numbers[i].transform;
            rect.position = new Vector2(Display.main.renderingWidth - 10, 30 + i * 40);
            numbers[i].AddComponent<CanvasRenderer>();
            Text text = numbers[i].AddComponent<Text>();
            text.text = Inventory.inventory[i].Value.ToString();
            text.font = font;
            text.fontSize = 16;
            text.color = Color.white;
            text.alignment = TextAnchor.UpperRight;
            text.transform.position = new Vector3(Display.main.renderingWidth -52, Display.main.renderingHeight - 80 - i * 40, -1);//new Vector3(0, 0, 0);
        }
        print("texts done");
	}
	
    void OnGUI()
    {
        for(int i = 0; i < Inventory.inventory.Count; i++)
        {
            //find name and texture
            if(theChosenOne == i)
            {
                GUI.DrawTexture(new Rect(Display.main.renderingWidth - 40, 8 + i * 40, 32, 32), selection);
            }
            GUI.DrawTexture(new Rect(Display.main.renderingWidth - 40, 8 + i * 40, 32, 32), findTexture(i));
        }
    }

    private Texture2D findTexture(int choise)
    {
        int index = -1;
        for (int j = 0; j < itemNames.Length; j++)
        {
            if (itemNames[j] == Inventory.inventory[choise].Key)
            {
                index = j;
            }
        }
        if (index != -1)
        {
            return itemTextures[index];
        }
        else
        {
            return defaultTexture;
        }
    }

    public void choose(int choise)
    {
        theChosenOne = choise;
    }

	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Inventory.inventory.Count > 0)
            {
                print("Count: " + Inventory.inventory.Count);
                if (Inventory.inventory[theChosenOne].Key == "Health Potion")
                {
                    PlayerHealth.Playerhealth += 10;
                    if (PlayerHealth.Playerhealth > Inventory.maxHealth)
                    {
                        PlayerHealth.Playerhealth = Inventory.maxHealth;
                    }
                }
                else if (Inventory.inventory[theChosenOne].Key == "Mana Potion")
                {
                    PlayerHealth.Playermana += 10;
                    if (PlayerHealth.Playermana > Inventory.maxMana)
                    {
                        PlayerHealth.Playermana = Inventory.maxMana;
                    }
                }
                else if (Inventory.inventory[theChosenOne].Key == "Speed Potion")
                {
                    PlayerEffect eff = new PlayerEffect();
                    eff.timer = 5.0f;
                    eff.type = 3;
                    eff.amount = 5.0f;
                    Inventory.nopeus += 5.0f;
                    player.effects.Add(eff);
                }
                else if (Inventory.inventory[theChosenOne].Key == "Fire Orb")
                {
                    GameObject o = Instantiate(orb);
                    o.GetComponent<OrbShooting>().element = Element.Fire;
                    o.GetComponent<SpriteRenderer>().sprite = Sprite.Create(findTexture(theChosenOne), new Rect(0, 0, 64, 64), new Vector2(0, 0));
                }
                else if (Inventory.inventory[theChosenOne].Key == "Ice Orb")
                {
                    GameObject o = Instantiate(orb);
                    o.GetComponent<OrbShooting>().element = Element.Ice;
                    o.GetComponent<SpriteRenderer>().sprite = Sprite.Create(findTexture(theChosenOne), new Rect(0, 0, 64, 64), new Vector2(0, 0));
                }
                else if (Inventory.inventory[theChosenOne].Key == "Air Orb")
                {
                    GameObject o = Instantiate(orb);
                    o.GetComponent<OrbShooting>().element = Element.Air;
                    o.GetComponent<SpriteRenderer>().sprite = Sprite.Create(findTexture(theChosenOne), new Rect(0, 0, 64, 64), new Vector2(0, 0));
                }
                else
                {
                    return;
                }
                numbers[theChosenOne].GetComponent<Text>().text = (Inventory.inventory[theChosenOne].Value - 1).ToString();
                if (Inventory.inventory[theChosenOne].Value - 1 == 0)
                {
                    print("deleting...");
                    Inventory.inventory.RemoveAt(theChosenOne);
                    DestroyObject(numbers[theChosenOne]);
                    numbers.RemoveAt(theChosenOne);
                    for(int i = 0; i < Inventory.inventory.Count; i++)
                    {
                        numbers[i].transform.position = new Vector3(Display.main.renderingWidth - 52, Display.main.renderingHeight - 80 - i * 40, -1);
                    }
                    if(theChosenOne > Inventory.inventory.Count - 1)
                    {
                        theChosenOne = Inventory.inventory.Count - 1;
                    }
                    print("delete done.");
                }
                else
                {
                    Inventory.inventory[theChosenOne] = new System.Collections.Generic.KeyValuePair<string, int>(Inventory.inventory[theChosenOne].Key, Inventory.inventory[theChosenOne].Value - 1);
                }
            }
        }
	}
}
