using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR.Util
{
    public class SpawnInfo
    {
        // :: classes
        [System.Serializable]
        public class Unit
        {
            // :: variables
            public int index;
            public int amount;
            // :: constructors
            public Unit(Unit other) : this(other.index, other.amount) { }
            public Unit(int index, int amount)
            {
                this.index = index;
                this.amount = amount;
            }
            // :: class functions
            public bool IsEmpty() { return amount == 0; }
        };
        [System.Serializable]
        public class Spot
        {
            // :: variables
            public int index;
            public List<Unit> units;
            // :: constructors
            public Spot(Spot other) : this(other.index, other.units) { }
            public Spot(int index, List<Unit> units)
            {
                this.index = index;
                this.units = new List<Unit>();
                foreach (Unit unit in units) this.units.Add(new Unit(unit));
            }
            // :: class functions
            public bool IsEmpty() { return units.Count == 0; }
        }
        [System.Serializable]
        public class Wave
        {
            // :: variables
            public float rate;
            public List<Spot> spots;
            // :: constructors
            public Wave(Wave other) : this(other.rate, other.spots) { }
            public Wave(float rate, List<Spot> spots)
            {
                this.rate = rate;
                this.spots = new List<Spot>();
                foreach (Spot spot in spots) this.spots.Add(new Spot(spot));
            }
            // :: class functions
            public bool IsEmpty() { return spots.Count == 0; }
        }
        // :: variables
        public List<Vector3> positions = new List<Vector3>();
        public List<GameObject> prefabs = new List<GameObject>();
    }
}