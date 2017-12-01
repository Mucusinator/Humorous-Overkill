using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventHandler;


public class CombinedScript : MonoBehaviour
{

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

    /// <summary>
    /// These booleans are responsible for checking if you are reloading your rifle or shotgun currently.
    /// </summary>
    public bool isReloadingRifle, isReloadingShotgun;

    /// <summary>
    /// This animatior is resonsible for the reloading mechanic of the weapons.
    /// </summary>
    public Animator animator;
    /// <summary>
    /// These two audio clips are the sounds when you fire the Rifle or shotgun.
    /// </summary>
    public AudioClip LazerSound;
    public AudioClip shotgunSound;
    //Audio manager
    private AudioManager m_audioManager;
    /// <summary>
    /// This is a public int of the currently selected weapon.
    /// </summary>
     
    public int SelectedWeapon = 0;
    /// <summary>
    /// This will store the two different weapon types.
    /// </summary>
    public GunType gunType;
   
    /// <summary>
    ///  This is where the player raycast of the camera will begin from.
    /// </summary>
    public GameObject StartOfPlayerRaycast;
    /// <summary>
    /// This is where the start of the weapon raycast is held.
    /// </summary>
    public GameObject WeaponRaycast;
    /// <summary>
    ///  this is the end of the gun shown as a game object.
    /// </summary>
    public GameObject EndOfGun;
    /// <summary>
    ///  This is the first person camera.
    /// </summary>
    public Camera fpsCam;

    /// <summary>
    /// This is the Audio clip for when the enemys are killed.
    /// </summary>
    public AudioClip enemyDeathSound;


    /// <summary>
    /// Set the current game state to be null by default.
    /// </summary>
    public GameEvent currentState = GameEvent._NULL_;



   
    /// <summary>
    /// This boolean is used to toggle the glitch rifle effect.
    /// </summary>
    public bool glitchRifleEffect;

    /// <summary>
    /// This is the timer used to help in calculating reload timers.
    /// </summary>
    float m_timer = 0;
    



    /// <summary>
    /// BELOW IS BACHELOR STUFF, THIS STUFF IS TO BE DISABLED FOR THE ADVANCED DIPLOMA VERSION OF THE GAME.
    /// </summary>
    
    /// <summary>
    ///  Indirection is the reflection vector, this is used for reflection of lazers.
    ///</summary>
    Vector3 inDirection;


    /// <summary>
    /// This struct contains all the values that I use for bachelor items.
    /// </summary>
    [System.Serializable]
    public struct BachelorStuff
    {
        // A bool to turn on and off Reflective shots.
        public bool ReflectiveShots;
        // A int for a percentage of damage that the reflective shots will have.
        public int ReflectMultiplier;
     

        public bool showStatistics;

        public Text enemiesKilled;

        public int killedCount;

        public float damageDealt;

        public int ReflectAmount;


        public LineRenderer lineRenderer;

    }


    [SerializeField]
    public BachelorStuff stuff;



    // Keys for the PlayerPrefs.
    //private const string enemiesKilled, timesJumped, damagedDealt;


    // This is the two different weapon types.
    public enum GunType
    {
        SHOTGUN,
        RIFLE

    }

    void Awake() {
        EventManager<GameEvent>.Add(HandleMessage);
        m_audioManager = FindObjectOfType<AudioManager>();
        m_audioManager.AddSound(LazerSound);
        m_audioManager.AddSound(shotgunSound);
        m_audioManager.AddSound(enemyDeathSound);
    }


    void Start()
    {
        EventManager<GameEvent>.InvokeGameState(this, null, null, GetType(), GameEvent._NULL_);
        currentShotgunAmmo = magTubeSize;


        // Default the playerPrefs
        PlayerPrefs.SetInt("enemiesKilled", 0);
        PlayerPrefs.SetInt("timesJumped", 0);
        PlayerPrefs.SetInt("damageDealt", 0);
        PlayerPrefs.SetInt("shotgunRoundsFired", 0);
    }

