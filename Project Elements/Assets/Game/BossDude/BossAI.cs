using UnityEngine;
using System.Collections;

public class BossAI : MonoBehaviour {

	public Transform RightHand;
	public Transform LeftHand;

	Vector2 RightHandStartPos;
	Vector2 LeftHandStartPos;

	float timer;
	float stateTimer;
	int state;

	Vector2 targetPoint;

	float radius = 5.0f;

	// Use this for initialization
	void Start () {
		state = 0;
		RightHandStartPos = new Vector2 (4.0f, -2.5f);
		LeftHandStartPos = new Vector2 (-4f, 4f);

		RightHand.parent = transform;
		LeftHand.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		stateTimer += Time.deltaTime;

		//transitions
		if (state == 0 && timer > 3.0f) {
			print ("State 1");
			state = 1;
			stateTimer = 0.0f;
		} else if (state == 1 && stateTimer > 2.0f) {
			print ("State 2");
			state = 2;
			stateTimer = 0;
		} else if (state == 2 && stateTimer > 0.5f) {
			print ("State 3");
			state = 3;
			stateTimer = 0;
		} else if (state == 3 && stateTimer > 2.0f) {
			print ("State reset");
			state = 0;
			timer = 0;
		}

		//actions

		Vector3 pos = transform.position;

		//TODO: move center of rotation lower
		Vector2 target = new Vector2(0, 0);
		Vector2 targetLeft = new Vector2(0, 0);
		float startAngle = Mathf.PI / 6;//0.0f;
		Vector2 rotationCenter = new Vector2(3.0f, -2.5f);

		switch (state) {
		case 0:
			target = RightHandStartPos;
			targetLeft = LeftHandStartPos;
			break;
		case 1:
			//move to the start position of the swing
			Vector2 moveStart = new Vector2 (Mathf.Cos (startAngle) * radius, Mathf.Sin (startAngle) * radius);
			target = Vector2.Lerp (RightHandStartPos, moveStart + rotationCenter, stateTimer / 2.0f);
			break;
		case 2:
			float angle = -(Mathf.PI / 2 + startAngle) * (stateTimer*2) + startAngle;
			target = new Vector2 (Mathf.Cos (angle) * radius, Mathf.Sin (angle) * radius) + rotationCenter;
			break;
		case 3:
			Vector2 moveEnd = new Vector2 (0, -radius ) + rotationCenter;
			target = Vector2.Lerp (moveEnd, RightHandStartPos, stateTimer / 2.0f);
			break;
		case 4:
			//move to the start position of the swing
			moveStart = new Vector2 (Mathf.Cos (startAngle) * radius, Mathf.Sin (startAngle) * radius);
			targetLeft = Vector2.Lerp (RightHandStartPos, moveStart + rotationCenter, stateTimer / 2.0f);
			break;
		case 5:
			angle = -(Mathf.PI / 2 + startAngle) * (stateTimer*2) + startAngle;
			targetLeft = new Vector2 (Mathf.Cos (angle) * radius, Mathf.Sin (angle) * radius) + rotationCenter;
			break;
		case 6:
			moveEnd = new Vector2 (0, -radius ) + rotationCenter;
			targetLeft = Vector2.Lerp (moveEnd, RightHandStartPos, stateTimer / 2.0f);
			break;
		}
			
		Vector2 targetDirection = (target - (Vector2)RightHand.localPosition);

		float interpVelocity = targetDirection.magnitude * 5.0f;

		targetPoint = RightHand.localPosition + (Vector3)(targetDirection.normalized * interpVelocity * Time.deltaTime);

		RightHand.localPosition = (Vector3)targetPoint;
	}
}
