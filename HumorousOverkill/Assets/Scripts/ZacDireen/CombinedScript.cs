using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventHandler;

//[BindListener("Enemy", typeof(EnemyManager))]
//[BindListener("Player", typeof(PlayerManager))]
//[BindListener("UI", typeof(UIManager))]
public class CombinedScript : MonoBehaviour {

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
    public bool isReloading;
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
    /// <summary>
    ///  this is the end of the gun shown as a game object.
    /// </summary>
    public GameObject EndOfGun;
    /// <summary>
    ///  This is the first person camera.
    /// </summary>
    public Camera fpsCam;

    private bool stillGlitching;

    private bool switchingWeapon;




    // This boolean is for the rifle, to show if it is active or not.
    // public bool isRifleSelected;

    // This boolean is for if the user has access to the rifle.

    public bool glitchRifleEffect;


    public float m_timer = 0;
    // THIS IS BACHELOR STUFF 


    [System.Serializable]
    public struct BachelorStuff
    {
        // A bool to turn on and off Reflective shots.
        public bool ReflectiveShots;
        // A int for a percentage of damage that the reflective shots will have.
        public int ReflectMultiplier;
        // Testing the damage of the reflective shots.
        public Text enemyHealth;

        public bool showEnemyHealth;




    }

    [SerializeField] public BachelorStuff stuff;


    // This is the two different weapon types.
    public enum GunType
    {
        SHOTGUN,
        RIFLE

    }
    
    void Start() {

        currentShotgunAmmo = magTubeSize;
    }

    // Update is called once per frame
    void Update()
    {
        showEnemyHealth();




        DisplayAmmo();


        RightClickSwitching();


        if (gunType == GunType.SHOTGUN)
        {
            checkReloadShotgun();
        }





        //if (gunType == GunType.SHOTGUN)
        if (gunType == GunType.RIFLE)
        {
            checkReloadRifle();
        }



        if (Input.GetKey(KeyCode.R))
        {
            ManualReloading();
        }



        if (Input.GetKey(KeyCode.Mouse0) && gunType == GunType.RIFLE && !isReloading)
        {

            shootRifle();


        }




        GlitchCheck();
        Glitching();


        if (Input.GetKeyDown(KeyCode.Mouse0) && gunType == GunType.SHOTGUN && Time.time >= nextTimeToFire)
        {
            shootShotgun();
        }

    }



    void showEnemyHealth()
    {
        //if (stuff.showEnemyHealth == true)
        //{
        //    stuff.enemyHealth.enabled = true;
        //}
        //else
        //{
        //    stuff.enemyHealth.enabled = false;
        //}

    }
    void checkReloadShotgun()
    {
        if (currentShotgunAmmo <= 0)
        {

            if (ReloadTimer(reloadShotgunTime))
            {
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
            }
        }
    }

    void checkReloadRifle()
    {
        if (maxRifleAmmo == 0 && currentRifleAmmo == 0)
        {
            //SelectedWeapon = 0;
            gunType = GunType.SHOTGUN;
            glitchRifleEffect = false;
        }

        if (isReloading)
        {

            glitchRifleEffect = false;


        }

        if (currentRifleAmmo <= 0 && maxRifleAmmo > 0)
        {
            gunType = GunType.RIFLE;

            if (ReloadTimer(reloadRifleTime))
            {
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
            }

        }

    }

    void shootRifle()
    {
        if (currentRifleAmmo > 0 && Time.time >= nextTimeToFire)
        {

            nextTimeToFire = Time.time + 60f / RoundsPerMinute;

            Shoot();
        }
    }

    void shootShotgun()
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


    void ManualReloading()
    {
        switch (gunType)
        {
            case GunType.SHOTGUN:
                maxShotgunAmmo += currentShotgunAmmo;
                currentShotgunAmmo = 0;
                //StartCoroutine(Reload());
                break;
            case GunType.RIFLE:
                maxRifleAmmo += currentRifleAmmo;
                currentRifleAmmo = 0;
                //StartCoroutine(Reload());
                break;
            default:
                break;
        }

    }

    void GlitchCheck()
    {

        if (Input.GetKeyUp(KeyCode.Mouse0) && gunType == GunType.RIFLE)
        {
            glitchRifleEffect = false;

        }



        if (Input.GetKeyDown(KeyCode.Mouse0) && gunType == GunType.RIFLE)
        {

            glitchRifleEffect = true;


        }


        if (gunType == GunType.SHOTGUN)
        {
            glitchRifleEffect = false;

        }
    }

