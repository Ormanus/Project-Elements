using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {

	//public static float hahmoval;//hahmo, jonka valkkaat sliderista
    
    public static float sliderRedValue = 255.0f;
    public static float sliderBlueValue = 255.0f;
    public static float sliderGreenValue = 255.0f;

    public Texture2D sliderBG;
    public Texture2D sliderKnob;
    public Texture2D buttonTexture;

    public Font font;

    public GUIStyle myStyle = null;
    public GUIStyle basicvaluestyle;
    public GUIStyle headlineStyle;
    public GUIStyle basicheadlinestyle;

    public GUIStyle sliderBackground;
    public GUIStyle sliderHandle;

    public GUIStyle buttonStyle;


    // Use this for initialization
    void Start () {
        //hahmoval = (PlayerPrefs.GetFloat ("talletettuhahmo"));
        Inventory.nopeus = 10f;
        Inventory.maxHealth = 10f;
        Inventory.maxMana = 10f;
	}
	
    float calculateRemainingPoints()
    {
        return 150f - Inventory.nopeus - Inventory.maxHealth - Inventory.maxMana;
    }

	void OnGUI(){

        float remainingPoints = calculateRemainingPoints();

        buttonStyle.normal.background = buttonTexture;
        buttonStyle.alignment = TextAnchor.MiddleCenter;
        buttonStyle.normal.textColor = Color.yellow;
        buttonStyle.fixedHeight = 24;
        buttonStyle.fontSize = 16;
        buttonStyle.font = font;

        headlineStyle.fontSize = 25;
        headlineStyle.fontStyle = FontStyle.Bold;
        headlineStyle.padding = new RectOffset(0, 25, 25, 25); //saadaan otsikko muuttamaan sijaintia
        headlineStyle.normal.textColor = Color.white;

        basicvaluestyle.normal.textColor = new Color(1.0f, 0.9f, 0.3f);

        basicheadlinestyle.normal.textColor = Color.white;

        int x = Display.main.renderingWidth / 2 - 100;
        int y = 64;// Display.main.renderingHeight / 8;

        GUILayout.BeginArea(new Rect(x, y/*(Screen.height / 2) - 420*/, 250, 600));
        GUILayout.Box("Character selection", headlineStyle);


        if (GUILayout.Button("Back", buttonStyle))
        {
            SceneManager.LoadScene("MainMenu");
        }

        sliderBackground.normal.background = sliderBG;
        sliderHandle.normal.background = sliderKnob;
        sliderHandle.fixedHeight = 16;
        sliderHandle.fixedWidth = 16;


        GUILayout.Label("Difficulty", basicheadlinestyle);
		Inventory.vaikeustas = GUILayout.HorizontalSlider(Inventory.vaikeustas, 0.0f, 20.0f, sliderBackground, sliderHandle);
		if (Inventory.vaikeustas > 5)
        {
            GUILayout.Label(" Hard", basicvaluestyle);
        }
		if (Inventory.vaikeustas < 5)
        {
            GUILayout.Label(" Easy", basicvaluestyle);
        }


        GUILayout.Label("Speed", basicheadlinestyle);
		Inventory.nopeus = GUILayout.HorizontalSlider(Inventory.nopeus, 10f, remainingPoints + Inventory.nopeus, sliderBackground, sliderHandle);
		if (Inventory.nopeus > 1)
        {
			GUILayout.Label(" " + Inventory.nopeus + " ", basicvaluestyle);
        }
		if (Inventory.nopeus < 1)
			GUILayout.Label(" " + Inventory.nopeus + " ", basicvaluestyle);

        remainingPoints = calculateRemainingPoints();

        GUILayout.Label("Health", basicheadlinestyle);
		Inventory.maxHealth = GUILayout.HorizontalSlider(Inventory.maxHealth, 10f, remainingPoints + Inventory.maxHealth, sliderBackground, sliderHandle);

		if (Inventory.maxHealth > 1)
        {
            GUILayout.Label(" " + Inventory.maxHealth + " ", basicvaluestyle);
        }
        if (Inventory.maxHealth < 1)
        {
			GUILayout.Label(" " + Inventory.maxHealth + " ", basicvaluestyle);
        }

        remainingPoints = calculateRemainingPoints();

        GUILayout.Label("Mana ", basicheadlinestyle);
		Inventory.maxMana = GUILayout.HorizontalSlider(Inventory.maxMana, 10f, remainingPoints + Inventory.maxMana, sliderBackground, sliderHandle);

        if (Inventory.maxMana > 1)
        {
            GUILayout.Label(" " + Inventory.maxMana + " ", basicvaluestyle);
        }
        if (Inventory.maxMana < 1)
        {
			GUILayout.Label(" " + Inventory.maxMana + " ", basicvaluestyle);
        }

        remainingPoints = calculateRemainingPoints();

        Inventory.nopeus = Mathf.Round(Inventory.nopeus * 2) / 2.0f;
        Inventory.maxHealth = Mathf.Round(Inventory.maxHealth * 2) / 2.0f;
        Inventory.maxMana = Mathf.Round(Inventory.maxMana * 2) / 2.0f;

        //aseta maksimihealth

        //Debug.Log (Inventory.vaikeustas);

		

		
        GUILayout.Label("Character color", basicheadlinestyle);
        GUILayout.Label("Red ", basicheadlinestyle);
        sliderRedValue = GUILayout.HorizontalSlider(sliderRedValue, 0.0F, 255.0F, sliderBackground, sliderHandle);

        GUILayout.Label("Green", basicheadlinestyle);
        sliderGreenValue = GUILayout.HorizontalSlider(sliderGreenValue, 0.0F, 255.0F, sliderBackground, sliderHandle);

        GUILayout.Label("Blue ", basicheadlinestyle);
        sliderBlueValue = GUILayout.HorizontalSlider( sliderBlueValue, 0.0F, 255.0F, sliderBackground, sliderHandle);

        

        if (GUILayout.Button("Start Game", buttonStyle))
        {
            //TODO: reset all
            PlayerHealth.Playerhealth = Inventory.maxHealth;
            if(Inventory.inventory == null)
            {
                Inventory.inventory = new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, int>>();
            }
            Inventory.inventory.Clear();
            SceneManager.LoadScene("InterLevelScene");
        }

		Inventory.varihahmolle = new Color(sliderRedValue / 255, sliderGreenValue / 255, sliderBlueValue / 255, 1f);

        GUILayout.EndArea();

        gameObject.GetComponent<SpriteRenderer>().color = Inventory.varihahmolle;


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}


		

	
