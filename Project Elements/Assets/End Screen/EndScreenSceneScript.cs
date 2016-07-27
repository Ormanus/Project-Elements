using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndScreenSceneScript : MonoBehaviour {
	public float currentHealth;
	void Start() {
        //PlayerPrefs.SetFloat ("healtti", 3000);
        GameSceneLevelLoading.levelNumber = 0;
        currentHealth = 0;
    }
	void Update() {

	}
	void endGame(){

	}
	void OnGUI(){
		GUILayout.BeginArea (new Rect ((Screen.width / 2) - 50, (Screen.height / 2), 200, 300));
		GUILayout.Label ("Health nyt: " + currentHealth);
		if (currentHealth == 0) {
			GUILayout.Label ("Game over. Try again?");
			if (GUILayout.Button ("Retry"))
				SceneManager.LoadScene ("CharacterSelection");
			//GUILayout.Label ("Main menu");
			if (GUILayout.Button ("Quit to main menu"))
				SceneManager.LoadScene ("MainMenu");
		}
		GUILayout.EndArea ();
		
		//if (currentHealth == 0)
		//	endGame ();
	}
}
