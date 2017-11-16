using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

namespace FranciscoRomano.Util
{
    public class GIFFile
    {
        // :: variables
        public GIF89a.Header header;
        public GIF89a.LogicalScreenDescriptor logicalScreenDescriptor;
        public GIF89a.ColorTable globalColorTable;
        // :: constructors
        public GIFFile(byte[] bytes)
        {
            int currentIndex = 0;
            // get header information
            header = new GIF89a.Header(bytes, currentIndex);
            currentIndex += GIF89a.Header.SIZE;
            // get logical screen descriptor
            logicalScreenDescriptor = new GIF89a.LogicalScreenDescriptor(bytes, 6);
            currentIndex += GIF89a.LogicalScreenDescriptor.SIZE;
            // check if global color table exists
            if (logicalScreenDescriptor.globalColorTableFlag)
            {
                // get global color table
                int size = 1 << (logicalScreenDescriptor.sizeOfGlobalColorTable + 1);
                globalColorTable = new GIF89a.ColorTable(bytes, 13, size);
                currentIndex += size;
            }

            int zz = 0;
        }
    }
}

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

/*
        public Header header;
        public struct Header
        {
            // :: variables [ Header ]
            public string version;
            public string signature;
            // :: variables [ LogicalScreenDescriptor ]
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
            // :: complex variables [ LogicalScreenDescriptor ]
            public bool sortFlag { get { return (packedField & 0x08) == packedField; } }
            public uint colorResolution { get { return Convert.ToUInt32((packedField & 0x70) >> 4) + 1; } }
            public uint globalColorTableSize { get { return Convert.ToUInt32((packedField & 0x07)) + 1; } }
            public bool globalColorTableFlag { get { return (packedField & 0x80) == packedField; } }
        }
 */
