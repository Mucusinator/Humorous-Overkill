using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF
{
    public class ImageBlock
    {
        // :: variables
        public V89a.ImageData imageData;
        public V89a.ColorTable localColorTable;
        public V89a.ImageDescriptor imageDescriptor;
        // :: constants
        public int Offset { get { return imageDescriptor.Offset + localColorTable.Offset + imageData.Offset; } }
        // :: constructors
        public ImageBlock(byte[] bytes, int index)
        {
            int size = 0;
            // read image descriptor
            imageDescriptor = new V89a.ImageDescriptor(bytes, index);
            if (imageDescriptor.localColorTableFlag)
            {
                size = 1 << (imageDescriptor.sizeOfLocalColorTable + 1);
            }
            // read local color table
            localColorTable = new V89a.ColorTable(bytes, index + imageDescriptor.Offset, size);
            // read image data
            imageData = new V89a.ImageData(bytes, index + imageDescriptor.Offset + localColorTable.Offset);
        }
    }
}