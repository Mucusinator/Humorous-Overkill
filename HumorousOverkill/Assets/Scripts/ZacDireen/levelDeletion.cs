using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelDeletion : MonoBehaviour {

    public GameObject RoomModel;
    public GameObject EnemySpawner;




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            RoomModel.SetActive(false);
            EnemySpawner.SetActive(false);
        }
    }
}
