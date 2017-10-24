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
    public Vector3 target;

    void Start ()
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

    void Update ()
    {
        roll();
	}

    void roll()
    {
        Vector3 direction = (target - transform.position).normalized;
        modelTransform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        modelTransform.Rotate(new Vector3(0, rollSpeed * 360 / donutCircumference, 0) * Time.deltaTime);
        transform.Translate(direction * rollSpeed * Time.deltaTime, Space.World);
    }

    void findCircumference()
    {
        BoxCollider donutCollider = GetComponentInChildren<BoxCollider>();

        float size = donutCollider.size.x;
        Debug.Log("the diameter of the donut is " + size);

        donutCircumference = (size * Mathf.PI * transform.localScale.y);
    }
}
