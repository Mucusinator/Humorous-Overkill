using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KILL : MonoBehaviour {

	void OnTriggerEnter(Collider c)
    {
        if (c.transform.tag == "Enemy")
        {
            Destroy(c.gameObject);
        }
    }
}
