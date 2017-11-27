using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]

public class EndCheck : MonoBehaviour
{
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
            EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(PlayerManager), GameEvent.STATE_WIN_SCREEN);
        }
    }

}
