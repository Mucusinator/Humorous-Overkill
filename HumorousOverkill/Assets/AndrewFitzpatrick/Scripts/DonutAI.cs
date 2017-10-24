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
	}
	
	void Update ()
    {
        roll();
	}

    void roll()
    {
        transform.Rotate(new Vector3(0, rollSpeed, 0) * Time.deltaTime);
        transform.Translate(transform.right * rollSpeed * Time.deltaTime);
    }
}
