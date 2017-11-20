using System;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FranciscoRomano.UI.classes
{
    public class TextInfo
    {
        // :: variables
        public UnityEngine.UI.Text component = null;
        // :: functions
        public void Update(int value) { Update(value.ToString()); }
        public void Update(float value) { Update(value.ToString()); }
        public void Update(string value) { component.text = value; }
        public static TextInfo Create(UIProperty property)
        {
            TextInfo obj = new TextInfo();
            // set variables
            obj.component = property.GetComponent<UnityEngine.UI.Text>();
            // return object
            return obj;
        }
    }
}