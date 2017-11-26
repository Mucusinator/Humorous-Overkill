using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class Header
    {
        // :: variables
        public string signature;
        public string version;
        // :: constants
        public int Offset { get { return 6; } }
        // :: constructors/destructors
        public Header(byte[] bytes, int index)
        {
            signature = BitConverter.ToString(bytes, index, 3);
            version = BitConverter.ToString(bytes, index + 3, 3);
        }
    }
}