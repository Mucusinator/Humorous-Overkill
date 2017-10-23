using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct GameInfo {
    public int m_lightIntensity;
    public Color m_LightColor;
}

[RequireComponent(typeof(PlayerManager))]
public class GameManager : GameEventListener {
    [SerializeField] GameInfo m_gameInfo;

    public override void HandleEvent (GameEvent e) {
        switch (e) {
        // Player
        case GameEvent.PICKUP_AMMO:
        case GameEvent.PICKUP_HEALTH:
            this.GetComponent<PlayerManager>().HandleEvent(e);
            break;
        default:
            break;
        }
    }

}
