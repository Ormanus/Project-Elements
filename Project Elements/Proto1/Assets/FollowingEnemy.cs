using UnityEngine;
using System.Collections;

public class FollowingEnemy : MonoBehaviour {

    //   GameObject pelaaja;
    //   public float speed;
    //// Use this for initialization
    //void Start () {
    //       pelaaja = GameObject.FindGameObjectWithTag("pelaaja");
    //}

    //// Update is called once per frame
    //void Update () {
    //       transform.position = Vector2.MoveTowards(transform.position, pelaaja.transform.position, speed);
    //   }
    public GameObject target;
    public float speed = 2f;
    private float minDistance = 1f;
    private float range;
    public static bool follow = false;
    int health = 10;
    public static int DamageToGiveEnemy = 1;
    public GameObject partikkeli;
    public GameObject partikkeli2;
    public GameObject EnemyBullet;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("pelaaja");
        InvokeRepeating("ShootBullet", 1f, 5f);
    }
    void Update()
    {
        //range = Vector2.Distance(transform.position, target.position);

        //if (range > minDistance)
        //{
        //if (follow) {
            //Debug.Log(target.transform.position);

            //transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.LookRotation(Vector3.forward, target.transform.position - transform.position);
        //}


        if (health <= 0)
        {
            Instantiate(partikkeli2,transform.position,transform.rotation);
            Destroy(gameObject);
            Points.points++;
        }
        //}
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "bullet")
        {
            Destroy(other.gameObject);
            Instantiate(partikkeli,transform.position,transform.rotation);
            health -= DamageToGiveEnemy;
        }

    }

    void ShootBullet()
    {
        Instantiate(EnemyBullet,transform.position,transform.rotation);

    }
}
