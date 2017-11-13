using UnityEngine;
using UnityEditor;
using EventHandler;
using System.Collections;
using FranciscoRomano.Spawn;
using System.Collections.Generic;

namespace FR.Util
{
    public class SpawnInfo
    {
        // :: functions
        //public static Spot currentSpot = null;
        public static Unit currentUnit = null;
        public static Group currentWave = null;
        //public static bool isEditingSpot = false;
        public static bool isEditingUnit = false;
        public static bool isEditingWave = false;

        public static void OnSceneGUI(Transform transform)
        {
            if (isEditingWave)
            {
                foreach (Unit unit in currentWave.units)
                {
                    foreach (Point spot in unit.points)
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
                    currentWave = new Group();
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
            foreach (Point spot in unit.points)
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
            if (GUILayout.Button("Add Spot")) unit.points.Add(new Point());
            if (GUILayout.Button("Back")) isEditingUnit = false;
        }
        public static void OnInspectorGUI(Group wave)
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