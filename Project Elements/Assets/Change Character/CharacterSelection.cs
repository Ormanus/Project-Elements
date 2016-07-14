using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	//public static float hahmoval;//hahmo, jonka valkkaat sliderista
    public static Color varihahmolle; //väri
    public static float sliderRedValue;
    public static float sliderBlueValue;
    public static float sliderGreenValue;

    public GUIStyle myStyle = null;
    public GUIStyle basicvaluestyle;
    public GUIStyle headlineStyle;
    public GUIStyle basicheadlinestyle;

    public static float nopeus;
    public static float voima;
    public static float hahmoval;//hahmo, jonka valkkaat sliderista
    public static float vaikeustas;//vaikeustason valinta sliderista, löytyy hardista ja softista
    public static float health;
	// Use this for initialization
	void Start () {
		//hahmoval = (PlayerPrefs.GetFloat ("talletettuhahmo"));
	}
	

	void OnGUI(){

        //basicheadlinestyle.padding = new RectOffset(0, 0, 25, 0);
        headlineStyle.fontSize = 25;
        headlineStyle.fontStyle = FontStyle.Bold;
        headlineStyle.padding = new RectOffset(0, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

        basicvaluestyle.normal.textColor = Color.black;

        GUILayout.BeginArea(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 420, 250, 600));
        GUILayout.Box("Character selection", headlineStyle);






        if (GUILayout.Button("Back"))
        {
            SceneManager.LoadScene("MainMenu");
        }


        GUILayout.Label("Difficulty");
        vaikeustas = GUILayout.HorizontalSlider(vaikeustas, 0.0f, 10.0f);
        if (vaikeustas > 5)
        {
            GUILayout.Label(" Hard", basicvaluestyle);
        }
        if (vaikeustas < 5)
        {
            GUILayout.Label(" Easy", basicvaluestyle);
        }


        GUILayout.Label("Speed");
        nopeus = GUILayout.HorizontalSlider(nopeus, 0.0f, 20.0f);
        if (nopeus > 1)
        {
            GUILayout.Label(" " + nopeus + " ", basicvaluestyle);
        }
        if (nopeus < 1)
            GUILayout.Label(" " + nopeus + " ", basicvaluestyle);

        GUILayout.Label("Health");
        health = GUILayout.HorizontalSlider(health, 0.0f, 20.0f);
        if (health > 1)
        {
            GUILayout.Label(" " + health + " ", basicvaluestyle);
        }
        if (health < 1)
        {
            GUILayout.Label(" " + health + " ", basicvaluestyle);
        }

        GUILayout.Label("Strength ");
        voima = GUILayout.HorizontalSlider(voima, 0.0f, 20.0f);
        if (voima > 1)
        {
            GUILayout.Label(" " + voima + " ", basicvaluestyle);
        }
        if (voima < 1)
        {
            GUILayout.Label(" " + voima + " ", basicvaluestyle);
        }
       

        

		

		
        GUILayout.Label("Character's RGB values");
        GUILayout.Label("Red value ");
        sliderRedValue = GUILayout.HorizontalSlider(sliderRedValue, 0.0F, 255.0F);

        GUILayout.Label("Green value");
        sliderGreenValue = GUILayout.HorizontalSlider(sliderGreenValue, 0.0F, 255.0F);

        GUILayout.Label("Blue value ");
        sliderBlueValue = GUILayout.HorizontalSlider( sliderBlueValue, 0.0F, 255.0F);

        if (GUILayout.Button("Start Game"))
        {

            SceneManager.LoadScene("GameTestScene");


        }

		varihahmolle = new Color(sliderRedValue / 255, sliderBlueValue / 255, sliderGreenValue / 255, 1f);//viedään tämä gamesceneen

        GUILayout.EndArea();

        gameObject.GetComponent<SpriteRenderer>().color = varihahmolle;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
