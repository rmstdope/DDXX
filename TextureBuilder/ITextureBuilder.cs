using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
{
    public interface ITextureBuilder
    {
        ITexture2D Generate(IGenerator generator, int width, int height, int numMipLevels, SurfaceFormat format);
    }
}
