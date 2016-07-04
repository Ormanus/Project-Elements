using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour {


    public GameObject enemy;
    public Transform spawnpoint;
    public Transform spawnpoint2;
    public Transform spawnpoint3;
    public float aika = 20;
    public float seuraavaAika = 10;
    Text aikatext;
    public Text aika2;
    public Text leveltext;
    public float Spawnaika = 5;
    public int leveli = -1;

    public bool onetime = false;
    // Use this for initialization
    void Start() {
        aikatext = GetComponent<Text>();
        spawnenemy2();
    }
	
	// Update is called once per frame
	void Update () {
        aikatext.text = "Next Break: " + aika.ToString("0");
        aika2.text = "Next Level: " + seuraavaAika.ToString("0");
        leveltext.text = "Level: " + leveli.ToString();
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
                aika = 20;
                seuraavaAika = 10;
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
        if (Spawnaika > 0.5f)
        {
            Spawnaika -= 0.25f;
            leveli++;
        }
        
        InvokeRepeating("SpawnEnemy", 1f, Spawnaika);
    }

    void SpawnEnemy()
    {
        int randomspawnpoint;

        randomspawnpoint = Random.Range(1,4);

        switch (randomspawnpoint)
        {
            case 1:
                Instantiate(enemy, spawnpoint.position, spawnpoint.rotation);
                break;

            case 2:
                Instantiate(enemy, spawnpoint2.position, spawnpoint2.rotation);

                break;

            case 3:
                Instantiate(enemy, spawnpoint3.position, spawnpoint3.rotation);
                break;

        }
        
        
    }
}
