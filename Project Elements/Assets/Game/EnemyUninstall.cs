using UnityEngine;
using System.Collections;

public class EnemyUninstall : MonoBehaviour { //when enemy leaves the show

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		//if (EnemyHealt.EnemyHealtti == 0)
			//Debug.Log ("0 enemyhealtti");
	}
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "bullet")
		{
//		Debug.Log ("mikonjuttu" + EnemyHealt.EnemyHealtti);
		}

	}
}
