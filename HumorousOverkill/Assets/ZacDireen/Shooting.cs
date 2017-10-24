using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour {

    public float gunDamage = 10f;
    public float range = 100f;
    public Camera fpsCam;
    public ParticleSystem flash;
    public float impactForce = 30f;

    public Animator animator;

    public float ShotsPerMinute = 100f;


    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 2.5f;


    private bool isReloading;

    private float nextTimeToFire = 0f;
    public FireRate FireRateSelection;


    public enum FireRate {
        SEMIAUTO,
        FULLAUTO

    }

    public enum GunType
    {
        SHOTGUN,
        RIFLE

    }


    [SerializeField]
    private float ShotsPerSecond;


    void Start()
    {
        currentAmmo = maxAmmo;
    }


	void Update () {


        if (isReloading)
        {
            return;
        }

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }



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



    IEnumerator Reload()
    {

        isReloading = true;
        animator.SetBool("Reloading", true);
        yield return new WaitForSeconds(reloadTime - 0.25f);
        animator.SetBool("Reloading", false);
        yield return new WaitForSeconds(0.25f);
        currentAmmo = maxAmmo;
        isReloading = false;
        
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
            Debug.DrawLine(fpsCam.transform.position, hit.point, Color.black, 3.0f);

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
        currentAmmo--;
    }



    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
}