    public void HandleMessage(object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;
        if (e.arg == GameEvent._NULL_)
            if (e.type == typeof(AudioManager)) {
                //m_audioManager = (AudioManager)s;
                //m_audioManager.AddSound(LazerSound);
                //m_audioManager.AddSound(shotgunSound);
            }
        switch (e.arg)
        {
            case GameEvent.STATE_MENU:
            case GameEvent.STATE_RESTART:
            case GameEvent.STATE_CONTINUE:
            case GameEvent.STATE_PAUSE:
            case GameEvent.STATE_DIFFICULTY:
            case GameEvent.STATE_LOSE_SCREEN:
            case GameEvent.STATE_START:
            case GameEvent.STATE_WIN_SCREEN:
                currentState = e.arg;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {




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



        if (Input.GetKey(KeyCode.Mouse0) && gunType == GunType.RIFLE && !isReloadingRifle)
        {
            animator.SetBool("IsFiring", true);
            shootRifle();
        }
        if (currentRifleAmmo > 0)
        {
            animator.SetBool("HasAmmo", true);
        }
        else
        {
            animator.SetBool("HasAmmo", false);
        }



        GlitchCheck();
        Glitching();


        if (Input.GetKeyDown(KeyCode.Mouse0) && gunType == GunType.SHOTGUN && Time.time >= nextTimeToFire)
        {
            shootShotgun();
        }
        if (Input.GetKeyDown(KeyCode.Mouse0) && gunType == GunType.RIFLE && currentRifleAmmo > 0) {
            m_audioManager.PlaySound(LazerSound,true);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            animator.SetBool("IsFiring", false);
            m_audioManager.StopSound(LazerSound);

            stuff.lineRenderer.positionCount = 0;
        }

        if (stuff.showStatistics)
        {
            ShowDeadCount();
        }

    }





    void checkReloadShotgun()
    {
        if (currentShotgunAmmo <= 0)
        {

            if (ReloadTimerShotgun(reloadShotgunTime))
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

        if (isReloadingRifle)
        {

            glitchRifleEffect = false;


        }

        if (currentRifleAmmo <= 0 && maxRifleAmmo > 0)
        {
            gunType = GunType.RIFLE;

            if (ReloadTimerRifle(reloadRifleTime))
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

            if (stuff.ReflectiveShots == true)
            {
                ShootReflect();
            }
            else
            {
                Shoot();
            }
            
           
        }
    }

    void shootShotgun()
    {
        if (currentShotgunAmmo > 0 )
        {
            if (currentState == GameEvent.STATE_START || currentState == GameEvent.STATE_CONTINUE)
            {
                ShotgunMuzzleEffect.Play();

                currentShotgunAmmo--;

                animator.Play("Release");
                m_audioManager.PlaySound(shotgunSound,false);
                for (int i = 0; i < pelletCount; i++)
                {

                    ShootRay();

                }

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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (gunType == GunType.RIFLE)
            {
                animator.SetBool("IsRifle", false);
                gunType = GunType.SHOTGUN;

                //isRifleSelected = false;
                // switchingWeapon = true;
            }
            else
            {

                if (currentRifleAmmo > 0 || maxRifleAmmo > 0)
                {
                    animator.SetBool("IsRifle", true);
                    gunType = GunType.RIFLE;
                    //SelectedWeapon = 1;
                    //isRifleSelected = true;
                    //switchingWeapon = true;
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
    bool ReloadTimerShotgun(float time)
    {
        if (m_timer <= 0)
        {
            m_timer = time;
        }

        if (m_timer > 0)
        {
            isReloadingShotgun = true;
            m_timer -= Time.deltaTime;

            if (m_timer <= 0)
            {
                isReloadingShotgun = false;
                return true;
            }
        }
        else
        {
            isReloadingShotgun = false;
            return true;
        }
        return false;

    }


    bool ReloadTimerRifle(float time)
    {
        if (m_timer <= 0)
        {
            m_timer = time;
        }

        if (m_timer > 0)
        {
            isReloadingRifle = true;
            m_timer -= Time.deltaTime;

            if (m_timer <= 0)
            {
                isReloadingRifle = false;
                return true;
            }
        }
        else
        {
            isReloadingRifle = false;
            return true;
        }
        return false;

    }
    void Glitching()
    {
        if (glitchRifleEffect == true && !isReloadingRifle && gunType == GunType.RIFLE)
        {

            fpsCam.GetComponent<GlitchPostRender>().offset += 0.003f * Time.deltaTime;
            if (fpsCam.GetComponent<GlitchPostRender>().offset > 0.003f)
            {

                fpsCam.GetComponent<GlitchPostRender>().offset = 0.01f;
                //stillGlitching = true;

                fpsCam.GetComponent<GlitchPostRender>().offset = 0.003f;
                //stillGlitching = true;

            }
        }
        else
        {

            fpsCam.GetComponent<GlitchPostRender>().offset -= 0.01f * Time.deltaTime;
            if (fpsCam.GetComponent<GlitchPostRender>().offset < 0)
            {
                fpsCam.GetComponent<GlitchPostRender>().offset = 0;
                //stillGlitching = false;
            }
        }
        if (gunType == GunType.SHOTGUN)
        {
            fpsCam.GetComponent<GlitchPostRender>().offset -= 0.01f * Time.deltaTime;
            if (fpsCam.GetComponent<GlitchPostRender>().offset < 0)
            {
                fpsCam.GetComponent<GlitchPostRender>().offset = 0;
                //switchingWeapon = false;
            }
        }

    }





    void ShootReflect()
    {
        //This is the starting point of the raycast for the reflective shots. 
        Transform startingRaycastPoint = EndOfGun.transform;

        // this is the starting ray for the raycast.
        Ray ray = new Ray(startingRaycastPoint.position, startingRaycastPoint.forward);


        // the amount of points the line will render.
        int points = stuff.ReflectAmount;


        //stuff.lineRenderer.SetVertexCount(points);
        stuff.lineRenderer.positionCount = points;

        stuff.lineRenderer.SetPosition(0, EndOfGun.transform.position);
        //remove a round of ammunition 
        currentRifleAmmo--;
        // play the rifle effect.
        RifleMuzzleEffect.Play();

        // THe two different Raycast hit variables to gather information on the collision. 
        RaycastHit hit, hit2;



        Debug.DrawRay(fpsCam.transform.position, fpsCam.transform.forward * Range, Color.red, 3.0f);


        // For the amount of reflections
        for (int i = 0; i <= stuff.ReflectAmount; i++)
        {
            // if we havent hit anything initally
            if (i == 0)
            {
                // if we are able to hit it from the camera to the point of impact....
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward * Range, out hit, RifleRange))
                {
                    // then test to see if we can hit them from the end of the gun to the point of inpact.
                    if (Physics.Raycast(EndOfGun.transform.position, hit.point - transform.position, out hit2, RifleRange))
                    {

                        //set the inDirection 
                        inDirection = Vector3.Reflect(hit2.point - EndOfGun.transform.position, hit2.normal);
                        // cast the reflected ray, using the hit point as the origin and the reflected direction
                        ray = new Ray(hit2.point, inDirection);

                        Debug.DrawRay(ray.origin, Vector3.Reflect(hit2.point - EndOfGun.transform.position, hit2.normal), Color.green, 5);

                        // Draw the normal - can only seen at the scene tab, for debugging purposes
                        Debug.DrawRay(hit2.point, hit.normal * 3, Color.blue, 5);
                        //represent the ray using a line that can only be viewed at scene tab
                        Debug.DrawRay(hit2.point, inDirection * 100, Color.magenta, 5);



                        if (stuff.ReflectAmount == 1)
                        {
                            stuff.lineRenderer.positionCount = ++points;
                        }

                        stuff.lineRenderer.SetPosition(i + 1, hit2.point);
                        if (hit.collider.gameObject.tag == "Enemy")
                        {
                            if (hit.collider.gameObject.GetComponent<CupcakeAI>() != null)
                            {
                                //hit.collider.gameObject.GetComponent<CupcakeAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, RifleDamage);
                                EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)RifleDamage, typeof(CupcakeAI), GameEvent.ENEMY_DAMAGED);

                                PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + RifleDamage);

                                if (hit.collider.gameObject.GetComponentInParent<CupcakeAI>().myInfo.health <= 0)
                                {
                                    m_audioManager.PlaySound(enemyDeathSound, false);
                                    stuff.killedCount++;
                                    PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                                }

                            }
                            if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                            {
                                EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject.transform.parent.gameObject, (float)RifleDamage, typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                                PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + RifleDamage);
                                if (hit.collider.gameObject.GetComponentInParent<DonutAI>().myInfo.health <= 0)
                                {
                                    m_audioManager.PlaySound(enemyDeathSound, false);
                                    stuff.killedCount++;
                                    PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                                }


                            }

                            if (stuff.ReflectAmount == 0)
                            {
                                stuff.lineRenderer.SetPosition(i + 1, hit2.point);
                            }
                            //stuff.lineRenderer.SetPosition(i + 1, hit2.point);
                        }

                    }
                }
            }
            else
            {
             
                if (Physics.Raycast(ray.origin, ray.direction, out hit2, 100))
                {
                    inDirection = Vector3.Reflect(inDirection, hit2.normal);

                    ray = new Ray(hit2.point, inDirection);

                    //Draw the normal - can only be seen at the Scene tab, for debugging purposes  
                    Debug.DrawRay(hit2.point, hit2.normal * 3, Color.blue, 5);
                    //represent the ray using a line that can only be viewed at the scene tab  
                    Debug.DrawRay(hit2.point, inDirection * 100, Color.magenta, 5);

                    //Print the name of the object the cast ray has hit, at the console  
                    Debug.Log("Object name: " + hit2.transform.name);



                    //add a new vertex to the line renderer  
                    stuff.lineRenderer.positionCount = ++points;
                    //set the position of the next vertex at the line renderer to be the same as the hit point  
                    stuff.lineRenderer.SetPosition(i + 1, hit2.point);

                }



            }
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

                    if (hit.collider.gameObject.tag == "Enemy")
                    {
                        if (hit.collider.gameObject.GetComponent<CupcakeAI>() != null)
                        {
                            //hit.collider.gameObject.GetComponent<CupcakeAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, RifleDamage);
                            EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)RifleDamage, typeof(CupcakeAI), GameEvent.ENEMY_DAMAGED);

                            PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + RifleDamage);

                            if (hit.collider.gameObject.GetComponentInParent<CupcakeAI>().myInfo.health <= 0)
                            {
                                m_audioManager.PlaySound(enemyDeathSound, false);
                                stuff.killedCount++;
                                PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                            }

                        }
                        if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                        {
                            EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject.transform.parent.gameObject, (float)RifleDamage, typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                            PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + RifleDamage);
                            if (hit.collider.gameObject.GetComponentInParent<DonutAI>().myInfo.health <= 0)
                            {
                                m_audioManager.PlaySound(enemyDeathSound, false);
                                stuff.killedCount++;
                                PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                            }


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

        }


    }

