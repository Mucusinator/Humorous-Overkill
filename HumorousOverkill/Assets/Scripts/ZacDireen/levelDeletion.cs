using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDeletion : MonoBehaviour {

    //public GameObject RoomModel;
    //public GameObject EnemySpawner;
    public GameObject LevelOne, LevelTwo, LevelThree;

    public GameObject DoorOne, DoorTwo, DoorThree;

    public GameObject ColliderOne, ColliderTwo, ColliderThree;

    //private int index = 0;



    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (gameObject == ColliderOne)
            {
                LevelOne.SetActive(false);
                if (DoorOne != null)
                {
                    DoorOne.SetActive(true);
                }
                gameObject.SetActive(false);
            }
            if (gameObject == ColliderTwo)
            {
                LevelTwo.SetActive(false);
                if (DoorTwo != null)
                {
                    DoorTwo.SetActive(true);
                }
                gameObject.SetActive(false);
            }
            if (gameObject == ColliderThree)
            {
                LevelThree.SetActive(false);
                if (DoorThree != null)
                {
                    DoorThree.SetActive(true);
                }
                gameObject.SetActive(false);
            }
        }
    }
}
