using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class imageChange : MonoBehaviour {
    // Two images, one for confetti, one for laser, one for the base.
    public Sprite confettiImage, LaserImage;
        
    public Image baseImage;
    // THe combined weapon script.
    public CombinedScript weaponScript;

	// Update is called once per frame
	void Update () {

        if (weaponScript.gunType == CombinedScript.GunType.RIFLE)
        {
            baseImage.sprite = LaserImage;
        }
        else
        {
            baseImage.sprite = confettiImage;
        }
	}
}
