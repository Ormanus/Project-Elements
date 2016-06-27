using UnityEngine;
using System.Collections;

public class GameTestScenePauseMenu : MonoBehaviour {

	bool pau4m = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.P)) {
			Application.LoadLevelAdditive (1);
		}
	}
	void OnGUI() {
		if (GUI.Button (new Rect (Screen.width - 155, 50, 100, 104), "paum4")) {
			if (pau4m == false) { 
				pau4m = true;
				Time.timeScale = 1;
				return;
			}
			if (pau4m == true) {
				pau4m = false;
				Time.timeScale = 0;

				return;
			}
		}
	}

}
