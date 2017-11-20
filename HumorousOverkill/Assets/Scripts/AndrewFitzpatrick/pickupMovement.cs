using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupMovement : MonoBehaviour
{
    public float spinSpeed;
    private float currentRotation;

    public float floatSpeed;
    public float floatMagnitude;

    private Vector3 startPos;
	
    void Start()
    {
        startPos = transform.position;
    }

	// Update is called once per frame
	void Update ()
    {
        // float
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;

        // spin
        currentRotation += spinSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
	}
}
