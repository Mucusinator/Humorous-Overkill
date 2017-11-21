using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]

public class Pickup : MonoBehaviour
{
    public float amount;
    public GameEvent type;
	
	void Start ()
    {
        // set collider as trigger
        GetComponent<BoxCollider>().isTrigger = true;
	}

    void OnTriggerEnter(Collider collider)
    {
        // check if player
        if (collider.tag == "Player")
        {

            EventManager<GameEvent>.InvokeGameState(this, null, amount, typeof(PlayerManager), type);

            // send event to player
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(type, amount);
            // destroy current game object
            Destroy(gameObject);
        }
    }

}
