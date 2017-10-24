using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawnManager : MonoBehaviour
{
    public bool m_TEST_SPAWN = false;

    public Vector2 m_region = new Vector2(1, 1);
    public List<Vector2> m_targets = new List<Vector2>();
    public List<EnemyWave> m_waves = new List<EnemyWave>();
    public Color m_debugColor1 = new Color(0.0f, 1.0f, 0.0f, 0.5f);
    public Color m_debugColor2 = new Color(0.0f, 0.5f, 0.0f, 0.5f);

    void Start ()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.size = new Vector3(m_region.x, 1, m_region.y);
        collider.center = new Vector3(0, collider.size.y / 2, 0);
        collider.isTrigger = true;
    }

    void Update()
    {
        if (m_TEST_SPAWN)
        {
            NextSpawn();
            m_TEST_SPAWN = false;
        }
    }

    public void NextSpawn()
    {
        EnemyWave wave = m_waves[0];
        if (wave.units.Count == 0) return;

        int index = Random.Range(0, wave.units.Count - 1);
        Vector2 target = m_targets[Random.Range(0, m_targets.Count - 1)];
        EnemyUnit unit = wave.units[index];
        

        unit.amount--;
        Destroy(Instantiate(wave.prefabs[unit.index], transform.position + new Vector3(target.x, 0, target.y), new Quaternion()), 5);


        if (unit.amount == 0) wave.units.RemoveAt(index);

        wave.units[index] = unit;
        m_waves[0] = wave;
    }




    public void OnDrawGizmos()
    {
        Gizmos.color = m_debugColor1;
        Gizmos.DrawCube(transform.position, new Vector3(m_region.x, 0, m_region.y));

        Gizmos.color = m_debugColor2;
        foreach (Vector2 target in m_targets)
        {
            Gizmos.DrawSphere(transform.position + new Vector3(target.x, 0, target.y), 0.1f);
        }
    }
}
