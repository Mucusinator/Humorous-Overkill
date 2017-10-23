using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shotgunShoot : MonoBehaviour {

    public float variance = 1.0f;  // This much variance 
    public float distance = 50.0f; // at this distance
    public int pelletCount = 8;
    public Camera fpsCam;
    public float range = 50.0f;
    public float pelletDamage = 8f;
    public float impactForce = 30.0f;
    //These 2 controls the spread of the cone
    public float scaleLimit = 60f;
    public float z = 10f;

    // Update is called once per frame
    //void Update () {
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        for (var i = 0; i < pelletCount; i++)
    //        {
    //            var v3Offset = transform.up * Random.Range(0.0f, variance);
    //            v3Offset = Quaternion.AngleAxis(Random.Range(0.0f, 360.0f), transform.forward) * v3Offset;
    //            var v3Hit = transform.forward + v3Offset;
    //            RaycastHit hitShotgun;
    //             if(Physics.Raycast(fpsCam.transform.position, v3Hit, out hitShotgun, distance))
    //            {
    //                Debug.Log(hitShotgun.transform.name);
    //            Target shotgunTarget = hitShotgun.transform.GetComponent<Target>();
    //            // Position an object to test pattern
    //            //var tr = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
    //            //tr.localScale = new Vector3(0.1f, 0.1f, 0.1f);

    //                if (shotgunTarget != null)
    //                {
    //                    shotgunTarget.TakeDamage(pelletDamage);
    //                }
    //            }
    //        }
    //    }

    //}


    void Update()
    {

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
        float randomRadius = scaleLimit;
        //  The Ray-hits will be in a circular area
        randomRadius = Random.Range(0, scaleLimit);

        float randomAngle = Random.Range(0, 2 * Mathf.PI);

        //Calculating the raycast direction
        Vector3 direction = new Vector3(
            randomRadius * Mathf.Cos(randomAngle),
            randomRadius * Mathf.Sin(randomAngle),
            z
        );

        //Make the direction match the transform
        //It is like converting the Vector3.forward to transform.forward
        direction = fpsCam.transform.TransformDirection(direction.normalized);

        //Raycast and debug
         Ray r = new Ray(fpsCam.transform.position, direction);
        //Ray r = new Ray(fpsCam.transform.position, fpsCam.transform.forward);
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
