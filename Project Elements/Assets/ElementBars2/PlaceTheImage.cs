using UnityEngine;
using System.Collections;

public class PlaceTheImage : MonoBehaviour {
    /* Numpad2 muuntaa väriä, ja Numpad3 asettaa värin valkoiseksi */
    Color oldcolor;
    public Camera mainCamera;
    public Vector3 screenPosition = new Vector3(200, 200, 50);
    public Color AloitusColor = Color.yellow;
    public Color LopetusColor = Color.blue;
    public float kesto = 2.0F;
    public Renderer rend;
    // Use this for initialization
    void Start () {

        oldcolor = GetComponent<Renderer>().material.color;
	
	}
	
	// Update is called once per frame
	void Update () {
        float Lerptoiminto = Mathf.PingPong(Time.time, kesto) / kesto;
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            //pausemode = pause ();

            //arvo = 20;
            Debug.Log("no mo");
            GetComponent<Renderer>().material.color = oldcolor;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            //pausemode = pause ();

            //arvo = 20;
            Debug.Log("no mo2");


            GetComponent<Renderer>().material.color = Color.Lerp(AloitusColor, LopetusColor, Lerptoiminto);
        }
        transform.position = mainCamera.ScreenToWorldPoint(screenPosition);
    }
}
