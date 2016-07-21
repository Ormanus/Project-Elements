//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections;

//public class MusicManager : MonoBehaviour {
	
//	public Slider musicSlider;
//	public AudioSource song;
//	public static float volumeLevel;

//	public void Start()
//	{
//		//AudioSource music = GetComponent<AudioSource> (); 
//		//Adds a listener to the main slider and invokes a method when the value changes.
//		//mainSlider.value = 0.5f;
//		musicSlider.onValueChanged.AddListener (delegate {
//			ValueChangeCheck ();
//		});

//	}

//	// Invoked when the value of the slider changes.
//	public void ValueChangeCheck()
//	{
//		volumeLevel = musicSlider.value;
//		Debug.Log (musicSlider.value);
//	}

	
//	// Update is called once per frame
//	void Update () {
	
//	}

//}
