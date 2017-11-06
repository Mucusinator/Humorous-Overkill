using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStage
{
    // :: variables
    public int waveUnits;
    public FR.SpawnStage stage;
    // :: initializers
    public EnemyStage()
    {
        // initialize
        waveUnits = 0;
        stage = new FR.SpawnStage();
    }
    // :: class functions
    public void reset()
    {
        // reset values
        stage.Reset();
    }
    public void nextWave()
    {
        // change wave
        stage.NextWave();
    }
    public void removeUnit()
    {
        // check status
        if (waveUnits > 0)
        {
            // remove unit
            waveUnits--;
        }
    }
    public bool isEmpty()
    {
        // check status
        return stage.IsStageEmpty();
    }
    public bool isComplete()
    {
        // check status
        return isEmpty() && !(waveUnits > 0);
    }
    public bool isWaveEmpty()
    {
        // check if wave empty
        return stage.IsWaveEmpty();
    }
    public bool isWaveComplete()
    {
        // check if wave complete
        return isWaveEmpty() && !(waveUnits > 0);
    }
    public GameObject createUnit(Transform parent)
    {
        // check status
        if (isWaveEmpty()) return null;
        waveUnits++;
        // create enemy unit
        return stage.CreateUnit(Quaternion.identity, parent);
    }
}