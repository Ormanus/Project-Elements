using UnityEngine;
using System.Collections;

public class BossMove : MonoBehaviour {

    public float TimeBetweenMove = 5;
    public float TimeToMove = 5;
    public bool moving = false;
    Vector3 MoveDirection;
    float DistanceBetweenHomePoint;
    public Transform HomePoint;
    public float speed = 3;


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        
        DistanceBetweenHomePoint = Vector3.Distance(transform.position,HomePoint.position);

        Debug.Log(DistanceBetweenHomePoint);

        if (DistanceBetweenHomePoint > 4f)
        {
            transform.position = Vector2.MoveTowards(transform.position, HomePoint.transform.position, speed * Time.deltaTime);
            TimeBetweenMove = 5;
            TimeToMove = 5;

        }
        else {

            if (moving)
            {
                TimeToMove -= Time.deltaTime;
                GetComponent<Rigidbody2D>().velocity = MoveDirection;

                if (TimeToMove <= 0)
                {
                    moving = false;

                    TimeToMove = 5;

                }


            }
            else
            {
                TimeBetweenMove -= Time.deltaTime;

                if (TimeBetweenMove <= 0)
                {
                    moving = true;
                    TimeBetweenMove = 5;

                    MoveDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);

                }
            }
        }
	}
}
