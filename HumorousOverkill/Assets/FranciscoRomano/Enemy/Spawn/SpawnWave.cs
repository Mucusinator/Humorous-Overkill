using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnWave
    {
        // :: variables
        public List<SpawnPoint> points = new List<SpawnPoint>();
        // :: initializers
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return points.Count == 0;
        }
        public GameObject Instantiate(Quaternion rotation, Transform parent)
        {
            // create unit
            return Instantiate(Random.Range(0, points.Count), rotation, parent);
        }
        public GameObject Instantiate(int point, Quaternion rotation, Transform parent)
        {
            // create unit
            return Instantiate(Random.Range(0, points[point].units.Count), point, rotation, parent);
        }
        public GameObject Instantiate(int unit, int point, Quaternion rotation, Transform parent)
        {
            // create unit
            GameObject obj = points[point].Instantiate(unit, rotation, parent);
            // check if depleted
            if (points[point].isEmpty())
            {
                // remove point
                points.RemoveAt(point);
            }
            // return instance
            return obj;
        }
    }
}