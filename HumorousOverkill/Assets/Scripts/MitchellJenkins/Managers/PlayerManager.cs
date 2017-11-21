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

public class PlayerManager : MonoBehaviour {
    public Player m_ply;
    private bool isFirstPickup = true;

    // Weapon Script.
    public CombinedScript m_weapon;

    // Player info
    public PlayerInfo m_playerInfo;
    public PlayerInfo GetPlayerInfo { get { return m_playerInfo; } }

    void Awake () {
        EventManager<GameEvent>.Add(HandleMessage);
    }
    void start () { 
        EventManager<GameEvent>.InvokeGameState(this, null, m_playerInfo, typeof(Player), GameEvent._NULL_);
    }

    public void HandleMessage (object s, __eArg<GameEvent> e) {
        if (s == (System.Object)this) return;
        switch (e.arg) {
        case GameEvent._NULL_:
            if (e.type == typeof(CombinedScript))
                m_weapon = (CombinedScript)s;
            break;
        case GameEvent.PICKUP_HEALTH:
            // calls a function add health to the player
            m_ply.AddHealth((int)e.value);
            break;
        case GameEvent.PICKUP_RIFLEAMMO:
            // Calls the add ammo function from the ammo script using the enum.
            if (!isFirstPickup)
                m_weapon.maxRifleAmmo += (int)e.value;
            else {
                Debug.Log(e.ToString() + " :: " + e.value);
                m_weapon.currentRifleAmmo += (int)e.value;
                isFirstPickup = false;
                m_weapon.gunType = CombinedScript.GunType.RIFLE;
            }
            break;
        case GameEvent.PICKUP_SHOTGUNAMMO:
            m_weapon.maxShotgunAmmo += (int)e.value;
            break;
        default:
            break;
        }
    }
}
