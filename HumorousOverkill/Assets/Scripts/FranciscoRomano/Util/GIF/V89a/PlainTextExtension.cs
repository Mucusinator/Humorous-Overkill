using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class PlainTextExtension
    {
        // :: variables
        public byte blockSize;
        public uint left;
        public uint top;
        public uint width;
        public uint height;
        public byte characterWidth;
        public byte characterHeight;
        public byte foregroundColorIndex;
        public byte backgroundColorIndex;
        public DataSubBlocks plainTextData;
        // :: constants
        public int Offset { get { return plainTextData.Offset + 16; } }
        // :: constructors/destructors
        public PlainTextExtension(byte[] bytes, int index)
        {
            blockSize = bytes[index + 2];
            left = BitConverter.ToUInt16(bytes, index + 3);
            top = BitConverter.ToUInt16(bytes, index + 5);
            width = BitConverter.ToUInt16(bytes, index + 7);
            height = BitConverter.ToUInt16(bytes, index + 9);
            characterWidth = bytes[index + 11];
            characterHeight = bytes[index + 12];
            foregroundColorIndex = bytes[index + 13];
            backgroundColorIndex = bytes[index + 14];
            plainTextData = new DataSubBlocks(bytes, index + 15);
        }
    }
}