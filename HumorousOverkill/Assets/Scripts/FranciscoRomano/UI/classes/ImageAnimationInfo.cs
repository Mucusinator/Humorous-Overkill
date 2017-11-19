using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.UI.classes
{
    public class ImageAnimationInfo
    {
        // :: variables
        public List<Sprite> sprites = new List<Sprite>();
        public UnityEngine.UI.Image component = null;
        // :: functions
        public void Update(float t)
        {
            // calculate index
            float index = Mathf.Clamp(sprites.Count * t, 0, sprites.Count);
            // update image sprite
            component.sprite = sprites[(int)index];
        }
        public static ImageAnimationInfo Create(UIProperty property)
        {
            ImageAnimationInfo obj = new ImageAnimationInfo();
            // set variables
            obj.sprites = property.sprites;
            obj.component = property.GetComponent<UnityEngine.UI.Image>();
            // return object
            return obj;
        }
    }
}