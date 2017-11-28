using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// opens and closes doors
public class doorScript : MonoBehaviour
{
    [Tooltip("how far up to move when opening")]
    public float openHeight;

    [Tooltip("time in seconds that it takes for the door to open / close")]
    public float openCloseTime = 1.0f;

    // points to move between
    private Vector3[] points = new Vector3[2];

    [Tooltip("When the door has closed this many times it will never open again")]
    public int closeCount = 1;

    // whether the door is open
    public bool open = false;

    // current close factor
    private float currentFactor = 0.0f;

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
            currentFactor = Mathf.Min(currentFactor + Time.deltaTime / openCloseTime, 1.0f);
        }
        else
        {
            currentFactor = Mathf.Max(currentFactor - Time.deltaTime / openCloseTime, 0.0f);
        }

        // update position
        transform.position = Vector3.Lerp(points[0], points[1], currentFactor);
    }

    public void openDoor()
    {
        if(closeCount > 0)
        {
            open = true;
        }
    }

    public void closeDoor()
    {
        open = false;
        closeCount--;
    }
}
