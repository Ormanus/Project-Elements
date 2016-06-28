using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopSceneScript : MonoBehaviour {
	//health, ei alus tarv. muuta

	public static float health;

	// Use this for initialization
	void Start () {
		
		health = (PlayerPrefs.GetFloat("healtti"));	}
	public GUIStyle myStyle = null; 
	public GUIStyle headlineStyle = null;

	void OnGUI(){
		headlineStyle.fontSize = 25;
		headlineStyle.padding = new RectOffset (25, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

		if (health > 1) { 
			GUI.Label (new Rect (180, 214, 100, 20), " " + health + " ");
		}

		GUI.Box (new Rect (0, 0, 800, 600), "Welcome to shop upgrades etc. between levels", headlineStyle);
		if (GUI.Button (new Rect (240, 120, 150, 30), "Back")) {
			SceneManager.LoadScene ("MainMenu");
		}
		if (GUI.Button (new Rect (240, 70, 150, 30), "Save")) { 

			PlayerPrefs.SetFloat ("healtti", health);  
		}


		health = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), health, 0.0f, 100.0f); GUI.Label (new Rect (25, 137, 100, 20), "health-val");
		//GUILayout.EndVertical ();
		//GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
