using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.Spawn
{
    [System.Serializable]
    public class Group
    {
        // :: variables
        public float rate;
        public List<Unit> units;
        // :: constructors
        public Group() : this(0, new List<Unit>()) { }
        public Group(Group other) : this(other.rate, other.units) { }
        public Group(float rate, List<Unit> units)
        {
            this.rate = rate;
            this.units = new List<Unit>();
            foreach (Unit unit in units) this.units.Add(new Unit(unit));
        }
        // :: functions
        public bool IsEmpty() { return units.Count == 0; }
        public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (IsEmpty()) return null;
            int index = Random.Range(0, units.Count);
            return Create(units[index], position, rotation, parent);
        }
        public GameObject Create(Unit unit, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject instance = unit.Create(position, rotation, parent);
            if (unit.IsEmpty()) units.Remove(unit);
            return instance;
        }
    }
}