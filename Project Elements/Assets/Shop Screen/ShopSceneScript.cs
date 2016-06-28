using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ShopSceneScript : MonoBehaviour {
	//health, ei alus tarv. muuta

	public static float health;
	public static float pojot; //tasossa tehdyt jutut
	public static float tradehealth;
	public static float tradeablepojot;

	public float hSliderValue = 0.0F;
	// Use this for initialization
	void Start () {
		
	health = (PlayerPrefs.GetFloat("healtti"));
	pojot = (PlayerPrefs.GetFloat("pojolkm"));	
	}
	public GUIStyle myStyle = null; 
	public GUIStyle headlineStyle = null;

	void OnGUI(){
		headlineStyle.fontSize = 25;
		headlineStyle.padding = new RectOffset (25, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

		if (health > 1) { //näyttää ruudulla kasvattaessa
			GUI.Label (new Rect (180, 214, 100, 20), " " + health + " ");
		}
		if (pojot > 1) { //näyttää ruudulla kasvattaessa
			GUI.Label (new Rect (180, 244, 100, 20), " " + pojot + " ");
		}
		if (pojot > 1) { //näyttää ruudulla kasvattaessa
			GUI.Label (new Rect (180, 264, 100, 20), " " + tradehealth + " ");
		}
		if (pojot > 1) { //näyttää ruudulla kasvattaessa
			GUI.Label (new Rect (180, 284, 100, 20), " " + tradeablepojot + " ");
		}

		GUI.Box (new Rect (0, 0, 800, 600), "Welcome to shop upgrades etc. between levels.", headlineStyle);
		if (GUI.Button (new Rect (240, 120, 150, 30), "Back")) {
			SceneManager.LoadScene ("MainMenu");
		}
		if (GUI.Button (new Rect (240, 70, 150, 30), "Suorita maksu")) { 

			PlayerPrefs.SetFloat ("healtti", health);
			PlayerPrefs.SetFloat ("pojolkm", 20f);
		}
		GUILayout.BeginArea (new Rect (10,310,200,300));
		GUILayout.BeginVertical(GUI.skin.box);
		pojot = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), pojot, 0.0f, 100.0f); GUI.Label (new Rect (25, 127, 132, 20), "Tasotoimipistekertymä");
		health = GUI.HorizontalSlider(new Rect(25, 220, 100, 30), health, 0.0f, 100.0f); GUI.Label (new Rect (25, 203, 100, 20), "health-val");
		//GUILayout.Label ("Trade for gain of health");
		GUILayout.Label("add health based on your tasopistekertymä");
		tradehealth = GUILayout.HorizontalSlider(tradehealth, 0.0F, 100.0F); 
		if (GUILayout.Button ("get more health"))
			PlayerPrefs.SetFloat ("healtti", tradehealth + pojot);
		GUILayout.Label(".m,");
		tradeablepojot = GUILayout.HorizontalSlider(tradeablepojot, 0.0F, 100.0F); 

		GUILayout.EndVertical();
		GUILayout.EndArea ();

		pojot = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), pojot, 0.0f, 100.0f); GUI.Label (new Rect (25, 127, 132, 20), "Tasotoimipistekertymä");
		health = GUI.HorizontalSlider(new Rect(25, 220, 100, 30), health, 0.0f, 100.0f); GUI.Label (new Rect (25, 203, 100, 20), "health-val");
		//GUILayout.EndVertical ();
		//GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
