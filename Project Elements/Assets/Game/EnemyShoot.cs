using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {

    public float RoF;
    public GameObject bullet;
    public Element element;
    private float timer;
	// Use this for initialization
	void Start () {
        timer = Random.Range(0.0f, 1.0f);
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > RoF)
        {
            timer -= RoF;
            Transform target = GameObject.Find("Player").transform;

            Vector2 direction = target.transform.position - gameObject.transform.position;
            Debug.DrawRay(gameObject.transform.position, direction, new Color(1.0f, 0.0f, 0.0f), 1.0f);
            if(direction.magnitude < 10)
            {
                if (Physics2D.Raycast(gameObject.transform.position, direction, direction.magnitude, 256).collider == null)
                {
                    GameObject o = (GameObject)Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
                    o.GetComponent<EnemyBullet>().element = element;
                }
            }
        }
	}
}
