using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnPoint
    {
        // :: variables
        public Vector3 position = new Vector3();
        public List<SpawnUnit> units = new List<SpawnUnit>();

        // :: initializers
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return units.Count == 0;
        }
        public void Instantiate(Quaternion rotation, Transform parent)
        {
            // create unit
            Instantiate(Random.Range(0, units.Count), rotation, parent);
        }
        public GameObject Instantiate(int unit, Quaternion rotation, Transform parent)
        {
            // create unit
            GameObject obj = units[unit].Instantiate(position, rotation, parent);
            // check if depleted
            if (units[unit].isEmpty())
            {
                // remove unit
                units.RemoveAt(unit);
            }
            // return instance
            return obj;
        }
    }
}