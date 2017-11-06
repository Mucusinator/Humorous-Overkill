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
        public SpawnWave(SpawnWave other) : this(other.rate, other.points) {}
        public SpawnWave(float rate, List<SpawnPoint> points)
        {
            // initialize
            this.rate = rate;
            this.points = new List<SpawnPoint>();
            foreach (SpawnPoint point in points)
            {
                this.points.Add(new SpawnPoint(point));
            }
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
    }
}