using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopSceneScript : MonoBehaviour {
	void Start () {
	}
	public GUIStyle myStyle = null; 
	public GUIStyle headlineStyle = null;

	void OnGUI(){

		headlineStyle.fontSize = 25;
		headlineStyle.padding = new RectOffset (25, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

		GUI.Box (new Rect (702, 0, 800, 600), "Welcome to shop.", headlineStyle);

		if (GUI.Button (new Rect (420, 452, 150, 30), "Continue")) { 
            SceneManager.LoadScene("GameTestScene");
        }
	}
	// Update is called once per frame
	void Update () {
		
	}
}