    void ShowDeadCount()
    {
        //stuff.enemiesKilled.text = "Enemies Killed: " + stuff.killedCount;
        stuff.enemiesKilled.text = "Enemies Killed: ";

        stuff.enemiesKilled.text += PlayerPrefs.GetInt("enemiesKilled").ToString() + "\n";

        stuff.enemiesKilled.text += "Times Jumped: " + PlayerPrefs.GetInt("timesJumped").ToString() + "\n";

        stuff.enemiesKilled.text += "Damage Dealt: " + PlayerPrefs.GetFloat("damageDealt").ToString();

    }


  


    void ShootRay()
    {
        //Vector3 inDirection;
        //  Try this one first, before using the second one
        //  The Ray-hits will form a ring
        float randomRadius = spreadWidth;
        //  The Ray-hits will be in a circular area
        randomRadius = Random.Range(0, spreadWidth);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3
        (
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle), 
            Range

        );

        //Make the direction match the transform
        //It is like converting the Vector3.forward to transform.forward
        direction = fpsCam.transform.TransformDirection(direction.normalized);


        //Raycast and debug
        Ray r = new Ray(WeaponRaycast.transform.position, direction);

        // the object that gets hit from the raycast.
        RaycastHit hit;



        if (currentState == GameEvent.STATE_START || currentState == GameEvent.STATE_CONTINUE)
        {
            if (Physics.Raycast(r, out hit))
            {


                //inDirection = Vector3.Reflect(r.direction, -hit.normal);

                if (hit.collider.gameObject.tag == "Enemy")
                {


                    if (hit.collider.gameObject.GetComponent<CupcakeAI>() != null)
                    {
                        EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)PelletDamage, typeof(CupcakeAI), GameEvent.ENEMY_DAMAGED);
                        PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + PelletDamage);

                        if (hit.collider.gameObject.GetComponentInParent<CupcakeAI>().myInfo.health <= 0)
                        {
                            m_audioManager.PlaySound(enemyDeathSound, false);
                            stuff.killedCount++;
                            PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);

                        }
                    }
                    if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                    {
                        EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject.transform.parent.gameObject, (float)PelletDamage, typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                        PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + PelletDamage);
                        if (hit.collider.gameObject.GetComponentInParent<DonutAI>().myInfo.health <= 0)
                        {
                            m_audioManager.PlaySound(enemyDeathSound, false);
                            stuff.killedCount++;
                            PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                        }


                    }
                    Debug.Log("I have shot " + hit.collider.gameObject.name);




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
                                PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + PelletDamage * stuff.ReflectMultiplier/ 100);
                                if (hit.collider.gameObject.GetComponentInParent<CupcakeAI>().myInfo.health <= 0)
                                {
                                    m_audioManager.PlaySound(enemyDeathSound, false);
                                    stuff.killedCount++;
                                    PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                                }
                            }
                            if (hit.collider.gameObject.GetComponentInParent<DonutAI>() != null)
                            {
                                EventManager<GameEvent>.InvokeGameState(this, hit.collider.gameObject, (float)(PelletDamage * stuff.ReflectMultiplier / 100), typeof(DonutAI), GameEvent.ENEMY_DAMAGED);
                                PlayerPrefs.SetFloat("damageDealt", PlayerPrefs.GetFloat("damageDealt") + PelletDamage * stuff.ReflectMultiplier / 100);
                                if (hit.collider.gameObject.GetComponentInParent<DonutAI>().myInfo.health <= 0)
                                {
                                    m_audioManager.PlaySound(enemyDeathSound, false);
                                    stuff.killedCount++;
                                    PlayerPrefs.SetInt("enemiesKilled", PlayerPrefs.GetInt("enemiesKilled") + 1);
                                }
                            }
                            Debug.Log("I have shot " + hit.collider.gameObject.name);


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
    }
  


   
}
