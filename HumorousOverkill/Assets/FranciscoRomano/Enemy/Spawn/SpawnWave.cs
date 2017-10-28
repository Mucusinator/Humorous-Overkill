using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnWave
    {
        // :: variables
        public int activeUnits = 0;
        public List<Vector3> spawnPoints = new List<Vector3>();
        public List<SpawnUnit> spawnUnits = new List<SpawnUnit>();
        // :: initializers
        public SpawnWave(SpawnWave other)
        {
            activeUnits = other.activeUnits;
            spawnPoints = new List<Vector3>(other.spawnPoints);
            spawnUnits = new List<SpawnUnit>(other.spawnUnits);

        }
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return spawnUnits.Count == 0;
        }
        public bool isComplete()
        {
            // check if complete
            return activeUnits == 0;
        }
        public void RemoveUnit()
        {
            // remove unit
            activeUnits--;
        }
        public GameObject CreateUnit(Transform parent)
        {
            // create unit
            return CreateUnit(Quaternion.identity, parent);
        }
        public GameObject CreateUnit(Quaternion rotation, Transform parent)
        {
            // create unit
            Debug.Log("random between 0 - " + spawnUnits.Count);
            return CreateUnit(Random.Range(0, spawnUnits.Count), rotation, parent);
        }
        public GameObject CreateUnit(int unit, Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(unit, Random.Range(0, spawnPoints.Count), rotation, parent);
        }
        public GameObject CreateUnit(int unit, int point, Quaternion rotation, Transform parent)
        {
            // create unit
            Debug.Log("random = " + unit + ":: MAX(" + spawnUnits.Count + ")");
            return CreateUnit(spawnUnits[unit], spawnPoints[point], rotation, parent);
        }
        public GameObject CreateUnit(SpawnUnit unit, Vector3 position, Quaternion rotation, Transform parent)
        {
            // create unit
            activeUnits++;
            GameObject instance = unit.create(position, rotation, parent);
            // check if depleted
            if (unit.isEmpty())
            {
                // remove unit
                spawnUnits.Remove(unit);
                unit = null;
            }
            // return unit
            return instance;
        }
    }
}