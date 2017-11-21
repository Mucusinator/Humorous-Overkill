using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class ImageDescriptor
    {
        // :: variables
        public byte packedField;
        public ushort top;
        public ushort left;
        public ushort width;
        public ushort height;
        // :: constants
        public int Offset { get { return 10; } }
        public int reservedForFutureUse { get { return (packedField & 0x18) >> 3; } }
        public int sizeOfLocalColorTable { get { return (packedField & 0x07); } }
        public bool sortFlag { get { return (packedField | 0x20) == packedField; } }
        public bool interlacedFlag { get { return (packedField | 0x40) == packedField; } }
        public bool localColorTableFlag { get { return (packedField | 0x80) == packedField; } }
        // :: constructors
        public ImageDescriptor(byte[] bytes, int index)
        {
            left = BitConverter.ToUInt16(bytes, index + 1);
            top = BitConverter.ToUInt16(bytes, index + 3);
            width = BitConverter.ToUInt16(bytes, index + 5);
            height = BitConverter.ToUInt16(bytes, index + 7);
            packedField = bytes[index + 9];
        }
    }
}