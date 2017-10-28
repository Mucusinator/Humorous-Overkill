using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : GameEventListener
{
    public int spawnRate = 0;
    public int activeUnits = 0;
    public bool isWaveComplete = false;
    public List<FR.SpawnWave> waves = new List<FR.SpawnWave>();

    public void Start()
    {

    }

    public override void HandleEvent(GameEvent e)
    {
        // check status
        if (waves[0].isEmpty()) return;
        // check game event
        switch (e)
        {
            // remove unit
            case GameEvent.ENEMY_DIED:
                activeUnits--;
                break;
            // create unit
            case GameEvent.ENEMY_SPAWN:
                activeUnits++;
                GameObject unit = waves[0].Instantiate(Quaternion.identity, transform);
                Destroy(unit, 5);
                break;
        }
    }

    public void OnDrawGizmos()
    {
        if (waves.Count == 0) return;

        Gizmos.color = Color.green;
        foreach (FR.SpawnPoint point in waves[0].points)
        {
            Gizmos.DrawSphere(point.position + transform.position, 0.1f);
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        // check if player
        Debug.Log("Im aliveeeeee!!!!!!!!!!");
        if (collider.tag == "Player")
        {
            foreach (FR.SpawnPoint point in waves[0].points)
            {
                foreach (FR.SpawnUnit unit in point.units)
                {
                    point.Instantiate(Quaternion.identity, transform);
                }
            }
        }
    }

    public override void HandleEvent(GameEvent e, float value)
    {
        base.HandleEvent(e, value);
    }
}

// ################################################################################################## //
// ############################################# LATEST ############################################# //
// ################################################################################################## //
//void Start()
//{

//}

//void CheckCurrentState()
//{
//    // check stage state
//    if (m_stageComplete == false)
//    {
//        // check wave state
//        if (m_waveComplete == false)
//        {
//            // check wave active units
//            if (m_waves[0].activeUnits == 0)
//            {
//                m_waves.RemoveAt(0);
//                // update wave state
//                m_waveComplete = true;
//                // notify enemy manager
//                m_manager.HandleEvent(GameEvent.ENEMY_STATE_WAVE_END);
//            }
//        }
//        // check for waves left
//        if (m_waves.Count == 0)
//        {
//            // update stage state
//            m_stageComplete = true;
//            // notify enemy manager
//            m_manager.HandleEvent(GameEvent.ENEMY_STATE_STAGE_END);
//        }
//    }
//}

//public bool NextUnit()
//{
//    // validate call
//    if (m_waveComplete) return false;
//    if (m_stageComplete) return false;
//    if (m_waves.Count == 0) return false;
//    // spawn next unit
//    m_waves[0].Instantiate(m_points[Random.Range(0, m_points.Count)], transform);
//    // return true
//    return true;
//}

//public void OnTriggerEnter(Collider collider)
//{
//    // check if player entered
//    if (collider.tag == "Player")
//    {
//        // pass current spawner to manager
//        m_manager.HandleEvent(GameEvent.CLASS_TYPE_ENEMY_SPAWNER, this);
//    }
//}

//public override void HandleEvent(GameEvent e)
//{
//    // check if complete
//    if (m_stageComplete) return;
//    // check current event
//    switch (e)
//    {
//        // check if enemy died
//        case GameEvent.ENEMY_STATE_DIED:
//            // update active units
//            //SpawnerWave wave = m_waves[0];
//            //wave.activeUnits -= 1;
//            //m_waves[0] = wave;
//            m_waves[0].RemoveActiveUnit();
//            // check current state
//            CheckCurrentState();
//            // send event to manager
//            HandleEvent(e, m_waves[0].activeUnits);
//            break;
//        // check current wave state
//        case GameEvent.ENEMY_STATE_WAVE_BEGIN:
//            // update wave state
//            m_waveComplete = false;
//            break;
//        // check current stage state
//        case GameEvent.ENEMY_STATE_STAGE_BEGIN:
//            // update stage state
//            m_stageComplete = false;
//            break;
//    }
//}

// ################################################################################################## //
// ################################################################################################## //
// ################################################################################################## //


//public bool m_TEST_SPAWN = false;

//public Vector2 m_region = new Vector2(1, 1);
//public List<Vector2> m_targets = new List<Vector2>();
//public List<EnemyWave> m_waves = new List<EnemyWave>();
//public Color m_debugColor1 = new Color(0.0f, 1.0f, 0.0f, 0.5f);
//public Color m_debugColor2 = new Color(0.0f, 0.5f, 0.0f, 0.5f);

//void Start ()
//{
//    BoxCollider collider = gameObject.AddComponent<BoxCollider>();
//    collider.size = new Vector3(m_region.x, 1, m_region.y);
//    collider.center = new Vector3(0, collider.size.y / 2, 0);
//    collider.isTrigger = true;
//}

//void Update()
//{
//    if (m_TEST_SPAWN)
//    {
//        NextSpawn();
//        m_TEST_SPAWN = false;
//    }
//}

//public void NextSpawn()
//{
//    EnemyWave wave = m_waves[0];
//    if (wave.units.Count == 0) return;

//    int index = Random.Range(0, wave.units.Count - 1);
//    Vector2 target = m_targets[Random.Range(0, m_targets.Count - 1)];
//    EnemyUnit unit = wave.units[index];


//    unit.amount--;
//    Destroy(Instantiate(wave.prefabs[unit.index], transform.position + new Vector3(target.x, 0, target.y), new Quaternion()), 5);


//    if (unit.amount == 0) wave.units.RemoveAt(index);

//    wave.units[index] = unit;
//    m_waves[0] = wave;
//}

//public void NextWave(SpawnWave)
//{

//}

//public void OnDrawGizmos()
//{

//}