using UnityEngine;
using System.Collections;

public class pref2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load (PlayerPrefs.GetString("p4"), typeof(Sprite)) as Sprite;
		PlayerPrefs.SetFloat ("testi", 0.1F);
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (PlayerPrefs.GetString ("p4"));
		Debug.Log (PlayerPrefs.GetFloat ("meta2"));
	}
}
