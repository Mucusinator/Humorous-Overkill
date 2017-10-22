using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct PlayerInfo {
    int m_playerHealth;
    float m_playerSpeed;
    float m_playerJumpHeight;
    int m_pickupHealthAmount;
    int m_pickupAmmoAmount;
    float m_gunFireRate_type1;
    float m_gunFireRate_type2;
    float m_gunDamage_type1;
    float m_gunDamage_type2;
    int m_gunMaxAmmo;
}

public class GameManager : MonoBehaviour {
    [SerializeField] PlayerInfo plyinfo;
}
