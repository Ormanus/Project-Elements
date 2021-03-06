﻿using UnityEngine;
using System.Collections;

public class RandomMovingEnemy : MonoBehaviour {

    public float movespeed;

    Rigidbody2D rb;

    public bool iswalking;

    //public float walktime;
    private float walkcounter;

    //public float waittime;
    private float waitcounter;

    private int walkdirection;

    public Collider2D MoveArea;

    private Vector2 minwalk;
    private Vector2 maxwalk;

    private bool inwalkarea;

    private Animator anim;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();

        waitcounter = Random.Range(1,4);
        walkcounter = Random.Range(1,4);

        //if (MoveArea != null) {
            minwalk = MoveArea.bounds.min;
            maxwalk = MoveArea.bounds.max;
            inwalkarea = true;
        //}


        anim = GetComponent<Animator>();

    }
	
	// Update is called once per frame
	void Update () {


        anim.SetFloat("VelocityY", rb.velocity.y);
        anim.SetFloat("VelocityX", rb.velocity.x);


        if (iswalking)
        {
            walkcounter -= Time.deltaTime;
            

            switch (walkdirection)
            {
                case 0:
                    rb.velocity = new Vector2(0,movespeed);
                    if (inwalkarea && transform.position.y > maxwalk.y)
                    {
                        iswalking = false;
                        rb.velocity = Vector2.zero;
                        waitcounter = Random.Range(1, 4);
                    }

                    break;
                case 1:
                    rb.velocity = new Vector2(movespeed, 0);
                    if (inwalkarea && transform.position.x > maxwalk.x)
                    {
                        iswalking = false;
                        rb.velocity = Vector2.zero;
                        waitcounter = Random.Range(1, 4);
                    }

                    break;
                case 2:
                    rb.velocity = new Vector2(0, -movespeed);
                    if (inwalkarea && transform.position.y < minwalk.y)
                    {
                        iswalking = false;
                        rb.velocity = Vector2.zero;
                        waitcounter = Random.Range(1, 4);
                    }
                    break;
                case 3:
                    rb.velocity = new Vector2(-movespeed, 0);
                    if (inwalkarea && transform.position.x < minwalk.x)
                    {
                        iswalking = false;
                        rb.velocity = Vector2.zero;
                        waitcounter = Random.Range(1, 4);
                    }
                    transform.Rotate(0, 0, 0);
                    break;             
            }

            if (walkcounter < 0)
            {
                rb.velocity = Vector2.zero;         
                waitcounter = Random.Range(1,4);
                iswalking = false;
            }
        }
        else
        {
            waitcounter -= Time.deltaTime;
            if (waitcounter < 0)
            {
                ValitseSuunta();

            }

        }

	}

    public void ValitseSuunta()
    {
        walkdirection = Random.Range(0,4);
        iswalking = true;
        walkcounter = Random.Range(1,4);

    }


    //void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Player")
    //    {
    //        iswalking = false;
    //        rb.velocity = Vector2.zero;
    //        waitcounter = Random.Range(1, 4);
    //    }

    //}

    //void OnTriggerExit2D(Collider2D other)
    //{
    //    if (other.gameObject.tag == "movearea")
    //    {
    //        iswalking = false;
    //        rb.velocity = Vector2.zero;
    //        waitcounter = Random.Range(1, 4);
    //    }

    //}
}
