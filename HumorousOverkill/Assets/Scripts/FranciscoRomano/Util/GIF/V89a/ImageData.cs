using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class ImageData
    {
        // :: variables
        public byte LZWMinimumCodeSize;
        public List<byte[]> dataSubBlocks;
        // :: constants
        public int Offset {
            get {
                int size = 0;
                foreach (byte[] dataSubBlock in dataSubBlocks)
                {
                    size += dataSubBlock.Length + 1;
                }
                return size + 2;
            }
        }
        // :: constructors
        public ImageData(byte[] bytes, int index)
        {
            int offset = index + 1;
            dataSubBlocks = new List<byte[]>();
            LZWMinimumCodeSize = bytes[index];
            while (bytes[offset] != 0)
            {
                byte[] data = new byte[bytes[offset]];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = bytes[offset + (i + 1)];
                }
                dataSubBlocks.Add(data);
                offset += data.Length + 1;
            }
        }
    }
}