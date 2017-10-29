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
    // variables
    private float elapsedTime = 0.0f;
    [HideInInspector]
    public EnemySpawner spawner = null;
    // class functions [UnityEngine.MonoBehaviour]
    void OnGUI()
    {
        // check spawner status
        if (spawner == null) return;
        if (!spawner.IsStageComplete())
        {
            // draw box
            GUI.Box(new Rect(0, 0, 220, 180), "");
            // draw stage status
            GUI.Label(new Rect(10, 20, 200, 20), "-------------------- Stage --------------------");
            GUI.Label(new Rect(10, 40, 200, 20), "is complete? = " + spawner.IsStageComplete().ToString());
            // draw stage wave status
            GUI.Label(new Rect(10,  60, 200, 20), "---------------- Current  Wave ----------------");
            GUI.Label(new Rect(10,  80, 200, 20), "is empty? = " + spawner.IsWaveEmpty().ToString());
            GUI.Label(new Rect(10, 100, 200, 20), "is complete? = " + spawner.IsWaveComplete().ToString());
            GUI.Label(new Rect(10, 120, 200, 20), "spawn rate = " + spawner.GetWaveSpawnRate());
            GUI.Label(new Rect(10, 140, 200, 20), "active units = " + spawner.GetWaveActiveUnits());
            GUI.Label(new Rect(10, 160, 200, 20), "----------------------------------------------");
        }
    }
    void Update()
    {
        // check spawner status
        if (spawner == null) return;
        if (!spawner.IsStageComplete())
        {
            // check wave status
            if (spawner.IsWaveComplete())
            {
                // next wave
                elapsedTime = Time.time;
                spawner.HandleEvent(GameEvent.ENEMY_WAVE_NEXT);
            }
            else
            {
                // check elapsed time
                if (Time.time - elapsedTime > spawner.GetWaveSpawnRate())
                {
                    // next unit
                    elapsedTime = Time.time;
                    spawner.HandleEvent(GameEvent.ENEMY_DIED);
                    spawner.HandleEvent(GameEvent.ENEMY_SPAWN); // ###### REMOVE THIS LINE AFTER ###### //
                }
            }
        }
    }
    void OnDrawGizmos()
    {
        // check spawner status
        if (spawner == null) return;
        // display current stage
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        foreach (Vector3 point in spawner.enemyStage.points) Gizmos.DrawSphere(point + spawner.transform.position, 0.5f);
    }
    // class functions [GameEventListener]
    public override void HandleEvent(GameEvent e, float value)
    {
        
    }
    public override void HandleEvent(GameEvent e, Object value)
    {
        switch(e)
        {
            // called on trigger enter
            case GameEvent.CLASS_TYPE_ENEMY_SPAWNER:
                spawner = (EnemySpawner)value;
                spawner.Begin();
                break;
        }
    }
}