using UnityEngine;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[BindListener("EnemyManager", typeof(EnemyManager))]
public class EnemySpawner : EventHandle
{
    //[HideInInspector]
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
        enemyStage.reset();
    }
    public void Begin()
    {
        // check if activated
        if (!activated)
        {
            // reset stage
            activated = true;
            enemyStage.reset();
        }
    }
    public bool IsWaveEmpty()
    {
        // check status
        return enemyStage.isWaveEmpty();
    }
    public bool IsWaveComplete()
    {
        // check status
        return enemyStage.isWaveComplete();
    }
    public bool IsStageComplete()
    {
        // check status
        return activated && enemyStage.isComplete();
    }
    public float GetWaveSpawnRate()
    {
        // return spawn rate
        return enemyStage.stage.wave.rate;
    }
    public int GetWaveActiveUnits()
    {
        // return active units
        return enemyStage.waveUnits;
    }

    public override bool HandleEvent(GameEvent e)
    {
        // check game event
        switch (e)
        {
            // remove enemy unit
            case GameEvent.ENEMY_DIED:
                enemyStage.removeUnit();
                break;
            // create enemy unit
            case GameEvent.ENEMY_SPAWN:
                enemyStage.createUnit(transform);
                break;
            // continue to next wave
            case GameEvent.ENEMY_WAVE_NEXT:
                enemyStage.nextWave();
                break;
        }
        return true;
    }
}