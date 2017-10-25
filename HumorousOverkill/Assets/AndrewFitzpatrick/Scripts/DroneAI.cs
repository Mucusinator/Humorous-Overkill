using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneAI : GameEventListener
{
    // wandering
    [Header("Wander Behavior")]
    public float targetRadius;
    public float errorMargin;
    public float wanderSpeed;
    public float turnSpeed;
    public float avoidRadius;
    public bool showGizmos = false;
    private Vector3 currentTarget;
    private RaycastHit wanderHitInfo;

    // health / attacking
    [Header("Health / Attacking")]
    public float health;
    public float fireRate;
    public float accuracy;
    public float attackRange;
    public GameObject player;
    public PlayerManager playerManager;
    private float shotTimer = 0;
    private RaycastHit shotHitInfo;
    public GameObject projectile; // we are firing projectiles

    void Start()
    {
        // get values from manager
        // targetRadius
        // errorMargin
        // wanderSpeed
        // turnSpeed
        // avoidRadius
        // health
        // damage
        // fireRate
        // accuracy

        pickTarget();
        player = GameObject.FindGameObjectWithTag("Player");
        playerManager = GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>();
    }

	void Update ()
    {
        wander();
	}

    void wander()
    {
        // if we are within the margin of error pick a new target
        if ((transform.position - currentTarget).sqrMagnitude < Mathf.Pow(errorMargin, 2))
        {
            pickTarget();
        }

        if (Physics.Raycast(transform.position, transform.forward, out wanderHitInfo, avoidRadius))
        {
            if (wanderHitInfo.collider.gameObject.tag == "Avoid")
            {
                //Debug.Log("Hit at " + hitInfo.point);
                //Debug.DrawLine(hitInfo.point, hitInfo.point + hitInfo.normal, Color.blue);
                //Debug.Break();
                currentTarget += wanderHitInfo.normal * avoidRadius;
            }
        }

        Vector3 direction = currentTarget - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        transform.Translate(transform.forward * wanderSpeed * Time.deltaTime, Space.World);

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
        currentTarget = transform.position + getRandomVector(targetRadius);

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

        return playerOffset.magnitude < Mathf.Pow(attackRange, 2);
    }

    // shoot at player
    void shootPlayer()
    {
        // increase shot timer
        shotTimer += Time.deltaTime;

        if(shotTimer > (1 / fireRate))
        {
            // pick a point around the player using accuracy
            Vector3 accuracyOffset = Random.insideUnitCircle * accuracy;
            accuracyOffset.z = accuracyOffset.y;
            accuracyOffset.y = 0;

            Vector3 shotPoint = player.transform.position + accuracyOffset;

            // shoot at the point
            Debug.DrawLine(transform.position + transform.forward, shotPoint, Color.yellow);
            GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().SendEvent(GameEvent.PICKUP_HEALTH); // change to take damage
            Debug.Log("I hit the player");

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
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, targetRadius);

            // display margin of error
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, errorMargin);

            // show currentVelocity
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * wanderSpeed);

            // display current target
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentTarget, 0.5f);
        }
    }

    public virtual void HandleEvent()
    {

    }
}
