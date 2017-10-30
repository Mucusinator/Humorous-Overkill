using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventHandler;

[BindListener("Enemy", typeof(EnemyManager))]
[BindListener("Player",typeof(PlayerManager))] 
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
    public float maxRifleAmmo = 15.0f;
    // This is the amount of ammo the rifle currently has.
    private float currentRifleAmmo;
    // THis is the magazine size.
    public float rifleMagSize;
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
    private int currentShotgunAmmo;
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
    // This is the Fire Rate of the rifle.
    public FireRate fireRate;
    // This is the two different weapon types.
    public GunType gunType;
    // This is where the player raycast of the camera will begin from.
    public GameObject StartOfPlayerRaycast;
    // Weapon Raycast.
    public GameObject WeaponRaycast;
    // This is the UI text element for the UI.
    public Text Ammo;


    // This is the Fire Rate of the rifle.
    public enum FireRate
    {
        SEMIAUTO,
        FULLAUTO

    }
    // This is the two different weapon types.
    public enum GunType
    {
        SHOTGUN,
        RIFLE

    }



    






    // Use this for initialization
    void Start () {

        SelectWeapon();
        currentRifleAmmo = rifleMagSize;
        currentShotgunAmmo = magTubeSize;
        currentShotgunAmmo = 0;
    }
	
	// Update is called once per frame
	void Update () {

        int previousSelectedWeapon = SelectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedWeapon >= transform.childCount - 1)
            {
                SelectedWeapon = 0;

            }
            else
            {
                SelectedWeapon++;

                //gunType++;    

            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedWeapon <= 0)
            {
                SelectedWeapon = transform.childCount - 1;


            }
            else
            {
                SelectedWeapon--;


            }
        }
        if (previousSelectedWeapon != SelectedWeapon)
        {
            SelectWeapon();
        }

        if (gunType == GunType.RIFLE)
        {
            Ammo.text = currentRifleAmmo + " / " + maxRifleAmmo;
        }
        if (gunType == GunType.SHOTGUN)
        {
            Ammo.text = currentShotgunAmmo + " / " + maxShotgunAmmo;
        }

        if (Input.GetKeyDown(KeyCode.Mouse1) && gunType == GunType.RIFLE)
        {
            if (fireRate == FireRate.FULLAUTO)
            {
                fireRate = FireRate.SEMIAUTO;
            }
            else
            {
                fireRate = FireRate.FULLAUTO;
            }
        }


  


        if (SelectedWeapon == 0)
        {
            if (isReloading)
            {
                return;
            }

            if (currentRifleAmmo <= 0 && maxRifleAmmo > 0)
            {
                StartCoroutine(Reload());
              
                return;
            }
        }
        
        //if (gunType == GunType.SHOTGUN)
        if(SelectedWeapon == 1)
        {
            if (maxShotgunAmmo == 0 && currentShotgunAmmo == 0)
            {
                SelectedWeapon = 0;
                gunType = GunType.RIFLE;
                SelectWeapon();
            }

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
        //if (SelectedWeapon == 0)
        //{
        //    gunType = GunType.RIFLE;

        //}
        //if (SelectedWeapon == 1)
        //{

        //    gunType = GunType.SHOTGUN;
        //}

        

   

        if (Input.GetButtonDown("Fire1") && gunType == GunType.RIFLE && fireRate == FireRate.SEMIAUTO && Time.time >= nextTimeToFire)
        {
            
            if (currentRifleAmmo > 0)
            {
                nextTimeToFire = Time.time + 60f / RoundsPerMinute;
                Shoot();
            }
        }
        if (Input.GetButton("Fire1") && gunType == GunType.RIFLE && fireRate == FireRate.FULLAUTO && Time.time >= nextTimeToFire)
        {
            if (currentRifleAmmo > 0)
            {
                nextTimeToFire = Time.time + 60f / RoundsPerMinute;
                Shoot();
            }
        }
        if (Input.GetButtonDown("Fire1") && gunType == GunType.SHOTGUN && Time.time >= nextTimeToFire)
        {
            if (currentShotgunAmmo > 0)
            {
                ShotgunMuzzleEffect.Play();
                currentShotgunAmmo--;
                for (int i = 0; i < pelletCount; ++i)
                {
                    ShootRay();
                }
                

            }
        }






        }

    IEnumerator Reload()
    {
        if (gunType == GunType.RIFLE)
        {
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
        currentRifleAmmo--;
        RifleMuzzleEffect.Play();
        // A variable that will store the imformation gathered from the raycast.
        RaycastHit hit;
        Debug.DrawRay(StartOfPlayerRaycast.transform.position, StartOfPlayerRaycast.transform.forward * Range, Color.red, 3.0f);
        // If we hit something with our shot raycast.
        //if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) ;
        if (Physics.Raycast(StartOfPlayerRaycast.transform.position, StartOfPlayerRaycast.transform.forward*Range, out hit, RifleRange));
        {
            if (hit.transform != null)
            {

                Debug.DrawRay(WeaponRaycast.transform.position, hit.point - transform.position, Color.blue, 3.0f);

                // Put in place the takeDamage event handler for the game manager here.
                //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(GameEvent.)

                Debug.Log(hit.transform.name);
                //Target target = hit.transform.GetComponent<Target>();



                if (hit.transform.tag == "Target")
                {
                    hit.transform.gameObject.GetComponent<DroneAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
                    //hit.transform.gameObject.GetComponent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
                    //target.TakeDamage(RifleDamage);
                    
                }
            }
        }
        
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
        direction = StartOfPlayerRaycast.transform.TransformDirection(direction.normalized);

        //Raycast and debug
        Ray r = new Ray(WeaponRaycast.transform.position, direction);

        // the object that gets hit from the raycast.
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {
            

            Debug.DrawLine(WeaponRaycast.transform.position, hit.point, Color.black, 3.0f);


            //Target shotgunTarget = hit.transform.GetComponent<Target>();
            //if (shotgunTarget != null)
            //{
            //    shotgunTarget.TakeDamage(PelletDamage);
            //}
            if (hit.transform.tag == "Target")
            {
                GetEventListener("Enemy").HandleEvent(GameEvent.ENEMY_DAMAGED, hit.transform.gameObject);

                hit.transform.gameObject.GetComponent<DroneAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
                //hit.transform.gameObject.GetComponent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
            }

        }
    }
    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }
}
