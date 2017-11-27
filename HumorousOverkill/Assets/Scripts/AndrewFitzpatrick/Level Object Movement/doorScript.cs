using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// opens and closes doors
public class doorScript : MonoBehaviour
{
    public GameObject openPoint;
    public EnemySpawner attachedWave;
    private Vector3[] points = new Vector3[2];
    bool open = false;

    private float currentFactor = 0.0f;

    void Start()
    {
        // setup points
        points[0] = transform.position;
        points[1] = openPoint.transform.position;
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

	void OnTriggerEnter(Collider other)
    {
        // when the player enters the door trigger
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("player has entered the door");
            // the wave is finished so open
            if(attachedWave.IsGroupComplete())
            {
                Debug.Log("Wave is complete");
                // the wave is complete
                open = true;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // when the player exits the door trigger
        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<Player>() != null)
        {
            Debug.Log("player has exited the door");
            open = false;
        }
    }
}
