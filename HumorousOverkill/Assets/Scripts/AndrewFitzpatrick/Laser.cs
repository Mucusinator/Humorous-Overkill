using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public GameObject laserPrefab; // laser prefab gameObject
    public float laserSpeed; // how fast to move the lasers UVs
    public GameObject shootPoint; // where to shoot from

    private RaycastHit hitInfo;

    private Vector3 screenCenter;
    private List<GameObject> laserParts = new List<GameObject>();

    void Awake()
    {
        // find ScreenCenter
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        // cleanup from last frame
        cleanup();

		if(Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out hitInfo))
        {
            // find rotation
            Quaternion direction = Quaternion.LookRotation((hitInfo.point - shootPoint.transform.position).normalized);

            for (float step = 0.0f; step < 1.0f; step += 1.0f / hitInfo.distance)
            {
                GameObject currentLaserPart = Instantiate(laserPrefab, Vector3.Lerp(shootPoint.transform.position, hitInfo.point, step), direction, transform);

                // make changes, move UVs, etc.

                laserParts.Add(currentLaserPart);
            }
        }
	}

    // destroys all laser parts
    void cleanup()
    {
        // destroy each laser part
        foreach(GameObject currentLaserPart in laserParts)
        {
            Destroy(currentLaserPart);
        }

        // clear the list
        laserParts.Clear();
    }
}
