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
        public float spawnrate;
        public List<Vector3> points;
        public List<SpawnUnit> units;
        // :: initializers
        public SpawnWave()
        {
            // default values
            units = new List<SpawnUnit>();
            points = new List<Vector3>();
            spawnrate = 1.0f;
            activeUnits = 0;
        }
        public SpawnWave(SpawnWave other)
        {
            // shallow copy
            units = new List<SpawnUnit>(other.units);
            points = new List<Vector3>(other.points);
            spawnrate = other.spawnrate;
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
        public GameObject CreateUnit(Transform parent)
        {
            // create unit
            return CreateUnit(Quaternion.identity, parent);
        }
        public GameObject CreateUnit(Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(Random.Range(0, units.Count), rotation, parent);
        }
        public GameObject CreateUnit(int unit, Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(unit, Random.Range(0, points.Count), rotation, parent);
        }
        public GameObject CreateUnit(int unit, int point, Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(units[unit], points[point], rotation, parent);
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