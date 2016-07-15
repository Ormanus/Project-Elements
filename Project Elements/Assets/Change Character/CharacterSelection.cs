using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	//public static float hahmoval;//hahmo, jonka valkkaat sliderista
    
    public static float sliderRedValue = 255.0f;
    public static float sliderBlueValue = 255.0f;
    public static float sliderGreenValue = 255.0f;

    public GUIStyle myStyle = null;
    public GUIStyle basicvaluestyle;
    public GUIStyle headlineStyle;
    public GUIStyle basicheadlinestyle;

    
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
		Inventory.vaikeustas = GUILayout.HorizontalSlider(Inventory.vaikeustas, 0.0f, 20.0f);
		if (Inventory.vaikeustas > 5)
        {
            GUILayout.Label(" Hard", basicvaluestyle);
        }
		if (Inventory.vaikeustas < 5)
        {
            GUILayout.Label(" Easy", basicvaluestyle);
        }


        GUILayout.Label("Speed");
		Inventory.nopeus = GUILayout.HorizontalSlider(Inventory.nopeus, 0.1f, 20.0f);
		if (Inventory.nopeus > 1)
        {
			GUILayout.Label(" " + Inventory.nopeus + " ", basicvaluestyle);
        }
		if (Inventory.nopeus < 1)
			GUILayout.Label(" " + Inventory.nopeus + " ", basicvaluestyle);

        GUILayout.Label("Health");
		Inventory.maxHealth = GUILayout.HorizontalSlider(Inventory.maxHealth, 0.1f, 100.0f);
		if (Inventory.maxHealth > 1)
        {
            GUILayout.Label(" " + Inventory.maxHealth + " ", basicvaluestyle);
        }
        if (Inventory.maxHealth < 1)
        {
			GUILayout.Label(" " + Inventory.maxHealth + " ", basicvaluestyle);
        }

        GUILayout.Label("Mana ");
		Inventory.maxMana = GUILayout.HorizontalSlider(Inventory.maxMana, 0.1f, 5.0f);
        if (Inventory.maxMana > 1)
        {
            GUILayout.Label(" " + Inventory.maxMana + " ", basicvaluestyle);
        }
        if (Inventory.maxMana < 1)
        {
			GUILayout.Label(" " + Inventory.maxMana + " ", basicvaluestyle);
        }
       

        //aseta maksimihealth

		Debug.Log (Inventory.vaikeustas);

		

		
        GUILayout.Label("Character's RGB values");
        GUILayout.Label("Red value ");
        sliderRedValue = GUILayout.HorizontalSlider(sliderRedValue, 0.0F, 255.0F);

        GUILayout.Label("Green value");
        sliderGreenValue = GUILayout.HorizontalSlider(sliderGreenValue, 0.0F, 255.0F);

        GUILayout.Label("Blue value ");
        sliderBlueValue = GUILayout.HorizontalSlider( sliderBlueValue, 0.0F, 255.0F);

        

        if (GUILayout.Button("Start Game"))
        {
            //TODO: reset all
            PlayerHealth.Playerhealth = Inventory.maxHealth;
            if(Inventory.inventory == null)
            {
                Inventory.inventory = new System.Collections.Generic.List<string>();
            }
            Inventory.inventory.Clear();
            SceneManager.LoadScene("GameScene");
        }

		Inventory.varihahmolle = new Color(sliderRedValue / 255, sliderGreenValue / 255, sliderBlueValue / 255, 1f);

        GUILayout.EndArea();

        gameObject.GetComponent<SpriteRenderer>().color = Inventory.varihahmolle;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


		

	
