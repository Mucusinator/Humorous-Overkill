using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class GraphicControlExtension
    {
        // :: variables
        public byte blockSize;
        public byte packedFields;
        public ushort delayTime;
        public byte transparentColorIndex;
        // :: constants
        public int Offset { get { return 8; } }
        public int reserved { get { return (packedFields & 0xE0) >> 5; } }
        public int disposalMethod { get { return (packedFields & 0x1C) >> 2; } }
        public bool userInputFlag { get { return (packedFields | 0x02) == packedFields; } }
        public bool transparentColorFlag { get { return (packedFields | 0x01) == packedFields; } }
        // :: constructors/destructors
        public GraphicControlExtension(byte[] bytes, int index)
        {
            blockSize = bytes[index];
            packedFields = bytes[index + 1];
            delayTime = BitConverter.ToUInt16(bytes, index + 2);
            transparentColorIndex = bytes[index + 4];
        }
    }
}