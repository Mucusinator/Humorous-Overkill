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
    bool isDynamic = true;
    public Type type = Type.NONE;
    public GameEvent triggerEvent;
    public List<Sprite> sprites;
}