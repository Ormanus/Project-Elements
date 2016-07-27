using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealt : MonoBehaviour {

    public float EnemyHealtti;
    Image HealthImage;
    public GameObject EnemyHitParticle;
    public GameObject ManaCrystal;
    public Element element;

    private float maxHealth;
        
    // Use this for initialization
    void Start () {
        maxHealth = EnemyHealtti;
        HealthImage = transform.FindChild("EnemyHealthCanvas").FindChild("Health").GetComponent<Image>();       
	}
	
	// Update is called once per frame
	void Update () {

        if (EnemyHealtti < 0)
        {
            EnemyHealtti = 0;
        }

        if (EnemyHealtti > 4)
        {
            EnemyHealtti = 4;

        }

        if (EnemyHealtti <= 0)
        {
            //Instantiate(EnemyHitParticle,transform.position,transform.rotation);
            int r = Random.Range(0, 4);
            for (int i = 0; i < r; i++)
            {
                GameObject o = Instantiate(ManaCrystal);
                o.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50, 50), Random.Range(-50, 50)));
                o.transform.position = transform.position;
            }
            Destroy(gameObject);
        }
        HealthImage.transform.localScale = new Vector3(EnemyHealtti / maxHealth, 1, 1);
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "bullet")
        {
            float damage = other.gameObject.GetComponent<Bullet>().damage;
            if (element != other.gameObject.GetComponent<Bullet>().element)
                damage *= 2;
            EnemyHealtti -= damage;
            Destroy(other.gameObject);
        }

    }

}
