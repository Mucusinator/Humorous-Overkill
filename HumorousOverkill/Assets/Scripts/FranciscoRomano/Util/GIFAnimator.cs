using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//http://web.archive.org/web/20050217131148/http://www.danbbs.dk/~dino/whirlgif/lzw.html

public class GIFAnimator : MonoBehaviour
{
    private int index;
    private float previousTime = 0;

    public float delay = 0.2f;
    public TextAsset file;
    public UnityEngine.UI.Image image;
    public List<Sprite> sprites = new List<Sprite>();


    void Start()
    {
        // Get GIF textures
        //StartCoroutine(UniGif.GetTextureListCoroutine(file.bytes, (gifTexList, loopCount, width, height) => {
        //    foreach (UniGif.GifTexture giftexture in gifTexList)
        //    {
        //        sprites.Add(Sprite.Create(giftexture.m_texture2d, new Rect(0, 0, width, height), new Vector2(1, 1)));
        //    }
        //    previousTime = Time.time;
        //}));

        //FranciscoRomano.Util.GIF.V89aData gifData;
        //gifData = FranciscoRomano.Util.GIF.V89aReader.Read(file.bytes);
        //for (int i = 0; i < gifData.images.Count; i++)
        //{
        //    sprites.Add(Decompress(gifData, i));
        //}

        string value = "ABACABA";
        Debug.Log("result       = " + value);

        string cpd_value = "";
        List<int> cpd_stream = LZW.compress(value, "ABCD".ToCharArray());
        foreach (int n in cpd_stream) cpd_value += n + ",";
        Debug.Log("compressed   = " + cpd_value.Substring(0, cpd_value.Length - 1) + " [[ EXPECTED >> 0,1,0,2,4,0 ]]");

        string dpd_value = LZW.decompress(cpd_stream, "ABCD".ToCharArray());
        Debug.Log("decompressed = " + dpd_value + " [[ EXPECTED >> " + value + " ]]");

    }

    void Update()
    {
        if (sprites.Count == 0) return;
        if (Time.time - previousTime > delay)
        {
            previousTime = Time.time;
            image.sprite = sprites[index++];
            if (!(index < sprites.Count)) index = 0;
        }
    }

    //private Sprite Decompress(FranciscoRomano.Util.GIF.V89aData data, int index)
    //{
    //    FranciscoRomano.Util.GIF.V89a.ColorTable colorTable = data.header.globalColorTable;
    //    FranciscoRomano.Util.GIF.V89aData.Image image = data.images[index];
    //    int height = (int)data.header.logicalScreenDescriptor.height;
    //    int width = (int)data.header.logicalScreenDescriptor.width;
    //    Texture2D texture = new Texture2D(width, height);
    //    List<int> indexList = Decompress(image.imageData);

    //    if (image.localColorTable.colors.Length > 0)
    //    {
    //        colorTable = image.localColorTable;
    //    }

    //    int i = 0;
    //    int minY = (int)image.imageDescriptor.top;
    //    int minX = (int)image.imageDescriptor.left;
    //    int maxX = minX + (int)image.imageDescriptor.width;
    //    int maxY = minY + (int)image.imageDescriptor.height;

    //    for (int y = minY; y < maxY; y++)
    //    {
    //        for (int x = minX; x < maxX; x++)
    //        {
    //            if (i < indexList.Count)
    //            {
    //                if (indexList[i] < colorTable.colors.Length)
    //                {
    //                    texture.SetPixel(x, y, colorTable.colors[indexList[i++]]);
    //                }
    //            }
    //        }
            
    //    }

    //    texture.Apply();
    //    texture.filterMode = FilterMode.Point;

    //    Sprite sprite = Sprite.Create(texture, new Rect(0, 0, width, height), new Vector2(1, 1));

    //    return sprite;
    //}

    //private List<int> Decompress(FranciscoRomano.Util.GIF.V89a.ImageData imageData)
    //{
    //    LzwTools.Decoder decoder = new LzwTools.Decoder((byte)(imageData.LZWMinimumCodeSize + 1));
    //    return decoder.Decode(imageData.dataSubBlocks.packedBytes);
    //}

    private class LZW
    {
        public static List<int> compress(string datastream, char[] tablelist)
        {
            // create table
            Dictionary<string, int> table = new Dictionary<string, int>();
            foreach (char n in tablelist) table.Add("" + n, table.Count);
            // initial setup
            string currentKey = "";
            List<int> codestream = new List<int>();
            // iterate through data stream
            foreach (char nextKey in datastream)
            {
                // check if table contains data
                if (table.ContainsKey(currentKey + nextKey))
                {
                    // increment value
                    currentKey = currentKey + nextKey;
                }
                else
                {
                    // append new table values
                    table.Add(currentKey + nextKey, table.Count);
                    // append new code values
                    codestream.Add(table[currentKey]);
                    // updata current key
                    currentKey = "" + nextKey;
                }
            }
            // final append & return
            codestream.Add(table[currentKey]);
            return codestream;
        }

        public static string decompress(List<int> codestream, char[] tablelist)
        {
            // create table
            Dictionary<int, string> table = new Dictionary<int, string>();
            foreach (char n in tablelist) table.Add(table.Count, "" + n);
            // initial setup
            int previousKey = codestream[0];
            string datastream = table[previousKey];
            // iterate through data stream
            for (int i = 1; i < codestream.Count; i++)
            {
                // check if table contains data
                if (table.ContainsKey(codestream[i]))
                {
                    datastream += table[codestream[i]];
                    string temp = table[previousKey];
                    char k = table[codestream[i]][0];
                    table.Add(table.Count, temp + k);
                    previousKey = codestream[i];
                }
                else
                {
                    string temp = table[previousKey];
                    char k = temp[0];
                    datastream += temp + k;
                    table.Add(table.Count, temp + k);
                    previousKey = codestream[i];
                }
            }
            // return
            return datastream;
        }
    }
}