﻿using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {


    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    
    public Vector3 offset;
    Vector3 targetPos;


    GameObject target;
	// Use this for initialization
	void Start () {
        targetPos = transform.position;
        target = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {
        //transform.position = new Vector3(pelaaja.transform.position.x, pelaaja.transform.position.y, Camera.main.transform.position.z);


        Vector3 posNoZ = transform.position;
        posNoZ.z = target.transform.position.z;

        Vector3 targetDirection = (target.transform.position - posNoZ);

        interpVelocity = targetDirection.magnitude * 5f;

        targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

        transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);


    }

    void FixedUpdate()
    {
        
    }
}
