using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Transitions: MonoBehaviour {

    private float timer;

	// Use this for initialization
	void Start () {
        timer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if (timer > 2.0f)
        {
            SceneManager.LoadScene("MainMenu");
        }
	}
}
