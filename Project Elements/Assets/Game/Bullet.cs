using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    Rigidbody2D rb;
    public float amount;
    //public Vector3 sp;
    //public Vector3 dir;

    public Transform Bulletspawn;
    public Vector2 shoot;
    //public GameObject ShootDirection;
    
    // Use this for initialization
    void Start() {

        //ShootDirection = GameObject.FindGameObjectWithTag("ShootDirection");

        Bulletspawn = GameObject.Find("BulletSpawn").transform;

        shoot = Bulletspawn.transform.up;

        rb = GetComponent<Rigidbody2D>();


        // sp = Camera.main.WorldToScreenPoint(transform.position);
        //dir = (Input.mousePosition - sp).normalized;
}

	
	// Update is called once per frame
	void Update () {
        //rb.velocity = rb.velocity = Vector2.up;      
        rb.AddForce(shoot*amount);
        Destroy(gameObject,5);
    }
}
