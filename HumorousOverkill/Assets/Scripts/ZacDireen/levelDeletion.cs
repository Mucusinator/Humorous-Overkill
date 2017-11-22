using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDeletion : MonoBehaviour {

    //public GameObject RoomModel;
    //public GameObject EnemySpawner;
    public GameObject LevelOne, LevelTwo, LevelThree;
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
                    break;
                case 2:
                    LevelTwo.SetActive(false);
                    break;
                case 3:
                    LevelThree.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }
}
