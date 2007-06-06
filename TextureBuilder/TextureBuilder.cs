using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class TextureBuilder : ITextureBuilder
    {
        private ITextureFactory textureFactory;

        public TextureBuilder(ITextureFactory textureFactory)
        {
            this.textureFactory = textureFactory;
        }

        public ITexture Generate(IGenerator generator, int width, int height, int numMipLevels, Format format)
        {
            return textureFactory.CreateFromFunction(width, height, numMipLevels, Usage.None, format, Pool.Managed, generator.GetPixel);
        }
    }
}
