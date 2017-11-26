using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class ColorTable
    {
        // :: variables
        public UnityEngine.Color[] colors;
        // :: constants
        public int Offset { get { return colors.Length * 3; } }
        // :: constructors/destructors
        public ColorTable(byte[] bytes, int size, int index)
        {
            colors = new UnityEngine.Color[size];
            for (int i = 0; i < size; i++)
            {
                colors[i] = new UnityEngine.Color();
                colors[i].r = bytes[index + (i * 3)] / 255.0f;
                colors[i].g = bytes[index + (i * 3) + 1] / 255.0f;
                colors[i].b = bytes[index + (i * 3) + 2] / 255.0f;
            }
        }
        public ColorTable(byte[] bytes, ImageDescriptor obj, int index) : this(bytes, obj.localColorTableFlag ? 1 << (obj.sizeOfLocalColorTable + 1) : 0, index) {}
        public ColorTable(byte[] bytes, LogicalScreenDescriptor obj, int index) : this(bytes, obj.globalColorTableFlag ? 1 << (obj.sizeOfGlobalColorTable + 1) : 0, index) {}
    }
}