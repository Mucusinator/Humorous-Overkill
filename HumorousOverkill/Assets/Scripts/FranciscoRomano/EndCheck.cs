using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]

public class EndCheck : MonoBehaviour
{
    public AudioClip clip;
    public EnemySpawner spawner;
	
	void Start ()
    {
        // set collider as trigger
        GetComponent<BoxCollider>().isTrigger = true;
        FindObjectOfType<AudioManager>().AddSound(clip);
	}

    void OnTriggerStay(Collider collider)
    {
        // check if player
        if (collider.tag == "Player" && spawner.IsStageComplete())
        {
            FindObjectOfType<AudioManager>().StopMusic(0);
            FindObjectOfType<AudioManager>().PlaySound(clip, false);
            EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(PlayerManager), GameEvent.STATE_WIN_SCREEN);
        }
    }

}
