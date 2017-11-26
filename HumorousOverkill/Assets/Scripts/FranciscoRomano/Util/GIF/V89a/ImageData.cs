using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class ImageData
    {
        // :: variables
        public byte LZWMinimumCodeSize;
        public DataSubBlocks dataSubBlocks;
        // :: constants
        public int Offset { get { return dataSubBlocks.Offset + 2; } }
        // :: constructors/destructors
        public ImageData(byte[] bytes, int index)
        {
            LZWMinimumCodeSize = bytes[index];
            dataSubBlocks = new DataSubBlocks(bytes, index + 1);
        }
    }
}