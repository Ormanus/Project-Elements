using UnityEngine;
using System.Collections;

public class GameTestSceneGlowChanger : MonoBehaviour {
	Color oldcolor;

	// Use this for initialization
	void Start () {
		oldcolor = gameObject.GetComponent<SpriteRenderer> ().color;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKeyDown(KeyCode.M)) {
			//pausemode = pause ();
		
		//arvo = 20;
			Debug.Log ("no mo");
			gameObject.GetComponent<SpriteRenderer> ().color = oldcolor;
		}
		if (Input.GetKeyDown(KeyCode.K)) {
			//pausemode = pause ();

			//arvo = 20;
			Debug.Log ("no mo2");
			gameObject.GetComponent<SpriteRenderer> ().color = Color.black;
		}

	}

}
