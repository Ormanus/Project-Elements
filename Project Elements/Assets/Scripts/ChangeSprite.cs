using UnityEngine;
using System.Collections;

public class ChangeSprite : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
		//string aT =gameObject.GetComponent<ViewModel> ().get_imgpolku ().ToString();
		//Debug.Log (aT);
		//MapGenerator mp = GetComponent<MapGenerator> ();
		//mp.GenerateMap ();
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load (PlayerPrefs.GetString("imagepath"), typeof(Sprite)) as Sprite;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
