using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// opens and closes doors
public class doorScript : MonoBehaviour
{
    public float openHeight;
    private Vector3[] points = new Vector3[2];

    // door can only open once
    private bool canOpen = true;
    public bool open = false;

    private float currentFactor = 0.0f;

    // which side is the entrance
    Vector3 entrance = Vector3.zero;

    void Start()
    {
        // setup points
        points[0] = transform.position;
        points[1] = transform.position + Vector3.up * openHeight;
    }

    void Update()
    {
        // update factor
        if (open)
        {
            currentFactor = Mathf.Min(currentFactor + Time.deltaTime, 1.0f);
        }
        else
        {
            currentFactor = Mathf.Max(currentFactor - Time.deltaTime, 0.0f);
        }

        // update position
        transform.position = Vector3.Lerp(points[0], points[1], currentFactor);
    }

    public void openDoor()
    {
        if(canOpen)
        {
            open = true;
        }
    }

    public void closeDoor()
    {
        open = false;
        canOpen = false;
    }
}
