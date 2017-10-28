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
        SpawnUnit(SpawnUnit other)
        {
            // shallow copy
            amount = other.amount;
            prefab = other.prefab;
        }
        // :: class functions
        public bool isEmpty()
        {
            // check if depleted
            return amount == 0;
        }
        public SpawnUnit clone()
        {
            // return shallow copy
            return new SpawnUnit(this);
        }
        public GameObject create(Vector3 position, Quaternion rotation, Transform parent)
        {
            amount--;
            // instantiate prefab
            return Object.Instantiate(prefab, parent.position + position, rotation, parent);
        }
    }
}