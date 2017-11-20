using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class GraphicControlExtension
    {
        // :: variables
        public byte byteSize;
        public byte packedField;
        public byte transparentColorIndex;
        public ushort delayTime;
        // :: constants
        public int Offset { get { return 8; } }
        public int disposalMethod { get { return (packedField & 0x1C) >> 2; } }
        public int reservedForFutureUse { get { return (packedField & 0xE0) >> 5; } }
        public bool userInputFlag { get { return (packedField | 0x02) == packedField; } }
        public bool transparentColorFlag { get { return (packedField | 0x01) == packedField; } }
        // :: constructors
        public GraphicControlExtension(byte[] bytes, int index)
        {
            byteSize = bytes[index];
            packedField = bytes[index + 1];
            delayTime = BitConverter.ToUInt16(bytes, index + 2);
            transparentColorIndex = bytes[index + 4];
        }
    }
}