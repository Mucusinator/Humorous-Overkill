using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class decalScript : MonoBehaviour
{
    [Tooltip("Drag the decal here")]
    public Texture2D texture;

	void Start ()
    {
        GetComponent<Renderer>().material.SetTexture("_MainTex", texture);
	}
}
