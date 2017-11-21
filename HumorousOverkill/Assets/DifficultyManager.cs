using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour {

    public CombinedScript combinedScript;
    public EnemyManager enemyManager;
    public PlayerManager playerManager;


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
                enemyManager.defaultDonutInfo.health = 40;
                enemyManager.defaultDroneInfo.health = 40;
                //enemyManager.spawner.stage.current.rate = 5;
                foreach (FranciscoRomano.Spawn.Group group in enemyManager.spawner.stage.groups)
                {
                    group.rate = 5;
                }

                break;
            case GameEvent.DIFFICULTY_MEDI:
                break;
            case GameEvent.DIFFICULTY_HARD:
                break;
            case GameEvent.DIFFICULTY_NM:
                break;
        }
    }
}

