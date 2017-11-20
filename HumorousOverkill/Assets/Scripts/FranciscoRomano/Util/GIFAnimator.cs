using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GIFAnimator : MonoBehaviour
{
    public TextAsset file;
    public List<Sprite> sprites = new List<Sprite>();

    void Start()
    {
        FranciscoRomano.Util.GIF.Data gif = new FranciscoRomano.Util.GIF.Data(file.bytes);

        sprites = FranciscoRomano.Util.BAD_GifParserScript.createSprites(gif);
    }
}