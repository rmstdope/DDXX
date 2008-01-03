using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
{
    public class TextureBuilder : ITextureBuilder
    {
        private ITextureFactory textureFactory;

        public TextureBuilder(ITextureFactory textureFactory)
        {
            this.textureFactory = textureFactory;
        }

        public ITexture2D Generate(IGenerator generator, int width, int height, int numMipLevels, SurfaceFormat format)
        {
            return textureFactory.CreateFromFunction(width, height, numMipLevels, TextureUsage.None, format, generator.GetPixel);
        }
    }
}
