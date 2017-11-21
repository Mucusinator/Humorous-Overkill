using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class easyColliders : MonoBehaviour
{
    public enum AXIS { X, Y };

    public AXIS axis = AXIS.X;

    public Vector3 collidersize = new Vector3(1, 1, 1);

    private RaycastHit hitInfo;

    private Vector3 screenCenter;

    public GameObject colliderPrefab;

    private GameObject currentCollider;

    void Start()
    {
        screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        currentCollider = Instantiate(colliderPrefab, Vector3.zero, Quaternion.identity);
    }

	// Update is called once per frame
	void Update ()
    {
        changeAxis();

        // draw collider on wall
        if (Physics.Raycast(Camera.main.ScreenPointToRay(screenCenter), out hitInfo))
        {
            currentCollider.transform.position = hitInfo.point;
            currentCollider.transform.localScale = collidersize;
            currentCollider.transform.rotation = Quaternion.FromToRotation(-Vector3.forward, hitInfo.normal);

            if (Input.GetMouseButtonDown(0))
            {
                // place and start again
                currentCollider.GetComponent<BoxCollider>().isTrigger = false;
                Destroy(currentCollider.GetComponent<MeshRenderer>());
                Destroy(currentCollider.GetComponent<MeshFilter>());
                currentCollider = Instantiate(colliderPrefab, Vector3.zero, Quaternion.identity);
            }
        }

        float scrollDelta = Input.mouseScrollDelta.y;

        if(scrollDelta != 0)
        {
            switch(axis)
            {
                case AXIS.X:
                    collidersize.x += scrollDelta;
                    break;
                case AXIS.Y:
                    collidersize.y += scrollDelta;
                    break;
            }
        }
    }

    void changeAxis()
    {
        if(Input.GetKeyDown(KeyCode.X))
        {
            axis = AXIS.X;
        }
        else if(Input.GetKeyDown(KeyCode.Y))
        {
            axis = AXIS.Y;
        }
    }
}
