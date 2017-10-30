using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[EventHandler.BindListener("playerManager", typeof(PlayerManager))]
[EventHandler.BindListener("enemyManager", typeof(EnemyManager))]
public class DonutAI : EventHandler.EventHandle
{
    enum ANIMATIONSTATE { ROLL, DEPLOY, RAISEGUN, SHOOT, LOWERGUN, GETUP };

    // stores stats
    public DonutEnemyInfo myInfo;

    private bool deployed;
    private float donutCircumference;
    private Transform modelTransform;
    private float deployTimer = 0;
    private GameObject target;
    private Animator myAnimator;

    void Start()
    {
        // get default values from enemyManager
        myInfo = GetEventListener("enemyManager").gameObject.GetComponent<EnemyManager>().defaultDonutInfo;

        // calculate circumference (needed for rolling)
        findCircumference();

        // find modelTransform
        modelTransform = GetComponentsInChildren<Transform>()[1];

        // store animator
        myAnimator = GetComponent<Animator>();

        // use player as target
        target = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // either deploy or roll
        if(deployed)
        {
            deploySequence();
        }
        else
        {
            roll();
        }
    }

    // attempts to approach the player by rolling
    void roll()
    {
        // find direction to target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;

        // rotate parent to look at target
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.up * 90) * Quaternion.LookRotation(direction), myInfo.turnSpeed * Time.deltaTime);

        // A wheel moves forward a distance equal to its circumference with each rotation.
        //myAnimator.GetCurrentAnimatorStateInfo(0).speedMultiplier = 

        // roll forward
        transform.Translate(-transform.right * myInfo.rollSpeed * Time.deltaTime, Space.World);

        // look for player
        // deploy if within attackRange
        if((transform.position - target.transform.position).magnitude < Mathf.Pow(myInfo.attackRange, 2))
        {
            deployed = true;
        }
    }

    // sets donutCircumference
    void findCircumference()
    {
        // access the boxCollider of the mesh
        BoxCollider donutCollider = GetComponentInChildren<BoxCollider>();

        // get the "x" size of the collider (actually y)
        float size = donutCollider.size.x;

        // debug the diameter of the mesh
        Debug.Log("the diameter of the donut is " + size);

        // circumference is 2PIr aka PI * diameter
        // also takes into account scaling
        donutCircumference = (size * Mathf.PI * transform.localScale.y);
    }

    // fall over and attack player
    void deploySequence()
    {
        // if the current animation is finished
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !myAnimator.IsInTransition(0))
        {
            // switch to the next animation / do stuff
            switch (myAnimator.GetInteger("animationState"))
            {
                case (int)ANIMATIONSTATE.ROLL:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.DEPLOY);
                    break;
                case (int)ANIMATIONSTATE.DEPLOY:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.RAISEGUN);
                    break;
                case (int)ANIMATIONSTATE.RAISEGUN:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.SHOOT);
                    // TODO: actually shoot
                    break;
                case (int)ANIMATIONSTATE.SHOOT:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.LOWERGUN);
                    break;
                case (int)ANIMATIONSTATE.LOWERGUN:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.GETUP);
                    break;
                case (int)ANIMATIONSTATE.GETUP:
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.ROLL);
                    deployed = false;
                    break;
                default:
                    break;
            }
        }
    }
}