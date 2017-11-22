﻿using UnityEngine;

// Struct holding all the Game info
[System.Serializable] struct GameInfo {
    public int m_lightIntensity;
    public Color m_LightColor;
    public Color m_UIColor;
}

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(EnemyManager))]
public class GameManager : MonoBehaviour {
    [SerializeField] GameInfo m_gameInfo;

    Loading m_loading;

    void Awake () {
        EventManager<GameEvent>.Add(HandleMessage);
    }
    void Start () {
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent._NULL_);
        EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(UIManager), GameEvent.STATE_MENU);
    }

    public void HandleMessage(object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;
        switch (e.arg) {
        case GameEvent.STATE_MENU:
        case GameEvent.STATE_PAUSE:
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (e.type == GetType())
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        case GameEvent.STATE_START:
        case GameEvent.STATE_RESTART:
        case GameEvent.STATE_CONTINUE:
            if (m_loading.IsComplete())
            {
                m_loading.gameObject.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                if (e.type == GetType())
                    EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            }
            break;
        case GameEvent.PICKUP_RIFLEAMMO:
        case GameEvent.PICKUP_SHOTGUNAMMO:
        case GameEvent.PICKUP_HEALTH:
            if (e.type == GetType())
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        case GameEvent.DIFFICULTY_EASY:
        case GameEvent.DIFFICULTY_MEDI:
        case GameEvent.DIFFICULTY_HARD:
        case GameEvent.DIFFICULTY_NM:
            EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        case GameEvent._NULL_:
            if (e.type == typeof(Loading)) {
                m_loading = (Loading)s;
            }
            break;
        }
    }
}
