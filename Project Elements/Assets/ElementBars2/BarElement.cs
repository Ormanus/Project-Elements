using UnityEngine;
using System.Collections;

public class BarElement : MonoBehaviour {
	public Camera mainCamera;
	public Vector3 screenPosition = new Vector3(200,200,50);
	public GameObject changeColourText;
	float aikaaon = 0f;
    public Texture tekstuuri;
    public GameObject uusElementtibar;
    public GameObject use;
    bool kerran = false;
    public GameObject[] foundobjects;
     public int lkm;

	// Use this for initialization
	void Start () {
		//transform.position = GetComponent<Camera> ().ScreenToWorldPoint (screenPosition);
        

            //aikaajaljella = 50.0F;

            //if(aikaajaljella < 0)
            //{
            //	wöidötaieiainakaiehkäjöömjöö();
            //}


            //if(GameObject.Find("BarElemente") == null)

            //Destroy (gameObject, 1);


            //GetComponent<Renderer> ().material.color = Color.blue;
            //GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/wepon0") as Texture;

            //use = Instantiate(GameObject.Find("BarElemente"));
        
        
	}


	// Update is called once per frame
	void Update () {
       // kerran = false;
		if (Input.GetKeyDown(KeyCode.Keypad8)) {
			GetComponent<Renderer> ().material.color = Color.black;
		}
		aikaaon += Time.deltaTime / 2;
		if (Input.GetKeyDown(KeyCode.PageUp)) {
			
			//aikaajaljella = 50.0F;

			//if(aikaajaljella < 0)
			//{
			//	wöidötaieiainakaiehkäjöömjöö();
			//}
            

        //if(GameObject.Find("BarElemente") == null)
        
        //Destroy (gameObject, 1);

            
			//GetComponent<Renderer> ().material.color = Color.blue;
            //GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/wepon0") as Texture;


        }
        //tässä yritetään timeria
		//Debug.Log (aikaaon);
        if (aikaaon == 1 || aikaaon > 1)
        {

            Invoke("deleteclones", 0);
            if (!kerran)
            {
                
                //Invoke("createanobject", 0);
                kerran = true;
                
                //aikaaon = 0f;
            }
            aikaaon = 0f;
            
        }
        

		GetComponent<Renderer> ().material.SetFloat ("_Cutoff", aikaaon);//Mathf.InverseLerp(0, Screen.width, (int)aikaaon)); 
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            GetComponent<Renderer>().material.color = Color.black;
        }
		//Debug.Log(Input.mousePosition.x);
		//GetComponent<Renderer> ().material.color = Color.blue; 
		transform.position = mainCamera.ScreenToWorldPoint (screenPosition);


	}
    void createanobject()
    {
        Instantiate(GameObject.Find("BarElemente"));
    }
    void deleteclones()
    {
        //GameObject[] CloneObjects = GameObject.FindGameObjectsWithTag("finderofclones");
        //foreach (object go in allObjects)

        //System.Collections.Generic.List<GameObject> list = new System.Collections.Generic.List<GameObject>(foundobjects);
        //list.Remove(GameObject.Find("BarElemente(Clone)"));
        //foundobjects = list.ToArray();

        foreach (GameObject object2 in GameObject.FindGameObjectsWithTag("finderofclones"))
        {
            //    lkm = lkm + 1;
            //}

            //foreach (GameObject object2 in GameObject.FindGameObjectsWithTag("finderofclones"))
            //{



            Debug.Log("objektin nimi:" + object2.name);
            if (object2.name == "BarElemente(Clone)") { DestroyObject(object2); }
            //lkm = lkm + 1;

            //}
        }
    }
    void FindObjectsWithTag()
    {
        var objects = GameObject.FindGameObjectsWithTag("finderofclones");
        foundobjects = new GameObject[objects.Length];
        foundobjects = objects;
    }


}
