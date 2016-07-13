using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsSceneScript : MonoBehaviour {

	public static float hahmoval;//hahmo, jonka valkkaat sliderista
	public static float vaikeustas;//vaikeustason valinta sliderista, löytyy hardista ja softista
	public static float health;
	public static float nopeus;
	public static float voima;

    public GUIStyle myStyle = null;
    public GUIStyle basicvaluestyle;
    public GUIStyle headlineStyle = null;
    // Use this for initialization
	void Start () {

    }

	void OnGUI(){

        headlineStyle.fontSize = 25;
        headlineStyle.fontStyle = FontStyle.Bold;
        headlineStyle.padding = new RectOffset(0, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia

        basicvaluestyle.normal.textColor = Color.black;

        GUILayout.BeginArea(new Rect((Screen.width / 2)-100, (Screen.height / 2)-200, 250, 400));
        GUILayout.Box("Options", headlineStyle);

		
		
		

		
		if (GUILayout.Button ("Back")) {
			SceneManager.LoadScene ("MainMenu");
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
            GUILayout.Label( " " + nopeus + " ", basicvaluestyle);
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
            GUILayout.Label( " " + health + " ", basicvaluestyle);
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
        if (GUILayout.Button("Continue"))
        {

            SceneManager.LoadScene("CharacterSelection");
        }

        GUILayout.EndArea();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
