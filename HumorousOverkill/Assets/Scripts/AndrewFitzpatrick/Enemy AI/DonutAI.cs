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

    // whether to display gizmos in the editor
    public bool showMovementGizmos = false;
    public bool showAttackGizmos = false;

    // private variables
    private bool deployed;
    private float donutCircumference;
    private GameObject player;
    private Animator myAnimator;
    private RaycastHit shootHitInfo;
    private Vector3 currentTarget;
    private RaycastHit rollHitInfo;
    private float shotTimer = 0;

    public bool freeze = false;
    private bool dead = false;

    // confetti prefab
    public GameObject confetti;

    // what types of pickups to drop
    public List<GameObject> pickupPrefabs;

    // layer mask to use when spawning pickups
    private LayerMask pickupSpawnLayerMask;

    // reference to scoremanager
    private scoreManager scoremanager;

    // event stuff
    __eHandle<object, __eArg<GameEvent>> events;

    // flash variables
    [Tooltip("Color to flash when taking damage")]
    public Color flashColor;
    [Tooltip("How fast to flash when taking damage")]
    public float flashSpeed;
    // flash timer
    private float flashTimer = 0;
    private bool flashing = false;

    #endregion

    // runs when this script is first created (regardless of whether it is enabled)
    public void Awake()
    {
        // set up event handling stuff
        events = new __eHandle<object, __eArg<GameEvent>>(HandleEvent);
        __event<GameEvent>.HandleEvent += events;

        // find and store needed components
        findComponents();

        // calculate the circumference of the donut (this is used to roll at a believable speed regardless of the size of the donut)
        findCircumference();

        // pick an initial target
        pickTarget();

        // set up the pickup spawn layer mask to exclude objects tagged Player and Enemy
        pickupSpawnLayerMask = LayerMask.GetMask("Player", "Enemy");
    }

    // stores references to needed components
    void findComponents()
    {
        // store a reference to the player GameObject (if it exists)
        if (GameObject.FindObjectOfType<Player>() != null)
        {
            player = GameObject.FindObjectOfType<Player>().gameObject;
        }
        else
        {
            Debug.Log("Donut could not find the player");
        }

        // store a reference to the attached Animator component (if it exists)
        if (GetComponent<Animator>() != null)
        {
            myAnimator = GetComponent<Animator>();
        }
        else
        {
            Debug.Log("Donut does not have an Animator component");
        }

        // get default info from EnemyManager (if it exists)
        if (GameObject.FindObjectOfType<EnemyManager>() != null)
        {
            myInfo = GameObject.FindObjectOfType<EnemyManager>().defaultDonutInfo;
        }
        else
        {
            Debug.Log("Donut could not find an EnemyManager");
        }

        // store a reference to the score manager (if it exists)
        if (GameObject.FindObjectsOfType<scoreManager>().Length != 0)
        {
            scoremanager = GameObject.FindObjectsOfType<scoreManager>()[0];
        }
        else
        {
            Debug.Log("Donut could not find score manager");
        }
    }

    // this is the main loop
    void Update()
    {
        // if the game is not pasued
        if(!freeze)
        {
            // deploy or roll
            if (deployed)
            {
                deploySequence();
            }
            else
            {
                roll();
            }

            // flash if needed
            if (!dead)
            {
                if (flashing)
                {
                    flash();
                }
            }
        }
    }

    #region functions

    // attempts to approach the target by rolling
    void roll()
    {
        // if we are near the target repick a target
        if (nearTarget())
        {
            pickTarget();
        }

        // if we are near the player deploy
        if (nearPlayer())
        {
            deployed = true;
            myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.DEPLOY);
        }

        // avoid walls and other enemies
        if (Physics.Raycast(transform.position, -transform.right, out rollHitInfo, myInfo.avoidRadius))
        {
            if (rollHitInfo.collider.gameObject.tag == "Avoid" || rollHitInfo.collider.gameObject.tag == "Enemy")
            {
                if (Time.timeScale != 0)
                {
                    currentTarget += rollHitInfo.normal * myInfo.avoidRadius;
                }
            }
        }

        // find direction to target
        Vector3 toPlayer = (currentTarget - transform.position).normalized;
        toPlayer.y = 0;

        // rotate to look at target using lerping and turnSpeed
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.up * 90) * Quaternion.LookRotation(toPlayer), myInfo.turnSpeed * Time.deltaTime);

        // roll forward
        transform.Translate(-transform.right * myInfo.rollSpeed * Time.deltaTime, Space.World);
    }

    // sets donutCircumference
    void findCircumference()
    {
        // get the boxCollider of the child mesh
        BoxCollider donutCollider = GetComponentInChildren<BoxCollider>();

        // get the "x" size of the collider (actually y)
        // this also takes into account scaling
        float size = donutCollider.size.x * transform.localScale.y;

        // circumference is 2PIr aka PI * diameter
        donutCircumference = (size * Mathf.PI);

        // position the donut nicely above the ground
        transform.position += Vector3.up * size / 2;

        // set roll animation speed to match size
        myAnimator.SetFloat("rollSpeed", (1.0f / donutCircumference) * myInfo.rollSpeed);
    }

    // fall over and attack player
    void deploySequence()
    {
        // find direction to player
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.y = 0;

        // snap rotation to look at player
        transform.rotation = Quaternion.Euler(Vector3.up * 90) * Quaternion.LookRotation(direction);

        // store the current animation state
        ANIMATIONSTATE currentAnimationState = (ANIMATIONSTATE)myAnimator.GetInteger("animationState");

        // shoot at the player if the current animation state is shoot
        if (currentAnimationState == ANIMATIONSTATE.SHOOT)
        {
            shootPlayer();
        }

        // if the current animation is finished
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !myAnimator.IsInTransition(0))
        {
            // switch on the current animation
            switch (myAnimator.GetInteger("animationState"))
            {
                // transition from getup back to rolling
                case (int)ANIMATIONSTATE.GETUP:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.ROLL);
                    deployed = false;
                    break;
                // stop shooting at the player when they leave our range
                case (int)ANIMATIONSTATE.SHOOT:
                    if (!nearPlayer())
                    {
                        myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.LOWERGUN);
                    }
                    break;
                default:
                    // transition into the next animation by default
                    myAnimator.SetInteger("animationState", myAnimator.GetInteger("animationState") + 1);
                    break;
            }
        }
    }

    // break apart and destroy
    // do manager stuff
    void die()
    {
        // debug that die has been called to check that it doesn't happen more than once
        Debug.Log("die has been called");

        // set our color back to normal (if it is changed from flashing)
        changeColor(Color.white);

        // tell enemy manager that an enemy has died
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(EnemyManager), GameEvent.ENEMY_SPAWNER_REMOVE);

        // tell score manager that a cupcake has died (if it exists)
        if (scoremanager != null)
        {
            scoremanager.updatePoints(scoreManager.ENEMYTYPE.DONUT);
        }

        // disable collider to prevent more deaths
        if(GetComponentInChildren<BoxCollider>() != null)
        {
            GetComponentInChildren<BoxCollider>().enabled = false;
        }

        // maybe drop a pickup
        dropPickup();

        // find the regular model and the broken one
        GameObject model = GetComponentsInChildren<Transform>(true)[1].gameObject;
        GameObject brokenModel = GetComponentsInChildren<Transform>(true)[2].gameObject;

        // parent the broken model to the root object (keep the same rotation)
        brokenModel.transform.parent = transform;

        // swap the models (the broken model will then do physics)
        model.SetActive(false);
        brokenModel.SetActive(true);

        // disable this script
        this.enabled = false;

        // destroy this gameobject after 5 seconds
        Destroy(this.gameObject, 5.0f);
    }

    // stop handling events when we are destroyed
    public void OnDestroy()
    {
        __event<GameEvent>.HandleEvent -= events;
    }

    // drops a pickup with a random chance
    void dropPickup()
    {
        // randomly drop a pickup (on the floor)
        if (Random.Range(0, 100) < myInfo.pickupDropRate * 100)
        {
            // raycast downwards
            RaycastHit ammoDropRay;

            // spawn the pickup on the floor
            if (Physics.Raycast(transform.position, -Vector3.up, out ammoDropRay, pickupSpawnLayerMask))
            {
                Instantiate(pickupPrefabs[Random.Range(0, pickupPrefabs.Count)], ammoDropRay.point, Quaternion.identity);
            }
        }
    }

    // changes the color of each child mesh
    void changeColor(Color newColor)
    {
        foreach (MeshRenderer child in GetComponentsInChildren<MeshRenderer>())
        {
            child.material.SetColor("_Color", newColor);
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

            // spawn confetti (if it has been set up)
            if(confetti != null)
            {
                GameObject newConfetti = Instantiate(confetti, transform.position, Quaternion.LookRotation(aimPoint));
                newConfetti.GetComponent<ParticleSystem>().Play();
                Destroy(newConfetti, 1.0f);
            }
            else
            {
                Debug.Log("Donut confetti has not been assigned");
            }

            // actually check if the player was hit
            if (Physics.Raycast(transform.position, aimPoint, out shootHitInfo))
            {
                // if the player was hit by the shot
                if (shootHitInfo.collider.gameObject.tag == "Player" && shootHitInfo.collider.gameObject.GetComponent<Player>() != null)
                {
                    // send the player a PLAYER_DAMAGE event
                    EventManager<GameEvent>.InvokeGameState(this, shootHitInfo.collider.gameObject, myInfo.damage, null, GameEvent.PLAYER_DAMAGE);
                }
            }
        }
    }

    public void HandleEvent(object sender, __eArg<GameEvent> e)
    {
        // if we are not the sender or we are not the target return
        if (sender == (object)this)
        {
            return;
        }
        if (e.target != (object)this.gameObject)
        {
            return;
        }

        switch (e.arg)
        {
            case GameEvent.ENEMY_DAMAGED:
                if (!dead)
                {
                    // subtract health
                    myInfo.health -= (float)e.value;

                    // flash
                    flashTimer = 0.0f;
                    flashing = true;

                    // if the health is now 0 die
                    if (myInfo.health <= 0)
                    {
                        dead = true;
                        die();
                    }
                }
                break;
            case GameEvent.STATE_PAUSE:
                freeze = true;
                break;
            case GameEvent.STATE_CONTINUE:
                freeze = false;
                break;
        }
    }

    public void flash()
    {
        flashTimer += flashSpeed * Time.deltaTime;

        // change color
        changeColor(Color.Lerp(flashColor, Color.white, flashTimer));

        if (flashTimer >= 1.0f)
        {
            flashing = false;
            flashTimer = 0.0f;
        }
    }

    #endregion

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
}