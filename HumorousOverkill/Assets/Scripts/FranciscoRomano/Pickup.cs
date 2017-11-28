using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider))]

public class Pickup : MonoBehaviour
{
    public int amount;
    public GameEvent type;
    public List<AudioClip> clips = new List<AudioClip>();
    private float previousTime = 0;
	
	void Awake()
    {
        // set collider as trigger
        GetComponent<BoxCollider>().isTrigger = true;
        foreach (AudioClip clip in clips)
        {
            FindObjectOfType<AudioManager>().AddSound(clip);
        }
        previousTime = Time.time;
	}

    void Update()
    {
        if ((Time.time - previousTime) > 30)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // check if player
        if (collider.tag == "Player")
        {
            FindObjectOfType<AudioManager>().PlaySound(clips[Random.Range(0, clips.Count)], false);
            EventManager<GameEvent>.InvokeGameState(this, null, amount, typeof(PlayerManager), type);

            // send event to player
            //GameObject.FindGameObjectWithTag("Manager").GetComponent<PlayerManager>().HandleEvent(type, amount);
            // destroy current game object
            Destroy(gameObject);
        }
    }

}
