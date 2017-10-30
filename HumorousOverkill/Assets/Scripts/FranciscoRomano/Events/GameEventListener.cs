using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameEventListener : MonoBehaviour
{
    private GameEventListener m_listener = null;
    
    void Awake ()
    {
        // fetch parent listener
        GameEventListener listener = GetComponentInParent<GameEventListener>();
        // check if different
        if (listener != this)
        {
            // set parent listener
            m_listener = listener;
        }
    }
    
    public void SendEvent(GameEvent e)
    {
        // check if exists
        if (m_listener != null)
        {
            // handle event in parent
            m_listener.HandleEvent(e);
        }
    }

    public virtual void HandleEvent (GameEvent e)
    {
        // handle event here
        Debug.LogWarning("GameEventListener :: Please override 'void HandleEvent(GameEvent e)' function!");
    }

    public virtual void HandleEvent(GameEvent e, float value)
    {
        // handle event here
        Debug.LogWarning("GameEventListener :: Please override 'void HandleEvent(GameEvent e, float value)' function!");
    }

    public virtual void HandleEvent(GameEvent e, Object value)
    {
        // handle event here
        Debug.LogWarning("GameEventListener :: Please override 'void HandleEvent(GameEvent e, Object value)' function!");
    }
}
