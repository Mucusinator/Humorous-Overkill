using System;
using UnityEngine;
using UnityEditor;
using EventHandler;
using System.Collections;
using FranciscoRomano.Spawn;
using System.Collections.Generic;

public class SpawnEditor
{
    // :: variables
    public enum Status
    {
        NONE,
        CHANGE,
        REMOVE,
        RETURN,
    }

    public class Msg
    {
        public string text;
        public Status type;
        public Msg(string text, Status type)
        {
            this.text = text;
            this.type = type;
        }
    }
    
    public static Unit currentUnit = null;
    public static Group currentGroup = null;
    public static Stage currentStage = null;

    // :: functions [single]
    public static void OnSceneGUI(Transform transform)
    {
        if (currentUnit != null)
        {
            Handles.color = Color.magenta;
            foreach (Point point in currentUnit.points)
            {
                Handles.DrawSolidDisc(transform.position + transform.rotation * point.position, Vector3.up, 0.5f);
            }
            SceneView.RepaintAll();
        }
        else if (currentGroup != null)
        {
            Handles.color = Color.magenta;
            foreach (Unit unit in currentGroup.units)
            {
                foreach (Point point in unit.points)
                {
                    Handles.DrawSolidDisc(transform.position + transform.rotation * point.position, Vector3.up, 0.5f);
                }
            }
            SceneView.RepaintAll();
        }
        else if (currentStage != null)
        {
            Handles.color = Color.magenta;
            foreach (Group group in currentStage.groups)
            {
                foreach (Unit unit in group.units)
                {
                    foreach (Point point in unit.points)
                    {
                        Handles.DrawSolidDisc(transform.position + transform.rotation * point.position, Vector3.up, 0.5f);
                    }
                }
            }
            SceneView.RepaintAll();
        }
    }
    public static void OnInspectorGUI()
    {
        Msg backMsg = new Msg("back", Status.RETURN);
        if (currentUnit != null)
        {
            if (OnInspectorGUI(currentUnit, "Unit", backMsg, true) == Status.RETURN)
            {
                Debug.Log("unit back..");
                currentUnit = null;
            }
        }
        else if (currentGroup != null)
        {
            if (OnInspectorGUI(currentGroup, "Wave", backMsg, true) == Status.RETURN)
            {
                currentGroup = null;
            }
        }
        else if (currentStage != null)
        {
            OnInspectorGUI(currentStage, "Stage", null, true);
        }
    }
    public static Status OnInspectorGUI(Msg message)
    {
        string text = message.text;
        float width = 20 + (text.Length > 1 ? text.Length * 5 : 0);
        // check if button pressed
        if (GUILayout.Button(text, GUILayout.Width(width)))
        {
            return message.type;
        }
        // return default inspector status
        return Status.NONE;
    }
    public static Status OnInspectorGUI(Unit unit, string label, Msg message, bool edit)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.largeLabel, GUILayout.Height(20));
        if (result == Status.NONE && edit == false) result = OnInspectorGUI(new Msg("edit", Status.CHANGE));
        if (result == Status.NONE && message != null) result = OnInspectorGUI(message);
        EditorGUILayout.EndHorizontal();
        // display class variables
        if (edit)
        {
            EditorGUILayout.BeginVertical("Button");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Prefab");
            unit.prefab = (GameObject) EditorGUILayout.ObjectField("", unit.prefab, typeof(GameObject), true);
            OnInspectorGUI(unit.points, "Points");
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
    public static Status OnInspectorGUI(Point point, string label, Msg message, bool edit)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.largeLabel, GUILayout.Height(20));
        if (result == Status.NONE && edit == false) result = OnInspectorGUI(new Msg("edit", Status.CHANGE));
        if (result == Status.NONE && message != null) result = OnInspectorGUI(message);
        EditorGUILayout.EndHorizontal();
        // display class variables
        if (edit)
        {
            EditorGUILayout.BeginVertical("Button");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("position");
            point.position = EditorGUILayout.Vector3Field("", point.position);
            EditorGUILayout.LabelField("amount");
            point.amount = EditorGUILayout.IntSlider("", point.amount, 0, 50);
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
    public static Status OnInspectorGUI(Group group, string label, Msg message, bool edit)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.largeLabel, GUILayout.Height(20));
        if (result == Status.NONE && edit == false) result = OnInspectorGUI(new Msg("edit", Status.CHANGE));
        if (result == Status.NONE && message != null) result = OnInspectorGUI(message);
        EditorGUILayout.EndHorizontal();
        // display class variables
        if (edit)
        {
            EditorGUILayout.BeginVertical("Button");
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rate");
            group.rate = EditorGUILayout.Slider("", group.rate, 0, 50);
            OnInspectorGUI(group.units, "Units");
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
    public static Status OnInspectorGUI(Stage stage, string label, Msg message, bool edit)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label, EditorStyles.largeLabel, GUILayout.Height(20));
        if (edit == false) result = OnInspectorGUI(new Msg("edit", Status.CHANGE));
        if (message != null) result = OnInspectorGUI(message);
        EditorGUILayout.EndHorizontal();
        // display class variables
        if (edit)
        {
            EditorGUILayout.BeginVertical("Button");
            EditorGUILayout.Space();
            OnInspectorGUI(stage.groups, "Waves");
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
    // :: functions [dynamic array]
    public static Status OnInspectorGUI(List<Unit> list, string label)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        if (GUILayout.Button("+", GUILayout.Width(20))) list.Add(new Unit());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        // display class variables
        if (list.Count > 0)
        {
            Msg listMsg = new Msg("delete", Status.REMOVE);
            EditorGUILayout.BeginVertical("Button");
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.Space();
                Status listStatus = OnInspectorGUI(list[i], "Unit " + (i + 1), listMsg, false);
                if (listStatus == Status.REMOVE)
                {
                    list.RemoveAt(i);
                }
                else if (listStatus == Status.CHANGE)
                {
                    currentUnit = list[i];
                }
            }
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
    public static Status OnInspectorGUI(List<Point> list, string label)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        if (GUILayout.Button("+", GUILayout.Width(20))) list.Add(new Point());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        // display class variables
        if (list.Count > 0)
        {
            Msg listMsg = new Msg("delete", Status.REMOVE);
            EditorGUILayout.BeginVertical("Button");
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.Space();
                Status listStatus = OnInspectorGUI(list[i], "Point " + (i + 1), listMsg, true);
                if (listStatus == Status.REMOVE)
                {
                    list.RemoveAt(i);
                }
            }
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
    public static Status OnInspectorGUI(List<Group> list, string label)
    {
        Status result = Status.NONE;
        // display header
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(label);
        if (GUILayout.Button("+", GUILayout.Width(20))) list.Add(new Group());
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        // display class variables
        if (list.Count > 0)
        {
            Msg listMsg = new Msg("delete", Status.REMOVE);
            EditorGUILayout.BeginVertical("Button");
            for (int i = 0; i < list.Count; i++)
            {
                EditorGUILayout.Space();
                Status listStatus = OnInspectorGUI(list[i], "Wave " + (i + 1), listMsg, false);
                if (listStatus == Status.CHANGE)
                {
                    currentGroup = list[i];
                }
                else if (listStatus == Status.REMOVE)
                {
                    list.RemoveAt(i);
                }
            }
            EditorGUILayout.EndVertical();
        }
        // return default inspector status
        return result;
    }
}