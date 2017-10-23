using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct PlayerInfo {
    public int m_playerHealth;
    public float m_playerSpeed;
    public float m_playerJumpHeight;
    public int m_pickupHealthAmount;
    public int m_pickupAmmoAmount;
    public float m_gunFireRate_type1;
    public float m_gunFireRate_type2;
    public float m_gunDamage_type1;
    public float m_gunDamage_type2;
    public int m_gunMaxAmmo;
}

public class PlayerManager : GameEventListener {
    public Player m_ply;

    // Weapon Script.
    //public Weapon m_weapon;
    [SerializeField] PlayerInfo m_playerInfo;

    public override void HandleEvent (GameEvent e) {
        switch (e) {
        case GameEvent.PICKUP_HEALTH:
            m_ply.AddHealth(m_playerInfo.m_pickupHealthAmount);
            break;
        case GameEvent.PICKUP_AMMO:
            // Calls the add ammo function from the ammo script using the enum.
            //m_weapon.AddAmmo(m_playerInfo.m_pickupAmmoAmount);
            break;
        default:
            break;
        }
    }

}
