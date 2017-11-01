using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Struct holding all the player info
[System.Serializable] public struct PlayerInfo {
    public float m_playerHealth; // 100
    public float m_playerSpeed; //10
    public float m_playerJumpHeight; // 2
    public int m_pickupHealthAmount; // 20
    public int m_pickupAmmoAmount; // 30

    // Type_1 = Shotgun
    // Type_2 = Laser Rainbow Gun 
    public float m_gunFireRate_type1; // 2
    public float m_gunFireRate_type2; // 10
    public float m_gunDamage_type1; // 3
    public float m_gunDamage_type2; // 1
    public int m_gunMaxAmmo_type1; // 6
    public int m_gunMaxAmmo_type2; // 700
    public float m_gunReloadSpeed_type1;
    public float m_gunReloadSpeed_type2;
}

public class PlayerManager : EventHandler.EventHandle {
    public Player m_ply;

    // Weapon Script.
    public CombinedScript m_weapon;

    // Player info
    public PlayerInfo m_playerInfo;
    public PlayerInfo GetPlayerInfo { get { return m_playerInfo; } }

    // Override for the handle event system
    public override bool HandleEvent (GameEvent e, float value) {
        switch (e) {
        case GameEvent.PICKUP_HEALTH:
            // calls a function add health to the player
            m_ply.AddHealth((int)value);
            break;
        case GameEvent.PICKUP_AMMO:
                // Calls the add ammo function from the ammo script using the enum.
                m_weapon.currentRifleAmmo += (int)value;
                m_weapon.currentShotgunAmmo += (int)value;
                break;
        default:
            break;
        } return true;
    }

}
