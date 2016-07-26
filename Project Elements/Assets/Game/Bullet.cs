using UnityEngine;
using System.Collections;

public enum Element
{
    Fire,
    Ice,
    Air
}

public class Bullet : MonoBehaviour {

    Rigidbody2D rb;
    public float amount;
    public Element element;
    //public Vector3 sp;
    //public Vector3 dir;

    public Transform Bulletspawn;
    public Vector2 shoot;
    //public GameObject ShootDirection;
    
    // Use this for initialization
    void Start() {

        //ShootDirection = GameObject.FindGameObjectWithTag("ShootDirection");

        Bulletspawn = GameObject.Find("BulletSpawn").transform;

        Vector2 delta = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Bulletspawn.transform.position;

        float direction = Mathf.Atan2(delta.y, delta.x) + 3.14159265f * 2.0f;

        shoot = new Vector2(Mathf.Cos(direction), Mathf.Sin(direction));

        rb = GetComponent<Rigidbody2D>();

        Destroy(gameObject, 5);
        // sp = Camera.main.WorldToScreenPoint(transform.position);
        //dir = (Input.mousePosition - sp).normalized;
        rb.velocity = shoot * 10;

        Vector3 mousepos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.rotation = Quaternion.LookRotation(Vector3.forward,Bulletspawn.position - mousepos);
    }

	
	// Update is called once per frame
	void Update () {
        //rb.velocity = rb.velocity = Vector2.up;      
        //rb.AddForce(shoot*amount);
       
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 8)
        {
            Destroy(gameObject);
        }
        else if(other.gameObject.layer == 13)
        {
            PlayerHealth.Playermana -= 4.0f;
            Destroy(gameObject);
        }
    }
}
