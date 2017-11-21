using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Popup : MonoBehaviour
{
    public int index = 0;
    public bool fadeIn = true;
    public bool running = false;
    public float fadeInSpeed = 0.1f;
    public float fadeOutDelay = 5.0f;
    public float fadeOutSpeed = 1.0f;
    public Sprite[] sprites;
    public UnityEngine.UI.Image image;
    private float currentTime = 0;

	void Start ()
    {
        EventManager<GameEvent>.Add(HandleMessage);
	}

    void Update()
    {
        if (!running) return;
        if (fadeIn)
        {
            if (image.color.a < 1)
            {
                Color color1 = image.color;
                color1.a = Mathf.Min(color1.a + Time.deltaTime * fadeInSpeed, 1);
                image.color = color1;
                currentTime = Time.time;
            }
            else
            {
                if (currentTime < (Time.time + fadeOutDelay))
                {
                    fadeIn = false;
                };
            }
        }
        else
        {
            if (image.color.a > 0)
            {
                Color color1 = image.color;
                color1.a = Mathf.Max(color1.a - Time.deltaTime * fadeOutSpeed, 0);
                image.color = color1;
            }
            else
            {
                fadeIn = true;
                NextSprite();
            }
        }
    }

    void ResetAll()
    {
        index = 0;
        fadeIn = true;
        running = false;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
        image.sprite = sprites[index++];
    }

    void NextSprite()
    {
        if (index < sprites.Length)
        {
            image.sprite = sprites[index++];
        }
        else
        {
            running = false;
        }
    }

    public void HandleMessage(object sender, __eArg<GameEvent> e)
    {
        if (sender == (object)this) return;
        switch (e.arg)
        {
            case GameEvent.STATE_START:
            case GameEvent.STATE_RESTART:
                ResetAll();
                break;
        }        
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "player") running = true;
    }
}