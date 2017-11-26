using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class DataSubBlocks
    {
        // :: variables
        private int count;
        public List<byte> packedBytes;
        // :: constants
        public int Offset { get { return packedBytes.Count + count; } }
        // :: constructors/destructors
        public DataSubBlocks(byte[] bytes, int index)
        {
            count = 0;
            int offset = index;
            packedBytes = new List<byte>();
            while (bytes[offset] != 0)
            {
                int size = bytes[offset++];
                for (int i = 0; i < size; i++)
                {
                    packedBytes.Add(bytes[offset + i]);
                }
                offset += size;
                count++;
            }
        }
    }
}