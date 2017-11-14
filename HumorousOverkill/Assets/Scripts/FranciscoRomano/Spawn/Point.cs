using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.Spawn
{
    [System.Serializable]
    public class Point
    {
        // :: variables
        public int amount;
        public Vector3 position;
        // :: constructors
        public Point() : this(0, new Vector3()) { }
        public Point(Point other) : this(other.amount, other.position) { }
        public Point(int amount, Vector3 position)
        {
            this.amount = amount;
            this.position = new Vector3(position.x, position.y, position.z);
        }
        // :: functions
        public bool IsEmpty()
        {
            return amount == 0;
        }
        public GameObject Create(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent)
        {
            if (IsEmpty()) return null; else amount--;
            return Object.Instantiate(prefab, this.position + position, rotation, parent);
        }
    }
}