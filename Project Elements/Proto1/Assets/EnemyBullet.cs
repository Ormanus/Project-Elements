using UnityEngine;
using System.Collections;

public class EnemyBullet : MonoBehaviour {

    Rigidbody2D rb;
    public GameObject pelaaja;
    public float amount;
    public Vector3 sp;
    public Vector3 dir;

    // Use this for initialization
    void Start()
    {
        pelaaja = GameObject.FindGameObjectWithTag("pelaaja");
        rb = GetComponent<Rigidbody2D>();

        sp = transform.position;
        dir = (pelaaja.transform.position - sp).normalized;
    }


    // Update is called once per frame
    void Update()
    {
        //rb.velocity = rb.velocity = Vector2.up;
        rb.AddForce(dir * amount);
        Destroy(gameObject, 5);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "building")
        {
            Destroy(gameObject);

        }

    }
}
