using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DroneAI : MonoBehaviour
{
    // wandering
    [Header("Wander Behavior")]
    public float targetRadius;
    public float errorMargin;
    public float wanderSpeed;
    public float turnSpeed;
    public float avoidRadius;
    public bool showGizmos = false;
    private Vector3 currentTarget;
    private RaycastHit hitInfo;

    void Start()
    {
        pickTarget();
    }

	void Update ()
    {
        wander();
	}

    void wander()
    {
        // if we are within the margin of error pick a new target
        if ((transform.position - currentTarget).sqrMagnitude < Mathf.Pow(errorMargin, 2))
        {
            pickTarget();
        }

        // avoid walls
        //Debug.DrawRay(transform.position, transform.forward.normalized * avoidRadius, Color.cyan);
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, avoidRadius))
        {
            if (hitInfo.collider.gameObject.tag == "Avoid")
            {
                Debug.Log("I will avoid " + hitInfo.collider.gameObject.name);
                pickTarget();
            }
        }

        Vector3 direction = currentTarget - transform.position;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), turnSpeed * Time.deltaTime);

        //transform.LookAt(currentTarget);
        transform.Translate(transform.forward * wanderSpeed * Time.deltaTime, Space.World);
    }

    void pickTarget()
    {
        // make a good guess
        currentTarget = transform.position + getRandomVector(targetRadius);

        // TODO: avoid walls etc
    }

    Vector3 getRandomVector(float radius)
    {
        Vector2 vec2D = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vec2D.x, 0, vec2D.y);
    }

    void OnDrawGizmosSelected()
    {
        if(showGizmos)
        {
            // display targetRadius
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, targetRadius);

            // display margin of error
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, errorMargin);

            // show currentVelocity
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * wanderSpeed);

            // display current target
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(currentTarget, 0.5f);
        }
    }
}
