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
    Image imageSource = null;
    AudioSource source = null;
    public AudioClip clip = null;
    public List<Sprite> sprites = new List<Sprite>();

    // :: functions
    void Start()
    {
        Begin();
        //source = AddComponent<AudioSource>();
        //imageSource = AddComponent<Image>();
    }
    void Update()
    {
        if (!running) return;
        if (!complete && index < sprites.Count)
        {
            if ((Time.time - passedTime) > spriteDelay)
            {
                imageSource.sprite = sprites[index++];
            }
        }
        else
        {
            source.Stop();
            running = false;
            complete = true;
            // send event again to manager
        }
    }
    public bool IsComplete()
    {
        return complete == true;
    }
    public void Begin()
    {
        if (running) return;
        index = 0;
        source.Stop();
        source.clip = clip;
        source.loop = false;
        source.playOnAwake = false;
        source.Play();
        running = true;
        complete = false;
        passedTime = Time.time;
        spriteDelay = clip.length / sprites.Count;
        imageSource.sprite = sprites[index++];
    }
}