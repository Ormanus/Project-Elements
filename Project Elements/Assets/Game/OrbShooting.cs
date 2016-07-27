using UnityEngine;
using System.Collections;

public class OrbShooting : MonoBehaviour
{

    public float RoF;
    public GameObject bullet;
    public Element element;
    private float timer;
    // Use this for initialization
    void Start()
    {
        timer = Random.Range(0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > RoF)
        {
            timer -= RoF;
            GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
            if(targets.Length > 0)
            {
                Transform target = targets[0].transform;
                Vector2 delta = targets[0].transform.position - transform.position;
                float distance = delta.SqrMagnitude();
                for (int i = 1; i < targets.Length; i++)
                {
                    delta = targets[i].transform.position - transform.position;
                    float d = delta.SqrMagnitude();
                    if (d < distance)
                    {
                        distance = d;
                        target = targets[i].transform;
                    }
                }

                distance = Mathf.Sqrt(distance);

                Vector2 direction = target.transform.position - gameObject.transform.position;
                if (distance < 10)
                {
                    if (Physics2D.Raycast(gameObject.transform.position, direction, distance, 256).collider == null)
                    {
                        GameObject o = Instantiate(bullet);
                        o.transform.position = gameObject.transform.position;
                        o.transform.eulerAngles = new Vector3(0, 0, 180 * Mathf.Atan2(direction.y, direction.x) / Mathf.PI);
                        Bullet b = o.GetComponent<Bullet>();
                        b.element = element;
                        b.damage = 0.25f;
                        b.direction = Mathf.Atan2(direction.y, direction.x);
                    }
                }
            }
        }
    }
}
