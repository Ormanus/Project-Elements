using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {


    Rigidbody2D rb;

    Transform target;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        
    }
    
    //public float speed;
    //Vector3 move = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);
	// Update is called once per frame
	void Update () {

        target.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
           

        if (Input.GetAxisRaw("Horizontal") > 0.5f)
        {
            rb.velocity = target.position;           
        }
        if (Input.GetAxisRaw("Horizontal") < -0.5f)
        {
            rb.velocity = -Vector2.right;
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f)
        {
            rb.velocity = Vector2.up;
        }
        if (Input.GetAxisRaw("Vertical") < -0.5f)
        {
            rb.velocity = -Vector2.up;
        }


        //if (Input.GetKey(KeyCode.W))
        //{

        //    //transform.Translate(Vector2.up * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.A))
        //{
        //    transform.Translate(-Vector2.right * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.S))
        //{
        //    transform.Translate(-Vector2.up * Time.deltaTime);
        //}

        //if (Input.GetKey(KeyCode.D))
        //{
        //    transform.Translate(Vector2.right * Time.deltaTime);
        //}


        //Vector3 mousePos = Input.mousePosition;
        //mousePos.z = 5.23f;

        //Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
        //mousePos.x = mousePos.x - objectPos.x;
        //mousePos.y = mousePos.y - objectPos.y;

        //float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
