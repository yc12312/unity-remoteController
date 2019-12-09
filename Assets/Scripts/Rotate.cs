using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
	
    //change accord to weapon

	// Update is called once per frame
	void Update () {
        if(ChangeWeapon.index == 2)
            transform.Rotate(50 * Time.deltaTime, 0, 0);
        else
            transform.Rotate(0, 0, 50 * Time.deltaTime);
    }
}
