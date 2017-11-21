using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class ColorTable
    {
        public class Color { public byte r, g, b; }
        // :: variables
        public Color[] colors;
        // :: constants
        public int Offset { get { return colors.Length * 3; } }
        // :: constructors
        public ColorTable(byte[] bytes, int index, int size)
        {
            colors = new Color[size];
            for (int i = 0; i < size; i++)
            {
                colors[i] = new Color();
                colors[i].r = bytes[index + (i * 3)];
                colors[i].g = bytes[index + (i * 3) + 1];
                colors[i].b = bytes[index + (i * 3) + 2];
            }
        }
    }
}