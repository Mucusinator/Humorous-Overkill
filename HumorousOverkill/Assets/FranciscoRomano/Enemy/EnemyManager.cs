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
    private float elapsedTime = 0.0f;
    public EnemySpawner spawner = null;

    void Start()
    {
        elapsedTime = Time.time;
        spawner = GetComponentInChildren<EnemySpawner>();
    }

    void Update()
    {
        if (spawner.stage.isComplete())
        {
            Debug.Log("Finished!");
            return;
        }

        if (spawner.stage.isWaveComplete())
        {
            Debug.Log("wave");
            elapsedTime = Time.time;
            spawner.HandleEvent(GameEvent.ENEMY_WAVE_NEXT);
        }
        else
        {
            if (Time.time - elapsedTime > spawner.stage.getWaveSpawnRate())
            {
                Debug.Log("unit");
                elapsedTime = Time.time;
                if (spawner.stage.isWaveEmpty())
                {
                    spawner.HandleEvent(GameEvent.ENEMY_DIED);
                }
                else
                {
                    spawner.HandleEvent(GameEvent.ENEMY_SPAWN);
                }
            }
        }
        
    }

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