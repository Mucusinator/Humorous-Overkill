using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KILL : MonoBehaviour {

	void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "Enemy")
        {
            EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(EnemyManager), GameEvent.ENEMY_SPAWNER_REMOVE);
            Destroy(c.gameObject);
        }
    }
}
