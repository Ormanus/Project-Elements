using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class m2 : MonoBehaviour {
	Vector3 mousePos;
	public Transform target; //the object you want to rotate
    public GameObject objekti;
	Vector3 objectPos;
	float angle;
    Vector2 paikka;
    public float speed;
    public GameObject playerdeathparticle;
    //public static int PelaajanHealth = 10;


    //Damage Image Variables-----

    public Image damageImage;





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

        paikka = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Quaternion q = Quaternion.FromToRotation(target.position, paikka);

        rb.velocity = Vector2.zero;

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            //rb.velocity = Vector2.right;
            transform.Translate(Vector2.right * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            //rb.velocity = -Vector2.right;
            transform.Translate(-Vector2.right * Time.deltaTime);
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            //transform.position = Vector2.MoveTowards(transform.position, paikka, speed);
           transform.Translate(Vector2.up * Time.deltaTime);

            //rb.velocity = Vector2.up;
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            //transform.position = Vector2.MoveTowards(transform.position, paikka, speed);
            //rb.velocity = -Vector2.up;
            transform.Translate(-Vector2.up * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(objekti,target.position,target.rotation);

        }

        if (Healthtest.Playerhealth <= 0)
        {
            //Destroy(gameObject);
            gameObject.SetActive(false);
            Instantiate(playerdeathparticle,transform.position,transform.rotation);
            

        }

        

        
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "enemy") {
            Healthtest.Playerhealth -= 0.25f * Time.deltaTime;
        }
    }
}