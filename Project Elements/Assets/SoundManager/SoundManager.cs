using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SoundManager : MonoBehaviour {
	
	public Slider soundSlider;

	public void Start()
	{
		//AudioSource music = GetComponent<AudioSource> (); 
		//Adds a listener to the main slider and invokes a method when the value changes.
		//mainSlider.value = 0.5f;
		soundSlider.onValueChanged.AddListener (delegate {
			ValueChangeCheck ();
		});

	}

	// Invoked when the value of the slider changes.
	public void ValueChangeCheck()
	{
		Debug.Log (soundSlider.value);
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
