using UnityEngine;
using System.Collections;


public class j9p0 : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		GUI.BeginGroup(new Rect(Screen.width / 2 - 400, Screen.height / 2 - 300, 800, 600));
		GUI.Box(new Rect(0, 0, 800, 600), "Valitse jothain00m");


		if (GUI.Button(new Rect(10, 70, 100, 30), "Öptiöns_0m")) Application.LoadLevel (2);
			
		GUI.EndGroup();

	}
}
