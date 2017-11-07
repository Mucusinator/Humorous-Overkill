using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class placeDecals : MonoBehaviour
{
    // list of decal textures
    public List<Texture2D> decalTextures = new List<Texture2D>();

    // prefab decal
    public GameObject decalPrefab;

    // global decal size
    public Vector3 decalSize;

    private RaycastHit hitInfo;

    void Update ()
    {
        // get raycast info
        if(Physics.Raycast(Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0)), out hitInfo))
        {
            // if the designer clicks the left mouse button
            if(Input.GetMouseButtonDown(0))
            {
                // create a new decal
                // decal is spawned at the hit point + the hit normal * (very small number)
                // it is rotated to "stick" to the wall
                GameObject currentDecal = Instantiate(decalPrefab, hitInfo.point + hitInfo.normal * 0.1f, Quaternion.FromToRotation(-Vector3.forward, hitInfo.normal));

                // resize the new decal to the specified size
                currentDecal.transform.localScale = decalSize;

                // child the decal to this object
                currentDecal.transform.parent = gameObject.transform;

                // randomly change the decals material to use one of the decal textures
                currentDecal.GetComponent<Renderer>().material.SetTexture("_MainTex", decalTextures[Random.Range(0, decalTextures.Count)]);
            }
        }
	}
}
