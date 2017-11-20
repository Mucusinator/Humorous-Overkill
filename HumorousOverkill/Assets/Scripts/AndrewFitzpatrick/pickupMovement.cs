using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupMovement : MonoBehaviour
{
    public float rotationSpeed;
    private float currentRotation;

    public float floatSpeed;
    public float floatMagnitude;
    public float floatHeightOffset;

    private Vector3 startPos;
    private Vector3 pivotPoint;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // float
        transform.position = startPos + Vector3.up * (floatHeightOffset + Mathf.Sin(Time.time * floatSpeed) * floatMagnitude);

        // spin
        currentRotation += rotationSpeed * Time.deltaTime;

        // this will prevent errors from happening after a LONG time
        currentRotation %= 360;

        transform.rotation = Quaternion.Euler(0, currentRotation, 0);
    }
}