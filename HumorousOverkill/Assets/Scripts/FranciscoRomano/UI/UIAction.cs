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
        GetEventListener("uimanager").HandleEvent(type);
    }
}