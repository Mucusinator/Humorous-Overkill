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

    public override bool HandleEvent (GameEvent e) {
        switch (e) {
        // UI
        case GameEvent.GAME_STATE_CONTINUE:
        case GameEvent.GAME_STATE_MENU:
        case GameEvent.GAME_STATE_PAUSE:
        case GameEvent.GAME_STATE_RESTART:
        case GameEvent.GAME_STATE_START:
            this.GetComponent<UIManager>().HandleEvent(e);
            break;
        // Player
        case GameEvent.PICKUP_AMMO:
        case GameEvent.PICKUP_HEALTH:
            this.GetComponent<PlayerManager>().HandleEvent(e);
            break;
        default:
            break;
        } return true;
    }

}
