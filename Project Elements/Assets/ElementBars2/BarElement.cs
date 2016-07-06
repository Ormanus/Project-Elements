using UnityEngine;
using System.Collections;

public class BarElement : MonoBehaviour {
	public Camera mainCamera;
	public Vector3 screenPosition = new Vector3(200,200,50);
	public GameObject changeColourText;
	float aikaaon = 0f;
    public Texture tekstuuri;

	// Use this for initialization
	void Start () {
		//transform.position = GetComponent<Camera> ().ScreenToWorldPoint (screenPosition);
	}


	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Keypad8)) {
			GetComponent<Renderer> ().material.color = Color.black;
		}
		aikaaon += Time.deltaTime / 29;
		if (Input.GetKeyDown(KeyCode.PageUp)) {
			
			//aikaajaljella = 50.0F;

			//if(aikaajaljella < 0)
			//{
			//	wöidötaieiainakaiehkäjöömjöö();
			//}

            
			//GetComponent<Renderer> ().material.color = Color.blue;
            //GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/wepon0") as Texture;


        }
		Debug.Log ((int)aikaaon);
		GetComponent<Renderer> ().material.SetFloat ("_Cutoff", aikaaon);//Mathf.InverseLerp(0, Screen.width, (int)aikaaon)); 

		//Debug.Log(Input.mousePosition.x);
		//GetComponent<Renderer> ().material.color = Color.blue; 
		transform.position = mainCamera.ScreenToWorldPoint (screenPosition);


	}


}
