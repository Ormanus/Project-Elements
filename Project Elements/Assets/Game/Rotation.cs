using UnityEngine;
using System.Collections;

public class Rotation : MonoBehaviour {

    public Transform target;
    public float distance;
    public float speed;

    private float totalTime;
    private Rigidbody2D rb;
	// Use this for initialization
	void Start () {
        target = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        transform.position = target.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        totalTime += Time.deltaTime * speed;
        Vector2 targetPoint = target.position + new Vector3(Mathf.Cos(totalTime) * distance, Mathf.Sin(totalTime) * distance);

        Vector2 delta = targetPoint - (Vector2)transform.position;
        rb.AddForce(delta.normalized * 50.0f * delta.magnitude);
    }
}
