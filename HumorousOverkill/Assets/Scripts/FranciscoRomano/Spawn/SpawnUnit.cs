using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnUnit
    {
        // :: variables
        public int amount;
        public GameObject prefab;
        // :: initializers
        public SpawnUnit() : this(null, 0) {}
        public SpawnUnit(SpawnUnit other) : this(other.prefab, other.amount) {}
        public SpawnUnit(GameObject prefab, int amount)
        {
            // initialize
            this.amount = amount;
            this.prefab = prefab;
        }
        // :: class functions
        public bool IsEmpty()
        {
            // check if depleted
            return !(amount > 0);
        }
        public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            amount--;
            // instantiate prefab
            return Object.Instantiate(prefab, parent.position + position, rotation, parent);
        }
    }
}