using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : GameEventListener
{
    public void Test()
    {

    }

    public void HandleTriggerEvent(GameEvent e)
    {
        HandleEvent(e);
    }

    override public void HandleEvent(GameEvent e)
    {
        switch (e)
        {
            // handle game states
            case GameEvent.GAME_STATE_MENU:
                break;
            case GameEvent.GAME_STATE_START:
                break;
            case GameEvent.GAME_STATE_PAUSE:
                break;
            case GameEvent.GAME_STATE_RESTART:
                break;
            case GameEvent.GAME_STATE_CONTINUE:
                break;
            // default handle
            default:
                base.HandleEvent(e);
                break;
        }
    }
}