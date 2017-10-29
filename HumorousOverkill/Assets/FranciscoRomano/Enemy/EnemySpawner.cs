using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : GameEventListener
{
    public bool activated = false;
    public EnemyStage enemyStage = new EnemyStage();
    public EnemyManager enemyManager = null;
    
    void Start()
    {
        // get enemy manager
        enemyManager = GetComponentInParent<EnemyManager>();
    }
    void OnTriggerEnter(Collider collider)
    {
        // check if player
        if (collider.tag == "Player")
        {
            // notify manager
            enemyManager.HandleEvent(GameEvent.CLASS_TYPE_ENEMY_SPAWNER, this);
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
        return enemyStage.wave.spawnRate;
    }
    public int GetWaveActiveUnits()
    {
        // return active units
        return enemyStage.wave.activeUnits;
    }

    public override void HandleEvent(GameEvent e)
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
    }
}