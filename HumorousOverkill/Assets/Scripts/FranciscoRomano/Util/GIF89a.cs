using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util
{
    public static class GIF89a
    {
        // color
        public class Color
        {
            // :: variables
            public byte r;
            public byte g;
            public byte b;
            public byte a;
            // :: constructors
            public Color(byte r, byte g, byte b, byte a)
            {
                this.r = r;
                this.g = g;
                this.b = b;
                this.a = a;
            }
        }

        // header
        public class Header
        {
            // :: variables
            public string version;
            public string signature;
            // :: constants
            public static int SIZE = 6;
            // :: constructors
            public Header(byte[] bytes, int index)
            {
                version = BitConverter.ToString(bytes, index, 3);
                signature = BitConverter.ToString(bytes, index + 3, 3);
            }
        }

        // logical screen descriptor
        public class LogicalScreenDescriptor
        {
            // :: variables
            public ushort screenWidth;
            public ushort screenHeight;
            public byte packedField;
            public byte backgroundColorIndex;
            public byte pixelAspectRatio;
            // :: constants
            public static int SIZE = 7;
            // :: constructors
            public LogicalScreenDescriptor(byte[] bytes, int index)
            {
                screenWidth = BitConverter.ToUInt16(bytes, index);
                screenHeight = BitConverter.ToUInt16(bytes, index + 2);
                packedField = bytes[index + 4];
                backgroundColorIndex = bytes[index + 5];
                pixelAspectRatio = bytes[index + 6];
            }
            public bool globalColorTableFlag { get { return (packedField & 0x40) == packedField; } }
            public int colorResolution { get { return (packedField & 0x70) >> 4; } }
            public bool sortFlag { get { return (packedField & 0x04) == packedField; } }
            public int sizeOfGlobalColorTable { get { return (packedField & 0x07); } }
        }

        // color table
        public class ColorTable
        {
            // :: variables
            public List<Color> colors;
            // :: constructors
            public ColorTable(byte[] bytes, int index, int size)
            {
                colors = new List<Color>();
                for (int i = 0; i < size; i++)
                {
                    byte r = bytes[index + (i * 3 + 0)];
                    byte g = bytes[index + (i * 3 + 1)];
                    byte b = bytes[index + (i * 3 + 2)];
                    colors.Add(new Color(r, g, b, 1));
                }
            }
        }

        // graphics control extension
        public class GraphicsControlExtension
        {
            // :: variables
            public byte byteSize;
            public byte packedFiled;
            public ushort delayTime;
            public byte transparentColorIndex;
            // :: constants
            public static byte Label = 0x21;
            public static byte Introducer = 0x21;
            public static byte BlockTerminator = 0x00;
            // :: constructors
            public GraphicsControlExtension(byte[] bytes, int index)
            {
                byteSize = bytes[index];
                packedFiled = bytes[index + 1];
                delayTime = BitConverter.ToUInt16(bytes, index + 2);
                transparentColorIndex = bytes[index + 4];
            }
        }

        // image descriptor
        public class ImageDescriptor
        {
            // :: variables
            public ushort left;
            public ushort top;
            public ushort width;
            public ushort height;
            public byte packedField;
            // :: constants
            public static byte Separator = 0x2C;
            // :: constructors
            public ImageDescriptor(byte[] bytes, int index)
            {
                left = BitConverter.ToUInt16(bytes, index);
                top = BitConverter.ToUInt16(bytes, index + 2);
                width = BitConverter.ToUInt16(bytes, index + 4);
                height = BitConverter.ToUInt16(bytes, index + 6);
                packedField = bytes[index + 8];
            }
        }


        // :: variables
        // :: constructors
        
        // image data
        public class ImageData
        {

        }

        // plain text extension
        // application extension
        // comment extension
        // trailer
    }
}