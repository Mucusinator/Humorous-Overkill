﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventHandler;

[BindListener("Enemy", typeof(EnemyManager))]
[BindListener("Player",typeof(PlayerManager))]
[BindListener("UI",typeof(UIManager))] 
public class CombinedScript : EventHandle {

    // Rifle/Laser Variables.

    // This is the rifles damage.
    public float RifleDamage = 10f;
    // This is the rifles range.
    public float RifleRange = 100f;
    // This is the rifles muzzle effect.
    public ParticleSystem RifleMuzzleEffect;
    // This is the force applied to an object when it gets hit with rifle ammo.
    public float impactForce = 30.0f;
    // This is the amount maximum amount of ammo the rifle has.
    public int maxRifleAmmo = 15;
    // This is the amount of ammo the rifle currently has.
    public int currentRifleAmmo;
    // THis is the magazine size.
    public int rifleMagSize;
    // This is the reload time of the rifle.
    public float reloadRifleTime = 2.5f;
    // private field showing the shots per second.
    [SerializeField]
    private float ShotsPerSecond;
    // this this is the rate of fire of the weapon.
    public float RoundsPerMinute = 600.0f;
    // this is the nextTimeToFire.
    private float nextTimeToFire = 0f;


    // Shotgun Variables.

    // This is the shotgun's damage per pellet.
    public float PelletDamage = 3.0f;
    // This is the amount of pellets the shotgun has.
    public int pelletCount = 8;
    // This is the mag tube size.
    public int magTubeSize = 6;
    // This is the Shotguns muzzle effect.
    public ParticleSystem ShotgunMuzzleEffect;
    // This is the amount of force applied to the target when they are hit with a pellet.
    public float pelletForce = 30.0f;
    // This is the Shotguns cone spread.
    public float spreadWidth = 2f;
    // This is the shotguns range.
    public float Range = 10f;
    // This is the amount maximum amount of ammo the shotgun has.
    public int maxShotgunAmmo = 8;
    // This is the amount of ammo the shotgun currently has.
    public int currentShotgunAmmo;
    // This is the reload time of the shotgun.
    public float reloadShotgunTime = 2.5f;
    // This is the shotgun delay so it is not spammable.
    public float FireDelay = 0.4f;



    // Shared/Unique variables. variables
    
    // This boolean tests if you are already reloading or not.
    private bool isReloading;
    // This animatior is resonsible for the reloading mechanic of the weapons.
    public Animator animator;
    // This is a public int of the currently selected weapon.
    public int SelectedWeapon = 0;
    // This is the two different weapon types.
    public GunType gunType;
    // This is where the player raycast of the camera will begin from.
    public GameObject StartOfPlayerRaycast;
    // Weapon Raycast.
    public GameObject WeaponRaycast;
    // This is the UI text element for the UI.
    //public Text Ammo;

    public GameObject EndOfGun;

    public Camera fpsCam;

    public LineRenderer shotTrail;

    // This boolean is for the rifle, to show if it is active or not.
    public bool isRifleSelected;

    // This boolean is for if the user has access to the rifle.


    // This is the two different weapon types.
    public enum GunType
    {
        SHOTGUN,
        RIFLE

    }



    






    // Use this for initialization
    void Start () {

        SelectWeapon();
        //currentRifleAmmo = rifleMagSize;
        currentShotgunAmmo = magTubeSize;
        //currentShotgunAmmo = 0;
        shotTrail = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        //GameObject.FindGameObjectWithTag("UI").GetComponent<UIManager>().HandleEvent(GameEvent.UI_AMMO_CUR);
        if (gunType == GunType.RIFLE)
        {
            GetEventListener("UI").HandleEvent(GameEvent.UI_AMMO_CUR, (float)currentRifleAmmo);
            GetEventListener("UI").HandleEvent(GameEvent.UI_AMMO_MAX, (float)maxRifleAmmo);
        }
        else if (gunType == GunType.SHOTGUN)
        {

            GetEventListener("UI").HandleEvent(GameEvent.UI_AMMO_CUR, (float)currentShotgunAmmo);
            GetEventListener("UI").HandleEvent(GameEvent.UI_AMMO_MAX, (float)maxShotgunAmmo);

        }

        int previousSelectedWeapon = SelectedWeapon;

        //if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        //{
        //    if (SelectedWeapon >= transform.childCount - 1)
        //    {
        //        SelectedWeapon = 0;

        //    }
        //    else
        //    {
        //        SelectedWeapon++;

        //        //gunType++;    

        //    }
        //}
        //if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        //{
        //    if (SelectedWeapon <= 0)
        //    {
        //        SelectedWeapon = transform.childCount - 1;


        //    }
        //    else
        //    {
        //        SelectedWeapon--;


        //    }
        //}
        //if (previousSelectedWeapon != SelectedWeapon)
        //{
        //    SelectWeapon();
        //}

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (isRifleSelected)
            {
                gunType = GunType.SHOTGUN;
                SelectedWeapon = 0;
                SelectWeapon();
                isRifleSelected = false;
            }
            else
            {
                if (!isRifleSelected)
                {
                    if (currentRifleAmmo > 0 || maxRifleAmmo > 0)
                    {
                        gunType = GunType.RIFLE;
                        SelectedWeapon = 1;
                        SelectWeapon();
                        isRifleSelected = true;
                    }
                }
            }

        }












