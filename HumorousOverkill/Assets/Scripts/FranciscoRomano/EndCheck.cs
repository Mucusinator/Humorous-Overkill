using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]

public class EndCheck : MonoBehaviour
{
    public AudioClip clip;
    public EnemySpawner spawner;

    private bool isStageDone = false;
	
	void Start ()
    {
        // set collider as trigger
        GetComponent<BoxCollider>().isTrigger = true;
        //FindObjectOfType<AudioManager>().AddSound(clip);
	}

    private void Update () {
        if (spawner.IsStageComplete() && !isStageDone) {
            isStageDone = true;
            EventManager<GameEvent>.InvokeGameState(this, null, null, null, GameEvent.STATE_HIGHSCORE);
        }
    }

}
