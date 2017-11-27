using System;
using UnityEngine;
using UnityEditor;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

namespace Spawner
{
    class RegionEditor
    {
        // :: variables
        public int index;
        public float height;
        public Vector3[] points;
        public static Color MainColor = new Color(1, 0, 1);
        // :: constructor
        public RegionEditor(GameObject obj)
        {
            // get object components
            Transform transform = obj.GetComponent<Transform>();
            BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
            // calculate editor values
            index = 0;
            points = new Vector3[4];
            height = boxCollider.size.y;
            Vector3 position = transform.position + boxCollider.center;
            points[0] = new Vector3( boxCollider.size.x,-position.y, boxCollider.size.z) / 2.0f + position;
            points[1] = new Vector3( boxCollider.size.x,-position.y,-boxCollider.size.z) / 2.0f + position;
            points[2] = new Vector3(-boxCollider.size.x,-position.y,-boxCollider.size.z) / 2.0f + position;
            points[3] = new Vector3(-boxCollider.size.x,-position.y, boxCollider.size.z) / 2.0f + position;
        }
        // :: class functions
        public static void SetRegion(RegionEditor editor, GameObject obj)
        {
            // get object components
            Transform transform = obj.GetComponent<Transform>();
            BoxCollider boxCollider = obj.GetComponent<BoxCollider>();
            // update object components
            Vector3 size = new Vector3();
            size.y = Mathf.Abs(editor.height);
            size.x = Mathf.Abs(editor.points[2].x - editor.points[0].x);
            size.z = Mathf.Abs(editor.points[2].z - editor.points[0].z);
            float offsetX = Mathf.Lerp(editor.points[0].x, editor.points[2].x, 0.5f) + transform.position.x;
            float offsetZ = Mathf.Lerp(editor.points[0].z, editor.points[2].z, 0.5f) + transform.position.z;
            // update collider
            boxCollider.size = size;
            boxCollider.center = new Vector3(0, 0, 0);
            transform.position = new Vector3(offsetX, size.y / 2, offsetZ);

        }
        public static void OnSceneGUI(RegionEditor editor, Event e)
        {
            // display gui
            OnSceneGUI_Update(editor, e);
            OnSceneGUI_Render(editor, e);
        }
        private static void OnSceneGUI_Update(RegionEditor editor, Event e)
        {
            // check mouse down
            if (e.type == EventType.mouseDown && e.button == 0)
            {
                // calculate position
                editor.points[editor.index] = GetMousePosition(e, editor.height);
                // update current index
                editor.index = (editor.index + 2) % 4;
            }
            // check current index
            else if (editor.index > 0)
            {
                // calculate position
                editor.points[2] = GetMousePosition(e, editor.height);
                editor.points[1] = new Vector3(editor.points[0].x, editor.points[0].y, editor.points[2].z);
                editor.points[3] = new Vector3(editor.points[2].x, editor.points[0].y, editor.points[0].z);
            }
            // prevent unity from deselecting object
            if (e.type == EventType.layout) HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
        }
        private static void OnSceneGUI_Render(RegionEditor editor, Event e)
        {
            // set values
            Handles.color = MainColor;
            Vector3 sizeY = Vector3.up * editor.height;
            // draw region height
            Handles.DrawLine(editor.points[0], editor.points[0] + sizeY);
            Handles.DrawLine(editor.points[1], editor.points[1] + sizeY);
            Handles.DrawLine(editor.points[2], editor.points[2] + sizeY);
            Handles.DrawLine(editor.points[3], editor.points[3] + sizeY);
            // draw region box base
            Handles.DrawDottedLine(editor.points[0], editor.points[1], 5);
            Handles.DrawDottedLine(editor.points[1], editor.points[2], 5);
            Handles.DrawDottedLine(editor.points[2], editor.points[3], 5);
            Handles.DrawDottedLine(editor.points[3], editor.points[0], 5);
        }
        private static Vector3 GetMousePosition(Event e, float height)
        {
            // get screen to world position
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            // calculate point length
            float length = (height - ray.origin.y) / ray.direction.y;
            // calculate world coord
            Vector3 world = ray.origin + ray.direction * length;
            // return world coord
            return world;
        }
    }
}
class EnemySpawnerRegionEditor
{
    // :: variables
    public int index;
    public bool isEditing;
    public float maximumX;
    public float minimumY;
    public Vector3[] points;
    public Vector3 position;
    private Transform m_transform;
    private BoxCollider m_collider;
    public static Color color = new Color(1.0f, 0.0f, 1.0f, 0.5f);
    // :: initializers
    public EnemySpawnerRegionEditor(EnemySpawner spawner)
    {
        // initialize
        index = 0;
        points = new Vector3[4];
        isEditing = false;
        m_collider = spawner.GetComponent<BoxCollider>();
        m_transform = spawner.transform;
    }
    // :: class functions
    public void render(Event e)
    {
        // set values
        Handles.color = color;
        Vector3 sizeY = Vector3.up * maximumX;
        // draw region height
        Handles.DrawLine(position + points[0], position + points[0] + sizeY);
        Handles.DrawLine(position + points[1], position + points[1] + sizeY);
        Handles.DrawLine(position + points[2], position + points[2] + sizeY);
        Handles.DrawLine(position + points[3], position + points[3] + sizeY);
        // draw region box base
        Handles.DrawDottedLine(position + points[0], position + points[1], 5);
        Handles.DrawDottedLine(position + points[1], position + points[2], 5);
        Handles.DrawDottedLine(position + points[2], position + points[3], 5);
        Handles.DrawDottedLine(position + points[3], position + points[0], 5);
    }
    public void update(Event e)
    {
        // check if editing
        if (index > 0)
        {
            // calculate position
            points[2] = GetMousePoint(e);
            points[1] = new Vector3(points[0].x, points[0].y, points[2].z);
            points[3] = new Vector3(points[2].x, points[0].y, points[0].z);
        }
        // check if left mouse pressed
        if (e.type == EventType.mouseDown && e.button == 0)
        {
            // calculate position
            points[index * 2] = GetMousePoint(e);
            // check current index
            index = (index == 1) ? 0 : index + 1;
        }
        // prevent unity from deselecting object
        if (e.type == EventType.layout) HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
    }
    public void setdata()
    {
        index = 0;
        // calculate size
        Vector3 size = new Vector3();
        size.y = Mathf.Abs(maximumX);
        size.x = Mathf.Abs(points[2].x - points[0].x);
        size.z = Mathf.Abs(points[2].z - points[0].z);
        float offsetX = Mathf.Lerp(points[0].x, points[2].x, 0.5f) + position.x;
        float offsetZ = Mathf.Lerp(points[0].z, points[2].z, 0.5f) + position.z;
        // update collider
        m_collider.size = size;
        m_collider.center = new Vector3(0, 0, 0);
        m_collider.GetComponent<Transform>().position = new Vector3(offsetX, m_collider.size.y / 2, offsetZ);
    }
    public void prepair()
    {
        index = 0;
        isEditing = false;
        // update collider
        m_transform.position = new Vector3(m_transform.position.x, m_collider.size.y / 2, m_transform.position.z);
        // calculate new values
        maximumX = m_collider.size.y;
        minimumY = m_transform.position.y;
        position = m_transform.position + m_collider.center;
        points[0] = new Vector3( m_collider.size.x,-m_collider.size.y, m_collider.size.z) / 2.0f;
        points[1] = new Vector3( m_collider.size.x,-m_collider.size.y,-m_collider.size.z) / 2.0f;
        points[2] = new Vector3(-m_collider.size.x,-m_collider.size.y,-m_collider.size.z) / 2.0f;
        points[3] = new Vector3(-m_collider.size.x,-m_collider.size.y, m_collider.size.z) / 2.0f;
    }
    private Vector3 GetMousePoint(Event e)
    {
        // get screen to world position
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        // calculate point length
        float length = (minimumY - ray.origin.y) / ray.direction.y;
        // calculate world coord
        Vector3 world = ray.origin + ray.direction * length;
        // return local coord
        return world - position;
    }
}