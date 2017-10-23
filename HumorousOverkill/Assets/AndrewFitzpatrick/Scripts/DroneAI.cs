using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DroneAI : MonoBehaviour
{
    // wandering
    [Header("Wander Behavior")]
    public float wanderRadius;
    public float wanderJitter;
    public float wanderDistance;
    private Vector3 previousTarget;

	void Update ()
    {
        // target random point on wander radius
        Vector3 target = getRandomVector(wanderRadius);

        // add jitter and renormalize to wander radius
        target += getRandomVector(wanderJitter);
        target.Normalize();
        target *= wanderRadius;

        target += transform.forward.normalized * wanderDistance;

        transform.Translate(target * Time.deltaTime);
	}

    Vector3 getRandomVector(float radius)
    {
        Vector2 vec2D = Random.insideUnitCircle.normalized * radius;
        return new Vector3(vec2D.x, 0, vec2D.y);
    }

    void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.up, wanderRadius);
        UnityEditor.Handles.color = Color.yellow;
        UnityEditor.Handles.DrawWireDisc(transform.position + Vector3.forward * wanderRadius, Vector3.up, wanderJitter);
    }
}
