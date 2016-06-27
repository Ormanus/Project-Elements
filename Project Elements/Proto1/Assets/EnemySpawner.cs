using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {


    public GameObject enemy;
    public Transform spawnpoint;
    public float aika = 30;
    public float seuraavaAika = 20;
    Text aikatext;
    public Text aika2;

    public bool onetime = false;
    // Use this for initialization
    void Start() {
        aikatext = GetComponent<Text>();
        spawnenemy2();
    }
	
	// Update is called once per frame
	void Update () {
        aikatext.text = "Next Break: " + aika.ToString();
        aika2.text = "Next Level: " + seuraavaAika;
        if (aika > 0) {
            aika -= Time.deltaTime;
        }
        

        if (aika <= 0)
        {
            aika = 0;
            CancelInvoke();
            if (seuraavaAika > 0) {
                seuraavaAika -= Time.deltaTime;
            }

            if (seuraavaAika <= 0)
            {
                aika = 30;
                seuraavaAika = 20;
                onetime = true;

            }
        }

        if (onetime)
        {
            spawnenemy2();
            onetime = false;
            
        }
            

    }


    void spawnenemy2()
    {
            InvokeRepeating("SpawnEnemy", 1f, 3f);
    }

    void SpawnEnemy()
    {
        Instantiate(enemy,spawnpoint.position,spawnpoint.rotation);
    }
}
