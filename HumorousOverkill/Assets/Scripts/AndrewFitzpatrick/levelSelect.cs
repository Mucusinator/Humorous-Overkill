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

        }
	}
}
