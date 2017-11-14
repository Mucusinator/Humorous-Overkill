using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EventHandler.BindListener("playerManager", typeof(PlayerManager))]
[EventHandler.BindListener("enemyManager", typeof(EnemyManager))]
public class CupcakeAI : EventHandler.EventHandle
{
    #region variables

    public DroneEnemyInfo myInfo;
    public bool showGizmos = false;

    private Vector3 currentTarget;
    private RaycastHit wanderHitInfo;

    // health / attacking
    [Header("Health / Attacking")]
    private GameObject player; // reference to the player
    public GameObject projectile; // projectile prefab
    private float shotTimer = 0;

    // whether to change colors based on health
    public bool enableColorChanges = false;

    // freeze for when the game is paused
    public bool freeze = false;
    // dead for the explosion
    public bool dead = false;

    private float startHealth;

    #endregion

    public override void Awake()
    {
        base.Awake();

        myInfo = GetEventListener("enemyManager").gameObject.GetComponent<EnemyManager>().defaultDroneInfo;

        startHealth = myInfo.health;

        pickTarget();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update ()
    {
        if(!freeze && !dead)
        {
            wander();
        }
        else if(dead)
        {
            HandleEvent(GameEvent.ENEMY_DIED);
        }
	}

    void wander()
    {
        // if we are within the margin of error pick a new target
        if ((transform.position - currentTarget).sqrMagnitude < Mathf.Pow(myInfo.errorMargin, 2))
        {
            pickTarget();
        }

        // avoid walls and otgher enemies
        if (Physics.Raycast(transform.position, transform.forward, out wanderHitInfo, myInfo.avoidRadius))
        {
            if (wanderHitInfo.collider.gameObject.tag == "Avoid" || wanderHitInfo.collider.gameObject.tag == "Enemy")
            {
                currentTarget += wanderHitInfo.normal * myInfo.avoidRadius;
            }
        }

        Vector3 direction = currentTarget - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), myInfo.turnSpeed * Time.deltaTime);

        transform.Translate(transform.forward * myInfo.wanderSpeed * Time.deltaTime, Space.World);

        if(nearPlayer())
        {
            shootPlayer();
        }
        else
        {
            shotTimer = 0;
        }
    }

    #region functions

    void pickTarget()
    {
        currentTarget = transform.position + getRandomVector(myInfo.targetRadius);
    }

    Vector3 getRandomVector(float radius)
    {
        Vector2 vec2D = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vec2D.x, 0, vec2D.y);
    }

    // returns true if the player is within range
    bool nearPlayer()
    {
        Vector3 playerOffset = player.transform.position - transform.position;
        playerOffset.y = 0;

        return playerOffset.magnitude < Mathf.Pow(myInfo.attackRange, 2);
    }

    // shoot at player
    void shootPlayer()
    {
        // increase shot timer
        shotTimer += Time.deltaTime;

        // when shot timer reaches 1 / fire rate
        if(shotTimer > (1 / myInfo.fireRate))
        {
            // pick a point around the player using accuracy
            Vector3 accuracyOffset = Random.insideUnitCircle * myInfo.accuracy;
            accuracyOffset.z = accuracyOffset.y;
            accuracyOffset.y = 0;

            Vector3 shotPoint = player.transform.position + accuracyOffset;

            // create the projectile
            GameObject currentProjectile = Instantiate(projectile, transform.position + transform.forward, Quaternion.LookRotation(shotPoint - (transform.position + transform.forward)) * Quaternion.Euler(0, 0, 0)) as GameObject;
            Debug.DrawRay(transform.position + transform.forward, shotPoint - (transform.position + transform.forward), Color.cyan, 0.5f);
            //Debug.Break();
            // add impulse force to the projectile towards the player at shotForce
            currentProjectile.GetComponent<Rigidbody>().AddForce((shotPoint - currentProjectile.transform.position).normalized * myInfo.shotForce, ForceMode.Impulse);
            // destroy the projectile after a set time
            Destroy(currentProjectile, myInfo.projectileLifetime);

            // reset shot timer
            shotTimer = 0;
        }
    }

    // disable all AI and explode into pieces
    void die()
    {
        if(enableColorChanges)
        {
            changeColor(Color.black);
        }

        // tell enemy manager that an enemy has died
        if(GetEventListener("enemyManager") != null)
        {
            GetEventListener("enemyManager").HandleEvent(GameEvent.ENEMY_SPAWNER_REMOVE);
        }

        // disable animation
        GetComponent<Animator>().enabled = false;

        GetComponent<BoxCollider>().enabled = false;

        // loop through children
        Transform[] childTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform child in childTransforms)
        {
            // add boxCollider
            MeshCollider newCollider = child.gameObject.AddComponent<MeshCollider>();
            newCollider.convex = true;

            // add RigidBody
            child.gameObject.AddComponent<Rigidbody>();

            // add explosion force to launch the model
            child.GetComponent<Rigidbody>().AddExplosionForce(myInfo.explosionForce, transform.position, myInfo.explosionRadius);
        }

        // disable this script to prevent any more actions
        this.enabled = false;

        // fully delete this gameObject after a set number of seconds
        Destroy(this.gameObject, 5);
}

    void changeColor(Color newColor)
    {
        if (enableColorChanges)
        {
            Transform[] childTransforms = GetComponentsInChildren<Transform>();
            foreach (Transform child in childTransforms)
            {
                if (child.gameObject.GetComponent<Renderer>() != null)
                {
                    child.gameObject.GetComponent<Renderer>().material.SetColor("_Color", newColor);
                }
            }
        }
    }

    public override bool HandleEvent(GameEvent e, float value)
    {
        // Health is depleted
        if (e == GameEvent.ENEMY_DAMAGED)
        {
            // subtract health
            myInfo.health -= value;

            // change color if enabled
            if(enableColorChanges)
            {
                changeColor(Color.Lerp(Color.red, Color.white, 1.0f / startHealth * myInfo.health));
            }

            // if the health is now 0 die
            if (myInfo.health <= 0)
            {
                die();
            }
        }

        return true; // TODO
    }

    #endregion

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (showGizmos)
        {
            // display targetRadius
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, myInfo.targetRadius);

            // display margin of error
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, myInfo.errorMargin);

            // show currentVelocity
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * myInfo.wanderSpeed);

            // display current target
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentTarget, 0.5f);
        }
    }
#endif
}
