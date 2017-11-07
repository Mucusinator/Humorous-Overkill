using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class EnemyStage : FR.SpawnStage
{
    // :: variables
    public int activeUnits;
    // :: initializers
    public EnemyStage() : base()
    {
        // initialize
        activeUnits = 0;
    }
    // :: class functions
    public new void Reset()
    {
        // reset stage
        base.Reset();
        activeUnits = 0;
    }
    public void RemoveUnit()
    {
        // remove unit
        if (activeUnits > 0) activeUnits--;
    }
    public bool IsWaveComplete()
    {
        // check if wave complete
        return IsWaveEmpty() && !(activeUnits > 0);
    }
    public bool IsStageComplete()
    {
        // check status
        return IsStageEmpty() && !(activeUnits > 0);
    }
    public GameObject CreateUnit(Transform parent)
    {
        // check status
        if (IsWaveEmpty()) return null;
        // create enemy unit
        activeUnits++;
        return CreateUnit(Quaternion.identity, parent);
    }
}