using UnityEngine;
using System.Collections;

public class ManaCrystal : MonoBehaviour {

    private Transform player;
    private Rigidbody2D rb;

	void Start () {
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        Vector2 delta = player.position - transform.position;
        float distance = delta.magnitude;
        if (distance < 3)
        {
            rb.AddForce(delta.normalized * 20000 / (distance * 10 + 0.1f) * Time.deltaTime);
            if(distance < 0.25)
            {
                Inventory.money += 10;
                DestroyObject(gameObject);
            }
        }
	}
}
