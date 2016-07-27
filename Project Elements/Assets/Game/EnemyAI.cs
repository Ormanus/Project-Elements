using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {

    public bool isAwake;

    private Vector2 target;
    private Transform player;
    private float timer;
    private Vector2 velocity;

	void Start () {
        target = transform.position;
        timer = Random.Range(0.0f, 1.0f);
        isAwake = false;
    }
	
    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

	void FixedUpdate () {
        timer += Time.deltaTime;
        if (timer > 1)
        {
            timer -= 1;
            Transform player = GameObject.Find("Player").transform;

            Vector2 direction = player.transform.position - transform.position;
            Debug.DrawRay(transform.position, direction, new Color(0.0f, 1.0f, 0.0f), 1.0f);
            if (direction.magnitude < 20)
            {
                if (Physics2D.Raycast(gameObject.transform.position, direction, direction.magnitude, 256).collider == null)
                {
                    isAwake = true;
                    target = player.position;
                }
            }
        }
        if(isAwake)
        {
            Vector2 delta = target - (Vector2)transform.position;
            if (delta.magnitude > 1)
            {
                velocity = delta.normalized;
            }
            else
            {
                isAwake = false;
                velocity = new Vector2(0, 0);
            }
        }
    }
}
