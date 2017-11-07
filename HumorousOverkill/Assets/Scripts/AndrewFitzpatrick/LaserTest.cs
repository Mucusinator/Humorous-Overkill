using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest : MonoBehaviour
{
    RaycastHit hit = new RaycastHit();
    public GameObject laser;
  
    private Camera cam;
    private List<GameObject> laserParts = new List<GameObject>(); // list to prevent scene clutter
    public List<Color> laserColors = new List<Color>(); // list of colors for the laser to cycle through
    public GameObject shootPoint; // point that laser shoots from
    private float colorOffset = 0; // current color index
    public float laserSpeed = 1; // speed that laser changes colors

    void Awake()
    {
        // assign cam
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update ()
    {
        // update colorOffset
        colorOffset += laserSpeed * Time.deltaTime;
     

        // update ray
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));

        // draw in editor
        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.yellow);

        // if the mouse is clicked
        if (Input.GetMouseButton(0))
        {
            // cleanup old laser parts
            cleanup();

            // draw new parts if the raycast hit anything solid
            if (Physics.Raycast(ray.origin, ray.direction * 1000.0f, out hit))
            {
                // find distance and rotation towards hit point
                float dist = hit.distance;
                Vector3 relativePos = hit.point - shootPoint.transform.position;
                Quaternion rotation = Quaternion.LookRotation(relativePos);

                // instantiate laser parts
                for (int i = 0; i < dist * 9; i++)
                {
                    // lerp from shoot point to hit point
                    Vector3 lerpPos = Vector3.Lerp(hit.point, shootPoint.transform.position, 1.0f - (1.0f / (dist * 9)) * i);

                    // instantiate the current laser part
                    GameObject currentPart = Instantiate(laser, lerpPos, rotation) as GameObject;

                    // set the current laser parts color
                    //int currentColor = (laserColors.Count - 1 - (i + (int)colorOffset) % laserColors.Count);
                    //currentPart.GetComponentInChildren<Renderer>().material.SetColor("_TintColor", laserColors[currentColor]);
                    currentPart.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(Time.time, 0);



                    // parent the laser part and add it to the list
                    currentPart.transform.parent = transform;
                    laserParts.Add(currentPart);
                }
            }

        }

        // when the left mouse button is released clean up laser parts
        if (Input.GetMouseButtonUp(0))
        {
            cleanup();
        }
    }

    // removes all laser parts
    void cleanup()
    {
        // destroy each part
        foreach (GameObject laserPart in laserParts)
        {
            Destroy(laserPart);
        }
        // clear list
        laserParts.Clear();
    }
}
