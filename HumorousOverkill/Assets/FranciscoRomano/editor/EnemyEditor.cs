using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(EnemyManager))]
public class EnemyEditor : Editor
{
    EnemyManager enemyManager;

    void OnEnable()
    {
        enemyManager = target as EnemyManager;
    }

    void OnSceneGUI()
    {
        Event guiEvent = Event.current;

        // Get Point where Y-Axis is = 0.0f
        // origin + dir * length = P
        // origin.y + dir.y * length = planeHeight;
        // .'. length = (planeHeight - origin.y) / dir.y;

        // find point where y axis is zero
        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float height = 0.0f;
        float length = (height - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 target = mouseRay.origin + mouseRay.direction * length;

        // check if left mouse pressed
        if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0)
        {
            Undo.RecordObject(enemyManager, "Add Point");
            enemyManager.m_editor_spawnpoints.Add(target);
            Debug.Log("add :: { " + target.x + ", " + target.y + ", " + target.z + " }");
        }
        
        // draw points on screen
        for (int i = 0; i < enemyManager.m_editor_spawnpoints.Count; i++)
        {
            Vector3 linePoint = enemyManager.m_editor_spawnpoints[(i + 1) % enemyManager.m_editor_spawnpoints.Count];
            Handles.color = Color.black;
            Handles.DrawDottedLine(enemyManager.m_editor_spawnpoints[i], linePoint, 4);
            Handles.color = Color.magenta;
            Handles.DrawSolidDisc(enemyManager.m_editor_spawnpoints[i], Vector3.up, 0.5f);
        }

        // prevent unity from deselecting object
        if (guiEvent.type == EventType.layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }
}