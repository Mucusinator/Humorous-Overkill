using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public GameObject goal;

    [Tooltip("How long it takes for the platform to travel to and from the goal")]
    public float[] travelTimes = new float[2];

    [Tooltip("Whether to draw line")]
    public bool drawLine = false;

    [Tooltip("Color of line")]
    public Color lineColor = Color.white;

    private Vector3[] points = new Vector3[2];

    // private stuff
    private float currentFactor = 0.0f;
    private bool hasPlayer = false;

    void Start()
    {
        // first point is the current position
        points[0] = transform.position;

        // second point is the goal
        points[1] = goal.transform.position;
    }

	void Update ()
    {
        // update factor
        if (hasPlayer)
        {
            currentFactor = Mathf.Min(currentFactor + Time.deltaTime / travelTimes[0], 1.0f);
        }
        else
        {
            currentFactor = Mathf.Max(currentFactor - Time.deltaTime / travelTimes[1], 0.0f);
        }

        // update position
        transform.position = Vector3.Lerp(points[0], points[1], currentFactor);
	}

    void OnTriggerStay(Collider other)
    {
        // parent Player
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player jumped on " + gameObject.name);
            other.gameObject.transform.parent = transform;
            hasPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // parent Player
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player jumped off " + gameObject.name);
            other.gameObject.transform.parent = null;
            hasPlayer = false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(drawLine)
        {
            // draw line toward goal
            Gizmos.color = lineColor;
            Gizmos.DrawLine(points[0], points[1]);
        }
    }
#endif
}
