﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FR
{
    [System.Serializable]
    public class SpawnPoint
    {
        // :: variables
        public Vector3 position;
        public List<SpawnUnit> units;
        // :: initializers
        public SpawnPoint() : this(new Vector3(), new List<SpawnUnit>()) {}
        public SpawnPoint(SpawnPoint other) : this(other.position, other.units){}
        public SpawnPoint(Vector3 position) : this(position, new List<SpawnUnit>()) {}
        public SpawnPoint(Vector3 position, List<SpawnUnit> units)
        {
            // initialize
            this.units = new List<SpawnUnit>();
            this.position = position;
            foreach (SpawnUnit unit in units)
            {
                this.units.Add(new SpawnUnit(unit));
            }
        }
        // :: class functions
        public bool IsEmpty()
        {
            // check if depleted
            return units.Count == 0;
        }
        public GameObject CreateUnit(Quaternion rotation, Transform parent)
        {
            // create unit
            return CreateUnit(units[Random.Range(0, units.Count)], rotation, parent);
        }
        public GameObject CreateUnit(SpawnUnit unit, Quaternion rotation, Transform parent)
        {
            // create unit
            GameObject instance = unit.Create(position, rotation, parent);
            // check if empty
            if (unit.IsEmpty()) units.Remove(unit);
            // return instance
            return instance;
        }
    }
}