using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIProperty : MonoBehaviour
{
    public enum Type
    {
        NONE,
        TEXT,
        IMAGE_ANIMATION,
    }
    // :: variables
    public Type type = Type.NONE;
    public GameEvent triggerEvent;
    public List<Sprite> sprites = new List<Sprite>();
    // :: functions
    public void UpdateComponent(float amount)
    {
        switch (type)
        {
            case Type.TEXT:
                UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text>();
                text.text = amount.ToString();
                break;
            case Type.IMAGE_ANIMATION:
                UnityEngine.UI.Image imageA = GetComponent<UnityEngine.UI.Image>();
                imageA.sprite = sprites[(int)Mathf.Clamp((sprites.Count  - 1) * amount, 0, sprites.Count - 1)];
                break;
        }
    }
}