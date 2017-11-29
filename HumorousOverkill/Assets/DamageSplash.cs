using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSplash : MonoBehaviour
{
    public Sprite splash1;
    public Sprite splash2;
    public Sprite splash3;
    public float fadeSpeed = 5;
    private UnityEngine.UI.Image imageSource;

	void Awake()
    {
        EventManager<GameEvent>.Add(HandleMessage);
        fadeSpeed = 1 / fadeSpeed;
        imageSource = GetComponent<UnityEngine.UI.Image>();
        imageSource.color = new Color(imageSource.color.r, imageSource.color.g, imageSource.color.b, 0);
    }

    public void HandleMessage(object sender, __eArg<GameEvent> e)
    {
        switch (e.arg)
        {
            case GameEvent.UI_HEALTH:
                // update image
                if ((float)e.value < 0.55f)
                {
                    if ((float)e.value < 0.35f)
                    {
                        if ((float)e.value < 0.15f) imageSource.sprite = splash3;
                        else imageSource.sprite = splash2;
                    }
                    else imageSource.sprite = splash1;
                }
                // update fade effect
                float delta = (float)e.value < 0.55f ? 1 : -1;
                float alpha = Mathf.Clamp(fadeSpeed * delta * Time.deltaTime, 0, 1);
                imageSource.color = new Color(imageSource.color.r, imageSource.color.g, imageSource.color.b, alpha);
                break;
        }
    }
	 
}