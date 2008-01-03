using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Dope.DDXX.ContentProcessor
{
    [ContentProcessor]
    public class NormalMapTextureProcessor : ContentProcessor<TextureContent, TextureContent>
    {
        public override TextureContent Process(TextureContent input, ContentProcessorContext context)
        {
            ConvertToVector4(input);
            ReencodeNormalMap(input);
            ConvertToByte4(input);
            GenerateMipMaps(input);
            return input;
        }

        private static void ReencodeNormalMap(TextureContent input)
        {
            foreach (MipmapChain mipmapChain in input.Faces)
            {
                foreach (PixelBitmapContent<Vector4> bitmap in mipmapChain)
                {
                    for (int x = 0; x < bitmap.Width; x++)
                    {
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            Vector4 encoded = bitmap.GetPixel(x, y);
                            bitmap.SetPixel(x, y, 2 * encoded - Vector4.One);
                        }
                    }
                }
            }
        }

        private static void GenerateMipMaps(TextureContent input)
        {
            input.GenerateMipmaps(false);
        }

        private static void ConvertToByte4(TextureContent input)
        {
            input.ConvertBitmapType(typeof(PixelBitmapContent<NormalizedByte4>));
        }

        private static void ConvertToVector4(TextureContent input)
        {
            input.ConvertBitmapType(typeof(PixelBitmapContent<Vector4>));
        }
    }
}
