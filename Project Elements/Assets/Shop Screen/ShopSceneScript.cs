using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopSceneScript : MonoBehaviour {
	void Start () {
        GameObject.Find("Stone").GetComponent<Item>().price = (int)(Inventory.maxHealth - PlayerHealth.Playerhealth) * 5;
	}
	public GUIStyle myStyle = null; 
	public GUIStyle headlineStyle = null;

	void OnGUI(){

		headlineStyle.fontSize = 25;
		headlineStyle.padding = new RectOffset (25, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

		if (GUI.Button (new Rect (64, Screen.height - 100, 150, 30), "Continue")) { 
            SceneManager.LoadScene("GameScene");
        }

        //TODO: draw inventory on the right side of the screen?
        //      auto-use items?
	}
	// Update is called once per frame
	void Update () {
		
	}
}
