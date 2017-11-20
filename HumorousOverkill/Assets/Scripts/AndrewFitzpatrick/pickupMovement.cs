using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupMovement : MonoBehaviour
{
    public float rotationSpeed;
    private float currentRotation;

    public float floatSpeed;
    public float floatMagnitude;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // float
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;

        // spin
        currentRotation += rotationSpeed * Time.deltaTime;

        // this will prevent errors from happening after a LONG time
        currentRotation %= 360;

        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }
}