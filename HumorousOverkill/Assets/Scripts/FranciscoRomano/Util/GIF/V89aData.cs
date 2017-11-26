using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FranciscoRomano.Util.GIF
{
    public class V89aData
    {
        // :: structures
        public struct Text
        {
            public V89a.PlainTextExtension plainTextExtension;
            public V89a.GraphicControlExtension graphicControlExtension;
        }
        public struct Image
        {
            public V89a.ImageData imageData;
            public V89a.ColorTable localColorTable;
            public V89a.ImageDescriptor imageDescriptor;
            public V89a.GraphicControlExtension graphicControlExtension;
        }
        public struct Header
        {
            public V89a.Header header;
            public V89a.ColorTable globalColorTable;
            public V89a.LogicalScreenDescriptor logicalScreenDescriptor;
        }
        // :: variables [header]
        public Header header = new Header();
        // :: variables [image/text]
        public List<Text> texts = new List<Text>();
        public List<Image> images = new List<Image>();
        // :: variables [extensions]
        public List<V89a.CommentExtension> commentExtensions = new List<V89a.CommentExtension>();
        public List<V89a.ApplicationExtension> applicationExtensions = new List<V89a.ApplicationExtension>();
    }
}