     using UnityEngine;
     using System.Collections;
     
     public class GameTestSceneMouseMoving : MonoBehaviour 
     {
	
		public float speed2 = 3f;

		private Vector3 targetti;

		void Start () {
			targetti = transform.position;
		}

		void Update () {
		
			if (Input.GetMouseButtonDown(0)) {
			
				targetti = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}
			transform.position = Vector3.MoveTowards(transform.position, targetti, speed2 * Time.deltaTime);
		}    
	}
     