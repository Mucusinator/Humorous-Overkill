using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnWave
    {
        // :: variables
        public int activeUnits;
        public float spawnRate;
        public List<SpawnUnit> units;
        // :: initializers
        public SpawnWave()
        {
            // default values
            units = new List<SpawnUnit>();
            spawnRate = 1.0f;
            activeUnits = 0;
        }
        public SpawnWave(SpawnWave other)
        {
            // shallow copy
            units = new List<SpawnUnit>(other.units);
            spawnRate = other.spawnRate;
            activeUnits = other.activeUnits;
        }
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return units.Count == 0;
        }
        public bool isComplete()
        {
            // check if complete
            return isEmpty() && activeUnits == 0;
        }
        public void RemoveUnit()
        {
            // remove unit
            activeUnits--;
        }
        public GameObject CreateUnit(Vector3 position, Transform parent)
        {
            // create unit
            return CreateUnit(position, Quaternion.identity, parent);
        }
        public GameObject CreateUnit(Vector3 position, Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(Random.Range(0, units.Count), position, rotation, parent);
        }
        public GameObject CreateUnit(int index, Vector3 position, Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(units[index], position, rotation, parent);
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
                units.Remove(unit);
                unit = null;
            }
            // return unit
            return instance;
        }
    }
}