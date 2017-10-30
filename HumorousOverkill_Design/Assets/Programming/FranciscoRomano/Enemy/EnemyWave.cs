using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct EnemyWave
{
    public List<EnemyUnit> units;
    public List<GameObject> prefabs;
}