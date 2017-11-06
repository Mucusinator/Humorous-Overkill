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

    private bool deployed;
    private float donutCircumference;
    private Transform modelTransform;
    private GameObject target;
    private Animator myAnimator;
    private RaycastHit hitInfo;

    public override void Awake()
    {
        base.Awake();

        // use player as target
        target = GameObject.FindGameObjectWithTag("Player");

        // calculate circumference (needed for rolling)
        findCircumference();

        Vector3 position = transform.position;
        position.y = 1.5f;
        transform.position = position;

        // get default values from enemyManager
        myInfo = GetEventListener("enemyManager").gameObject.GetComponent<EnemyManager>().defaultDonutInfo;

        // find modelTransform
        modelTransform = GetComponentsInChildren<Transform>()[1];

        // store animator
        myAnimator = GetComponent<Animator>();
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

        // deploy if within deployRange
        if(nearPlayer())
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

        // circumference is 2PIr aka PI * diameter
        // also takes into account scaling
        donutCircumference = (size * Mathf.PI * transform.localScale.y);
    }

    // fall over and attack player
    void deploySequence()
    {
        ANIMATIONSTATE currentAnimationState = (ANIMATIONSTATE)myAnimator.GetInteger("animationState");

        // if the current animation is finished
        switch (currentAnimationState)
        {
            case ANIMATIONSTATE.SHOOT:
                Vector3 aimPoint = -transform.right * myInfo.hitRange;
                Vector2 randomOffset = Random.insideUnitCircle * myInfo.accuracy;
                aimPoint.x += randomOffset.x;
                aimPoint.y += randomOffset.y;

                Debug.DrawRay(transform.position, aimPoint, Color.red);
                if (Physics.Raycast(transform.position, aimPoint, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        //GameObject.Find("hurt").GetComponent<Image>().color = new Color(1, 0, 0, 0.5f);
                        hitInfo.collider.gameObject.GetComponent<Player>().HandleEvent(GameEvent.PLAYER_DAMAGE, myInfo.damage);
                        Debug.Log("I have hit the player");
                    }
                    else
                    {
                        //GameObject.Find("hurt").GetComponent<Image>().color = new Color(1, 0, 0, 0.0f);
                    }
                }

                // keep shooting the player until they are out of range
                if (!nearPlayer())
                {
                    myAnimator.SetInteger("animationState", myAnimator.GetInteger("animationState") + 1);
                }
                else
                {
                    myAnimator.SetInteger("animationState", (int)ANIMATIONSTATE.SHOOT);
                }
                break;
        }

        // switch to the next animation
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !myAnimator.IsInTransition(0))
        {
            //GameObject.Find("hurt").GetComponent<Image>().color = new Color(1, 0, 0, 0.0f);

            // run next animation (if the player is withing range
            switch (myAnimator.GetInteger("animationState"))
            {
                case (int)ANIMATIONSTATE.GETUP:
                    // transition back to rolling
                    myAnimator.SetInteger("animationState", 0);
                    deployed = false;
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

    // returns wheather we are within the deploy range
    bool nearPlayer()
    {
        return (transform.position - target.transform.position).magnitude < Mathf.Pow(myInfo.deployRange, 2);
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
        // display accuracy / range
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange + Vector3.up * myInfo.accuracy);
        UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange - Vector3.up * myInfo.accuracy);
        UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange + transform.forward * myInfo.accuracy);
        UnityEditor.Handles.DrawLine(transform.position, transform.position - transform.right * myInfo.hitRange - transform.forward * myInfo.accuracy);
        UnityEditor.Handles.DrawWireDisc(transform.position -transform.right * myInfo.hitRange, transform.right, myInfo.accuracy);
    }
#endif
}