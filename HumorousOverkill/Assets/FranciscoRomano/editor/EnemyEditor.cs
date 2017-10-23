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

        Ray mouseRay = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition);
        float height = 0.0f;
        float length = (height - mouseRay.origin.y) / mouseRay.direction.y;
        Vector3 target = mouseRay.origin + mouseRay.direction * length;

        if (guiEvent.type == EventType.mouseDown && guiEvent.button == 0)
        {
            enemyManager.m_editor_spawnpoints.Add(target);
            Debug.Log("add :: { " + target.x + ", " + target.y + ", " + target.z + " }");
        }

        foreach (Vector3 position in enemyManager.m_editor_spawnpoints)
        {
            Handles.DrawSolidDisc(position, Vector3.up, 0.5f);
        }

        // prevent unity from deselecting object
        if (guiEvent.type == EventType.layout)
        {
            HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
    }
}