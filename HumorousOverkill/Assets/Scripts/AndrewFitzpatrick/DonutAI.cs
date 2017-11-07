using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[EventHandler.BindListener("playerManager", typeof(PlayerManager))]
[EventHandler.BindListener("enemyManager", typeof(EnemyManager))]
public class DonutAI : EventHandler.EventHandle
{
    enum ANIMATIONSTATE { ROLL, DEPLOY, RAISEGUN, SHOOT, LOWERGUN, GETUP };

    // stores stats
    public DonutEnemyInfo myInfo;

    public bool showMovementGizmos = false;
    public bool showAttackGizmos = false;

    private bool deployed;
    private float donutCircumference;
    private Transform modelTransform;
    private GameObject player;
    private Animator myAnimator;
    private RaycastHit shootHitInfo;
    private Vector3 currentTarget;
    private RaycastHit rollHitInfo;
    private float shotTimer = 0;

    // dead for the explosion
    public bool dead = false;

    public override void Awake()
    {
        base.Awake();

        // find the player
        player = GameObject.FindGameObjectWithTag("Player");

        // store animator
        myAnimator = GetComponent<Animator>();

        // get default info from enemyManager
        myInfo = GetEventListener("enemyManager").gameObject.GetComponent<EnemyManager>().defaultDonutInfo;

        // calculate circumference (needed for nice rolling)
        findCircumference();

        // find modelTransform
        modelTransform = GetComponentsInChildren<Transform>()[1];

        pickTarget();
    }

    void Update()
    {
        // either deploy or roll
        if (deployed)
        {
            deploySequence();
        }
        else
        {
            roll();
        }
    }

