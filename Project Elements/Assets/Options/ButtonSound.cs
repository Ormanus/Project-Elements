using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ButtonSound : MonoBehaviour {
	public GameObject soundPlay;

	// Use this for initialization
	void Start () {
		//soundPlay = GameObject.Find("AudioSourceGameObj");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void onClick() {
		AudioSource sound = soundPlay.GetComponent<AudioSource>();
		if (SoundManager.volumeLevel == 0F) { sound.volume = 0.6F;
		} else {
			sound.volume = SoundManager.volumeLevel;
		}
		sound.Play ();

	}
}
