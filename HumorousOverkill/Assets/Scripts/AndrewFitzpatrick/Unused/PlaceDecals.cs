using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceDecals : MonoBehaviour
{
    // list of decal textures
    public List<Texture2D> decalTextures = new List<Texture2D>();

    public List<Vector3> placementPositions = new List<Vector3>();
    public List<Quaternion> placementRotations = new List<Quaternion>();

    // prefab decal
    public GameObject decalPrefab;

    // global decal size
    public Vector3 decalSize;

    private RaycastHit hitInfo;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)), out hitInfo))
            {
                place(hitInfo.point + hitInfo.normal * 0.1f, Quaternion.FromToRotation(-Vector3.forward, hitInfo.normal));
            }
        }
    }

    public void place(Vector3 pos, Quaternion rot)
    {
        // spawn in decal
        GameObject currentDecal = Instantiate(decalPrefab, pos, rot) as GameObject;
        currentDecal.transform.localScale = decalSize;
        currentDecal.transform.parent = gameObject.transform.parent;
        currentDecal.GetComponent<Renderer>().material.SetTexture("_MainTex", decalTextures[Random.Range(0, decalTextures.Count)]);
    }
}
