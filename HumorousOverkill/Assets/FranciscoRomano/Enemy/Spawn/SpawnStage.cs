using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnStage
    {
        // :: variables
        public List<SpawnWave> waves = new List<SpawnWave>();
        // :: initializers
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return waves.Count == 0;
        }
        public GameObject Instantiate(Quaternion rotation, Transform parent)
        {
            // create unit
            return Instantiate(Random.Range(0, waves.Count), rotation, parent);
        }
        public GameObject Instantiate(int wave, Quaternion rotation, Transform parent)
        {
            // create unit
            return Instantiate(Random.Range(0, waves[wave].points.Count), wave, rotation, parent);
        }
        public GameObject Instantiate(int point, int wave, Quaternion rotation, Transform parent)
        {
            // create unit
            return Instantiate(Random.Range(0, waves[wave].points[point].units.Count), point, wave, rotation, parent);
        }
        public GameObject Instantiate(int unit, int point, int wave, Quaternion rotation, Transform parent)
        {
            // create unit
            GameObject obj = waves[wave].Instantiate(unit, point, rotation, parent);
            // check if depleted
            if (waves[wave].isEmpty())
            {
                // remove wave
                waves.RemoveAt(wave);
            }
            // return instance
            return obj;
        }
    }
}