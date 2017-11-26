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
        public uint left;
        public uint top;
        public uint width;
        public uint height;
        // :: constants
        public int Offset { get { return 10; } }
        public bool localColorTableFlag { get { return (packedField | 0x80) == packedField; } }
        public bool interlacedFlag { get { return (packedField | 0x40) == packedField; } }
        public bool sortFlag { get { return (packedField | 0x20) == packedField; } }
        public int reserved { get { return (packedField & 0x18) >> 3; } }
        public int sizeOfLocalColorTable { get { return (packedField & 0x07); } }
        // :: constructors/destructors
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