using UnityEngine;
using System.Collections;

public class VaihdaAse : MonoBehaviour {


    public GameObject Weapon1;
    public GameObject Weapon2;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeWeapon();

        }
	}

    void ChangeWeapon()
    {
        if (Weapon1.active == true)
        {

            Weapon1.SetActiveRecursively(false);
            Weapon2.SetActiveRecursively(true);
            FollowingEnemy.DamageToGiveEnemy = 1;
            

        }
        else
        {
            Weapon1.SetActiveRecursively(true);
            Weapon2.SetActiveRecursively(false);
            FollowingEnemy.DamageToGiveEnemy = 5;

        }

    }
}
