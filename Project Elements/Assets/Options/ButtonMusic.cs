using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonMusic : MonoBehaviour {
	public GameObject soundPlay;

	// Use this for initialization
	void Start () {
		//soundPlay = GameObject.Find("AudioSourceGameObj");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClick() {
		AudioSource music = soundPlay.GetComponent<AudioSource>();
		if (MusicManager.volumeLevel == 0F) { music.volume = 0.6F;
		} else {
			music.volume = MusicManager.volumeLevel;
		}
		music.Play ();

	}
}
