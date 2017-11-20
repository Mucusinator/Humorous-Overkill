using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeGen : MonoBehaviour
{
    // maze block prefab
    public GameObject mazeBlock;

    // maze image
    public Texture2D mazeSprite;

    // grid of lights
    public Vector2 lightGridSize;

    private Vector3 totalMazeSize;

	// Use this for initialization
	void Start ()
    {
        setupFloor();

        setupMaze();

        addLights();
	}

    void setupFloor()
    {
        // calculate total maze size
        totalMazeSize = new Vector3(mazeSprite.width * mazeBlock.transform.localScale.x, 1, mazeSprite.height * mazeBlock.transform.localScale.z);
        // resize floor (divide by 2)
        transform.GetChild(0).localScale = totalMazeSize / 2;

        // position is half that with 0 y
        Vector3 floorPosition = totalMazeSize / 4;
        floorPosition.y = 0;
        transform.GetChild(0).position = floorPosition;
    }

    void setupMaze()
    {
        for (int x = 0; x < mazeSprite.width / 2; x++)
        {
            for (int y = 0; y < mazeSprite.height / 2; y++)
            {
                // black pixel place block
                if (mazeSprite.GetPixel(x * 2, y * 2).r == 0)
                {
                    GameObject currentMazePart = Instantiate(mazeBlock, new Vector3(x * mazeBlock.transform.localScale.x, mazeBlock.transform.localScale.y / 2, y * mazeBlock.transform.localScale.z), Quaternion.identity, transform);
                    currentMazePart.name = "maze (" + x + ", " + y + ")";
                }
            }
        }
    }

    void addLights()
    {
        // create the light
        GameObject light = new GameObject();
        light.AddComponent<Light>();

        // point light no shadows
        light.GetComponent<Light>().type = LightType.Point;
        light.GetComponent<Light>().shadows = LightShadows.None;
        light.GetComponent<Light>().range = 250;

        Vector3 lightPosition = totalMazeSize / 4;
        lightPosition.y = 50;

        light.transform.position = lightPosition;

        light.name = "light";
        light.transform.parent = transform;
    }
}
