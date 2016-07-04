using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	Vector3 mousePos;
	public Transform BulletSpawn; //the object you want to rotate
    public GameObject BulletPrefab;
	Vector3 objectPos;
	float angle;
   
    public float speed;
    public GameObject playerdeathparticle;
    public GameObject playerhitParticle;
    //public static int PelaajanHealth = 10;

    Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		//rotation
		//Debug.Log("i" + Input.mousePosition);

		/*
		 * 
		 * Vector3 mousePos = Input.mousePosition;
		//mousePos.z = 5.23f;
		//mousePos=Camera.main.ScreenToWorldPoint(mousePos);
		Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
		//transform.position = Vector2.Lerp(transform.position, mousePos, 0.1f);
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;

		float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle));
*/

		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		transform.rotation = Quaternion.LookRotation(Vector3.forward, mousePos - transform.position);
             

        rb.velocity = Vector2.zero;

        //if (Input.GetAxisRaw("Horizontal") > 0.5f)
        //{
        //    rb.velocity = Vector2.right;
        //}
        //if (Input.GetAxisRaw("Horizontal") < -0.5f)
        //{
        //    rb.velocity = -Vector2.right;
        //}
        //if (Input.GetAxisRaw("Vertical") > 0.5f)
        //{           
        //    transform.position = Vector2.MoveTowards(transform.position, paikka, speed);
        //    //rb.velocity = Vector2.up;
        //}
        //if (Input.GetAxisRaw("Vertical") < -0.5f)
        //{
        //    //transform.position = Vector2.MoveTowards(transform.position, paikka, speed);
        //   rb.velocity = -Vector2.up;
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Instantiate(objekti,target.position,target.rotation);

        //}

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            //rb.velocity = Vector2.right;
            transform.Translate(Vector2.right * Time.deltaTime*speed);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //rb.velocity = -Vector2.right;
            transform.Translate(-Vector2.right * Time.deltaTime*speed);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            //transform.position = Vector2.MoveTowards(transform.position, paikka, speed);
            transform.Translate(Vector2.up * Time.deltaTime*speed);

            //rb.velocity = Vector2.up;
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.position = Vector2.MoveTowards(transform.position, paikka, speed);
            //rb.velocity = -Vector2.up;
            transform.Translate(-Vector2.up * Time.deltaTime*speed);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(BulletPrefab, BulletSpawn.position, BulletSpawn.rotation);

        }

        if (PlayerHealth.Playerhealth <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            Instantiate(playerdeathparticle, transform.position, transform.rotation);
            Time.timeScale = 0;


        }


    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "enemy") {
            PlayerHealth.Playerhealth -= 0.25f * Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemyBullet")
        {
            PlayerHealth.Playerhealth -= 0.25f;
            Instantiate(playerhitParticle,transform.position,transform.rotation);
            Destroy(other.gameObject);

        }

    }
}