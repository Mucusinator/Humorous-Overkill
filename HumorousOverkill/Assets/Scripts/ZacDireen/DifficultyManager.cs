using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour {

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
        if (sender != (object)this) return;
        switch (e.arg)
        {
            case GameEvent.DIFFICULTY_EASY:
                playerManager.m_playerInfo.m_playerHealth = 150;
                combinedScript.PelletDamage = 5.0f;
                combinedScript.RifleDamage = 40.0f;
                enemyManager.defaultDonutInfo.health = 25;
                enemyManager.defaultDroneInfo.health = 25;
           
                foreach (FranciscoRomano.Spawn.Group group in enemyManager.spawner.stage.groups)
                {
                    group.rate = 5.0f;
                    foreach (FranciscoRomano.Spawn.Unit unit in group.units)
                    {
                        foreach (FranciscoRomano.Spawn.Point point in unit.points)
                        {
                            point.amount = Random.Range(3,5);
                        }
                    }
                }
                enemyManager.defaultDonutInfo.pickupDropRate = 0.75f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.75f;

                break;
            case GameEvent.DIFFICULTY_MEDI:


                break;
            case GameEvent.DIFFICULTY_HARD:
                playerManager.m_playerInfo.m_playerHealth = 75;
                enemyManager.defaultDonutInfo.health = 80;
                enemyManager.defaultDroneInfo.health = 80;
                combinedScript.PelletDamage = 2.0f;
                combinedScript.RifleDamage = 25.0f;
                foreach (FranciscoRomano.Spawn.Group group in enemyManager.spawner.stage.groups)
                {
                    group.rate = 3.0f;
                    foreach (FranciscoRomano.Spawn.Unit unit in group.units)
                    {
                        foreach (FranciscoRomano.Spawn.Point point in unit.points)
                        {
                            point.amount = Random.Range(7, 10);
                        }
                    }
                }
                enemyManager.defaultDonutInfo.pickupDropRate = 0.3f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.3f;

                break;
            case GameEvent.DIFFICULTY_NM:
                
                playerManager.m_playerInfo.m_playerHealth = 25;
                enemyManager.defaultDonutInfo.health = 120;
                enemyManager.defaultDroneInfo.health = 120;
                combinedScript.PelletDamage = 1.5f;
                combinedScript.RifleDamage = 15.0f;
                foreach (FranciscoRomano.Spawn.Group group in enemyManager.spawner.stage.groups)
                {
                    group.rate = 1.0f;
                    foreach (FranciscoRomano.Spawn.Unit unit in group.units)
                    {
                        foreach (FranciscoRomano.Spawn.Point point in unit.points)
                        {
                            point.amount = Random.Range(10, 15);
                        }
                    }
                }
                enemyManager.defaultDonutInfo.pickupDropRate = 0.05f;
                enemyManager.defaultDroneInfo.pickupDropRate = 0.05f;
                NoSpawnPoints = true;
                break;
        }
    }
}

