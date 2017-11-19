using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF
{
    public class Header
    {
        // :: variables
        public V89a.Header header;
        public V89a.ColorTable globalColorTable;
        public V89a.LogicalScreenDescriptor logicalScreenDescriptor;
        // :: constants
        public int Offset { get { return header.Offset + logicalScreenDescriptor.Offset + globalColorTable.Offset; } }
        // :: constructors
        public Header(byte[] bytes, int index)
        {
            int size = 0;
            // read header
            header = new V89a.Header(bytes, index);
            // read logical screen descriptor
            logicalScreenDescriptor = new V89a.LogicalScreenDescriptor(bytes, index + header.Offset);
            if (logicalScreenDescriptor.globalColorTableFlag)
            {
                size = 1 << (logicalScreenDescriptor.sizeOfGlobalColorTable + 1);
            }
            // read global color table
            globalColorTable = new V89a.ColorTable(bytes, index + header.Offset + logicalScreenDescriptor.Offset, size);
        }
    }
}