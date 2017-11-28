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
    public void Reset()
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
            foreach (GameObject door in doors)
            {
                // make sure door script is not null
                if (door.GetComponent<doorScript>() != null)
                {
                    door.GetComponent<doorScript>().closeDoor();
                }
                else
                {
                    Debug.Log("door did not have doorScript attached");
                }
            }
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
        foreach (GameObject door in doors)
        {
            // make sure door script is not null
            if (door.GetComponent<doorScript>() != null)
            {
                door.GetComponent<doorScript>().openDoor();
            }
            else
            {
                Debug.Log("door did not have doorScript attached");
            }
        }
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
    public void HandleMessage(object sender, __eArg<GameEvent> e)
    {
        if (sender == (object)this) return;
        switch (e.arg)
        {
            case GameEvent.STATE_START:
            case GameEvent.STATE_RESTART:
                Reset();
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
    }
    public override bool HandleEvent(GameEvent e)
    {
        HandleMessage(null, new __eArg<GameEvent>(e, this, null, typeof(EnemySpawner)));
        return true;
    }
}