using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class killDrone : MonoBehaviour
{
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hitInfo;
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 1000, Color.yellow);
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.name == "Cupcake(Clone)")
                {
                    hitInfo.collider.gameObject.GetComponent<DroneAI>().HandleEvent(GameEvent.ENEMY_DAMAGED, 1);
                }
            }
        }
	}
}
