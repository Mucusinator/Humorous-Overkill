using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]

public class EndCheck : MonoBehaviour
{
    public int amount;
    public GameEvent type;
    public EnemySpawner spawner;
	
	void Start ()
    {
        // set collider as trigger
        GetComponent<BoxCollider>().isTrigger = true;
	}

    void OnTriggerStay(Collider collider)
    {
        // check if player
        if (collider.tag == "Player" && spawner.IsStageComplete())
        {

            EventManager<GameEvent>.InvokeGameState(this, null, amount, typeof(PlayerManager), GameEvent.STATE_RESTART);

            // send event to player
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(type, amount);
            // destroy current game object
            //Destroy(gameObject);
        }
    }

}
