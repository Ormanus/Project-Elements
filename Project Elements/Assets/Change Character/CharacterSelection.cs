using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {
	public static float hahmoval;//hahmo, jonka valkkaat sliderista


	public static string spritensijainti;



	// Use this for initialization
	void Start () {
		hahmoval = (PlayerPrefs.GetFloat ("talletettuhahmo"));
	}
	
	public GUIStyle myStyle = null; 

	void OnGUI(){

		if (hahmoval > 5.2) {
			Texture2D img2 = Resources.Load ("sprites/gamechar3") as Texture2D;
			if (img2) {
				GUI.DrawTexture (new Rect (150, 20, 20, 40), img2);
				spritensijainti = "sprites/" + img2.name;
			}
		} else if (hahmoval < 4.8) {
			Texture2D img2 = Resources.Load ("sprites/gamechar2") as Texture2D;
			if (img2) {
				GUI.DrawTexture (new Rect (150, 20, 20, 40), img2);
				spritensijainti = "sprites/" + img2.name;
			}
		}


		GUI.Box (new Rect (0, 0, 800, 600), "Choose values",myStyle);

		if (GUI.Button (new Rect (240, 70, 150, 30), "Start Game")) { 

		
			PlayerPrefs.SetFloat ("talletettuhahmo", hahmoval);
			PlayerPrefs.SetString ("spritelokaatio", spritensijainti);
            SceneManager.LoadScene("GameTestScene");
			//Application.LoadLevel (1); //Opens the GameTestScene

			  
		}

		hahmoval = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), hahmoval, 0.0f, 10.0f); GUI.Label (new Rect (25, 0, 100, 20), "Valitse kuvake");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
