using UnityEngine;
using System;
using System.Text;
using System.Collections.Generic;

namespace FranciscoRomano.Util
{
    /*  Copyright © 2016 Graeme Collins. All Rights Reserved.
Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
1. Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
3. The name of the author may not be used to endorse or promote products derived from this software without specific prior written permission.
THIS SOFTWARE IS PROVIDED BY GRAEME COLLINS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. */


    public class BAD_GifParserScript
    {
        public static List<Sprite> createSprites(GIF.Data gifData)
        {
            List<Sprite> sprites = new List<Sprite>();
            int canvasWidth = gifData.header.logicalScreenDescriptor.screenWidth;
            int canvasHeight = gifData.header.logicalScreenDescriptor.screenHeight;
            Color[] previousFrame = new Color[canvasWidth * canvasHeight];
            Color[] currentFrame = new Color[canvasWidth * canvasHeight];
            Color[] transparentFrame = new Color[canvasWidth * canvasHeight];

            // Create sprites
            //for (int i = 0; i < gifData.graphicControlExtensions.Count; i++)
            for (int i = 0; i < 1; i++)
            {
                GIF.V89a.GraphicControlExtension graphicsControlExt = gifData.graphicControlExtensions[i];
                GIF.V89a.ImageDescriptor imageDescriptor = gifData.imageBlockDictionary[graphicsControlExt].imageDescriptor;
                BAD_GifImageData imageData = prepair(gifData, i);
                int top = imageDescriptor.top;
                int left = imageDescriptor.left;
                int disposalMethod = graphicsControlExt.disposalMethod;
                Texture2D texture = new Texture2D(canvasWidth, canvasHeight);
                int transparencyIndex = graphicsControlExt.transparentColorFlag ? graphicsControlExt.transparentColorIndex : -1;

                // Determine base pixels
                if (i == 0)
                {
                    texture.SetPixels(transparentFrame);
                }
                else
                {
                    if (disposalMethod == 1)
                    {
                        texture.SetPixels(previousFrame);
                    }
                    else if (disposalMethod == 2)
                    {
                        texture.SetPixels(transparentFrame);
                    }
                    else if (disposalMethod == 3)
                    {
                        throw new NotImplementedException("Disposal method 3 is not implemented.");
                    }
                }

                // Set pixels from image data
                for (int j = 0; j < imageDescriptor.width; j++)
                {
                    for (int k = 0; k < imageDescriptor.height; k++)
                    {
                        int x = left + j;
                        int y = (canvasHeight - 1) - (top + k);
                        int colorIndex = imageData.colorIndices[j + k * imageDescriptor.width];
                        int pixelOffset = x + y * canvasWidth;

                        if (colorIndex != transparencyIndex)
                        {
                            GIF.V89a.ColorTable.Color gifColor = imageData.getColor(colorIndex);

                            currentFrame[pixelOffset] = new Color(gifColor.r / 255f, gifColor.g / 255f, gifColor.b / 255f);
                        }
                    }
                }

                // Set texture pixels and create sprite
                texture.SetPixels(currentFrame);
                texture.Apply();
                texture.filterMode = FilterMode.Point;
                sprites.Add(Sprite.Create(texture, new Rect(0f, 0f, canvasWidth, canvasHeight), new Vector2(1f, 1f)));

                // Store current frame as previous before continuing, and reset current frame
                currentFrame.CopyTo(previousFrame, 0);
                if (disposalMethod == 0 || disposalMethod == 2)
                {
                    currentFrame = new Color[currentFrame.Length];
                }
            }

            return sprites;
        }

        public static BAD_GifImageData prepair(GIF.Data gif, int index)
        {
            BAD_GifImageData bad_GifImageData = new BAD_GifImageData(gif);
            bad_GifImageData.graphicsControlExt = gif.graphicControlExtensions[index];
            bad_GifImageData.imageDescriptor = gif.imageBlockDictionary[gif.graphicControlExtensions[index]].imageDescriptor;
            bad_GifImageData.localColorTable = gif.imageBlockDictionary[gif.graphicControlExtensions[index]].localColorTable;
            bad_GifImageData.lzwMinimumCodeSize = gif.imageBlockDictionary[gif.graphicControlExtensions[index]].imageData.LZWMinimumCodeSize;
            foreach (byte[] bad_data in gif.imageBlockDictionary[gif.graphicControlExtensions[index]].imageData.dataSubBlocks)
            {
                foreach (byte bad_byte in bad_data)
                {
                    bad_GifImageData.blockBytes.Add(bad_byte);
                }
            }
            bad_GifImageData.decode();
            return bad_GifImageData;
        }
    }
}