using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mazeGen : MonoBehaviour
{
    // maze block prefab
    public GameObject mazeBlock;

    // maze image
    public Texture2D mazeSprite;

	// Use this for initialization
	void Start ()
    {
        Vector3 mazeSize = new Vector3(mazeSprite.width * mazeBlock.transform.localScale.x, 1, mazeSprite.height * mazeBlock.transform.localScale.z);

        // resize floor
        transform.GetChild(0).localScale = mazeSize / 2;
        transform.GetChild(0).position = mazeSize / 4;

		for (int x = 0; x < mazeSprite.width / 2; x++)
        {
            for (int y = 0; y < mazeSprite.height / 2; y++)
            {
                // black pixel place block
                if(mazeSprite.GetPixel(x * 2, y * 2).r == 0)
                {
                    GameObject currentMazePart = Instantiate(mazeBlock, new Vector3(x * mazeBlock.transform.localScale.x, mazeBlock.transform.localScale.y / 2, y * mazeBlock.transform.localScale.z), Quaternion.identity, transform);
                    currentMazePart.name = "maze (" + x + ", " + y + ")";
                }
            }
        }
	}
}
