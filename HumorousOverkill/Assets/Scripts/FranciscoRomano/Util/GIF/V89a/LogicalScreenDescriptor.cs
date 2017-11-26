using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class LogicalScreenDescriptor
    {
        // :: variables
        public uint width;
        public uint height;
        public byte packedFields;
        public byte backgroundColorIndex;
        public byte pixelAspectRatio;
        // :: constants
        public int Offset { get { return 7; } }
        public bool globalColorTableFlag { get { return (packedFields | 0x80) == packedFields; } }
        public int colorResolution { get { return (packedFields & 0x70) >> 4; } }
        public bool sortFlag { get { return (packedFields | 0x08) == packedFields; } }
        public int sizeOfGlobalColorTable { get { return (packedFields & 0x07); } }
        // :: constructors/destructors
        public LogicalScreenDescriptor(byte[] bytes, int index)
        {
            width = BitConverter.ToUInt16(bytes, index);
            height = BitConverter.ToUInt16(bytes, index + 2);
            packedFields = bytes[index + 4];
            backgroundColorIndex = bytes[index + 5];
            pixelAspectRatio = bytes[index + 6];
        }
    }
}