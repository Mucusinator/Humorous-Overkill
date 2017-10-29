using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStage
{
    // :: variables
    public int waveIndex;
    public FR.SpawnWave wave;
    public List<Vector3> points;
    public List<FR.SpawnWave> waves;
    // :: initializers
    public EnemyStage()
    {
        // initialize
        waves = new List<FR.SpawnWave>();
        points = new List<Vector3>();
        wave = null;
        waveIndex = 0;
    }
    // :: class functions
    public void reset()
    {
        // reset values
        wave = new FR.SpawnWave(waves[0]);
        waveIndex = 0;
    }
    public void nextWave()
    {
        // check status
        if (waveIndex + 1 == waves.Count) return;
        // change current wave
        int activeUnits = wave.activeUnits;
        wave = new FR.SpawnWave(waves[++waveIndex]);
        wave.activeUnits = activeUnits;
    }
    public void removeUnit()
    {
        // check status
        if (wave.activeUnits == 0) return;
        // remove enemy unit
        wave.removeUnit();
    }
    public bool isComplete()
    {
        // check status
        return wave.isComplete() && (waveIndex + 1) == waves.Count;
    }
    public bool isWaveEmpty()
    {
        // check if wave empty
        return wave.isEmpty();
    }
    public bool isWaveComplete()
    {
        // check if wave complete
        return wave.isComplete();
    }
    public GameObject createUnit(Transform parent)
    {
        // check status
        if (wave.isEmpty()) return null;
        // create enemy unit
        return wave.createUnit(points[Random.Range(0, points.Count)], Quaternion.identity, parent);
    }
}