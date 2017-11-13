using UnityEngine;
using EventHandler;
using System.Collections;
using FranciscoRomano.Spawn;
using System.Collections.Generic;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
[BindListener("EnemyManager", typeof(EnemyManager))]
public class EnemySpawner : EventHandle
{
    // :: variables
    [HideInInspector]
    public int units = 0;
    public bool active = false;
    [HideInInspector]
    public Stage stage = new Stage();
    public List<GameObject> doors = new List<GameObject>();
    // :: functions
    public void Restart()
    {
        units = 0;
        active = false;
        stage.Reset();
        foreach (GameObject door in doors) door.SetActive(false);
    }
    public void SpawnerNext()
    {
        stage.Next();
    }
    public void SpawnerBegin()
    {
        if (!active)
        {
            active = true;
            stage.Next();
            foreach (GameObject door in doors) door.SetActive(true);
        }
    }
    public void SpawnerCreate()
    {
        if (!stage.IsGroupEmpty())
        {
            units++;
            stage.Create(new Vector3(), new Quaternion(), transform);
        }
    }
    public void SpawnerFinish()
    {
        foreach (GameObject door in doors) door.SetActive(false);
    }
    public void SpawnerRemove()
    {
        if (units > 0) units--;
    }
    public bool IsStageComplete()
    {
        return stage.IsEmpty() && units == 0;
    }
    public bool IsGroupComplete()
    {
        return stage.IsGroupEmpty() && units == 0;
    }
    // :: functions [events]
    void OnTriggerEnter(Collider collider)
    {
        // check if player and not complete
        if (collider.tag == "Player")
        {
            // notify manager
            GetEventListener("EnemyManager").HandleEvent(GameEvent.CLASS_TYPE_ENEMY_SPAWNER, this);
        }
    }
    public override bool HandleEvent(GameEvent e)
    {
        switch (e)
        {
            case GameEvent.STATE_RESTART:
                Restart();
                break;
            case GameEvent.ENEMY_SPAWNER_NEXT:
                SpawnerNext();
                break;
            case GameEvent.ENEMY_SPAWNER_BEGIN:
                SpawnerBegin();
                break;
            case GameEvent.ENEMY_SPAWNER_CREATE:
                SpawnerCreate();
                break;
            case GameEvent.ENEMY_SPAWNER_FINISH:
                SpawnerFinish();
                break;
            case GameEvent.ENEMY_SPAWNER_REMOVE:
                SpawnerRemove();
                break;
        }
        return true;
    }
}