using UnityEngine;
using System.Collections;

public class m : MonoBehaviour {
	public static float s1;
	public static float s2;

	public static float m020;

	public static string m20;

	public static float p050;

	public static float kj08;
	// Use this for initialization
	void Start () {
		s2 = (PlayerPrefs.GetFloat ("m0900"));
		s1 = (PlayerPrefs.GetFloat ("m2"));
		p050=(PlayerPrefs.GetFloat ("m")); 
		//PlayerPrefs.SetFloat ("testi", 0.1F);
		kj08 = (PlayerPrefs.GetFloat("hw2"));
		m020 = (PlayerPrefs.GetFloat("meta2"));	}
	void OnGUI(){
		
		if (s2 > 5) { 
			GUI.Label (new Rect (180, 126, 100, 20), "hardista");
		}
		if (m020 > 1) { 
			GUI.Label (new Rect (180, 186, 100, 20), " " + m020 + " ");
		}
		if (p050 > 1) { 
			GUI.Label (new Rect (180, 158, 100, 20), " " + p050 + " ");
		}
		if (kj08 > 1) { 
			GUI.Label (new Rect (180, 214, 100, 20), " " + kj08 + " ");
		}
		
		GUI.BeginGroup (new Rect (Screen.width / 2 - 400, Screen.height / 2 - 300, 800, 600));
		GUI.Box (new Rect (0, 0, 800, 600), "öptiöns2");
		if (s1 > 5.2) {
			Texture2D img2 = Resources.Load ("sprites/gamechar3") as Texture2D;
			if (img2) {
				GUI.DrawTexture (new Rect (150, 20, 20, 40), img2);
				m20 = "sprites/" + img2.name;
			}
		} else if (s1 < 4.8) {
			Texture2D img2 = Resources.Load ("sprites/gamechar2") as Texture2D;
			if (img2) {
				GUI.DrawTexture (new Rect (150, 20, 20, 40), img2);
				m20 = "sprites/" + img2.name;
			}
		}
		if (GUI.Button (new Rect (240, 70, 150, 30), "Save")) { PlayerPrefs.SetFloat ("m2", s1);
			PlayerPrefs.SetString ("p4", m20); PlayerPrefs.SetFloat ("m0900", s2); 
			PlayerPrefs.SetFloat ("hw2", kj08);
			PlayerPrefs.SetFloat ("m", p050); PlayerPrefs.SetFloat ("meta2", m020);  
		}

		s1 = GUI.HorizontalSlider(new Rect(25, 25, 100, 30), s1, 0.0f, 10.0f); GUI.Label (new Rect (25, 0, 100, 20), "hahm2");
		s2 = GUI.HorizontalSlider(new Rect(25, 80, 100, 30), s2, 0.0f, 10.0f); GUI.Label (new Rect (25, 62, 100, 20), "tasov");

		p050 = GUI.HorizontalSlider(new Rect(25, 120, 100, 30), p050, 0.0f, 50.0f); GUI.Label (new Rect (25, 102, 100, 20), "speed2");
		m020 = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), m020, 0.0f, 100.0f); GUI.Label (new Rect (25, 137, 100, 20), "medik0");
		kj08 = GUI.HorizontalSlider(new Rect(25, 182, 100, 30), kj08, 0.0f, 30.0f); GUI.Label (new Rect (25, 167, 100, 20), "pauer0");
		GUI.EndGroup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
