using System;
using UnityEngine;
using UnityEditor;
using EventHandler;
using System.Collections;
using System.Collections.Generic;

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
            points[1] = new Vector3(points[0].x, points[0].y, points[2].z);
            points[2] = GetMousePoint(e);
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
        m_collider.center = new Vector3(offsetX, m_collider.size.y / 2, offsetZ);
    }
    public void prepair()
    {
        index = 0;
        isEditing = false;
        // update collider
        m_collider.center = new Vector3(m_collider.center.x, m_collider.size.y / 2, m_collider.center.z);
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