        if (SelectedWeapon == 0)
        {
            if (isReloading)
            {
                return;
            }

            if (currentShotgunAmmo <= 0 && maxShotgunAmmo > 0)
            {
                gunType = GunType.SHOTGUN;
                StartCoroutine(Reload());

                return;
            }
        }

        //if (gunType == GunType.SHOTGUN)
        if (SelectedWeapon == 1)
        {
            if (maxRifleAmmo == 0 && currentRifleAmmo == 0)
            {
                SelectedWeapon = 0;
                gunType = GunType.SHOTGUN;
                SelectWeapon();
            }

            if (isReloading)
            {

                return;
            }

            if (currentRifleAmmo <= 0 && maxRifleAmmo > 0)
            {
                gunType = GunType.RIFLE;
                StartCoroutine(Reload());

                return;
            }

        }



        if (Input.GetKey(KeyCode.R))
        {
            switch (gunType)
            {
                case GunType.SHOTGUN:
                    maxShotgunAmmo += currentShotgunAmmo;
                    currentShotgunAmmo = 0;
                    StartCoroutine(Reload());
                    break;
                case GunType.RIFLE:
                    maxRifleAmmo += currentRifleAmmo;
                    currentRifleAmmo = 0;
                    StartCoroutine(Reload());
                    break;
                default:
                    break;
            }
        }

        //if (Input.GetButtonDown("Fire1") && gunType == GunType.RIFLE)
        //{
        //    shotTrail.enabled = true;
        //}
        if (Input.GetButtonUp("Fire1") || isReloading && gunType == GunType.RIFLE)
        {
            shotTrail.enabled = false;
        }

        if (Input.GetButton("Fire1") && gunType == GunType.RIFLE)
        {

            if (currentRifleAmmo > 0)
            {
                nextTimeToFire = Time.time + 60f / RoundsPerMinute;
                //StartCoroutine(Shot());
                Shoot();
            }
        }
        else
        {
            //shotTrail.enabled = false;
        }


