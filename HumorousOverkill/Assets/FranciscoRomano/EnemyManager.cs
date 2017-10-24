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
    //[SerializeField] EnemyInfo m_enemyInfo;
    public List<EnemySpawner> m_enemySpawners = new List<EnemySpawner>();
    public List<EnemySpawnRegion> m_enemySpawnRegions = new List<EnemySpawnRegion>();

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (EnemySpawner spawner in m_enemySpawners)
        {
            if (spawner.target == null) continue;
            Gizmos.DrawWireSphere(spawner.target.transform.position, 1);
        }

        Gizmos.color = Color.green;
        foreach (EnemySpawnRegion region in m_enemySpawnRegions)
        {
            Gizmos.DrawWireCube(transform.position + region.position, new Vector3(region.size.x, 0, region.size.y));
        }
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

    public enum EnemyType
    {
        TYPE_0,
        TYPE_1,
    }

    [System.Serializable]
    public struct EnemySpawner
    {
        public EnemyType type;
        public GameObject target;
    }

    [System.Serializable]
    public struct EnemySpawnRegion
    {
        public Vector2 size;
        public Vector3 position;
    }
}