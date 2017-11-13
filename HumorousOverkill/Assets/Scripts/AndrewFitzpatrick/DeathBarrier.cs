using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DeathBarrier : MonoBehaviour
{
    void Awake()
    {
        // make sure the collider is a trigger
        GetComponent<BoxCollider>().isTrigger = true;
    }

	void OnTriggerEnter(Collider other)
    {
        // player has hit the death barrier
        if(other.gameObject.GetComponent<Player>() != null)
        {
            // send damage equal to the current health (guaranteed to kill) 
            float playerHealth = other.gameObject.GetComponent<Player>().m_ply.m_playerHealth;
            Debug.Log("Player has hit the respawn barrier. Sending " + playerHealth + " damage.");
            other.gameObject.GetComponent<Player>().HandleEvent(GameEvent.PLAYER_DAMAGE, playerHealth);
        }
    }
}
