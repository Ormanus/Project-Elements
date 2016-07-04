using UnityEngine;
using System.Collections;

public class Heal : MonoBehaviour {

    public GameObject healparticle;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "pelaaja")
        {
            Healthtest.Playerhealth += 0.25f;
            Instantiate(healparticle,transform.position,transform.rotation);
            Destroy(gameObject);
        }
    }
}
