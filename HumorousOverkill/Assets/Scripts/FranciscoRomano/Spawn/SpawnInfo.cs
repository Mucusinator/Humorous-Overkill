using FR.Util;
using UnityEngine;
using UnityEditor;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

namespace FR.Util
{
    public class SpawnInfo
    {
        // :: classes
        [System.Serializable]
        public class Spot
        {
            // :: variables
            public int amount;
            public Vector3 position;
            // :: constructors
            public Spot() : this(0, new Vector3()) { }
            public Spot(Spot other) : this(other.amount, other.position) { }
            public Spot(int amount, Vector3 position)
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
        [System.Serializable]
        public class Unit
        {
            // :: variables
            public List<Spot> spots;
            public GameObject prefab;
            // :: constructors
            public Unit() : this(null, new List<Spot>()) { }
            public Unit(Unit other) : this(other.prefab, other.spots) { }
            public Unit(GameObject prefab, List<Spot> spots)
            {
                this.prefab = prefab;
                this.spots = new List<Spot>();
                foreach (Spot spot in spots) this.spots.Add(new Spot(spot));
            }
            // :: functions
            public bool IsEmpty()
            {
                return spots.Count == 0;
            }
            public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
            {
                int index = Random.Range(0, spots.Count);
                return Create(spots[index], position, rotation, parent);
            }
            public GameObject Create(Spot spot, Vector3 position, Quaternion rotation, Transform parent)
            {
                GameObject instance = spot.Create(prefab, position, rotation, parent);
                if (spot.IsEmpty()) spots.Remove(spot);
                return instance;
            }
        };
        [System.Serializable]
        public class Wave
        {
            // :: variables
            public float rate;
            public List<Unit> units;
            // :: constructors
            public Wave() : this(0, new List<Unit>()) { }
            public Wave(Wave other) : this(other.rate, other.units) { }
            public Wave(float rate, List<Unit> units)
            {
                this.rate = rate;
                this.units = new List<Unit>();
                foreach (Unit unit in units) this.units.Add(new Unit(unit));
            }
            // :: functions
            public bool IsEmpty() { return units.Count == 0; }
            public GameObject Create(Vector3 position, Quaternion rotation, Transform parent)
            {
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
        // :: functions
        
        //public static Spot currentSpot = null;
        public static Unit currentUnit = null;
        public static Wave currentWave = null;
        //public static bool isEditingSpot = false;
        public static bool isEditingUnit = false;
        public static bool isEditingWave = false;

        public static void OnSceneGUI(Transform transform)
        {
            if (isEditingWave)
            {
                foreach (Unit unit in currentWave.units)
                {
                    foreach (Spot spot in unit.spots)
                    {
                        //Handles.color = isEditingSpot && spot == currentSpot ? Color.green : Color.red;
                        Handles.color = Color.magenta;
                        Handles.SphereHandleCap(
                            0,
                            transform.position + spot.position,
                            transform.rotation * Quaternion.identity,
                            0.5f,
                            EventType.Repaint
                        );
                    }
                }
            }
        }
        public static void OnInspectorGUI()
        {
            //if (isEditingSpot) OnInspectorGUI(currentSpot);
            if (isEditingUnit) OnInspectorGUI(currentUnit);
            else if (isEditingWave) OnInspectorGUI(currentWave);
            else
            {
                GUILayout.Label("Editing Wave", EditorStyles.boldLabel);
                if (GUILayout.Button("Create New Wave"))
                {
                    currentWave = new Wave();
                    isEditingWave = true;
                }
            }
        }
        //public static void OnInspectorGUI(Spot spot)
        //{
        //    EditorGUILayout.BeginHorizontal();
        //    GUILayout.Label("Editing Spot", EditorStyles.boldLabel);
        //    if (GUILayout.Button("Back")) isEditingSpot = false;
        //    EditorGUILayout.EndHorizontal();

        //    spot.amount = EditorGUILayout.IntField("amount", spot.amount);
        //    spot.position = EditorGUILayout.Vector3Field("position", spot.position);
        //}
        public static void OnInspectorGUI(Unit unit)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Editing Unit", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            unit.prefab = (GameObject) EditorGUILayout.ObjectField("prefab", unit.prefab, typeof(GameObject), true);
            int i = 0;
            GUILayout.Label("Editing Unit Points", EditorStyles.boldLabel);
            foreach (Spot spot in unit.spots)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("- Point " + i++);
                EditorGUILayout.BeginVertical();
                spot.position = EditorGUILayout.Vector3Field("position", spot.position);
                spot.amount = EditorGUILayout.IntField("amount", spot.amount);
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
                //EditorGUILayout.BeginHorizontal();
                //GUI.enabled = false;
                //EditorGUILayout.Vector3Field("position", spot.position);
                //GUI.enabled = true;
                //if (GUILayout.Button("Edit"))
                //{
                //    currentSpot = spot;
                //    isEditingSpot = true;
                //}
                //EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Spot")) unit.spots.Add(new Spot());
            if (GUILayout.Button("Back")) isEditingUnit = false;
        }
        public static void OnInspectorGUI(Wave wave)
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("Editing Wave", EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            wave.rate = EditorGUILayout.FloatField("rate", wave.rate);
            foreach (Unit unit in wave.units)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.enabled = false;
                EditorGUILayout.ObjectField("", unit.prefab, typeof(GameObject), true);
                GUI.enabled = true;
                if (GUILayout.Button("Edit"))
                {
                    currentUnit = unit;
                    isEditingUnit = true;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Unit")) wave.units.Add(new Unit());
        }
    }
}