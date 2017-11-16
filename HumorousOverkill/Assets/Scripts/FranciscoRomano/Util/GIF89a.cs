using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util
{
    public class GIF89a
    {
        public Header header;

        public GIF89a(byte[] bytes)
        {
            header = new Header(bytes, 0);
        }

        public struct Header
        {
            /* :: variables [ Header ] */
            public string version;
            public string signature;
            /* :: variables [ LogicalScreenDescriptor ] */
            public ushort width;
            public ushort height;
            public byte packedField;
            public byte pixelAspectRatio;
            public byte backgroundColorIndex;
            // :: constructors
            public Header(byte[] bytes, int index)
            {
                // initialize Header
                version = Encoding.UTF8.GetString(bytes, index, 3);
                signature = Encoding.UTF8.GetString(bytes, index + 3, 3);
                // initialize LogicalScreenDescriptor
                width = BitConverter.ToUInt16(bytes, index + 6);
                height = BitConverter.ToUInt16(bytes, index + 8);
                packedField = bytes[index + 10];
                pixelAspectRatio = bytes[index + 12];
                backgroundColorIndex = bytes[index + 11];
            }
            /* :: complex variables [ LogicalScreenDescriptor ] */
            public bool sortFlag { get { return (packedField & 0x08) == packedField; } }
            public uint colorResolution { get { return Convert.ToUInt32((packedField & 0x70) >> 4); } }
            public uint globalColorTableSize { get { return Convert.ToUInt32((packedField & 0x07)); } }
            public bool globalColorTableFlag { get { return (packedField & 0x80) == packedField; } }
        }

        // :: variables
        // :: constructors
        //public struct Header
        //{
        //    // :: variables
        //    public string version;
        //    public string signature;
        //    // :: constructors
        //    public Header(byte[] bytes, int index)
        //    {
        //        version = Encoding.UTF8.GetString(bytes, index, 3);
        //        signature = Encoding.UTF8.GetString(bytes, index + 3, 3);
        //    }
        //    public static int bSize { get { return 6; } }
        //}

        //public struct LogicalScreenDescriptor
        //{
        //    // :: variables
        //    public ushort width;
        //    public ushort height;
        //    public PackedField packedField;
        //    public byte pixelAspectRatio;
        //    public byte backgroundColorIndex;
        //    // :: constructors
        //    public LogicalScreenDescriptor(byte[] bytes, int index)
        //    {
        //        width = BitConverter.ToUInt16(bytes, index);
        //        height = BitConverter.ToUInt16(bytes, index + 2);
        //        packedField = new PackedField(bytes[index + 4]);
        //        pixelAspectRatio = bytes[index + 6];
        //        backgroundColorIndex = bytes[index + 5];
        //    }

        //    public struct PackedField
        //    {
        //        public bool sortFlag;
        //        public int colorResolution;
        //        public int globalColorTableSize;
        //        public bool globalColorTableFlag;

        //        public PackedField(byte value)
        //        {
        //            sortFlag = (value & 0x08) == value;
        //            colorResolution = (value & 0x70) >> 4;
        //            globalColorTableSize = (value & 0x07) >> 0;
        //            globalColorTableFlag = (value & 0x80) == value;
        //        }
        //    }
        //}

        //public struct ColorTable
        //{
        //    // :: variables
        //    public byte r;
        //    public byte g;
        //    public byte b;
        //    public byte a;
        //    // :: constructors
        //    public ColorTable(byte[] bytes, int index)
        //    {
        //        r = 0;
        //        g = 0;
        //        b = 0;
        //        a = 0;
        //    }
        //}
        //public struct GraphicsControlExtension
        //{

        //}
        //public struct ImageDescriptor
        //{

        //}
        //public struct ImageData
        //{

        //}
        //public struct PlainTextExtension
        //{

        //}
        //public struct ApplicationExtension
        //{

        //}
        //public struct CommentExtension
        //{

        //}
    }
}