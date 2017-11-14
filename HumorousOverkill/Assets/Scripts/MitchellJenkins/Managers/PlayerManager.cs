using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Struct holding all the player info
[System.Serializable] public struct PlayerInfo {
    public float m_playerHealth; // 100
    public float m_playerWalkSpeed; //10
    public float m_playerRunSpeed; // 15
    public float m_playerCrouchSpeed; // 5
    public float m_playerJumpHeight; // 2
    public int m_pickupHealthAmount; // 20
    public int m_pickupAmmoAmount; // 30
    public float m_cameraSensitivity; // 1
    public float m_cameraMinimumAngle; // -60
    public float m_cameraMaximumAngle; // 40

    // Type_1 = Shotgun
    // Type_2 = Laser Rainbow Gun 
    //public float m_gunFireRate_type1; // 2
    //public float m_gunFireRate_type2; // 10
    //public float m_gunDamage_type1; // 3
    //public float m_gunDamage_type2; // 1
    //public int m_gunMaxAmmo_type1; // 6
    //public int m_gunMaxAmmo_type2; // 700
    //public float m_gunReloadSpeed_type1;
    //public float m_gunReloadSpeed_type2;
}

[EventHandler.BindListener("Weapon", typeof(CombinedScript))]
public class PlayerManager : EventHandler.EventHandle {
    public Player m_ply;
    private bool isFirstPickup = true;

    // Weapon Script.
    public CombinedScript m_weapon;

    // Player info
    public PlayerInfo m_playerInfo;
    public PlayerInfo GetPlayerInfo { get { return m_playerInfo; } }

    // Override for the handle event system
    public override bool HandleEvent (GameEvent e, float value) {
        Debug.Log(e.ToString() + " :: " + value);
        switch (e) {
        case GameEvent.PICKUP_HEALTH:
            // calls a function add health to the player
            m_ply.AddHealth((int)value);
            break;
        case GameEvent.PICKUP_RIFLEAMMO:
                // Calls the add ammo function from the ammo script using the enum.
                if (!isFirstPickup)
                    m_weapon.maxRifleAmmo += (int)value;
                else {
                    m_weapon.currentRifleAmmo += (int)value;
                    isFirstPickup = false;
                    GetEventListener("Weapon").GetComponent<CombinedScript>().gunType = CombinedScript.GunType.RIFLE;
                }
                break;
            case GameEvent.PICKUP_SHOTGUNAMMO:
                m_weapon.maxShotgunAmmo += (int)value;
                break;
        default:
            break;
        } return true;
    }

}
