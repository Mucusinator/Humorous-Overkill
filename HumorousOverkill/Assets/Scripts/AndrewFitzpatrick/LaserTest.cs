using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTest : MonoBehaviour
{
    RaycastHit hit = new RaycastHit();
    public GameObject laser;
    private Camera cam;
    private List<GameObject> laserParts = new List<GameObject>();
    public List<Color> laserColors = new List<Color>();
    public GameObject unicorn;
    public GameObject shootPoint;
    public GameObject laser_hit;
    private float colorOffset = 0;
    public float laserSpeed = 1;

    void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update ()
    {
        colorOffset += laserSpeed * Time.deltaTime;

        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 1000.0f, Color.yellow);
        // mouse click
        if (Input.GetMouseButton(0))
        {
            cleanup();

            Vector3 scale = unicorn.transform.localScale;
            //if (scale.x > 0)
            //{
            //    scale.x -= 0.01f;
            //    scale.y -= 0.01f;
            //    scale.z -= 0.01f;
            //    unicorn.transform.localScale = scale;
            //}

            if (Physics.Raycast(ray.origin, ray.direction * 1000.0f, out hit))
            {
                if (hit.collider.gameObject.tag == "Avoid")
                {
                    float dist = hit.distance;

                    Vector3 relativePos = hit.point - shootPoint.transform.position;
                    Quaternion rotation = Quaternion.LookRotation(relativePos);

                    for(int i = 0; i < dist * 5; i++)
                    {
                        GameObject currentPart = Instantiate(laser, Vector3.Lerp(hit.point, shootPoint.transform.position, (1.0f / (dist * 5)) * i), rotation) as GameObject;
                        currentPart.GetComponent<Renderer>().material.SetColor("_Color1", laserColors[(i + (int)colorOffset) % laserColors.Count]);
                        currentPart.GetComponent<Renderer>().material.SetColor("_Color2", laserColors[laserColors.Count - 1 - (i + (int)colorOffset) % laserColors.Count]);
                        laserParts.Add(currentPart);
                    }
                    // Instantiate(laser_hit, hit.point, Quaternion.identity);
                }
            }
        }
        else
        {
            Vector3 scale = unicorn.transform.localScale;
            if (scale.x < 1)
            {
                scale.x += 0.01f;
                scale.y += 0.01f;
                scale.z += 0.01f;
                unicorn.transform.localScale = scale;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            cleanup();
        }
    }

    void cleanup()
    {
        // destroy all the laser parts
        foreach (GameObject laserPart in laserParts)
        {
            Destroy(laserPart);
        }
        laserParts.Clear();
    }
}
