using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnCollision : MonoBehaviour
{
    // destroys a gameobject when it collides with anything
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag != "Enemy" && other.gameObject.tag != "Manager")
        {
            Debug.Log(gameObject.name + " was destroyed when it collided with " + other.gameObject.name);
            Destroy(gameObject);
        }
    }
}
