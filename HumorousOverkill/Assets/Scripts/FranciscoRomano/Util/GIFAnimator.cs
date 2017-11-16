using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GIFAnimator : MonoBehaviour
{
    public TextAsset file;

    void Start()
    {
        FranciscoRomano.Util.GIFFile data = new FranciscoRomano.Util.GIFFile(file.bytes);

        //Debug.Log("header.version = " + data.header.version);
        //Debug.Log("header.signature = " + data.header.signature);

        //Debug.Log("header.width = " + data.header.width);
        //Debug.Log("header.height = " + data.header.height);

        //Debug.Log("header.packedField.globalColorTableFlag = " + data.header.globalColorTableFlag);
        //Debug.Log("header.packedField.colorResolution = " + data.header.colorResolution);
        //Debug.Log("header.packedField.sortFlag = " + data.header.sortFlag);
        //Debug.Log("header.packedField.globalColorTableSize = " + data.header.globalColorTableSize);

        //Debug.Log("header.backgroundColorIndex = " + data.header.backgroundColorIndex);
        //Debug.Log("header.pixelAspectRatio = " + data.header.pixelAspectRatio);
    }
}