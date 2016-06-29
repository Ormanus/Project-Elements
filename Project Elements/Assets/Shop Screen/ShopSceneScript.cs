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

		DrawQuad (new Rect (20, 20, 600, 400), Color.black);
		DrawQuad (new Rect (20, 20, 600, 10), Color.blue);
		DrawQuad (new Rect (20, 50, 600, 10), Color.blue);
		DrawQuad (new Rect (10, 20, 20, 400), Color.blue);
		DrawQuad (new Rect (580, 20, 20, 400), Color.blue);
		/*GUI.Button(new Rect(10, 80, 100, 20), new GUIContent("I have a tooltip", "The button overrides the box"));
		GUI.Label(new Rect(10, 40, 100, 40), GUI.tooltip, tempstyle);

		GUI.Button (new Rect (10,10,120,20), new GUIContent ("Click me", "This is the tooltip"));
		if (!string.IsNullOrEmpty(GUI.tooltip))
		GUI.Box (new Rect (10,40,120,40), GUI.tooltip);
		*/

		if (GUI.Button (new Rect (40, 25, 200, 20), new GUIContent ("Osta HP +20", "No joo\n tosi kivahko juttu \n Price 2"))) {
			health = health + 20; 
			pojot = pojot - 2;
		}
		if (!string.IsNullOrEmpty (GUI.tooltip)) {
			GUI.Box (new Rect (40, 48, 210, 80), GUI.tooltip);
			GUI.tooltip = null;
		}
		
		GUI.Button (new Rect (240,25,200,20), new GUIContent ("Osta HP +40", "No joo\n uber great\n Hinta 4"));
		if (!string.IsNullOrEmpty(GUI.tooltip))
			GUI.Box (new Rect (240,48,210,100), GUI.tooltip);
		//GUIUtility.ScaleAroundPivot (Vector2(2, 2), Vector2(328.0, 328.0));

		//GUI.Label (new Rect (200, 200, 256, 256), Color.blue );



		//GUI.Label (new Rect (25, 25, 100, 30), Color.black);

		headlineStyle.fontSize = 25;
		headlineStyle.padding = new RectOffset (25, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

		/*if (health > 1) { //näyttää ruudulla kasvattaessa
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
		}*/

		GUI.Box (new Rect (702, 0, 800, 600), "Welcome to shop. \n - upgrades etc. \n" +
			" - between levels place.", headlineStyle);
		if (GUI.Button (new Rect (20, 450, 150, 30), "Back")) {
			SceneManager.LoadScene ("MainMenu");
		}

		GUI.Label (new Rect (20, 480, 150, 30), "Money:" + pojot);
		GUI.Label (new Rect (20, 500, 150, 30), "HP:" + health);

		if (GUI.Button (new Rect (420, 450, 150, 30), "Continue")) { 

			PlayerPrefs.SetFloat ("healtti", health);
			PlayerPrefs.SetFloat ("pojolkm", 20f);
		}
		//GUILayout.BeginArea (new Rect (10,310,200,300));
		//GUILayout.BeginVertical(GUI.skin.box);
		//pojot = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), pojot, 0.0f, 100.0f); GUI.Label (new Rect (25, 127, 132, 20), "Tasotoimipistekertymä");
		//health = GUI.HorizontalSlider(new Rect(25, 220, 100, 30), health, 0.0f, 100.0f); GUI.Label (new Rect (25, 203, 100, 20), "health-val");
		//GUILayout.Label ("Trade for gain of health");
		/*GUILayout.Label("add health based on your tasopistekertymä");
		tradehealth = GUILayout.HorizontalSlider(tradehealth, 0.0F, 100.0F); 
		if (GUILayout.Button ("get more health"))
			PlayerPrefs.SetFloat ("healtti", tradehealth + pojot);
		GUILayout.Label(".m,");
		tradeablepojot = GUILayout.HorizontalSlider(tradeablepojot, 0.0F, 100.0F); 

		GUILayout.EndVertical();
		GUILayout.EndArea ();
		*/
		//pojot = GUI.HorizontalSlider(new Rect(25, 150, 100, 30), pojot, 0.0f, 100.0f); GUI.Label (new Rect (25, 127, 132, 20), "Tasotoimipistekertymä");
		//health = GUI.HorizontalSlider(new Rect(25, 220, 100, 30), health, 0.0f, 100.0f); GUI.Label (new Rect (25, 203, 100, 20), "health-val");
		//GUILayout.EndVertical ();
		//GUI.EndGroup ();
	}
	void DrawQuad(Rect position, Color color) {
	
		Texture2D texture = new Texture2D(1, 1);//memory eater? that's what they say or not
		texture.SetPixel(0,0,color);
		texture.Apply();
		GUI.skin.box.normal.background = texture;
		GUI.Box(position, GUIContent.none);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
