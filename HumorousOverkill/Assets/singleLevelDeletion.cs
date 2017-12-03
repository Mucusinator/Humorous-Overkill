using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This is a script fully created by Zackary Direen. 
/// it is responsible for getting rid of excess rooms and spawners that are not being used.
/// while also activating doors to not allow the player to go back, as well as to not see the deleted room.
/// </summary>
public class singleLevelDeletion : MonoBehaviour {

    //Takes a 4 game objects, a box Collider(the trigger), the level, the door, and if applicable, a box collider for behind the door.
    // (to prevent enemies going through the door.
    public GameObject boxCollider, level, door, behindDoor;


    /// <summary>
    /// This is a on trigger enter event.
    /// </summary>
    /// <param name="other"> the other object that collided with the boxCollider</param>
    void OnTriggerEnter(Collider other)
    {
        // If the player hit the trigger.
        if (other.gameObject.tag == "Player")
        {
            // disablew the level
            level.SetActive(false);

            //if applicable, enable the door.
            if (door != null)
            {
                door.SetActive(true);
            }
            // disable the trigger.
            boxCollider.SetActive(false);
            // if applicable, block behind the door with a giant collider.
            if (behindDoor != null)
            {
                behindDoor.SetActive(true);
            }
        }
    }
}
