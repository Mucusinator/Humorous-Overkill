using FR.Util;
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
    public SpawnInfo.Wave wave = null;
    public List<GameObject> doors = new List<GameObject>();
    public List<SpawnInfo.Wave> waves = new List<SpawnInfo.Wave>();

    public EnemyStage temp_stage = new EnemyStage();
    
    void OnTriggerEnter(Collider collider)
    {
        // check if player
        if (collider.tag == "Player")
        {
            // notify manager
            GetEventListener("EnemyManager").HandleEvent(GameEvent.CLASS_TYPE_ENEMY_SPAWNER, this);
            // update door objects
            foreach (GameObject door in doors) door.SetActive(true);
        }
    }

    public void Reset()
    {
        // reset spawner
        activated = false;
        temp_stage.Reset();
    }
    public void Begin()
    {
        // check if activated
        if (!activated)
        {
            // reset stage
            activated = true;
            temp_stage.Reset();
            temp_stage.NextWave();
        }
    }
    public bool IsWaveEmpty()
    {
        // check status
        return temp_stage.IsWaveEmpty();
    }
    public bool IsWaveComplete()
    {
        // check status
        return temp_stage.IsWaveComplete();
    }
    public bool IsStageComplete()
    {
        // check status
        return activated && temp_stage.IsStageComplete();
    }
    public float GetWaveSpawnRate()
    {
        // return spawn rate
        return temp_stage.wave == null ? 0 : temp_stage.wave.rate;
    }
    public int GetWaveActiveUnits()
    {
        // return active units
        return temp_stage.activeUnits;
    }

    public override bool HandleEvent(GameEvent e)
    {
        // check game event
        switch (e)
        {
            // remove enemy unit
            case GameEvent.ENEMY_DIED:
                temp_stage.RemoveUnit();
                break;
            // create enemy unit
            case GameEvent.ENEMY_SPAWN:
                temp_stage.CreateUnit(transform);
                break;
            // continue to next wave
            case GameEvent.ENEMY_WAVE_NEXT:
                temp_stage.NextWave();
                break;
        }
        return true;
    }
}