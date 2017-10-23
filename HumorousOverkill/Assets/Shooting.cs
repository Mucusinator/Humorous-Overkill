using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public float gunDamage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem flash;
    public float impactForce = 30f;
    


    public float ShotsPerMinute = 100f;

    private float nextTimeToFire = 0f;
    public FireRate FireRateSelection;


    public enum FireRate {
        SEMIAUTO,
        FULLAUTO

    }


    [SerializeField]
    private float ShotsPerSecond;
    


	void Update () {

        ShotsPerSecond = ShotsPerMinute / 60f;
        //If the user presses the left mouse button, perform the shoot function.

        if (FireRateSelection == FireRate.FULLAUTO)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
            {

                nextTimeToFire = Time.time + 60f / ShotsPerMinute;
                Shoot();

            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
            {

                nextTimeToFire = Time.time + 60f / ShotsPerMinute;
                Shoot();

            }
        }
	}


    void Shoot()
    {
        flash.Play();
        // A variable that will store the imformation gathered from the raycast.
        RaycastHit hit;

        // If we hit something with our shot raycast.
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) ;
        {

            // Put in place the takeDamage event handler for the game manager here.
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(GameEvent.)

            Debug.Log(hit.transform.name);
            Target target = hit.transform.GetComponent<Target>();

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }

            if (target != null)
            {
                target.TakeDamage(gunDamage);
            }

        }
    }

}

