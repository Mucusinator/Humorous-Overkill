using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EventHandler.BindListener("playerManager", typeof(PlayerManager))]
[EventHandler.BindListener("enemyManager", typeof(EnemyManager))]
public class DroneAI : EventHandler.EventHandle
{
    public DroneEnemyInfo myInfo;
    public bool showGizmos = false;

    private Vector3 currentTarget;
    private RaycastHit wanderHitInfo;

    // health / attacking
    [Header("Health / Attacking")]
    public GameObject player; // reference to the player
    public GameObject projectile; // projectile prefab
    private float shotTimer = 0;
    public bool freeze = false;

    void Start()
    {
        GetEventListener("playerManager").HandleEvent(GameEvent.ENEMY_SPAWN);
        myInfo = GetEventListener("enemyManager").gameObject.GetComponent<EnemyManager>().defaultDroneInfo;

        pickTarget();
        player = GameObject.FindGameObjectWithTag("Player");
    }

	void Update ()
    {
        if(!freeze)
        {
            wander();
        }
	}

    void wander()
    {
        // if we are within the margin of error pick a new target
        if ((transform.position - currentTarget).sqrMagnitude < Mathf.Pow(myInfo.errorMargin, 2))
        {
            pickTarget();
        }

        if (Physics.Raycast(transform.position, transform.forward, out wanderHitInfo, myInfo.avoidRadius))
        {
            if (wanderHitInfo.collider.gameObject.tag == "Avoid")
            {
                //Debug.Log("Hit at " + hitInfo.point);
                //Debug.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal, Color.blue);
                //Debug.Break();
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

    void pickTarget()
    {
        // make a good guess
        currentTarget = transform.position + getRandomVector(myInfo.targetRadius);

        // TODO: avoid walls etc
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

            // shoot at the point
            Debug.DrawLine(transform.position + transform.forward, shotPoint, Color.yellow);

            // create the projectile
            GameObject currentProjectile = Instantiate(projectile, transform.position + transform.forward, Quaternion.identity) as GameObject;
            // add impulse force to the projectile towards the player at shotForce
            currentProjectile.GetComponent<Rigidbody>().AddForce((shotPoint - currentProjectile.transform.position).normalized * myInfo.shotForce, ForceMode.Impulse);
            // destroy the projectile after a set time
            Destroy(currentProjectile, myInfo.projectileLifetime);

            // reset shot timer
            shotTimer = 0;
        }
    }

    void OnDrawGizmosSelected()
    {
        if(showGizmos)
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

    public override bool HandleEvent(GameEvent e)
    {
        return true; // TODO
    }
}
