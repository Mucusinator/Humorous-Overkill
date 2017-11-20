using UnityEngine;

// Struct holding all the Game info
[System.Serializable] struct GameInfo {
    public int m_lightIntensity;
    public Color m_LightColor;
    public Color m_UIColor;
}

[RequireComponent(typeof(PlayerManager))]
[RequireComponent(typeof(EnemyManager))]
public class GameManager : EventHandler.EventHandle {
    [SerializeField] GameInfo m_gameInfo;

    void Start () {
        EventManager<GameEvent>.Add(HandleMessage);
    }

    public void HandleMessage(object s, __eArg<GameEvent> e) {
        if (s == (object)this) return;
        switch (e.arg) {
        case GameEvent.STATE_CONTINUE:
        case GameEvent.STATE_MENU:
        case GameEvent.STATE_PAUSE:
        case GameEvent.STATE_RESTART:
        case GameEvent.STATE_START:
            if (e.type == GetType())
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        case GameEvent.PICKUP_RIFLEAMMO:
        case GameEvent.PICKUP_SHOTGUNAMMO:
        case GameEvent.PICKUP_HEALTH:
            if (e.type == GetType())
                EventManager<GameEvent>.InvokeGameState(this, null, null, null, e.arg);
            break;
        }
    }

    public override bool HandleEvent (GameEvent e) {
        switch (e) {
        // UI
        case GameEvent.STATE_CONTINUE:
        case GameEvent.STATE_MENU:
        case GameEvent.STATE_PAUSE:
        case GameEvent.STATE_RESTART:
        case GameEvent.STATE_START:
            this.GetComponent<UIManager>().HandleEvent(e);
            break;
        // Player
        case GameEvent.PICKUP_RIFLEAMMO:

        case GameEvent.PICKUP_SHOTGUNAMMO:

        case GameEvent.PICKUP_HEALTH:
            this.GetComponent<PlayerManager>().HandleEvent(e);
            break;
        default:
            break;
        } return true;
    }

}
