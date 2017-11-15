using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movePlatform : MonoBehaviour
{
    public GameObject startPoint;
    public GameObject endPoint;

    public float travelSpeed;

    private Vector3[] points = new Vector3[2];
    private float currentFactor = 0.0f;
    private bool returning = false;

    void Start()
    {
        // set points
        points[0] = startPoint.transform.position;
        points[1] = endPoint.transform.position;
    }

	void Update ()
    {
        // update factor
        currentFactor += Time.deltaTime / travelSpeed * (returning ? -1.0f : 1.0f);

        // ping pong
        if(currentFactor >= 1.0f || currentFactor <- 0.0f)
        {
            returning = !returning;
        }

        // set position
        transform.position = Vector3.Lerp(points[0], points[1], currentFactor);
	}

    void OnTriggerStay(Collider other)
    {
        // parent Player
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("Player jumped on " + gameObject.name);
            other.gameObject.transform.parent = transform;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // parent Player
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player jumped off " + gameObject.name);
            other.gameObject.transform.parent = null;
        }
    }
}
