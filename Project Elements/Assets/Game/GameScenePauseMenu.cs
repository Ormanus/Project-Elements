using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class GameScenePauseMenu : MonoBehaviour {


	bool pausemode = false;
	// Use this for initialization




 

 
 
     // Use this for initialization
     void Start () {
       
     }
     
     // Update is called once per frame
     void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			pausemode = pause ();
		}
        
     }
 
   

	bool pause() {
		if (Time.timeScale == 0f) {
			Time.timeScale = 1f;
			return (false);
		} else {
			Time.timeScale = 0f;
			return (true);
		}
	}
 

 
     void OnGUI ()
	{
      
       
		if (pausemode) {
			GUILayout.Label ("Game paused");
			if (GUILayout.Button ("Unpause the game"))
				pausemode = pause ();
			//GUILayout.Label ("Main menu");
			if (GUILayout.Button ("Main menu"))
				SceneManager.LoadScene ("MainMenu");
		}
	

         
	}
}
 



