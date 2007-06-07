using System;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    public interface ITextureFactory
    {
        ITexture CreateFromFile(string name);
        ICubeTexture CreateCubeFromFile(string name);
        ITexture CreateFullsizeRenderTarget(Format format);
        ITexture CreateFullsizeRenderTarget();
        ITexture CreateFromFunction(int width, int height, int numLevels, Usage usage, Format format, Pool pool, Fill2DTextureCallback callbackFunction);
        void RegisterTexture(string name, ITexture texture);
    }
}
