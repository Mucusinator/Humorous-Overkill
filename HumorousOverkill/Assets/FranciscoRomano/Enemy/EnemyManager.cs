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
    
    void OnGUI()
    {
        if (spawner == null) return;
        if (!spawner.stage.isComplete())
        {
            GUI.Box(new Rect(0, 0, 420, 180), "");
            // stage status
            GUI.Label(new Rect(10, 20, 400, 20), "---------------------------------------- Stage ----------------------------------------");
            GUI.Label(new Rect(10, 40, 400, 20), "is complete? = " + spawner.stage.isComplete().ToString());
            // current wave status
            GUI.Label(new Rect(10, 60, 400, 20), "------------------------------------ Current  Wave ------------------------------------");
            GUI.Label(new Rect(10, 80, 400, 20), "is empty? = " + spawner.stage.wave.isEmpty().ToString());
            GUI.Label(new Rect(10, 100, 400, 20), "is complete? = " + spawner.stage.wave.isComplete().ToString());
            GUI.Label(new Rect(10, 120, 400, 20), "spawn rate = " + spawner.stage.wave.spawnRate);
            GUI.Label(new Rect(10, 140, 400, 20), "active units = " + spawner.stage.wave.activeUnits);
            GUI.Label(new Rect(10, 160, 400, 20), "---------------------------------------------------------------------------------------");
        }
    }

    void Update()
    {
        // check if null
        if (spawner == null) return;
        // check if complete
        if (spawner.stage.isComplete()) return;
        // check if wave complete
        if (spawner.stage.isWaveComplete())
        {
            // next wave
            elapsedTime = Time.time;
            spawner.HandleEvent(GameEvent.ENEMY_WAVE_NEXT);
        }
        else
        {
            if (Time.time - elapsedTime > spawner.stage.getWaveSpawnRate())
            {
                // next unit
                elapsedTime = Time.time;
                spawner.HandleEvent(GameEvent.ENEMY_DIED);
                spawner.HandleEvent(GameEvent.ENEMY_SPAWN);
            }
        }
        
    }

    public override void HandleEvent(GameEvent e, Object value)
    {
        switch(e)
        {
            case GameEvent.CLASS_TYPE_ENEMY_SPAWNER:
                Debug.Log("Hello!");
                spawner = (EnemySpawner)value;
                break;
        }
    }
}