using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class OptionsSceneScript : MonoBehaviour {

	
	

    
    // Use this for initialization
	void Start () {

    }

	void OnGUI(){

        if (GUILayout.Button("Continue"))
        {

            SceneManager.LoadScene("CharacterSelection");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
