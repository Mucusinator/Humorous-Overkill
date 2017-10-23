using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public float gunDamage = 10f;
    public float range = 100f;
    public Camera fpsCam;
   

	

	void Update () {
        //If the user presses the left mouse button, perform the shoot function.
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
	}


    void Shoot()
    {
        // A variable that will store the imformation gathered from the raycast.
        RaycastHit hit;

        // If we hit something with our shot raycast.
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) ;
        {
            Debug.Log(hit.transform.name);
        }
    }

}

