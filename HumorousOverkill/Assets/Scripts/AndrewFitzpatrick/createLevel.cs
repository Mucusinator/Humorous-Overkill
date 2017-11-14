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
        [HideInInspector]
        public Vector3 totalFloorSize;
    }

    [System.Serializable]
    public class WallData
    {
        public List<GameObject> wallPrefabs;
        public int height;
        public Vector2 spacing;

        [HideInInspector]
        public List<GameObject> spawnedTiles = new List<GameObject>();
    }

    public FloorData floorData;
    public WallData wallData;

    void Start()
    {
        createFloor();
        createWalls();
        createLight();
        transform.position = -floorData.totalFloorSize / 2;
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
        floorData.totalFloorSize = new Vector3(floorData.gridSize.x * floorData.spacing.x, 0.1f, floorData.gridSize.y * floorData.spacing.y);

        BoxCollider floorCollider = floor.AddComponent<BoxCollider>();
        floorCollider.center = floorData.totalFloorSize / 2;
        floorCollider.size = floorData.totalFloorSize;

        // offset entire floor to center
        // floor.transform.position = -totalSize / 2;
    }

    void createWalls()
    {
        // create wall GameObject
        GameObject walls = Instantiate(new GameObject(), transform.position, Quaternion.identity, transform);
        walls.name = "walls";

        // create wall strips and add colliders
        GameObject[] wallStrips = new GameObject[4];
        for(int i = 0; i < 4; i++)
        {
            wallStrips[i] = Instantiate(new GameObject(), transform.position, Quaternion.identity, walls.transform);
            wallStrips[i].name = "wall strip";

            BoxCollider newCollider = wallStrips[i].AddComponent<BoxCollider>();

            Vector3 colliderSize = new Vector3((i % 2 == 0 ? floorData.gridSize.x : floorData.gridSize.y) * wallData.spacing.x, wallData.height * wallData.spacing.y, 0.1f);
            newCollider.size = colliderSize;
            newCollider.center = colliderSize / 2;
        }

        // spawn walls for wall strips
        // 4 wall strips
        for (int r = 0; r < 4; r++)
        {
            // length of strip
            for (int x = 0; x < (r % 2 == 0 ? floorData.gridSize.x : floorData.gridSize.y); x++)
            {
                // height of strip
                for (int i = 0; i < wallData.height; i++)
                {
                    // spawn position is the same for each wall
                    Vector3 spawnPos = new Vector3(x * wallData.spacing.x, i * wallData.spacing.y, 0);

                    GameObject newWall = Instantiate(wallData.wallPrefabs[Random.Range(0, wallData.wallPrefabs.Count)], spawnPos, Quaternion.identity, wallStrips[r].transform);
                    newWall.name = "wall";
                    wallData.spawnedTiles.Add(newWall);
                }
            }

            // line up wall strips
            wallStrips[r].transform.position = new Vector3(r > 1 ? floorData.totalFloorSize.x : 0, 0, (r > 0 && r < 3) ? floorData.totalFloorSize.z : 0);
            wallStrips[r].transform.rotation = Quaternion.Euler(0, r * 90, 0);
        }
    }

    void createLight()
    {
        GameObject light = Instantiate(new GameObject(), Vector3.up * wallData.height * wallData.spacing.y, Quaternion.identity);
        light.name = "light";
        Light lightComponent = light.AddComponent<Light>();
        lightComponent.range = Mathf.Sqrt(floorData.gridSize.sqrMagnitude + floorData.spacing.sqrMagnitude);
    }
}
