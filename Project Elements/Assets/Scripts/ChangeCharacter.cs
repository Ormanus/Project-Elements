using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ChangeCharacter : MonoBehaviour 
{
	public float SliderValue = 0.0f;
	public static string img_polku; //käytetään spriterenderissä toisessa scenessä
	public string kuvanyt;

	void Start() {
		kuvanyt = PlayerPrefs.GetString ("imagepath");
	}
	void Update() {
		//Debug.Log (hSliderValue);
	}

	void OnGUI() {
		SliderValue = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), SliderValue, 0.0f, 10.0f);
		Debug.Log (SliderValue);
		if (GUI.Button (new Rect (10, 70, 30, 30), "X")) {
			PlayerPrefs.SetString ("imagepath", kuvanyt);
			Application.LoadLevel (1);

			//myTexture = (Sprite)Resources.Load("sprites/gamechar2");

		}
		if (GUI.Button (new Rect (140, 70, 150, 30), "Save")) {
			PlayerPrefs.SetString ("imagepath", img_polku);
			Application.LoadLevel (2);
			//myTexture = (Sprite)Resources.Load("sprites/gamechar2");

		}

		if (SliderValue > 5) {
			Texture2D img2 = Resources.Load ("sprites/gamechar2") as Texture2D;
			GUI.Label(new Rect(120, 132, 100, 20), img2.name);
			if (img2) {
				GUI.DrawTexture (new Rect (20, 20, 20, 40), img2);
				img_polku = "sprites/" + img2.name;
				PlayerPrefs.SetString ("imagepath", img_polku);
			} 
		} else {
			Texture2D img2 = Resources.Load ("sprites/gamechar3") as Texture2D;
			GUI.Label(new Rect(120, 132, 100, 20), img2.name);
			if (img2) {
				GUI.DrawTexture (new Rect (20, 20, 20, 40), img2);
				img_polku = "sprites/" + img2.name;
				PlayerPrefs.SetString ("imagepath", img_polku);
			}
		}
	}

}
