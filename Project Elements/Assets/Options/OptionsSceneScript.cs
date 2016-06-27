using UnityEngine;
using System.Collections;

public class OptionsSceneScript : MonoBehaviour {
	public static float hahmoval;//hahmo, jonka valkkaat sliderista
	public static float vaikeustas;//vaikeustason valinta sliderista, löytyy hardista ja softista

	public static float health;

	public static float nopeus;

	public static float voima;

	// Use this for initialization
	void Start () {
		hahmoval = (PlayerPrefs.GetFloat ("talletettuhahmo"));
		vaikeustas = (PlayerPrefs.GetFloat ("valittuvaik"));

		nopeus=(PlayerPrefs.GetFloat ("nopeusarvo" +
			"")); 
		//PlayerPrefs.SetFloat ("testi", 0.1F);
		voima = (PlayerPrefs.GetFloat("voimavalue"));
		health = (PlayerPrefs.GetFloat("healtti"));	}
	public GUIStyle myStyle = null; 

	void OnGUI(){

		
		if (vaikeustas > 5) { 
			GUI.Label (new Rect (180, 126, 100, 20), "hardista",myStyle);
		}
		if (voima > 1) { 
			GUI.Label (new Rect (180, 186, 100, 20), " " + voima + " ");
		}
		if (nopeus > 1) { 
			GUI.Label (new Rect (180, 158, 100, 20), " " + nopeus + " ");
		}
		if (health > 1) { 
			GUI.Label (new Rect (180, 214, 100, 20), " " + health + " ");
		}

		GUI.Box (new Rect (0, 0, 800, 600), "Choose values",myStyle);

		if (GUI.Button (new Rect (240, 70, 150, 30), "Save")) { 

			PlayerPrefs.SetFloat ("valittuvaik", vaikeustas);
			PlayerPrefs.SetFloat ("talletettuhahmo", hahmoval);
			 
			PlayerPrefs.SetFloat ("voimavalue", voima);
			PlayerPrefs.SetFloat ("nopeusarvo", nopeus); 
			PlayerPrefs.SetFloat ("healtti", health);  
		}

		//hahmoval = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hahmoval, 0.0f, 10.0f); GUI.Label (new Rect (25, 0, 100, 20), "hahm2");
		vaikeustas = GUI.HorizontalSlider(new Rect(25, 80, 100, 30), vaikeustas, 0.0f, 10.0f); GUI.Label (new Rect (25, 62, 100, 20), "tasov");

		nopeus = GUI.HorizontalSlider(new Rect(25, 120, 100, 30), nopeus, 0.0f, 50.0f); GUI.Label (new Rect (25, 102, 100, 20), "nopeusarvo");
		health = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), health, 0.0f, 100.0f); GUI.Label (new Rect (25, 137, 100, 20), "health-val");
		voima = GUI.HorizontalSlider(new Rect(25, 182, 100, 30), voima, 0.0f, 30.0f); GUI.Label (new Rect (25, 167, 100, 20), "voima-arvo ");
		//GUILayout.EndVertical ();
		//GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
