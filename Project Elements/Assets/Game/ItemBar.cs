using UnityEngine;
using UnityEngine.UI;
//using System.Collections.Generic;

public class ItemBar : MonoBehaviour {

    public string[] itemNames;
    public Texture2D[] itemTextures;
    public Texture2D defaultTexture;
    public Texture2D selection;
    public Font font;

    private GameObject[] numbers;
    private int theChosenOne;

	void Start () {
        if(Inventory.inventory == null)
        {
            Inventory.inventory = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>();
        }
        numbers = new GameObject[Inventory.inventory.Count];
        print("Inv. count: " + Inventory.inventory.Count);
        for(int i = 0; i < Inventory.inventory.Count; i++)
        {
            numbers[i] = new GameObject();
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

            int index = -1;
            for(int j = 0; j < itemNames.Length; j++)
            {
                if(itemNames[j] == Inventory.inventory[i].Key)
                {
                    index = j;
                }
            }
            if(index != -1)
            {
                GUI.DrawTexture(new Rect(Display.main.renderingWidth - 40, 8 + i * 40, 32, 32), itemTextures[index]);
            }
            else
            {
                GUI.DrawTexture(new Rect(Display.main.renderingWidth - 40, 8 + i * 40, 32, 32), defaultTexture);
            }
        }
    }

    public void choose(int choise)
    {
        theChosenOne = choise;
        print("The Chosen One is now " + choise);
    }

	void Update () {
	
	}
}
