using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GameEventListener
{
    public GameObject m_playerStatsHealth = null;
    public GameObject m_playerStatsMaxAmmo = null;
    public GameObject m_playerStatsCurrentAmmo = null;

    public override void HandleEvent(GameEvent e)
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

    public override void HandleEvent(GameEvent e, float amount)
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