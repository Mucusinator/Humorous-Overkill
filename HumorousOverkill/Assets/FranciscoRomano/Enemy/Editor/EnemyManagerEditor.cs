using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EnemyManager))]
public class EnemyManagerEditor : Editor
{
    bool isEditingStage = false;

    int temp_stage_waves = 0;

    //EnemyManager enemyManager = null;

    void OnEnable()
    {
        // store class
        //enemyManager = (EnemyManager)target;
    }

    void OnSceneGUI()
    {
        Event e = Event.current;
        if (isEditingStage)
        {
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            float height = 0.0f;
            float length = (height - mouseRay.origin.y) / mouseRay.direction.y;
            Vector3 target = mouseRay.origin + mouseRay.direction * length;

            // check if left mouse pressed
            if (e.type == EventType.mouseDown && e.button == 0)
            {
                Debug.Log("add :: { " + target.x + ", " + target.y + ", " + target.z + " }");
            }

            // draw point on screen
            Handles.color = new Color(0.0f, 1.0f, 0.0f, 0.5f);
            Handles.DrawSolidDisc(target, Vector3.up, 0.5f);

            // prevent unity from deselecting object
            if (e.type == EventType.layout)
            {
                HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
            }
        }
    }

    public override void OnInspectorGUI()
    {
        // draw defaults
        DrawDefaultInspector();
        
        if (isEditingStage)
        {
            EditorGUILayout.LabelField("Creating Stage", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("waves:");
            temp_stage_waves = EditorGUILayout.IntField(temp_stage_waves);
            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Create")) isEditingStage = false;
            if (GUILayout.Button("Cancel")) isEditingStage = false;
        }
        else
        {
            EditorGUILayout.LabelField("Enemy Manager Editor", EditorStyles.boldLabel);


            isEditingStage = GUILayout.Button("Create Stage");
        }
    }

    //int selected = 0;
    //string[] options = new string[]
    //{
    //        "Stage1", "Stage2", "Stage3"
    //};

    //public override void OnInspectorGUI()
    //{
    //    DrawDefaultInspector();

    //    if (GUILayout.Button("Create New Stage"))
    //    {

    //    }


    //    GUILayout.Label("Current Selected Stage:");
    //    selected = EditorGUILayout.Popup(selected, options);
    //    //if (GUILayout.Button("Create Wave"))
    //    //{

    //    //}
    //}

    //void OnSceneGUI()
    //{
    //    Event guiEvent = Event.current;

    //    // Get Point where Y-Axis is = 0.0f
    //    // origin + dir * length = P
    //    // origin.y + dir.y * length = planeHeight;
    //    // .'. length = (planeHeight - origin.y) / dir.y;

    //    // find point where y axis is zero
    //    Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
    //    float height = 0.0f;
    //    float length = (height - mouseRay.origin.y) / mouseRay.direction.y;
    //    Vector3 target = mouseRay.origin + mouseRay.direction * length;

    //    // check if left mouse pressed
    //    if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0)
    //    {
    //        Undo.RecordObject(enemyManager, "Add Point");
    //        enemyManager.m_editor_spawnpoints.Add(target);
    //        Debug.Log("add :: { " + target.x + ", " + target.y + ", " + target.z + " }");
    //    }

    //    // draw points on screen
    //    for (int i = 0; i < enemyManager.m_editor_spawnpoints.Count; i++)
    //    {
    //        Vector3 linePoint = enemyManager.m_editor_spawnpoints[(i + 1) % enemyManager.m_editor_spawnpoints.Count];
    //        Handles.color = Color.black;
    //        Handles.DrawDottedLine(enemyManager.m_editor_spawnpoints[i], linePoint, 4);
    //        Handles.color = Color.magenta;
    //        Handles.DrawSolidDisc(enemyManager.m_editor_spawnpoints[i], Vector3.up, 0.5f);
    //    }

    //    // prevent unity from deselecting object
    //    if (guiEvent.type == EventType.layout)
    //    {
    //        HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    //    }
    //}
}