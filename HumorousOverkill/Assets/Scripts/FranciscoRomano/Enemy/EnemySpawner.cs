using UnityEngine;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[BindListener("EnemyManager", typeof(EnemyManager))]
public class EnemySpawner : EventHandle
{
    public int units = 0;
    public bool activated = false;
    public EnemyStage enemyStage = new EnemyStage();
    public List<GameObject> temp_Colliders = new List<GameObject>();
    
    void OnTriggerEnter(Collider collider)
    {
        // check if player
        if (collider.tag == "Player")
        {
            // notify manager
            GetEventListener("EnemyManager").HandleEvent(GameEvent.CLASS_TYPE_ENEMY_SPAWNER, this);
            // ## [TEMP] ## update all colliders
            foreach (GameObject obj in temp_Colliders)
            {
                obj.SetActive(true);
            }
        }
    }

    public void Reset()
    {
        // reset spawner
        activated = false;
        enemyStage.Reset();
    }
    public void Begin()
    {
        // check if activated
        if (!activated)
        {
            // reset stage
            activated = true;
            enemyStage.Reset();
            enemyStage.NextWave();
        }
    }
    public bool IsWaveEmpty()
    {
        // check status
        return enemyStage.IsWaveEmpty();
    }
    public bool IsWaveComplete()
    {
        // check status
        return enemyStage.IsWaveComplete();
    }
    public bool IsStageComplete()
    {
        // check status
        return activated && enemyStage.IsStageComplete();
    }
    public float GetWaveSpawnRate()
    {
        // return spawn rate
        return enemyStage.wave == null ? 0 : enemyStage.wave.rate;
    }
    public int GetWaveActiveUnits()
    {
        // return active units
        return enemyStage.activeUnits;
    }

    public override bool HandleEvent(GameEvent e)
    {
        // check game event
        switch (e)
        {
            // remove enemy unit
            case GameEvent.ENEMY_DIED:
                enemyStage.RemoveUnit();
                break;
            // create enemy unit
            case GameEvent.ENEMY_SPAWN:
                enemyStage.CreateUnit(transform);
                break;
            // continue to next wave
            case GameEvent.ENEMY_WAVE_NEXT:
                enemyStage.NextWave();
                break;
        }
        return true;
    }
}