using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnWave
    {
        // :: variables
        public float rate;
        public List<SpawnPoint> points;
        // :: initializers
        public SpawnWave() : this(1, new List<SpawnPoint>()) {}
        public SpawnWave(SpawnWave other) : this(other.rate, new List<SpawnPoint>(other.points)) {}
        public SpawnWave(float rate, List<SpawnPoint> points)
        {
            // initialize
            this.rate = rate;
            this.points = points;
        }
        // :: class functions
        public bool IsEmpty()
        {
            // check if depleted
            return points.Count == 0;
        }
        public GameObject CreateUnit(Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(points[Random.Range(0, points.Count)], rotation, parent);
        }
        public GameObject CreateUnit(SpawnPoint point, Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(point.units[Random.Range(0, point.units.Count)], point, rotation, parent);
        }
        public GameObject CreateUnit(SpawnUnit unit, SpawnPoint point, Quaternion rotation, Transform parent)
        {
            // create unit
            GameObject instance = point.CreateUnit(unit, rotation, parent);
            // check if empty
            if (point.IsEmpty()) points.Remove(point);
            // return instance
            return instance;
        }






        //public SpawnWave()
        //{
        //    // default values
        //    units = new List<SpawnUnit>();
        //    rate = 1.0f;
        //    activeUnits = 0;
        //}
        //public SpawnWave(SpawnWave other)
        //{
        //    // shallow copy
        //    units = new List<SpawnUnit>(other.units);
        //    rate = other.rate;
        //    activeUnits = other.activeUnits;
        //}

        //public int activeUnits;
        //public bool IsComplete()
        //{
        //    // check if complete
        //    return IsEmpty() && !(activeUnits > 0);
        //}
        //public List<SpawnUnit> units;
        //public GameObject CreateUnit(Vector3 position, Quaternion rotation, Transform parent)
        //{
        //    // create unit
        //    return CreateUnit(Random.Range(0, units.Count), position, rotation, parent);
        //}
        //public GameObject CreateUnit(int index, Vector3 position, Quaternion rotation, Transform parent)
        //{
        //    // create unit
        //    activeUnits++;
        //    GameObject instance = units[index].Create(position, rotation, parent);
        //    // check if depleted
        //    if (units[index].IsEmpty())
        //    {
        //        // remove unit
        //        units.RemoveAt(index);
        //    }
        //    // return unit
        //    return instance;
        //}
        //public void removeUnit()
        //{
        //    // remove unit
        //    activeUnits--;
        //}
    }
}