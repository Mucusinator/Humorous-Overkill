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
    public float pickupDropRate; // chance to drop ammo (0 = never 1 = always)
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
    public float maximumTargetHeight;
    public float accuracy;
    public float pickupDropRate;
    public float targetRadius;
    public float avoidRadius;
    public float errorMargin;
}

public class EnemyManager : EventHandler.EventHandle
{
    // :: variables
    private float elapsedTime = 0.0f;
    [HideInInspector]
    public EnemySpawner spawner = null;
    public DroneEnemyInfo defaultDroneInfo;
    public DonutEnemyInfo defaultDonutInfo;

    // :: functions
    public override void Awake()
    {
        base.Awake();
        EventManager<GameEvent>.Add(HandleMessage);
    }
    void Start()
    {
        EventManager<GameEvent>.InvokeGameState(this, null, defaultDroneInfo, GetType(), GameEvent._NULL_);
    }

    //void OnGUI()
    //{
    //    // check spawner status
    //    if (spawner == null) return;
    //    // draw box
    //    GUI.Box(new Rect(0, 0, 220, 180), "");
    //    // draw stage status
    //    GUI.Label(new Rect(10, 20, 200, 20), "-------------------- Stage --------------------");
    //    GUI.Label(new Rect(10, 40, 200, 20), "is complete? = " + spawner.IsStageComplete());
    //    // draw stage wave status
    //    GUI.Label(new Rect(10,  60, 200, 20), "--------------- Current Group ----------------");
    //    GUI.Label(new Rect(10,  80, 200, 20), "spawn rate = " + spawner.stage.current.rate);
    //    GUI.Label(new Rect(10, 100, 200, 20), "is complete? = " + spawner.IsGroupComplete());
    //    GUI.Label(new Rect(10, 120, 200, 20), "active units = " + spawner.units);
    //    GUI.Label(new Rect(10, 140, 200, 20), "----------------------------------------------");
    //}
    void Update()
    {
        // check spawner status
        if (spawner == null) return;
        // check spawner stage status
        if (!spawner.IsStageComplete())
        {
            // check spawner group status
            if (spawner.IsGroupComplete())
            {
                // next group
                elapsedTime = Time.time;
                spawner.HandleEvent(GameEvent.ENEMY_SPAWNER_NEXT);
            }
            // check spawner elapsed time
            else if (Time.time - elapsedTime > spawner.stage.current.rate)
            {
                // create unit
                elapsedTime = Time.time;
                spawner.HandleEvent(GameEvent.ENEMY_SPAWNER_CREATE);
            }
        }
        else
        {
            // finish spawner
            spawner.HandleEvent(GameEvent.ENEMY_SPAWNER_FINISH);
            spawner = null;
        }
    }
    // :: functions [events]
    public void HandleMessage(object sender, __eArg<GameEvent> e)
    {
        if (sender == (object)this) return;
        switch (e.arg)
        {
            case GameEvent.ENEMY_SPAWNER_CREATE:
            case GameEvent.ENEMY_SPAWNER_REMOVE:
                if (spawner == null) return;
                spawner.HandleEvent(e.arg);
                break;
        }
    }
    public override bool HandleEvent(GameEvent e)
    {
        HandleMessage(this, new __eArg<GameEvent>(e, this, null, typeof(EnemyManager)));
        return true;
    }
    public override bool HandleEvent(GameEvent e, Object value)
    {
        switch(e)
        {
            case GameEvent.CLASS_TYPE_ENEMY_SPAWNER:
                spawner = (EnemySpawner)value;
                if (!spawner.IsStageComplete())
                {
                    spawner.HandleEvent(GameEvent.ENEMY_SPAWNER_BEGIN);
                }
                break;
        }
        return true;
    }
}