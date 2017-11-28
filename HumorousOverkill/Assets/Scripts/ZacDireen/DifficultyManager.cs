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

    public struct DifficultySettings
    {
        public int playerHealth;
        public float PelletDamage;
        public float RifleDamage;
        public int DonutHealth;
        public int DroneHealth;
        public float spawnMultiplier;
        public int PickupMultiplier;
    }

    DifficultySettings EasyMode;
    DifficultySettings HardMode;
    DifficultySettings NightmareMode;


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
        //Debug.Log(e.arg.ToString());
        switch (e.arg)
        {
            case GameEvent.DIFFICULTY_EASY:
                playerManager.m_playerInfo.m_playerHealth = EasyMode.playerHealth;
                combinedScript.PelletDamage = EasyMode.PelletDamage;
                combinedScript.RifleDamage = EasyMode.RifleDamage;
                enemyManager.defaultDonutInfo.health = EasyMode.DonutHealth;
                enemyManager.defaultDroneInfo.health = EasyMode.DroneHealth;

                UpdateSpawners(EasyMode.spawnMultiplier);

                enemyManager.defaultDonutInfo.pickupDropRate = EasyMode.PickupMultiplier;
                enemyManager.defaultDroneInfo.pickupDropRate = EasyMode.PickupMultiplier;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_MEDI:
                //playerManager.m_playerInfo.m_playerHealth = 100;
                //enemyManager.defaultDonutInfo.health = 50;
                //enemyManager.defaultDroneInfo.health = 50;
                //combinedScript.PelletDamage = 3.5f;
                //combinedScript.RifleDamage = 32.5f;

                //UpdateSpawners(5, 7, 1.0f);

                //enemyManager.defaultDonutInfo.pickupDropRate = 0.3f;
                //enemyManager.defaultDroneInfo.pickupDropRate = 0.3f;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_HARD:
                playerManager.m_playerInfo.m_playerHealth = HardMode.playerHealth;
                enemyManager.defaultDonutInfo.health = HardMode.DonutHealth;
                enemyManager.defaultDroneInfo.health = HardMode.DroneHealth;
                combinedScript.PelletDamage = HardMode.PelletDamage;
                combinedScript.RifleDamage = HardMode.RifleDamage;

                UpdateSpawners(HardMode.spawnMultiplier);

                enemyManager.defaultDonutInfo.pickupDropRate = HardMode.PickupMultiplier;
                enemyManager.defaultDroneInfo.pickupDropRate = HardMode.PickupMultiplier;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                break;
            case GameEvent.DIFFICULTY_NM:

                playerManager.m_playerInfo.m_playerHealth = NightmareMode.playerHealth;
                enemyManager.defaultDonutInfo.health = NightmareMode.DonutHealth;
                enemyManager.defaultDroneInfo.health = NightmareMode.DroneHealth;
                combinedScript.PelletDamage = NightmareMode.PelletDamage;
                combinedScript.RifleDamage = NightmareMode.RifleDamage;

                UpdateSpawners(NightmareMode.spawnMultiplier);
                
                enemyManager.defaultDonutInfo.pickupDropRate = NightmareMode.PickupMultiplier;
                enemyManager.defaultDroneInfo.pickupDropRate = NightmareMode.PickupMultiplier;
                EventManager<GameEvent>.InvokeGameState(this, null, null, typeof(GameManager), GameEvent.STATE_START);
                NoSpawnPoints = true;
                break;
        }
    }

    void UpdateSpawners(float multiplier)
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
                    for (int iiii = 0; iiii < spawners[i].stage.groups[ii].units[iii].points.Count; iiii++)
                    {
                        spawners[i].stage.groups[ii].units[iii].points[iiii].amount = (int)(copyStages[i].groups[ii].units[iii].points[iiii].amount * multiplier);
                    }
                }
            }
        }
    }
}

