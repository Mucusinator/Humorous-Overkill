using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DonutAI : MonoBehaviour
{
    enum ANIMATIONSTATE { ROLL, DEPLOY, RAISEGUN, SHOOT, LOWERGUN, GETUP };

    #region variables

    // stores stats
    public DonutEnemyInfo myInfo;

    public bool showMovementGizmos = false;
    public bool showAttackGizmos = false;

    private bool deployed;
    private float donutCircumference;
    private GameObject player;
    private Animator myAnimator;
    private RaycastHit shootHitInfo;
    private Vector3 currentTarget;
    private RaycastHit rollHitInfo;
    private float shotTimer = 0;

    private bool dead = false;

    // what types of pickups to drop
    public List<GameObject> pickupPrefabs;

    private LayerMask pickupSpawnLayerMask;

    // reference to scoremanager
    private scoreManager scoremanager;

    #endregion

    public void Awake()
    {
        EventManager<GameEvent>.Add(HandleEvent);

        // find the player
        player = GameObject.FindObjectOfType<Player>().gameObject;

        // store animator
        myAnimator = GetComponent<Animator>();

        // get default info from enemyManager
        if(GameObject.FindObjectOfType<EnemyManager>() != null)
        {
            myInfo = GameObject.FindObjectOfType<EnemyManager>().defaultDonutInfo;
        }
        else
        {
            Debug.Log("Donut could not find an EnemyManager");
        }

        // calculate circumference (needed for nice rolling)
        findCircumference();

        pickTarget();

        pickupSpawnLayerMask = LayerMask.GetMask("Player", "Enemy");

        // find score manager (if it exists)
        if (GameObject.FindObjectsOfType<scoreManager>().Length != 0)
        {
            scoremanager = GameObject.FindObjectsOfType<scoreManager>()[0];
        }
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

    #region functions

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
                currentTarget += rollHitInfo.normal;
                currentTarget.y = transform.position.y;
            }
        }

        if (nearTarget())
        {
            pickTarget();
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

    // break apart and destroy
    // do manager stuff
    void die()
    {
        Debug.Log("die has been called");

        // tell enemy manager that an enemy has died
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(EnemyManager), GameEvent.ENEMY_SPAWNER_REMOVE);

        // tell score manager that a cupcake has died (if it exists)
        if (scoremanager != null)
        {
            scoremanager.updatePoints(scoreManager.ENEMYTYPE.DONUT);
        }

        // disable collider preventing more deaths
        GetComponentInChildren<BoxCollider>().enabled = false;

        dropPickup();

        // find the regular model and the broken one
        GameObject model = GetComponentsInChildren<Transform>(true)[1].gameObject;
        GameObject brokenModel = GetComponentsInChildren<Transform>(true)[2].gameObject;

        // parent the broken model to the root object (keep the same rotation)
        brokenModel.transform.parent = transform;

        // swap the models
        model.SetActive(false);
        brokenModel.SetActive(true);

        // disable this script to prevent any more actions
        this.enabled = false;

        // destroy this gameobject after 5 seconds
        Destroy(this.gameObject, 5.0f);
    }

    void dropPickup()
    {
        // randomly drop a pickup (on the floor)
        if (Random.Range(0, 100) < myInfo.pickupDropRate * 100)
        {
            RaycastHit ammoDropRay;
            if (Physics.Raycast(transform.position, -Vector3.up, out ammoDropRay, pickupSpawnLayerMask))
            {
                Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Count)], ammoDropRay.point, Quaternion.identity);
            }
        }
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

            // can't hit player (too far)
            if ((player.transform.position - transform.position).magnitude > myInfo.hitRange)
            {
                return;
            }

            // start off aiming straight at the player
            Vector3 aimPoint = player.transform.position - transform.position;

            // if the the player height offset is lower than maximumTargetHeight add that height independant of range
            float playerHeightOffset = player.transform.position.y - transform.position.y;
            Debug.Log(playerHeightOffset);

            // can't hit player (too high)
            if (playerHeightOffset > myInfo.maximumTargetHeight)
            {
                return;
            }

            // create a random offset using accuracy
            Vector2 randomOffset = Random.insideUnitCircle * myInfo.accuracy;

            // add the random offset to the aim point
            aimPoint.x += randomOffset.x;
            aimPoint.y += randomOffset.y;

            // show this shot as a red line if showAttackGizmos is enabled
            if (showAttackGizmos)
            {
                Debug.DrawRay(transform.position, aimPoint, Color.red, 0.5f);
            }

            if (Physics.Raycast(transform.position, aimPoint, out shootHitInfo))
            {
                // if the player was hit by the shot
                if (shootHitInfo.collider.gameObject.tag == "Player" && shootHitInfo.collider.gameObject.GetComponent<Player>() != null)
                {
                    // send the player a PLAYER_DAMAGE event
                    shootHitInfo.collider.gameObject.GetComponent<Player>().HandleEvent(GameEvent.PLAYER_DAMAGE, myInfo.damage);
                }
            }
        }
    }

    public void HandleEvent(object sender, __eArg<GameEvent> e)
    {
        // if we are not the sender or we are not the target return
        if (sender == (object)this)
        {
            Debug.Log("Donut broke because of sendr");
            return;
        }

        if (e.target != (object)this.gameObject)
        {
            Debug.Log("Donut broke because target was " + (GameObject)e.target);
            return;
        }

        Debug.Log("Donut is handle event");
        switch (e.arg)
        {
            case GameEvent.ENEMY_DAMAGED:
                if(!dead)
                {
                    myInfo.health -= (float)e.value;
                    if (myInfo.health <= 0)
                    {
                        dead = true;
                        die();
                    }
                }
                break;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (showAttackGizmos)
        {
            // draw cone representing hitRange / accuracy
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange + Vector3.up * myInfo.accuracy + Vector3.up * myInfo.maximumTargetHeight);
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange - Vector3.up * myInfo.accuracy + Vector3.up * myInfo.maximumTargetHeight);
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange + transform.forward * myInfo.accuracy + Vector3.up * myInfo.maximumTargetHeight);
            UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange - transform.forward * myInfo.accuracy + Vector3.up * myInfo.maximumTargetHeight);
            UnityEditor.Handles.DrawWireDisc(transform.position - transform.right * myInfo.hitRange + Vector3.up * myInfo.maximumTargetHeight, transform.right, myInfo.accuracy);
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

    #endregion
}