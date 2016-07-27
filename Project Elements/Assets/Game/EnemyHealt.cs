using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealt : MonoBehaviour {

    public float EnemyHealtti;
    Image HealthImage;
    public GameObject EnemyHitParticle;
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
            Instantiate(EnemyHitParticle,transform.position,transform.rotation);
            Inventory.money += 10;
            Destroy(gameObject);
        }
        HealthImage.transform.localScale = new Vector3(EnemyHealtti / maxHealth, 1, 1);
//		Debug.Log ("mikonjuttu" + EnemyHealtti.ToString("0.0"));
    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "bullet")
        {
            AILerp ai = GetComponent<AILerp>();
            if(ai)
            {
                ai.hasDetectedPlayer = true;
            }

            float damage = other.gameObject.GetComponent<Bullet>().damage;
            if (element != other.gameObject.GetComponent<Bullet>().element)
                damage *= 2;
            EnemyHealtti -= damage;
            Destroy(other.gameObject);
        }

    }

}
