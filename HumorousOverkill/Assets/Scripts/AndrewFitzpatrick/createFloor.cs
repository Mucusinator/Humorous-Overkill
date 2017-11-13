using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createFloor : MonoBehaviour
{
    public List<GameObject> floorPrefabs;
    public Vector2 gridSize;
    public Vector2 spacing;
    public Vector2 randomness;
    public float lookLerp;

    private List<GameObject> spawnedTeddys = new List<GameObject>();

    void Start()
    {
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                // find spawn position on grid
                Vector3 spawnPos = transform.position + new Vector3(x * spacing.x, 0, y * spacing.y);
                // add random offset
                spawnPos += new Vector3(Random.Range(-randomness.x, randomness.y), 0, Random.Range(-randomness.y, randomness.y));

                GameObject teddy = Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Count)], spawnPos, Quaternion.Euler(0, 90, 0) * Quaternion.LookRotation(-spawnPos, Vector3.up), transform);
                spawnedTeddys.Add(teddy);
            }
        }
    }

    void Update()
    {
        foreach (GameObject teddy in spawnedTeddys)
        {
            Vector3 camPos = Camera.main.transform.position;
            camPos.y = 0;
            Vector3 teddyPos = teddy.transform.position;
            teddyPos.y = 0;
            Vector3 lookPos = camPos - teddyPos;
            teddy.transform.rotation = Quaternion.Lerp(teddy.transform.rotation, Quaternion.Euler(0, 90, 0) * Quaternion.LookRotation(lookPos), lookLerp * Time.deltaTime);
        }
    }
}
