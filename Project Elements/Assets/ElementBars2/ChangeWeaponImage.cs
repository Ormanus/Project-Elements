using UnityEngine;
using System.Collections;

public class ChangeWeaponImage : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/wepon0") as Texture;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Insert))
        {

            //aikaajaljella = 50.0F;

            //if(aikaajaljella < 0)
            //{
            //	wöidötaieiainakaiehkäjöömjöö();
            //}


            //GetComponent<Renderer>().material.color = Color.blue;
            GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/wepon0") as Texture;


        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {

            //aikaajaljella = 50.0F;

            //if(aikaajaljella < 0)
            //{
            //	wöidötaieiainakaiehkäjöömjöö();
            //}


            //GetComponent<Renderer>().material.color = Color.yellow;
            GetComponent<Renderer>().material.mainTexture = Resources.Load("textures/wepon2") as Texture;


        }
    }
}