    // attempts to approach the target by rolling
    void roll()
    {
        // find direction to target
        Vector3 direction = (currentTarget - transform.position).normalized;
        direction.y = 0;

        // rotate to look at target using lerping and turnSpeed
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.up * 90) * Quaternion.LookRotation(direction), myInfo.turnSpeed * Time.deltaTime);

        // roll forward
        transform.Translate(-transform.right * myInfo.rollSpeed * Time.deltaTime, Space.World);

        // avoid walls and otgher enemies
        if (Physics.Raycast(transform.position, -transform.right, out rollHitInfo, myInfo.avoidRadius))
        {
            if (rollHitInfo.collider.gameObject.tag == "Avoid" || rollHitInfo.collider.gameObject.tag == "Enemy")
            {
                currentTarget += rollHitInfo.normal * myInfo.avoidRadius;
            }
        }

        if (nearTarget())
        {
            currentTarget = player.transform.position;
            currentTarget.y = 0;
        }

        // deploy if within deployRange
        if (nearPlayer())
        {
            deployed = true;
            myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.DEPLOY);
        }
    }

    // sets donutCircumference
    void findCircumference()
    {
        // get the boxCollider of the mesh
        BoxCollider donutCollider = GetComponentInChildren<BoxCollider>();

        // get the "x" size of the collider (actually y)
        // also takes into account scaling
        float size = donutCollider.size.x * transform.localScale.y;
        Debug.Log(size);

        // circumference is 2PIr aka PI * diameter
        donutCircumference = (size * Mathf.PI);

        // set height properly
        Vector3 position = transform.position;
        position.y = size / 2;
        transform.position = position;

        // set roll animation speed
        myAnimator.SetFloat("rollSpeed", (1.0f / donutCircumference) * myInfo.rollSpeed);
    }

    // fall over and attack player
    void deploySequence()
    {
        // find direction to player
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        // snap to look at player
        transform.rotation = Quaternion.Euler(Vector3.up * 90) * Quaternion.LookRotation(direction);

        ANIMATIONSTATE currentAnimationState = (ANIMATIONSTATE)myAnimator.GetInteger("animationState");

        // if the current animation is finished
        switch (currentAnimationState)
        {
            case ANIMATIONSTATE.SHOOT:
                shootPlayer();
                break;
        }

        // switch to the next animation
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !myAnimator.IsInTransition(0))
        {
            // run next animation (if the player is within range
            switch (myAnimator.GetInteger("animationState"))
            {
                case (int)ANIMATIONSTATE.GETUP:
                    // transition back to rolling
                    myAnimator.SetInteger("animationState", 0);
                    deployed = false;
                    break;
                case (int)ANIMATIONSTATE.SHOOT:
                    // keep shooting the player until they are out of range
                    if (!nearPlayer())
                    {
                        myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.LOWERGUN);
                        //myLineRenderer.enabled = false;
                    }
                    break;
                default:
                    // transition into the next animation
                    myAnimator.SetInteger("animationState", myAnimator.GetInteger("animationState") + 1);
                    break;
            }
        }
    }

    // TODO: make fancy
    void die()
    {
        // tell enemy manager that an enemy has died
        GetEventListener("enemyManager").HandleEvent(GameEvent.ENEMY_DIED);
        // destroy this gameobject
        Destroy(this.gameObject);
    }

    void pickTarget()
    {
        currentTarget = transform.position + getRandomVector(myInfo.targetRadius);
    }

    Vector3 getRandomVector(float radius)
    {
        Vector2 vec2D = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vec2D.x, 0, vec2D.y);
    }

    // returns true if we are near the player
    bool nearPlayer()
    {
        Vector3 position = transform.position;
        position.y = 0;
        Vector3 playerPosition = player.transform.position;
        playerPosition.y = 0;
        return (position - playerPosition).sqrMagnitude < Mathf.Pow(myInfo.deployRange, 2);
    }

    // returns true if we are near the target
    bool nearTarget()
    {
        return (currentTarget - transform.position).sqrMagnitude < Mathf.Pow(myInfo.errorMargin, 2);
    }

    public override bool HandleEvent(GameEvent e, float value)
    {
        switch (e)
        {
            case GameEvent.ENEMY_DAMAGED:
                myInfo.health -= value;
                if(myInfo.health <= 0)
                {
                    die();
                }
                break;
        }
        return true;
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (showAttackGizmos)
        {
            // draw cone representing hitRange / accuracy
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange + Vector3.up * myInfo.accuracy);
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange - Vector3.up * myInfo.accuracy);
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange + transform.forward * myInfo.accuracy);
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange - transform.forward * myInfo.accuracy);
            UnityEditor.Handles.DrawWireDisc(transform.position - transform.right * myInfo.hitRange, transform.right, myInfo.accuracy);
        }
        if (showMovementGizmos)
        {
            // display targetRadius in red
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, 5);

            // display margin of error in yellow
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, 3);

            // show current movement direction
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position - transform.right * myInfo.rollSpeed);

            // display current target as a green wire sphere
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentTarget, 0.5f);
        }
    }
#endif

    // shoots at the player
    void shootPlayer()
    {
        // increase shot timer
        shotTimer += Time.deltaTime;

        // when shot timer reaches 1 / fire rate
        if (shotTimer > (1 / myInfo.fireRate))
        {
            // reset shot timer
            shotTimer = 0;

            Vector3 aimPoint = -transform.right * myInfo.hitRange;
            Vector2 randomOffset = Random.insideUnitCircle * myInfo.accuracy;
            aimPoint.x += randomOffset.x;
            aimPoint.y += randomOffset.y;

            if(showAttackGizmos)
            {
                Debug.DrawRay(transform.position, aimPoint, Color.red);
            }

            if (Physics.Raycast(transform.position, aimPoint, out shootHitInfo))
            {
                // if the player was hit by the shot
                if (shootHitInfo.collider.gameObject.tag == "Player" && shootHitInfo.collider.gameObject.GetComponent<Player>() != null)
                {
                    // send the player a PLAYER_DAMAGE event
                    shootHitInfo.collider.gameObject.GetComponent<Player>().HandleEvent(GameEvent.PLAYER_DAMAGE, myInfo.damage);
                    Debug.Log("I have hit " + shootHitInfo.collider.gameObject.name);
                }
            }
        }
    }
}