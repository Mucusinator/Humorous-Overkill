using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStage
{
    // :: variables
    public int waveIndex;
    public FR.SpawnWave current;
    public List<FR.SpawnWave> waveList;
    // :: initializers
    public EnemyStage()
    {
        // initialize
        current = null;
        waveList = new List<FR.SpawnWave>();
        waveIndex = 0;
    }
    // :: class functions
    public void Reset()
    {
        // reset values
        current = new FR.SpawnWave(waveList[0]);
        waveIndex = 0;
    }
    public void NextWave()
    {
        // check status
        if (waveIndex + 1 == waveList.Count) return;
        // change current wave
        int activeUnits = current.activeUnits;
        current = new FR.SpawnWave(waveList[++waveIndex]);
        current.activeUnits = activeUnits;
    }
    public void RemoveUnit()
    {
        // check status
        if (current.activeUnits == 0) return;
        // remove enemy unit
        current.RemoveUnit();
    }
    public bool isComplete()
    {
        // check status
        return current.isComplete() && (waveIndex + 1) == waveList.Count;
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
    public float GetUnitSpawnRate()
    {
        // return wave spawnrate
        return current.spawnrate;
    }
    public GameObject CreateUnit(Transform parent)
    {
        // check status
        if (current.isEmpty()) return null;
        // create enemy unit
        return current.CreateUnit(parent);
    }
}