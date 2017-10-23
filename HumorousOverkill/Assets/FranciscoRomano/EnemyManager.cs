using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Struct holding all the Game info
[System.Serializable] struct EnemyInfo
{
    public int m_enemyHealth_type1;
    public int m_enemyHealth_type2;
    public float m_enemySpeed_type1;
    public float m_enemySpeed_type2;
    public float m_enemyDamage_type1;
    public float m_enemyDamage_type2;
    public int m_enemyAmmoDropRate;
}

public class EnemyManager : GameEventListener
{
    [SerializeField] EnemyInfo m_enemyInfo;

    //[HideInInspector]
    public List<Vector3> m_editor_spawnpoints = new List<Vector3>();

    void OnDrawGizmos()
    {
        //Gizmos.color = new Color(0, 1, 0, 0.3f);
        //foreach (Vector3 position in m_editor_spawnpoints)
        //{
        //    Gizmos.DrawSphere(position, 1);
        //}

    }

    public override void HandleEvent (GameEvent e)
    {

    }

    void OnSceneGUI()
    {
        if (Event.current.type == EventType.MouseDown)
        {
            Debug.Log("test");
        }
    }


}