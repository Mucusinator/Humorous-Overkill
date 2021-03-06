﻿using System;
using UnityEngine;
using UnityEditor;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{
    bool isEditingRegion = false;
    bool isEditingPoints = false;

    Transform transform = null;
    EnemySpawner component = null;
    PointsEditor pointsEditor = new PointsEditor();
    RegionEditor regionEditor = new RegionEditor();
    EnemySpawnerRegionEditor new_regionEditor = null;
    //EnemySpawnerRegionEditor region
    
    void OnEnable()
    {
        // store variable of target
        component = target as EnemySpawner;
        transform = component.transform;
        new_regionEditor = new EnemySpawnerRegionEditor(component);
    }

    void OnSceneGUI()
    {
        Event e = Event.current;

        if (isEditingRegion)
        {
            new_regionEditor.update(e);
            new_regionEditor.render(e);
            //regionEditor.render(e);
            //regionEditor.update(e);
        }
        else if (isEditingPoints)
        {
            pointsEditor.render(e);
            pointsEditor.update(e);
        }
    }

    public override void OnInspectorGUI()
    {
        // draw defaults
        DrawDefaultInspector();

        if (isEditingRegion)
        {
            //// component region layout
            //GUILayout.Label("Editing Region:", EditorStyles.boldLabel);
            //new_regionEditor.maximumX = Mathf.Max(EditorGUILayout.FloatField("height", new_regionEditor.maximumX), 1);
            //if (GUILayout.Button("Go Back")) { ResetDefaults(); };
            //if (GUILayout.Button("Confirm")) { new_regionEditor.setdata(); };
        }
        else if (isEditingPoints)
        {
            //// component points tools
            //GUILayout.Label("Editing Points:", EditorStyles.boldLabel);
            //for (int i = 0; i < pointsEditor.points.Count; i++)
            //{
            //    EditorGUILayout.BeginHorizontal();
            //    pointsEditor.points[i] = EditorGUILayout.Vector3Field("", pointsEditor.points[i]);
            //    if (GUILayout.Button("remove")) pointsEditor.points.RemoveAt(i);
            //    EditorGUILayout.EndHorizontal();
            //}
            //if (GUILayout.Button("Go Back")) { ResetDefaults(); };
            //if (GUILayout.Button("Confirm")) { ConfirmPoints(); };
        }
        else
        {
            //// default editor tools
            //GUILayout.Label("Enemy Spawner Editor:", EditorStyles.boldLabel);
            //if (GUILayout.Button("Edit Region")) { DisplayEditRegion(); }
            //if (GUILayout.Button("Edit Points")) { DisplayEditPoints(); }
            //RegionEditor.color = EditorGUILayout.ColorField("color_01", RegionEditor.color);
            //PointsEditor.color = EditorGUILayout.ColorField("color_01", PointsEditor.color);
        }
    }

    void ResetDefaults()
    {
        // reset values
        regionEditor.reset();
        isEditingPoints = false;
        isEditingRegion = false;
    }

    void ConfirmRegion()
    {
        // check if complete
        if (regionEditor.isComplete)
        {
            // calculate values
            float sizeX = Mathf.Abs(regionEditor.points[2].x - regionEditor.points[0].x);
            float sizeZ = Mathf.Abs(regionEditor.points[2].z - regionEditor.points[0].z);
            float offsetX = Mathf.Lerp(regionEditor.points[0].x, regionEditor.points[2].x, 0.5f);
            float offsetZ = Mathf.Lerp(regionEditor.points[0].z, regionEditor.points[2].z, 0.5f);
            // update current collider
            component.transform.position = new Vector3(offsetX, 0.5f, offsetZ);
            component.GetComponent<BoxCollider>().center = new Vector3();
            component.GetComponent<BoxCollider>().size = new Vector3(sizeX, 1.0f, sizeZ);
        }
    }

    //void ConfirmPoints()
    //{
    //    // clear component list
    //    component.enemyStage.points.Clear();
    //    foreach (Vector3 position in pointsEditor.points) component.enemyStage.points.Add(position - transform.position);
    //}

    void DisplayEditRegion()
    {
        // prepair editor
        isEditingRegion = true;
        new_regionEditor.prepair();
    }

    //void DisplayEditPoints()
    //{
    //    // prepair editor
    //    isEditingPoints = true;
    //    pointsEditor.points.Clear();
    //    foreach (Vector3 position in component.enemyStage.points) pointsEditor.points.Add(position + transform.position);
    //}

    class PointsEditor
    {
        public float height = 0;
        public List<Vector3> points = new List<Vector3>();
        public static Color color = new Color(1.0f, 0.0f, 1.0f, 0.5f);
        
        public void render(Event e)
        {
            // draw points
            Handles.color = color;
            foreach (Vector3 position in points) Handles.DrawSolidDisc(position, Vector3.up, 0.5f);
        }
        public void update(Event e)
        {
            // check if left mouse pressed
            if (e.type == EventType.mouseDown && e.button == 0)
            {
                // add new point
                points.Add(GetMousePoint(e));
            }
            // prevent unity from deselecting object
            if (e.type == EventType.layout) HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        Vector3 GetMousePoint(Event e)
        {
            // get screen to world position
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            // calculate point length
            float length = (height - ray.origin.y) / ray.direction.y;
            // calculate point
            return ray.origin + ray.direction * length;
        }
    }

    class RegionEditor
    {
        public bool isEditing = false;
        public bool isComplete = false;
        public float yAxis = 0.0f;
        public float height = 1.0f;
        public Vector3[] points = new Vector3[4];
        public static Color color = new Color(1.0f, 0.0f, 1.0f, 0.5f);

        public void reset()
        {
            // reset values
            isEditing = false;
            isComplete = false;
        }
        public void render(Event e)
        {
            // check if editing
            if (isEditing)
            {
                // calculate point
                points[1] = new Vector3(points[0].x, yAxis, points[2].z);
                points[2] = GetMousePoint(e);
                points[3] = new Vector3(points[2].x, yAxis, points[0].z);
            }
            // draw region lines
            Handles.color = color;
            Handles.DrawLine(points[0], points[2]);

            Handles.DrawLine(points[0], points[0] + Vector3.up * height);
            Handles.DrawLine(points[0] + Vector3.up * height, points[2]);

            Handles.DrawDottedLine(points[0], points[1], 5);
            Handles.DrawDottedLine(points[0], points[3], 5);
            Handles.DrawDottedLine(points[2], points[1], 5);
            Handles.DrawDottedLine(points[2], points[3], 5);
            // draw region points
            Handles.DrawSolidDisc(points[0], Vector3.up, 0.5f);
            Handles.DrawSolidDisc(points[2], Vector3.up, 0.5f);
        }
        public void update(Event e)
        {
            // check if left mouse pressed
            if (e.type == EventType.mouseDown && e.button == 0)
            {
                // calculate mouse point
                points[isEditing ? 2 : 0] = GetMousePoint(e);
                // toggle editable
                isEditing = !isEditing;
                // check if done
                if (!isEditing)
                {
                    // completed
                    isComplete = true;
                }
            }
            // prevent unity from deselecting object
            if (e.type == EventType.layout) HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        Vector3 GetMousePoint(Event e)
        {
            // get screen to world position
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            // calculate point length
            float length = (yAxis - ray.origin.y) / ray.direction.y;
            // calculate point
            return ray.origin + ray.direction * length;
        }
    }
}