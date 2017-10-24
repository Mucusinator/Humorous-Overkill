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
    public float deployTime;
    private bool deployed;
    public float donutCircumference;
    private Transform modelTransform;

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
    }

    void Update()
    {
        roll();
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
}