using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util.GIF
{
    public class V89aReader
    {
        // :: static variables
        private static int currentIndex;
        private static V89a.GraphicControlExtension currentGCExt;
        // :: constant variables
        private const byte TrailerLabel = 0x3B;
        private const byte ExtensionLabel = 0x21;
        private const byte ImageDescriptorLabel = 0x2C;
        private const byte CommentExtensionLabel = 0xFE;
        private const byte PlainTextExtensionLabel = 0x01;
        private const byte ApplicationExtensionLabel = 0xFF;
        private const byte GraphicControlExtensionLabel = 0xF9;
        // :: static functions
        public static V89aData Read(byte[] bytes)
        {
            currentIndex = 0;
            currentGCExt = null;
            V89aData result = new V89aData();
            // read header
            result.header = ReadHeader(bytes);
            // iterate through remaining bytes
            while (currentIndex < bytes.Length)
            {
                // check if complete
                if (bytes[currentIndex] == TrailerLabel) break;
                // check graphic control extension
                if (bytes[currentIndex] == ExtensionLabel && bytes[currentIndex + 1] == GraphicControlExtensionLabel)
                {
                    currentGCExt = ReadGraphicControlExtension(bytes);
                }
                // check remaining information blocks
                switch (bytes[currentIndex])
                {
                    // extension block
                    case ExtensionLabel:
                        switch (bytes[currentIndex + 1])
                        {
                            // [extension] comment
                            case CommentExtensionLabel:
                                result.commentExtensions.Add(ReadCommentExtension(bytes));
                                break;
                            // [extension] plain text
                            case PlainTextExtensionLabel:
                                result.texts.Add(ReadText(bytes));
                                break;
                            // [extension] application
                            case ApplicationExtensionLabel:
                                result.applicationExtensions.Add(ReadApplicationExtension(bytes));
                                break;
                        }
                        break;
                    // image descriptor block
                    case ImageDescriptorLabel:
                        result.images.Add(ReadImage(bytes));
                        break;
                }
            }
            // return data
            return result;
        }
        private static V89aData.Text ReadText(byte[] bytes)
        {
            V89aData.Text result = new V89aData.Text();
            // set graphic control extension
            result.graphicControlExtension = currentGCExt;
            currentGCExt = null;
            // read plain text extension
            result.plainTextExtension = new V89a.PlainTextExtension(bytes, currentIndex);
            currentIndex += result.plainTextExtension.Offset;
            // return data
            return result;
        }
        private static V89aData.Image ReadImage(byte[] bytes)
        {
            V89aData.Image result = new V89aData.Image();
            // set graphic control extension
            result.graphicControlExtension = currentGCExt;
            currentGCExt = null;
            // read image descriptor
            result.imageDescriptor = new V89a.ImageDescriptor(bytes, currentIndex);
            currentIndex += result.imageDescriptor.Offset;
            // read local color table (if exists)
            result.localColorTable = new V89a.ColorTable(bytes, result.imageDescriptor, currentIndex);
            currentIndex += result.localColorTable.Offset;
            // read image data
            result.imageData = new V89a.ImageData(bytes, currentIndex);
            currentIndex += result.imageData.Offset;
            // return data
            return result;
        }
        private static V89aData.Header ReadHeader(byte[] bytes)
        {
            V89aData.Header result = new V89aData.Header();
            // read header
            result.header = new V89a.Header(bytes, currentIndex);
            currentIndex += result.header.Offset;
            // read logical screen descriptor
            result.logicalScreenDescriptor = new V89a.LogicalScreenDescriptor(bytes, currentIndex);
            currentIndex += result.logicalScreenDescriptor.Offset;
            // read global color table (if exists)
            result.globalColorTable = new V89a.ColorTable(bytes, result.logicalScreenDescriptor, currentIndex);
            currentIndex += result.globalColorTable.Offset;
            // return data
            return result;
        }
        private static V89a.CommentExtension ReadCommentExtension(byte[] bytes)
        {
            // read comment extension
            V89a.CommentExtension result = new V89a.CommentExtension(bytes, currentIndex);
            currentIndex += result.Offset;
            // return data
            return result;
        }
        private static V89a.ApplicationExtension ReadApplicationExtension(byte[] bytes)
        {
            // read application extension
            V89a.ApplicationExtension result = new V89a.ApplicationExtension(bytes, currentIndex);
            currentIndex += result.Offset;
            // return data
            return result;
        }
        private static V89a.GraphicControlExtension ReadGraphicControlExtension(byte[] bytes)
        {
            // read graphic control extension
            V89a.GraphicControlExtension result = new V89a.GraphicControlExtension(bytes, currentIndex);
            currentIndex += result.Offset;
            // return data
            return result;
        }
    }
}