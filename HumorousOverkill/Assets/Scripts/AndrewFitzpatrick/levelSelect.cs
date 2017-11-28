using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelSelect : MonoBehaviour
{
    private GameObject player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindObjectOfType<Player>().gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            player.transform.position = new Vector3(68.2f, 4.1f, 50.7f);
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.transform.position = new Vector3(47.0f, 7.8f, 28.9f);
            player.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.transform.position = new Vector3(40.0f, 1.0f, 41.6f);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.transform.position = new Vector3(16.2f, 22.3f, 59.8f);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
