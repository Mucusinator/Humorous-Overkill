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
	}
	
	void Update ()
    {
        roll();
	}

    void roll()
    {
        transform.Rotate(new Vector3(0, 0, rollSpeed * 360 / donutCircumference) * Time.deltaTime);
        transform.Translate(-Vector3.right * rollSpeed * Time.deltaTime, Space.World);
    }

    void findCircumference()
    {
        BoxCollider donutCollider = GetComponentInChildren<BoxCollider>();

        float size = donutCollider.size.x;
        Debug.Log("the diameter of the donut is " + size);

        donutCircumference = (size * Mathf.PI * transform.localScale.y);
    }
}