        if (Input.GetButtonDown("Fire1") && gunType == GunType.SHOTGUN && Time.time >= nextTimeToFire)
        {
            if (currentShotgunAmmo > 0)
            {
                ShotgunMuzzleEffect.Play();
                currentShotgunAmmo--;
                for (int i = 0; i < pelletCount; ++i)
                {
                    //StartCoroutine(Shot());
                    ShootRay();

                }


            }
        }

    }
        



        
    

    private IEnumerator Shot()
    {
        var shotDelay = 0.5f;
        if (gunType == GunType.RIFLE)
        {
            //shotDelay = 0;
            shotTrail.enabled = true;
            yield return shotDelay;
            shotTrail.enabled = false;
        }
        if (gunType == GunType.SHOTGUN)
        {
            shotDelay = 0;
            shotTrail.enabled = true;
            yield return shotDelay;
            shotTrail.enabled = false;
        }
    }


    IEnumerator Reload()
    {
        if (gunType == GunType.RIFLE)
        {
            shotTrail.enabled = false;
            isReloading = true;
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadRifleTime - 0.25f);
            animator.SetBool("Reloading", false);
          
                yield return new WaitForSeconds(0.25f);
            
                //currentRifleAmmo = maxRifleAmmo;
            if (maxRifleAmmo < rifleMagSize)
            {
                currentRifleAmmo = maxRifleAmmo;
                maxRifleAmmo = 0;
            }
            else
            {
                currentRifleAmmo = rifleMagSize;
                maxRifleAmmo -= rifleMagSize;
            }
            isReloading = false;
        }
        if (gunType == GunType.SHOTGUN)
        {
            isReloading = true;
            animator.SetBool("Reloading", true);
            yield return new WaitForSeconds(reloadShotgunTime - 0.25f);
            animator.SetBool("Reloading", false);
            if (maxShotgunAmmo < magTubeSize)
            {
                currentShotgunAmmo = maxShotgunAmmo;
                maxShotgunAmmo = 0;
            }
            else
            {
                currentShotgunAmmo = magTubeSize;
                maxShotgunAmmo -= magTubeSize;
            }
            yield return new WaitForSeconds(0.25f);
            //currentShotgunAmmo = maxShotgunAmmo;
            isReloading = false;
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == SelectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
    void Shoot()
    {
        shotTrail.enabled = true;
        currentRifleAmmo--;
        RifleMuzzleEffect.Play();
        // A variable that will store the imformation gathered from the raycast.
        RaycastHit hit, hit2;
        //Debug.DrawRay(StartOfPlayerRaycast.transform.position, fpsCam.transform.forward * Range, Color.red, 3.0f);
        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * Range, Color.red, 3.0f);

        shotTrail.SetPosition(0, EndOfGun.transform.position);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward * Range, out hit, RifleRange))
        {

            // If we hit something with our shot raycast.
            //if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) ;
            Debug.DrawRay(EndOfGun.transform.position, hit.point - transform.position, Color.blue, 3.0f);
            if (Physics.Raycast(EndOfGun.transform.position, hit.point - transform.position, out hit2, RifleRange))
            {


                if (hit.transform != null)
                {
                    shotTrail.SetPosition(1, hit2.point);

                    // Put in place the takeDamage event handler for the game manager here.
                    //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(GameEvent.)

                    //Debug.Log(hit.transform.name);
                    //Target target = hit.transform.GetComponent<Target>();



                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        if(hit.collider.gameObject.GetComponent<DroneAI>() != null)
                        {
                            hit.collider.gameObject.GetComponent<DroneAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, RifleDamage);
                        }
                        if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                        {
                            hit.collider.gameObject.GetComponentInParent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, RifleDamage);
                        }
                        Debug.Log("I have shot " + hit.collider.gameObject.name);
                        //hit.transform.gameObject.GetComponent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
                        //target.TakeDamage(RifleDamage);

                    }
                }
            }
        }
        else
        {
            Vector3 centreCam = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            shotTrail.SetPosition(1, centreCam + (fpsCam.transform.forward * Range));
            //shotTrail.SetPosition(1, fpsCam.transform.position + (EndOfGun.transform.forward * Range));
        }
        //shotTrail.enabled = false;
    }
        
        
        
       
        
    


    void ShootRay()
    {
        //  Try this one first, before using the second one
        //  The Ray-hits will form a ring
        float randomRadius = spreadWidth;
        //  The Ray-hits will be in a circular area
        randomRadius = Random.Range(0, spreadWidth);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle),Range
            
        );

        //Make the direction match the transform
        //It is like converting the Vector3.forward to transform.forward
        direction = fpsCam.transform.TransformDirection(direction.normalized);

        
        //Raycast and debug
        Ray r = new Ray(WeaponRaycast.transform.position, direction);

        // the object that gets hit from the raycast.
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {

            Debug.DrawLine(StartOfPlayerRaycast.transform.position, hit.point, Color.black, 3.0f);


            //Target shotgunTarget = hit.transform.GetComponent<Target>();
            //if (shotgunTarget != null)
            //{
            //    shotgunTarget.TakeDamage(PelletDamage);
            //}
            if (hit.transform.tag == "Enemy")
            {
                GetEventListener("Enemy").HandleEvent(GameEvent.ENEMY_DAMAGED, hit.transform.gameObject);

                hit.transform.gameObject.GetComponent<DroneAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
                //hit.transform.gameObject.GetComponent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
            }

        }
        else
        {
            //Vector3 centreCam = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            Debug.DrawLine(StartOfPlayerRaycast.transform.position, fpsCam.transform.forward * Range, Color.green,3.0f);
        }
    }
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
}
