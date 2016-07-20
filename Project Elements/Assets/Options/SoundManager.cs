using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	public Slider soundSlider;
	public static float volumeLevel;

	public void Start()
	{
		//AudioSource music = GetComponent<AudioSource> (); 
		//Adds a listener to the main slider and invokes a method when the value changes.
		//mainSlider.value = 0.5f;
		soundSlider.onValueChanged.AddListener (delegate {
			ValueChangeCheck ();
		});
		soundSlider.value = volumeLevel;

	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{
		volumeLevel = soundSlider.value;
		Debug.Log (soundSlider.value);
	}



	
	// Update is called once per frame
	void Update () {
	
	}
}
