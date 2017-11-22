using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Loading : MonoBehaviour
{
    // :: variables
    int index = 0;
    bool running = false;
    bool complete = false;
    float passedTime = 0;
    float spriteDelay = 0;
    public Image imageSource = null;
    public AudioSource source = null;
    public AudioClip clip = null;
    public List<Sprite> sprites = new List<Sprite>();

    // :: functions
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        imageSource = gameObject.GetComponent<Image>();
        //Begin();

        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(Loading), GameEvent._NULL_);
    }
    void Update()
    {
        if (!running) return;
        if (!complete)
        {
            if (index < sprites.Count)
            {

                if ((Time.time - passedTime) > spriteDelay)
                {
                    imageSource.sprite = sprites[index++];
                    passedTime = Time.time;
                }
            }
            else
            {
                complete = true;
            }
        }
        else
        {
            source.Stop();
            running = false;
            complete = true;
            // send event again to manager
            EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
        }
    }
    public bool IsComplete()
    {
        return complete == true;
    }
    public void Begin()
    {
        if (running) return;
        if (complete) return;
        index = 0;
        source.Stop();
        source.clip = clip;
        source.loop = false;
        source.playOnAwake = false;
        source.Play();
        running = true;
        complete = false;
        passedTime = Time.time;
        spriteDelay = clip.length / (float)sprites.Count;
        imageSource.sprite = sprites[index++];
    }
}