using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class ApplicationExtension
    {
        // :: variables
        public byte blockSize;
        public byte[] identifier;
        public byte[] authenticationCode;
        public DataSubBlocks dataSubBlocks;
        // :: constants
        public int Offset { get { return dataSubBlocks.Offset + 15; } }
        // :: constructors/destructors
        public ApplicationExtension(byte[] bytes, int index)
        {
            blockSize = bytes[index + 2];
            identifier = new byte[8];
            Array.Copy(bytes, index + 3, identifier, 0, 8);
            authenticationCode = new byte[3];
            Array.Copy(bytes, index + 11, authenticationCode, 0, 3);
            dataSubBlocks = new DataSubBlocks(bytes, index + 14);
        }
    }
}