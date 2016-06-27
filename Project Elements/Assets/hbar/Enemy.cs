using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {



    public float TimeBetweenMove;
    private float TimeBetweenMoveCounter;
    public float TimeToMove;
    private float TimeToMoveCounter;


    public float movespeed;

    private Rigidbody2D rb;

    public bool Moving;

    private Vector3 direction;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();

        TimeBetweenMoveCounter = TimeBetweenMove;
        TimeToMoveCounter = TimeToMove;
	}
	
	// Update is called once per frame
	void Update () {

        if (Moving)
        {
            TimeToMoveCounter -= Time.deltaTime;
            rb.velocity = direction;

            if (TimeToMoveCounter < 0)
            {
                Moving = false;

            }

        }
        else
        {
            TimeBetweenMoveCounter -= Time.deltaTime;
            rb.velocity = Vector2.zero;
            if (TimeBetweenMoveCounter < 0)
            {
                
                TimeToMoveCounter = TimeToMove;

                direction = new Vector3(Random.Range(-1,1), Random.Range(-1,1),0);
                Moving = true;


            }

        }
	
	}
}
