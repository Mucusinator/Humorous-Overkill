using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct GameInfo {
    public int m_lightIntensity;
    public Color m_LightColor;
}

[RequireComponent(typeof(PlayerManager))]
public class GameManager : MonoBehaviour {
    [SerializeField] GameInfo m_gameInfo;
    [SerializeField] PlayerInfo m_playerInfo;
}
