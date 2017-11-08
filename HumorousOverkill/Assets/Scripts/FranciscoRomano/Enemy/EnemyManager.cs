using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Struct holding all the drone enemy info
[System.Serializable]
public struct DroneEnemyInfo
{
    public float targetRadius;
    public float errorMargin;
    public float damage;
    public float wanderSpeed;
    public float turnSpeed;
    public float avoidRadius;

    public float health; // health of the drone
    public float fireRate; // number of projectiles fired in one second
    public float accuracy; // accuracy of the drone when shooting (less is better)
    public float attackRange; // distance to fire at the player from
    public float projectileLifetime; // time that projectiles will exist before getting destroyed
    public float shotForce; // effects speed of projectiles
    public float ammoDropRate; // chance to drop ammo (0 = never 1 = always)
    public float explosionForce;
    public float explosionRadius;
}

// Struct holding all the donut enemy info
[System.Serializable]
public struct DonutEnemyInfo
{
    public float health;
    public float damage;
    public float fireRate;
    public float rollSpeed;
    public float turnSpeed;
    public float deployRange;
    public float hitRange;
    public float accuracy;
    public float ammoDropRate;
    public float targetRadius;
    public float avoidRadius;
    public float errorMargin;
}

public class EnemyManager : EventHandler.EventHandle
{
    // variables
    private float elapsedTime = 0.0f;
    [HideInInspector]
    public EnemySpawner spawner = null;

    public DroneEnemyInfo defaultDroneInfo;
    public DonutEnemyInfo defaultDonutInfo;

    // class functions [UnityEngine.MonoBehaviour]
    void OnGUI()
    {
        // check spawner status
        if (spawner == null) return;
        //if (!spawner.IsStageComplete())
        //{
            // draw box
            GUI.Box(new Rect(0, 0, 220, 180), "");
            // draw stage status
            GUI.Label(new Rect(10, 20, 200, 20), "-------------------- Stage --------------------");
            GUI.Label(new Rect(10, 40, 200, 20), "is complete? = " + spawner.IsStageComplete().ToString());
            // draw stage wave status
            GUI.Label(new Rect(10,  60, 200, 20), "---------------- Current  Wave ----------------");
            GUI.Label(new Rect(10,  80, 200, 20), "is empty? = " + spawner.IsWaveEmpty().ToString());
            GUI.Label(new Rect(10, 100, 200, 20), "is complete? = " + spawner.IsWaveComplete().ToString());
            GUI.Label(new Rect(10, 120, 200, 20), "spawn rate = " + spawner.GetWaveSpawnRate());
            GUI.Label(new Rect(10, 140, 200, 20), "active units = " + spawner.GetWaveActiveUnits());
            GUI.Label(new Rect(10, 160, 200, 20), "----------------------------------------------");
        //}
    }
    void Update()
    {
        // check spawner status
        if (spawner == null) return;
        if (!spawner.IsStageComplete())
        {
            // check wave status
            if (spawner.IsWaveComplete())
            {
                // next wave
                elapsedTime = Time.time;
                spawner.HandleEvent(GameEvent.ENEMY_WAVE_NEXT);
            }
            else
            {
                // check elapsed time
                if (Time.time - elapsedTime > spawner.GetWaveSpawnRate())
                {
                    // next unit
                    elapsedTime = Time.time;
                    //spawner.HandleEvent(GameEvent.ENEMY_DIED);
                    spawner.HandleEvent(GameEvent.ENEMY_SPAWN);
                }
            }
        }
        else
        {
            // ## [TEMP] ## update all colliders
            foreach (GameObject obj in spawner.doors)
            {
                obj.SetActive(false);
            }
        }
    }
    // class functions [EventHandle]
    public override bool HandleEvent(GameEvent e)
    {
        switch(e)
        {
            // enemy unit dies
            case GameEvent.ENEMY_DIED:
                if (spawner == null) return false;
                spawner.HandleEvent(e);
                break;
        }
        return true;
    }
    public override bool HandleEvent(GameEvent e, Object value)
    {
        switch(e)
        {
            // called on trigger enter
            case GameEvent.CLASS_TYPE_ENEMY_SPAWNER:
                spawner = (EnemySpawner)value;
                spawner.Begin();
                break;
        }
        return true;
    }
}