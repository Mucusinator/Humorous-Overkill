using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStage
{
    // :: variables
    [HideInInspector]
    public int waveIndex;
    public FR.SpawnWave current;
    public List<Vector3> points;
    [HideInInspector]
    public List<FR.SpawnWave> waves;
    // :: initializers
    public EnemyStage()
    {
        // initialize
        waves = new List<FR.SpawnWave>();
        points = new List<Vector3>();
        current = null;
        waveIndex = 0;
    }
    // :: class functions
    public void reset()
    {
        // reset values
        current = new FR.SpawnWave(waves[0]);
        waveIndex = 0;
    }
    public void nextWave()
    {
        // check status
        if (waveIndex + 1 == waves.Count) return;
        // change current wave
        int activeUnits = current.activeUnits;
        current = new FR.SpawnWave(waves[++waveIndex]);
        current.activeUnits = activeUnits;
    }
    public void removeUnit()
    {
        // check status
        if (current.activeUnits == 0) return;
        // remove enemy unit
        current.RemoveUnit();
    }
    public bool isComplete()
    {
        // check status
        return current.isComplete() && (waveIndex + 1) == waves.Count;
    }
    public bool isWaveEmpty()
    {
        // check if wave empty
        return current.isEmpty();
    }
    public bool isWaveComplete()
    {
        // check if wave complete
        return current.isComplete();
    }
    public float getWaveSpawnRate()
    {
        // return wave spawnrate
        return current.spawnRate;
    }
    public GameObject createUnit(Transform parent)
    {
        // check status
        if (current.isEmpty()) return null;
        // create enemy unit
        return current.CreateUnit(points[Random.Range(0, points.Count)], parent);
    }
}