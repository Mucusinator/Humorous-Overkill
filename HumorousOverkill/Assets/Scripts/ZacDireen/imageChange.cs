using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script was fully created by Zackary Direen. it is responsible for swapping crosshairs
/// as well as the image showing what weapon you have.
/// </summary>
public class imageChange : MonoBehaviour {
    // Two images, one for confetti, one for laser, one for the base.
    public Sprite confettiImage, LaserImage;
        
    public Image baseImage;
    // THe combined weapon script.
    public CombinedScript weaponScript;

	// Update is called once per frame
	void Update () {
        // if the gun is the rifle
        if (weaponScript.gunType == CombinedScript.GunType.RIFLE)
        {
            baseImage.sprite = LaserImage;
        }
        //if the gun is the shotgun.
        else
        {
            baseImage.sprite = confettiImage;
        }
	}
}
