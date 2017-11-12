using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.Spawn
{
    [System.Serializable]
    public class Unit
    {
        // :: variables
        public GameObject prefab;
        public List<Point> points;
        // :: constructors
        public Unit() : this(null, new List<Point>()) { }
        public Unit(Unit other) : this(other.prefab, other.points) { }
        public Unit(GameObject prefab, List<Point> points)
        {
            this.prefab = prefab;
            this.points = new List<Point>();
            foreach (Point spot in points) this.points.Add(new Point(spot));
        }
        // :: functions
        public bool IsEmpty()
        {
            return points.Count == 0;
        }
        public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (IsEmpty()) return null;
            int index = Random.Range(0, points.Count);
            return Create(points[index], position, rotation, parent);
        }
        public GameObject Create(Point spot, Vector3 position, Quaternion rotation, Transform parent)
        {
            GameObject instance = spot.Create(prefab, position + parent.position, rotation * parent.rotation, parent);
            if (spot.IsEmpty()) points.Remove(spot);
            return instance;
        }
    };
}