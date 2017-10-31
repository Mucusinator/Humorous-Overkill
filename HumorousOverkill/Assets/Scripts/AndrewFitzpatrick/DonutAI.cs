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
    private float deployTimer = 0;
    private GameObject target;
    private Animator myAnimator;
    private RaycastHit hitInfo;

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

        // circumference is 2PIr aka PI * diameter
        // also takes into account scaling
        donutCircumference = (size * Mathf.PI * transform.localScale.y);
    }

    // fall over and attack player
    void deploySequence()
    {
        // if the current animation is finished
        switch (myAnimator.GetInteger("animationState"))
        {
            case (int)ANIMATIONSTATE.SHOOT:
                Debug.Log("Donut is shooting");
                Debug.DrawRay(transform.position, -transform.right * 1000, Color.red);
                if (Physics.Raycast(transform.position, -transform.right, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        hitInfo.collider.gameObject.GetComponent<Player>().HandleEvent(GameEvent.PLAYER_DAMAGE, myInfo.damage);
                        GameObject.Find("hurt").GetComponent<Image>().color = Color.red;
                        Debug.Log("Donut has hit the player");
                    }
                }
                break;
        }

        // switch to the next animation
        if (myAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !myAnimator.IsInTransition(0))
        {
            GameObject.Find("hurt").GetComponent<Image>().color = new Color(1, 1, 1, 0);
            // next animation
            if (myAnimator.GetInteger("animationState") != (int)ANIMATIONSTATE.GETUP)
            {
                myAnimator.SetInteger("animationState", myAnimator.GetInteger("animationState") + 1);
            }
            else
            {
                // back to roll
                myAnimator.SetInteger("animationState", 0);
                deployed = false;
            }
        }
    }

    // TODO: make fancy
    void die()
    {
        // tell enemy manager that an enemy has died
        GetEventListener("enemyManager").HandleEvent(GameEvent.ENEMY_DIED, 0);

        // destroy this gameobject
        Destroy(this.gameObject);
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
}