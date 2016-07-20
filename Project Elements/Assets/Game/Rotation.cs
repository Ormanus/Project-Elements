using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 relativePos = (target.position + new Vector3(0, 0.5f, 0)) - transform.position;
        Quaternion rotation = Quaternion.LookRotation(relativePos);
        Quaternion current = transform.localRotation;

        transform.localRotation = Quaternion.Slerp(current, rotation, Time.deltaTime * 4);
        transform.Translate(0, 0, 10 * Time.deltaTime);
        //transform.RotateAround(target.position,Vector3.up,180*Time.deltaTime);
    }
}
