using UnityEngine;
using System.Collections;

public class PlaceTheImage : MonoBehaviour {
    public Camera mainCamera;
    public Vector3 screenPosition = new Vector3(200, 200, 50);

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
