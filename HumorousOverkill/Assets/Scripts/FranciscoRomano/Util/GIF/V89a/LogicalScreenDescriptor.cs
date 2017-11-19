using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class LogicalScreenDescriptor
    {
        // :: variables
        public byte packedField;
        public byte pixelAspectRatio;
        public byte backgroundColorIndex;
        public ushort screenWidth;
        public ushort screenHeight;
        // :: constants
        public int Offset { get { return 7; } }
        public int colorResolution { get { return (packedField & 0x70) >> 4; } }
        public int sizeOfGlobalColorTable { get { return (packedField & 0x07); } }
        public bool sortFlag { get { return (packedField | 0x08) == packedField; } }
        public bool globalColorTableFlag { get { return (packedField | 0x80) == packedField; } }
        // :: constructors
        public LogicalScreenDescriptor(byte[] bytes, int index)
        {
            screenWidth = BitConverter.ToUInt16(bytes, index);
            screenHeight = BitConverter.ToUInt16(bytes, index + 2);
            packedField = bytes[index + 4];
            backgroundColorIndex = bytes[index + 5];
            pixelAspectRatio = bytes[index + 6];
        }
    }
}