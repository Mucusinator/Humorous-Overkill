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
    public bool showGizmos = false;
    private Vector3 currentTarget;
    [SerializeField]
    private float turnTime = 0.0f;

	void Update ()
    {
        wander();
	}

    void wander()
    {
        // look at target and move forward
        Vector3 direction = (currentTarget - transform.position).normalized;
        Quaternion toRotation = Quaternion.FromToRotation(transform.forward, direction);

        turnTime = Mathf.Min(1.0f, turnTime + (1.0f / turnSpeed * Time.deltaTime);


        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnTime);
        transform.Translate(transform.forward * wanderSpeed * Time.deltaTime);

        // if we are within the margin of error pick a new target
        if((transform.position - currentTarget).sqrMagnitude < Mathf.Pow(errorMargin, 2))
        {
            pickTarget();
        }
    }

    void pickTarget()
    {
        currentTarget = transform.position + getRandomVector(targetRadius);

        // TODO: avoid walls etc

        turnTime = 0.0f;
    }

    Vector3 getRandomVector(float radius)
    {
        Vector2 vec2D = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vec2D.x, 0, vec2D.y);
    }

    void OnDrawGizmos()
    {
        if(showGizmos)
        {
            // display targetRadius
            UnityEditor.Handles.color = Color.red;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, targetRadius);

            // show currentVelocity
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawLine(transform.position, currentTarget);

            Gizmos.DrawWireSphere(currentTarget, 0.5f);
        }
    }
}
