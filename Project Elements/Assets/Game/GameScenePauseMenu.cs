using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;



public class GameScenePauseMenu : MonoBehaviour {

	public bool music_offon = true;
	public static bool sound_offon = true;
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
			music_offon = GUILayout.Toggle(music_offon, "music on or off");
			sound_offon = GUILayout.Toggle(sound_offon, "sound on or off");
			if (music_offon == false) {
				AudioSource music = Player.ASGO.GetComponent<AudioSource>();


				music.mute = true;
			}
			if (music_offon == true) {

				AudioSource music = Player.ASGO.GetComponent<AudioSource>();
				music.mute = false;
			}
			if (sound_offon == false) {
				AudioSource sound = Player.SoundGO.GetComponent<AudioSource>();
				sound.mute = true;


			}
			if (sound_offon == true) {
				AudioSource sound = Player.SoundGO.GetComponent<AudioSource>();
				sound.mute = false;


			}
		}
	

         
	}
}
 



