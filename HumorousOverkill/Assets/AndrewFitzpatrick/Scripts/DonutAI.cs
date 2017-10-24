using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutAI : MonoBehaviour
{
    public float health;
    public float damage;
    public float fireRate;
    public float rollSpeed;
    public float turnSpeed;
    public float attackRange;
    public List<float> deployStages;
    private bool deployed;
    public float donutCircumference;
    private Transform modelTransform;
    private float deployTimer = 0;

    // debug
    public GameObject target;

    void Start()
    {
        // get values from manager
        // health
        // damage
        // fireRate
        // rollSpeed
        // turnSpeed
        // attackRange
        // deployTime
        findCircumference();
        modelTransform = GetComponentsInChildren<Transform>()[1];
        changeColor(Color.white);
    }

    void Update()
    {
        if(deployed)
        {
            deploySequence();
        }
        else
        {
            roll();
        }
    }

    void roll()
    {
        // find direction to target
        Vector3 direction = (target.transform.position - transform.position).normalized;
        direction.y = 0;

        // rotate parent to look at target
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        // A wheel moves forward a distance equal to its circumference with each rotation.
        modelTransform.Rotate(new Vector3(0, rollSpeed * 360 / donutCircumference, 0) * Time.deltaTime);

        // roll forward
        transform.Translate(transform.forward * rollSpeed * Time.deltaTime, Space.World);

        // look for player
        // deploy if within attackRange
        if((transform.position - target.transform.position).magnitude < Mathf.Pow(attackRange, 2))
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
        // increase deployTimer
        deployTimer += Time.deltaTime;

        if (deployTimer < deployStages[0]) // number of seconds of fall over animation
        {
            // play fall over animation
            changeColor(Color.red);
            Debug.Log("falling over");
        }
        else if(deployTimer < deployStages[1])  // deployed
        {
            // shoot at player
            changeColor(Color.green);
            Debug.Log("deployed and shooting");
        }
        else if(deployTimer < deployStages[2]) // number of frames of get up animation
        {
            // play get up animation
            changeColor(Color.blue);
            Debug.Log("getting up");
        }
        else
        {
            // no longer deployed
            // resume movement
            deployed = false;
            deployTimer = 0;
            changeColor(Color.white);
            Debug.Log("moving to player");
        }
    }

    void changeColor(Color newColor)
    {
        MeshRenderer[] childMeshes = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer currentMesh in childMeshes)
        {
            currentMesh.material.SetColor("_Color", newColor);
        }
    }
}