    void RightClickSwitching()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isReloading)
        {
            if (gunType == GunType.RIFLE)
            {
                gunType = GunType.SHOTGUN;
                
                //isRifleSelected = false;
                switchingWeapon = true;
            }
            else
            {
               
                    if (currentRifleAmmo > 0 || maxRifleAmmo > 0)
                    {
                        gunType = GunType.RIFLE;
                        //SelectedWeapon = 1;
                        //isRifleSelected = true;
                        switchingWeapon = true;
                    }
                
            }

        }


    }
    void DisplayAmmo()
    {
        if (gunType == GunType.RIFLE)
        {
            EventManager<GameEvent>.InvokeGameState(this, null, (float)currentRifleAmmo, typeof(UIManager), GameEvent.UI_AMMO_CUR);
            EventManager<GameEvent>.InvokeGameState(this, null, (float)maxRifleAmmo, typeof(UIManager), GameEvent.UI_AMMO_MAX);
        }
        else
        {
            EventManager<GameEvent>.InvokeGameState(this, null, (float)currentShotgunAmmo, typeof(UIManager), GameEvent.UI_AMMO_CUR);
            EventManager<GameEvent>.InvokeGameState(this, null, (float)maxShotgunAmmo, typeof(UIManager), GameEvent.UI_AMMO_MAX);
        }


    }

    /// <summary>
    /// This is the reload function.
    /// </summary>
    /// <param name="time"> used to keep track of time.</param>
    /// <returns>void</returns>
    bool ReloadTimer(float time)
    {
        if (m_timer <= 0)
        {
            m_timer = time;
        }

        if (m_timer > 0)
        {
            isReloading = true;
            m_timer -= Time.deltaTime;

            if (m_timer <= 0)
            {
                isReloading = false;
                return true;
            }
        }
        else
        {
            isReloading = false;
            return true;
        }
        return false;

    }
    void Glitching()
    {
        if (glitchRifleEffect == true && !isReloading && gunType == GunType.RIFLE)
        {

            fpsCam.GetComponent<GlitchPostRender>().offset += 0.01f * Time.deltaTime;
            if (fpsCam.GetComponent<GlitchPostRender>().offset > 0.01f)
            {
                fpsCam.GetComponent<GlitchPostRender>().offset = 0.01f;
                stillGlitching = true;
            }
        }
        else
        {

            fpsCam.GetComponent<GlitchPostRender>().offset -= 0.01f * Time.deltaTime;
            if (fpsCam.GetComponent<GlitchPostRender>().offset < 0)
            {
                fpsCam.GetComponent<GlitchPostRender>().offset = 0;
                stillGlitching = false;
            }
        }
        if (gunType == GunType.SHOTGUN)
        {
            fpsCam.GetComponent<GlitchPostRender>().offset -= 0.01f * Time.deltaTime;
            if (fpsCam.GetComponent<GlitchPostRender>().offset < 0)
            {
                fpsCam.GetComponent<GlitchPostRender>().offset = 0;
                switchingWeapon = false;
            }
        }
        if (gunType == GunType.SHOTGUN)
        {

        }
    }


    private IEnumerator Shot()
    {
        var shotDelay = 0.5f;
        if (gunType == GunType.RIFLE)
        {
            //shotDelay = 0;
            //shotTrail.enabled = true;
            yield return shotDelay;
            //shotTrail.enabled = false;
        }
        if (gunType == GunType.SHOTGUN)
        {
            //shotDelay = 0;
            //shotTrail.enabled = true;
            yield return shotDelay;
            //shotTrail.enabled = false;
        }
    }

   
    void Shoot()
    {
        //shotTrail.enabled = true;
        //while(currentRifleAmmo > 0 && Input.GetButton("Fire1"))
        //{
        currentRifleAmmo--;
        RifleMuzzleEffect.Play();
        // A variable that will store the imformation gathered from the raycast.
        RaycastHit hit, hit2;
        //Debug.DrawRay(StartOfPlayerRaycast.transform.position, fpsCam.transform.forward * Range, Color.red, 3.0f);
        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * Range, Color.red, 3.0f);

        // TESTING
        //shotTrail.material.mainTextureOffset = new Vector2(Time.time, 0);


        //shotTrail.SetPosition(0, EndOfGun.transform.position);
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward * Range, out hit, RifleRange))
        {

            // If we hit something with our shot raycast.
            //if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range)) ;
            Debug.DrawRay(EndOfGun.transform.position, hit.point - transform.position, Color.blue, 3.0f);
            if (Physics.Raycast(EndOfGun.transform.position, hit.point - transform.position, out hit2, RifleRange))
            {


                if (hit.transform != null)
                {
                    //shotTrail.SetPosition(1, hit2.point);

                    // Put in place the takeDamage event handler for the game manager here.
                    //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(GameEvent.)

                    //Debug.Log(hit.transform.name);
                    //Target target = hit.transform.GetComponent<Target>();



                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        if (hit.collider.gameObject.GetComponent<CupcakeAI>() != null)
                        {
                            //hit.collider.gameObject.GetComponent<CupcakeAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, RifleDamage);
                            EventManager<GameEvent>.InvokeGameState(this, (GameObject)hit.collider.gameObject, (float)RifleDamage, typeof(CupcakeAI), GameEvent.ENEMY_DAMAGED);
                        }
                        if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                        {
                            EventManager<GameEvent>.InvokeGameState(this, (GameObject)hit.collider.gameObject.transform.parent.gameObject, (float)RifleDamage, typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                        }
                        Debug.Log("I have shot " + hit.collider.gameObject.name);
                        //hit.transform.gameObject.GetComponent<DonutAI>().HandleEvent(GameEvent.ENEMY_DAMAGED);
                        //target.TakeDamage(RifleDamage);


                        Target target = hit.transform.GetComponent<Target>();
                        if (target != null)
                        {
                            if (stuff.showEnemyHealth)
                            {
                                target.TakeDamage(PelletDamage);
                                stuff.enemyHealth.text = "Enemies health:" + target.health;
                            }
                        }
                    }

                }
            }
        }
        else
        {
            Vector3 centreCam = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

        }
       

    }


    void ShootRay()
    {
        Vector3 inDirection;
        //  Try this one first, before using the second one
        //  The Ray-hits will form a ring
        float randomRadius = spreadWidth;
        //  The Ray-hits will be in a circular area
        randomRadius = Random.Range(0, spreadWidth);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle), Range

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
            

            inDirection = Vector3.Reflect(r.direction, -hit.normal);
  
            if (hit.collider.gameObject.tag == "Enemy")
            {
                

                if (hit.collider.gameObject.GetComponent<CupcakeAI>() != null)
                {
                    EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)RifleDamage, typeof(CupcakeAI), GameEvent.ENEMY_DAMAGED);
                }
                if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                {
                    EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject.transform.parent.gameObject, (float)RifleDamage, typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                }
                Debug.Log("I have shot " + hit.collider.gameObject.name);


                Target target = hit.transform.GetComponent<Target>();
                if (target != null)
                {
                    if (stuff.showEnemyHealth)
                    {
                        target.TakeDamage(PelletDamage);
                        stuff.enemyHealth.text = "Enemies health:" + target.health;
                    }
                }
              
            }
            Debug.DrawLine(EndOfGun.transform.position, hit.point, Color.red, 5.0f);
            Vector3 incomingVec = hit.point - EndOfGun.transform.position;
            Vector3 reflectVec = Vector3.Reflect(incomingVec, hit.normal);

            Ray r2 = new Ray(hit.point, reflectVec);
            RaycastHit hit2;
            if (stuff.ReflectiveShots == true)
            {
                if (Physics.Raycast(r2, out hit2, 400))
                {
                    //Debug.DrawRay(hit2.point, -hit2.normal, Color.yellow, 5.0f);
                    //Debug.Log(gameObject);
                    Debug.DrawRay(hit.point, reflectVec, Color.green, 5.0f);
                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        if (hit.collider.gameObject.GetComponent<CupcakeAI>() != null)
                        {
                            EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)(PelletDamage * stuff.ReflectMultiplier / 100), typeof(CupcakeAI), GameEvent.ENEMY_DAMAGED);
                        }
                        if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                        {
                            EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)(PelletDamage * stuff.ReflectMultiplier / 100), typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                        }
                        Debug.Log("I have shot " + hit.collider.gameObject.name);


                    }
                    Target target = hit2.transform.GetComponent<Target>();
                    if (target != null)
                    {

                        //show the enemy health (testing)
                        //show the enemy health
                        if (stuff.showEnemyHealth)
                        {

                            target.TakeDamage((PelletDamage * stuff.ReflectMultiplier) / 100);
                            stuff.enemyHealth.text = "Enemies health:" + target.health;


                        }

                    }


                }
            }
            
            


        }
        else
        {
            //Vector3 centreCam = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));

            Debug.DrawLine(StartOfPlayerRaycast.transform.position, fpsCam.transform.forward * Range, Color.cyan, 3.0f);
        }
    }

    void OnEnable()
    {
        isReloading = false;
        animator.SetBool("Reloading", false);
    }

}
