using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class createLevel : MonoBehaviour
{
    [System.Serializable]
    public class FloorData
    {
        public List<GameObject> floorPrefabs;
        public Vector2 gridSize;
        public Vector2 spacing;

        [HideInInspector]
        public List<GameObject> spawnedTiles = new List<GameObject>();
    }

    [System.Serializable]
    public class WallData
    {
        public List<GameObject> wallPrefabs;
        public int height;
        public Vector2 spacing;

        public List<GameObject> spawnedTiles = new List<GameObject>();
    }

    public FloorData floorData;
    public WallData wallData;

    void Start()
    {
        createFloor();
        createWalls();
    }

    void createFloor()
    {
        // create floor GameObject
        GameObject floor = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
        floor.name = "floor";

        // add tiles as children
        for (int x = 0; x < floorData.gridSize.x; x++)
        {
            for (int y = 0; y < floorData.gridSize.y; y++)
            {
                // find spawn position on grid
                Vector3 spawnPos = new Vector3(x * floorData.spacing.x, 0, y * floorData.spacing.y);

                GameObject currentFloorTile = Instantiate(floorData.floorPrefabs[Random.Range(0, floorData.floorPrefabs.Count)], spawnPos, Quaternion.identity, floor.transform);
                floorData.spawnedTiles.Add(currentFloorTile);
            }
        }

        // add floor box collider
        Vector3 totalSize = new Vector3(floorData.gridSize.x * floorData.spacing.x, 0.1f, floorData.gridSize.y * floorData.spacing.y);

        BoxCollider floorCollider = floor.AddComponent<BoxCollider>();
        //floorCollider.center = totalSize / 2;
        floorCollider.size = totalSize;
    }

    void createWalls()
    {
        // create wall GameObject
        GameObject wall = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
        wall.name = "walls";
        
        // create walls 1
        for(int x = 0; x < floorData.gridSize.x * wallData.spacing.x; x++)
        {
            for (int i = 0; i < wallData.height; i++)
            {
                // find spawn position on grid
                Vector3 spawnPos = new Vector3(x * floorData.spacing.x / wallData.spacing.x, i * wallData.spacing.y, -floorData.gridSize.x * floorData.spacing.x / wallData.spacing.x);

                GameObject currentWallTile = Instantiate(wallData.wallPrefabs[Random.Range(0, wallData.wallPrefabs.Count)], spawnPos, Quaternion.identity, wall.transform);
                wallData.spawnedTiles.Add(currentWallTile);
            }
        }

        // create walls 2
        for (int x = 0; x < floorData.gridSize.x * wallData.spacing.x; x++)
        {
            for (int i = 0; i < wallData.height; i++)
            {
                // find spawn position on grid
                Vector3 spawnPos = new Vector3(x * floorData.spacing.x / wallData.spacing.x, i * wallData.spacing.y, floorData.gridSize.x * floorData.spacing.x / wallData.spacing.x);

                GameObject currentWallTile = Instantiate(wallData.wallPrefabs[Random.Range(0, wallData.wallPrefabs.Count)], spawnPos, Quaternion.Euler(0, 180, 0), wall.transform);
                wallData.spawnedTiles.Add(currentWallTile);
            }
        }

        // create walls 3
        for (int x = 0; x < floorData.gridSize.x * wallData.spacing.x; x++)
        {
            for (int i = 0; i < wallData.height; i++)
            {
                // find spawn position on grid
                Vector3 spawnPos = new Vector3(floorData.gridSize.x * floorData.spacing.x / wallData.spacing.x, i * wallData.spacing.y, x * floorData.spacing.x / wallData.spacing.x);

                GameObject currentWallTile = Instantiate(wallData.wallPrefabs[Random.Range(0, wallData.wallPrefabs.Count)], spawnPos, Quaternion.Euler(0, -90, 0), wall.transform);
                wallData.spawnedTiles.Add(currentWallTile);
            }
        }

        // create walls 4
        for (int x = 0; x < floorData.gridSize.x * wallData.spacing.x; x++)
        {
            for (int i = 0; i < wallData.height; i++)
            {
                // find spawn position on grid
                Vector3 spawnPos = new Vector3(-floorData.gridSize.x * floorData.spacing.x / wallData.spacing.x, i * wallData.spacing.y, x * floorData.spacing.x / wallData.spacing.x);

                GameObject currentWallTile = Instantiate(wallData.wallPrefabs[Random.Range(0, wallData.wallPrefabs.Count)], spawnPos, Quaternion.Euler(0, 90, 0), wall.transform);
                wallData.spawnedTiles.Add(currentWallTile);
            }
        }
    }
}
