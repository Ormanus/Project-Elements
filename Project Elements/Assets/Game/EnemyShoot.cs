using UnityEngine;
using System.Collections;

public class EnemyShoot : MonoBehaviour {

    public float RoF;
    public GameObject bullet;
    private float timer;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer > RoF)
        {
            timer -= RoF;
            Instantiate(bullet, gameObject.transform.position, gameObject.transform.rotation);
        }
	}
}
