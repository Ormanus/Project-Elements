using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemyHealt : MonoBehaviour {

    public float EnemyHealtti = 1;
     Image HealthImage;
    public GameObject EnemyHitParticle;
        
    // Use this for initialization
    void Start () {
        HealthImage = transform.FindChild("EnemyHealthCanvas").FindChild("Health").GetComponent<Image>();       
	}
	
	// Update is called once per frame
	void Update () {

        if (EnemyHealtti < 0)
        {
            EnemyHealtti = 0;
        }

        if (EnemyHealtti > 1)
        {
            EnemyHealtti = 1;

        }

        if (EnemyHealtti <= 0)
        {
            Instantiate(EnemyHitParticle,transform.position,transform.rotation);
            Destroy(gameObject);
            if(GameObject.FindGameObjectsWithTag("Enemy").Length == 10)
            {
                SceneManager.LoadScene("EndScreenScene");
            }
        }

        HealthImage.transform.localScale = new Vector3(EnemyHealtti, 1, 1);

    }


    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "bullet")
        {
            EnemyHealtti -= 0.25f;
            Destroy(other.gameObject);
        }

    }

}
