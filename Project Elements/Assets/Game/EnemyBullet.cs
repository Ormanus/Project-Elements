﻿using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    Rigidbody2D rb;
    public Transform target;
    public float speed;
    public Element element;
    public float damage = 1.0f;

    // Use this for initialization
    void Start() {

        //ShootDirection = GameObject.FindGameObjectWithTag("ShootDirection");
        target = GameObject.Find("Player").transform;

        Vector2 delta = target.transform.position - gameObject.transform.position;

        Vector2 velocity = delta.normalized * speed;

        rb = GetComponent<Rigidbody2D>();

        rb.velocity = velocity;

        Destroy(gameObject, 5);
        // sp = Camera.main.WorldToScreenPoint(transform.position);
        //dir = (Input.mousePosition - sp).normalized;
    }

	
	// Update is called once per frame
	void Update () {
        //rb.velocity = rb.velocity = Vector2.up;      
        //rb.velocity = 
       
    }


    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == 13)
        {
            PlayerHealth.Playermana -= 4.0f;
            Destroy(gameObject);
        }
    }
}
