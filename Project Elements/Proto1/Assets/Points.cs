using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Points : MonoBehaviour {

    public static int points;
    Text teksti;


	// Use this for initialization
	void Start () {
        teksti = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        teksti.text = "Points: " + points.ToString();
	}
}
