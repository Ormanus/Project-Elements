using UnityEngine;
using System.Collections;

public class rotate : MonoBehaviour {

    Vector3 mousePos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Vector3 mousePos = Input.mousePosition;
        ///*mousePos.z = 5.23f;*/

        //Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - objectPos.x;
        //mousePos.y = mousePos.y - objectPos.y;
        //Vector3 dir = Input.mousePosition - objectPos;

        ///*float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;*/
        //float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        ///*
        // * 
        // * transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle)); */
        //Debug.Log(angle);
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);



    }
}
