using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDeletion : MonoBehaviour {

    //public GameObject RoomModel;
    //public GameObject EnemySpawner;
    public GameObject LevelOne, LevelTwo, LevelThree;
    public GameObject DoorOne, DoorTwo, DoorThree;
    private int index = 0;



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            index++;
            switch (index)
            {
                case 1:
                    LevelOne.SetActive(false);
                    if (DoorOne != null)
                    {
                        DoorOne.SetActive(true);
                    }
                    break;
                case 2:
                    LevelTwo.SetActive(false);
                    if (DoorTwo != null)
                    {
                        DoorTwo.SetActive(true);
                    }
                    break;
                case 3:
                    LevelThree.SetActive(false);
                    if (DoorThree != null)
                    {
                        DoorThree.SetActive(true);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
