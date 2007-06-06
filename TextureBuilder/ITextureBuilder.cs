using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.TextureBuilder
{
    public interface ITextureBuilder
    {
        ITexture Generate(IGenerator generator, int width, int height, int numMipLevels, Format format);
    }
}
