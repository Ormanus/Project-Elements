using UnityEngine;
using System.Collections;

public class BossShoot : MonoBehaviour {
	
	private Transform target;
	private float speed = 10f;
	private Vector3 speedRot = Vector3.right * 50f;
	public float delta = 5.5f;  // Amount to move left and right from the start point
	private Vector3 startPos;
	int distance;
	public float RoF;
	public GameObject bullet;
	public Element element;
	//private float timer;
	// Use this for initialization

	
	void Start () {
		target = GameObject.FindGameObjectWithTag("Player").transform;
		Debug.Log (target.name);
		startPos = transform.position;
	}

	void Update ()
	{
		//if (GameObject.Find ("BarTop2") != null) {
			//transform.Rotate (speedRot * Time.deltaTime);
		//transform.position = Vector3.MoveTowards (transform.position, target.position, speed * Time.deltaTime);
		  // distance = 50;
		transform.position = (transform.position - target.transform.position).normalized;// * 50f + target.transform.position;

		GameObject o = (GameObject)Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);

			Vector3 v = startPos;
			v.x += delta * Mathf.Sin (Time.time * speed);
			//transform.position = v;
//		} else {
//			Destroy (this.gameObject, 10);
//		}
//
//		if (Input.GetKeyDown(KeyCode.F)){
//			Application.LoadLevel (0);
//		}



	}
		// Update is called once per frame
	// void Update () {
//		timer += Time.deltaTime;
//		if(timer > RoF)
//		{
//			timer -= RoF;
			// Transform target = GameObject.Find("Player").transform;

			//Vector2 direction = target.transform.position - gameObject.transform.position;
			//Debug.DrawRay(gameObject.transform.position, direction, new Color(1.0f, 0.0f, 0.0f), 1.0f);
			//if(direction.magnitude < 10)
			//{
				//if (Physics2D.Raycast(gameObject.transform.position, direction, direction.magnitude, 256).collider == null)
				//{
					//GameObject o = (GameObject)Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
					//o.GetComponent<EnemyBullet>().element = element;
				//}
			//}
//		}
	// }
}

