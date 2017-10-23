using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunShoot : MonoBehaviour {

    // The amount of pellets shot from the shotgun.
    public int pelletCount = 8;

    // The player camera. we use this for the start point of the raycast.
    public Camera fpsCam;
    // The damage of each pellet.
    public float pelletDamage = 8f;
    // The force each pellet has when it hits a target.
    public float impactForce = 30.0f;
    
    
    //This controls the spread width of the cone.
    public float spreadWidth = 2f;
    // This controls the range of the cone.
    public float range = 10f;

    


    void Update()
    {


        // If the player holds the fire button, for the amount of pellets, shoot a raycast.
        if (Input.GetButtonDown("Fire2"))
        {
            for (int i = 0; i < pelletCount; ++i)
            {
                ShootRay();
            }
        }
    }

    void ShootRay()
    {
        //  Try this one first, before using the second one
        //  The Ray-hits will form a ring
        float randomRadius = spreadWidth;
        //  The Ray-hits will be in a circular area
        randomRadius = Random.Range(0, spreadWidth);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle),
            range
        );

        //Make the direction match the transform
        //It is like converting the Vector3.forward to transform.forward
        direction = fpsCam.transform.TransformDirection(direction.normalized);

        //Raycast and debug
         Ray r = new Ray(fpsCam.transform.position, direction);
       
        // the object that gets hit from the raycast.
        RaycastHit hit;
        if (Physics.Raycast(r, out hit))
        {


            Debug.DrawLine(fpsCam.transform.position, hit.point, Color.black, 3.0f);


            Target shotgunTarget = hit.transform.GetComponent<Target>();
            if (shotgunTarget != null)
            {
                shotgunTarget.TakeDamage(pelletDamage);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }
    }
}
