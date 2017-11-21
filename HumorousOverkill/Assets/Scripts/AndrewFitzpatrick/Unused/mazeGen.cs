using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeGen : MonoBehaviour
{
    // maze block prefab
    public GameObject mazeBlock;

    // maze image
    public Texture2D mazeSprite;

    // total size of the maze
    private Vector3 totalMazeSize;

	// Use this for initialization
	void Start ()
    {
        createMaze();

        resizeFloor();

        addLight();
	}

    void createMaze()
    {
        //calculate total maze size
        totalMazeSize = new Vector3(mazeSprite.width * mazeBlock.transform.localScale.x, 1, mazeSprite.height * mazeBlock.transform.localScale.z);

        // spawn blocks
        for (int x = 0; x < mazeSprite.width / 2; x++)
        {
            for (int y = 0; y < mazeSprite.height / 2; y++)
            {
                // black pixel place block
                if (mazeSprite.GetPixel(x * 2, y * 2).r == 0)
                {
                    GameObject currentMazePart = Instantiate(mazeBlock, new Vector3(x * mazeBlock.transform.localScale.x, mazeBlock.transform.localScale.y / 2, y * mazeBlock.transform.localScale.z), Quaternion.identity, transform);
                    currentMazePart.name = "maze (" + x + ", " + y + ")";

                    currentMazePart.GetComponent<Renderer>().material.SetColor("_Color", randomColor());
                }
            }
        }
    }

    // resizes the floor and roof to fit the maze
    void resizeFloor()
    {
        Transform floorTransform = transform.GetChild(0);
        Transform roofTransform = transform.GetChild(1);

        floorTransform.localScale = roofTransform.localScale = totalMazeSize / 2;
        floorTransform.position = totalMazeSize / 4;
        roofTransform.position = floorTransform.position + Vector3.up * mazeBlock.transform.localScale.y;

        roofTransform.gameObject.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
    }

    // add a single light to illuminate the maze
    void addLight()
    {
        // create light gameobject as child
        GameObject lightObject = new GameObject();
        lightObject.name = "light";
        lightObject.transform.parent = transform;

        // add light component to the light
        Light light = lightObject.AddComponent<Light>();

        // adjust light parameters
        light.type = LightType.Point;
        light.shadows = LightShadows.None;

        // range is based on maze size without y component
        Vector3 flatMazeSize = totalMazeSize;
        flatMazeSize.y = 0;
        light.range = flatMazeSize.magnitude;

        // position light
        lightObject.transform.position = flatMazeSize / 4 + Vector3.up * 50.0f;
    }

    Color randomColor()
    {
        return new Color(Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, Random.Range(0, 255) / 255.0f, 1);
    }
}
