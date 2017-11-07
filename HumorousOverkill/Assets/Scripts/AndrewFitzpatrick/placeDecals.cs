using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecalInfo
{
    public DecalInfo(Vector3 p, Quaternion r, int i)
    {
        position = p;
        rotation = r;
        textureIndex = i;
    }

    public Vector3 position;
    public Quaternion rotation;
    public int textureIndex;
}

[ExecuteInEditMode]
public class placeDecals : MonoBehaviour
{
    // list of decal textures
    public List<Texture2D> decalTextures = new List<Texture2D>();

    // prefab decal
    public GameObject decalPrefab;

    // global decal size
    public Vector3 decalSize;

    public List<DecalInfo> decalInfo = new List<DecalInfo>();

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
                //GameObject currentDecal = Instantiate(decalPrefab, hitInfo.point + hitInfo.normal * 0.1f, Quaternion.FromToRotation(-Vector3.forward, hitInfo.normal));

                // resize the new decal to the specified size
                //currentDecal.transform.localScale = decalSize;

                // child the decal to this object
                //currentDecal.transform.parent = gameObject.transform;

                // randomly change the decals material to use one of the decal textures
                //currentDecal.GetComponent<Renderer>().material.SetTexture("_MainTex", decalTextures[Random.Range(0, decalTextures.Count)]);

                decalInfo.Add(new DecalInfo(hitInfo.point + hitInfo.normal * 0.1f, Quaternion.FromToRotation(-Vector3.forward, hitInfo.normal), Random.Range(0, decalTextures.Count)));
            }
        }
	}

//#if UNITY_EDITOR
    void onDrawGizmos()
    {
        Gizmos.color = Color.black;
        //foreach (DecalInfo currentDecalInfo in decalInfo)
        //{
            Gizmos.DrawWireCube(transform.position, decalSize);
        //}
    }
//#endif
}
