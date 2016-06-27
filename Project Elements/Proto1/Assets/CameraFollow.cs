using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


    public GameObject SeurattavaObjekti;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.position = new Vector3(SeurattavaObjekti.transform.position.x, SeurattavaObjekti.transform.position.y,transform.position.z);
	}
}
