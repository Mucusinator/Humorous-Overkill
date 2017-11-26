using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF.V89a
{
    public class CommentExtension
    {
        // :: variables
        public DataSubBlocks dataSubBlocks;
        // :: constants
        public int Offset { get { return dataSubBlocks.Offset + 3; } }
        // :: constructors/destructors
        public CommentExtension(byte[] bytes, int index)
        {
            dataSubBlocks = new DataSubBlocks(bytes, index + 2);
        }
    }
}