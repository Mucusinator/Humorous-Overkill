using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventAction : MonoBehaviour
{
    public GameEvent action;
    
    public void SendEvent()
    {
        // send event to parent listener
        GetComponentInParent<GameEventListener>().HandleEvent(action);
    }
}