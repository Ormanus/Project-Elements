using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float firerate = 0;
    public float damage = 10;
    public LayerMask notToHit;

    float timeToFire = 0;

    Transform Firepoint;

    public GameObject bullett;

    void Awake()
    {
        Firepoint = transform.FindChild("FirePoint");
        if (Firepoint = null)
        {
            Debug.LogError("Set FirePoint!");

        }
    }


	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.DrawLine(Firepoint.position, Input.mousePosition, Color.red);

        if (firerate == 0)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                //Shoot();

              
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time > timeToFire)
            {
                timeToFire = Time.time + 1 / firerate;
                Shoot();

            }
        }
    }


    void Shoot()
    {
        Vector2 mousePosition = new Vector2(Camera.main.ScreenToViewportPoint (Input.mousePosition).x, Camera.main.ScreenToViewportPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(Firepoint.position.x,Firepoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition,mousePosition-firePointPosition,100);

        Debug.DrawLine(firePointPosition,mousePosition,Color.red);


        
    }
}
