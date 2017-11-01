using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIProperty : MonoBehaviour
{
    public enum UIPropertyType
    {
        NONE,
        TEXT,
        IMAGE,
        BUTTON,
        OBJECT,
    }
    // :: variables
    public UIPropertyType type;
    public GameEvent gameEvent;
    // :: initializers
    // :: class functions
}