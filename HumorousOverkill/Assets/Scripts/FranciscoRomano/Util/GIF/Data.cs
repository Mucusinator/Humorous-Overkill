using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//https://github.com/klutch/UnityGifDecoder
//http://www.matthewflickinger.com/lab/whatsinagif/bits_and_bytes.asp

namespace FranciscoRomano.Util.GIF
{
    public class Data
    {
        // :: variables
        public int currentIndex;
        public Header header;
        public List<ImageBlock> imageBlocks;
        public List<V89a.CommentExtension> commentExtensions;
        public List<V89a.PlainTextExtension> plainTextExtensions;
        public List<V89a.ApplicationExtension> applicationExtensions;
        public List<V89a.GraphicControlExtension> graphicControlExtensions;
        public Dictionary<V89a.GraphicControlExtension, ImageBlock> imageBlockDictionary;
        // :: constants
        private const byte TrailerLabel = 0x3B;
        private const byte ExtensionLabel = 0x21;
        private const byte ImageDescriptorLabel = 0x2C;
        private const byte CommentExtensionLabel = 0xFE;
        private const byte PlainTextExtensionLabel = 0x01;
        private const byte ApplicationExtensionLabel = 0xFF;
        private const byte GraphicControlExtensionLabel = 0xF9;
        // :: constructors
        public Data(byte[] bytes)
        {
            // initialize
            header = CreateHeader(bytes);
            currentIndex = 0;
            imageBlocks = new List<ImageBlock>();
            commentExtensions = new List<V89a.CommentExtension>();
            plainTextExtensions = new List<V89a.PlainTextExtension>();
            applicationExtensions = new List<V89a.ApplicationExtension>();
            graphicControlExtensions = new List<V89a.GraphicControlExtension>();
            imageBlockDictionary = new Dictionary<V89a.GraphicControlExtension, ImageBlock>();
            // read file data
            ReadExtensions(bytes);
        }
        // :: read functions
        private void ReadExtensions(byte[] bytes)
        {
            // iterate through file bytes
            while ((currentIndex + 1) < bytes.Length)
            {
                // iterate through each extension
                switch (bytes[currentIndex + 1])
                {
                    // Comment Extension
                    case 0xFE:
                        if (bytes[currentIndex] != ExtensionLabel) break;
                        CreateCommentExtension(bytes);
                        continue;
                    // Plain Text Extension
                    case 0x01:
                        if (bytes[currentIndex] != ExtensionLabel) break;
                        CreatePlainTextExtension(bytes);
                        continue;
                    // Application Extension
                    case 0xFF:
                        if (bytes[currentIndex] != ExtensionLabel) break;
                        CreateApplicationExtension(bytes);
                        continue;
                    // [Default] Graphic Control Extension
                    default:
                        ReadGraphicControlExtension(bytes);
                        continue;
                }
                // increase current index
                currentIndex++;
            }
        }
        private void ReadGraphicControlExtension(byte[] bytes)
        {
            // check if Extension
            if (bytes[currentIndex] == ExtensionLabel)
            {
                // check if Plain Text Extension
                if (bytes[currentIndex + 1] == PlainTextExtensionLabel)
                {
                    CreatePlainTextExtension(bytes);
                    return;
                }
                // check if Graphic Control Extension
                else if (bytes[currentIndex + 1] == GraphicControlExtensionLabel && bytes[currentIndex + 7] == 0x00)
                {
                    V89a.GraphicControlExtension gcEXT = CreateGraphicControlExtension(bytes);
                    ImageBlock imINF = CreateImageBlock(bytes);
                    // add link to dictionary
                    imageBlockDictionary.Add(gcEXT, imINF);
                    return;
                }
            }
            // increase current index
            currentIndex++;
        }
        // :: create functions
        private Header CreateHeader(byte[] bytes)
        {
            Header obj = new Header(bytes, currentIndex);
            currentIndex += obj.Offset;
            return obj;
        }
        private ImageBlock CreateImageBlock(byte[] bytes)
        {
            ImageBlock obj = new ImageBlock(bytes, currentIndex);
            imageBlocks.Add(obj);
            currentIndex += obj.Offset;
            return obj;
        }
        private V89a.CommentExtension CreateCommentExtension(byte[] bytes)
        {
            V89a.CommentExtension obj = new V89a.CommentExtension();
            commentExtensions.Add(obj);
            currentIndex += obj.Offset;
            return obj;
        }
        private V89a.PlainTextExtension CreatePlainTextExtension(byte[] bytes)
        {
            V89a.PlainTextExtension obj = new V89a.PlainTextExtension();
            plainTextExtensions.Add(obj);
            currentIndex += obj.Offset;
            return obj;
        }
        private V89a.ApplicationExtension CreateApplicationExtension(byte[] bytes)
        {
            V89a.ApplicationExtension obj = new V89a.ApplicationExtension();
            applicationExtensions.Add(obj);
            currentIndex += obj.Offset;
            return obj;
        }
        private V89a.GraphicControlExtension CreateGraphicControlExtension(byte[] bytes)
        {
            V89a.GraphicControlExtension obj = new V89a.GraphicControlExtension(bytes, currentIndex);
            graphicControlExtensions.Add(obj);
            currentIndex += obj.Offset;
            return obj;
        }
    }
}