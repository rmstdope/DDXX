using System;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface ITextureFactory
    {
        ITexture CreateFromFile(string file);
        ITexture CreateFullsizeRenderTarget(Format format);
        ITexture CreateFullsizeRenderTarget();
    }
}
