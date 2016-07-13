using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

    public string itemName;
    public string description;
    public int price;
    public Vector2 size;
    public Texture2D background;

    private GUIContent content = new GUIContent();
    // Use this for initialization
    void Start () {
        content.image = background;
        content.text = "Item: " + itemName + "\n" + description + "\nCost: " + price + " GP";
    }
	
	void OnGUI ()
    {
	    //if(Input.GetMouseButtonDown(0))
        {
            //Vector2 center = size / 2;
            //Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GUI.skin.button.normal.background = (Texture2D)content.image;
            GUI.skin.button.hover.background = (Texture2D)content.image;
            GUI.skin.button.active.background = (Texture2D)content.image;

            Vector2 mousePos = Input.mousePosition;
            Vector2 delta = mousePos - (Vector2)transform.position;
            Vector2 pos = transform.position;
            pos.y -= size.y / 2;
            if (mousePos.x > pos.x && mousePos.y > pos.y && mousePos.x < pos.x + size.x && mousePos.y < pos.y + size.y)
            {
                GUI.contentColor = Color.yellow;
                Rect rect = new Rect(new Vector2(mousePos.x - 8,  Display.main.renderingHeight - mousePos.y - 8), new Vector2(128, 128));
                if(GUI.Button(rect, content))
                {
                    print(itemName + " bought!");
                    //if(Inventory.getGold() >= price)
                    //{
                    //  Inventory.Add(itemName);
                    //  Inventory.AddGold(-price);
                    //}
                }
            }
        }
	}

    void Update()
    {

    }
}
