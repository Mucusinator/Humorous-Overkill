using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    public CombinedScript combinedScript;
    public EnemyManager enemyManager;
    public PlayerManager playerManager;
    public bool NoSpawnPoints;

    void Start()
    {
        EventManager<GameEvent>.Add(HandleEvent);
    }

    void Update()

    {
    }
    public void HandleEvent(object sender, __eArg<GameEvent> e)
    {
        // if this event is being sent to itself, just skip it
        if (sender == (object)this) return;
        Debug.Log(e.arg.ToString());
        switch (e.arg)
        {
            case GameEvent.DIFFICULTY_EASY:
                playerManager.m_playerInfo.m_playerHealth = 150;
                combinedScript.PelletDamage = 5.0f;
                combinedScript.RifleDamage = 40.0f;
                enemyManager.defaultDonutInfo.health = 25;
                enemyManager.defaultDroneInfo.health = 25;

                UpdateSpawners(3, 5, 5.0f);
                
                enemyManager.defaultDonutInfo.pickupDropRate = 0.75f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.75f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_MEDI:

                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_HARD:
                playerManager.m_playerInfo.m_playerHealth = 75;
                enemyManager.defaultDonutInfo.health = 80;
                enemyManager.defaultDroneInfo.health = 80;
                combinedScript.PelletDamage = 2.0f;
                combinedScript.RifleDamage = 25.0f;

                UpdateSpawners(7, 10, 3.0f);

                enemyManager.defaultDonutInfo.pickupDropRate = 0.3f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.3f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_NM:

                playerManager.m_playerInfo.m_playerHealth = 25;
                enemyManager.defaultDonutInfo.health = 120;
                enemyManager.defaultDroneInfo.health = 120;
                combinedScript.PelletDamage = 1.5f;
                combinedScript.RifleDamage = 15.0f;

                UpdateSpawners(10, 15, 1.0f);
                
                enemyManager.defaultDonutInfo.pickupDropRate = 0.05f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.05f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                NoSpawnPoints = true;
                break;
        }
    }

    void UpdateSpawners(int min, int max, float rate)
    {
        EnemySpawner[] spawners = GameObject.FindObjectsOfType<EnemySpawner>();

        Debug.Log(spawners.Length);

        foreach (EnemySpawner spawner in spawners)
        {
            foreach (FranciscoRomano.Spawn.Group group in spawner.stage.groups)
            {
                group.rate = rate;
                foreach (FranciscoRomano.Spawn.Unit unit in group.units)
                {
                    foreach (FranciscoRomano.Spawn.Point point in unit.points)
                    {
                        point.amount = Random.Range(min, max);
                    }
                }
            }
        }
    }
}

