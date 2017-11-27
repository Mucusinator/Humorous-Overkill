using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{

    public CombinedScript combinedScript;
    public EnemyManager enemyManager;
    public PlayerManager playerManager;
    public bool NoSpawnPoints;
    private List<FranciscoRomano.Spawn.Stage> copyStages = new List<FranciscoRomano.Spawn.Stage>();

    void Start()
    {
        EventManager<GameEvent>.Add(HandleEvent);
        EnemySpawner[] spawners = GameObject.FindObjectsOfType<EnemySpawner>();

        foreach (EnemySpawner copy in spawners)
        {
            copyStages.Add(new FranciscoRomano.Spawn.Stage(copy.stage));
        }
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

                UpdateSpawners(3, 5, 0.75f);
                
                enemyManager.defaultDonutInfo.pickupDropRate = 0.75f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.75f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_MEDI:
                playerManager.m_playerInfo.m_playerHealth = 100;
                enemyManager.defaultDonutInfo.health = 50;
                enemyManager.defaultDroneInfo.health = 50;
                combinedScript.PelletDamage = 3.5f;
                combinedScript.RifleDamage = 32.5f;

                UpdateSpawners(5, 7, 1.0f);

                enemyManager.defaultDonutInfo.pickupDropRate = 0.3f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.3f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_HARD:
                playerManager.m_playerInfo.m_playerHealth = 75;
                enemyManager.defaultDonutInfo.health = 80;
                enemyManager.defaultDroneInfo.health = 80;
                combinedScript.PelletDamage = 2.0f;
                combinedScript.RifleDamage = 25.0f;

                UpdateSpawners(7, 10, 1.5f);

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

                UpdateSpawners(10, 15, 2.0f);
                
                enemyManager.defaultDonutInfo.pickupDropRate = 0.05f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.05f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                NoSpawnPoints = true;
                break;
        }
    }

    void UpdateSpawners(int min, int max, float multiplier)
    {
        EnemySpawner[] spawners = GameObject.FindObjectsOfType<EnemySpawner>();

        Debug.Log(spawners.Length);

        for(int i = 0; i < spawners.Length; i++)
        {
            for (int ii = 0; ii < spawners[i].stage.groups.Count; ii++)
            {
                spawners[i].stage.groups[ii].rate = copyStages[i].groups[ii].rate * multiplier;

                for (int iii = 0; iii < spawners[i].stage.groups[ii].units.Count; iii++)
                {
                    for (int iiii = 0; iiii < spawners[i].stage.groups[ii].units[iii].points.Count; iii++)
                    {
                        spawners[i].stage.groups[ii].units[iii].points[iiii].amount = Random.Range(min, max);
                    }
                }
            }
        }
    }
}

