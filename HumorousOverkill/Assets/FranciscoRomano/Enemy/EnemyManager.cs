using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Struct holding all the Game info
[System.Serializable] struct EnemyInfo
{
    public int m_enemyHealth_type1;
    public int m_enemyHealth_type2;
    public float m_enemySpeed_type1;
    public float m_enemySpeed_type2;
    public float m_enemyDamage_type1;
    public float m_enemyDamage_type2;
    public int m_enemyAmmoDropRate;
}

public class EnemyManager : GameEventListener
{

    public override void HandleEvent(GameEvent e)
    {
        switch(e)
        {
            default:
                break;
        }
    }

    public override void HandleEvent(GameEvent e, Object value)
    {
        switch(e)
        {
            default:
                break;
        }
    }
}