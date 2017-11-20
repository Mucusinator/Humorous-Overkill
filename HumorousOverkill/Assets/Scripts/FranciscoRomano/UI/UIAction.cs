using UnityEngine;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

[BindListener("uimanager", typeof(UIManager))]
public class UIAction : EventHandle
{
    public GameEvent type;

    public void SendEvent()
    {
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), type);
        //GetEventListener("uimanager").HandleEvent(type);
    }
}