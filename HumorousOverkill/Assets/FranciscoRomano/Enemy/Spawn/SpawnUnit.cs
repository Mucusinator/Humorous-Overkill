using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnUnit
    {
        // :: variables
        public int amount = 0;
        public GameObject prefab = null;

        // :: initializers
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return amount == 0;
        }
        public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
        {
            amount--;
            // instantiate prefab
            return Object.Instantiate(prefab, position, rotation, parent);
        }
    }
}