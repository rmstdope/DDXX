using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Dope.DDXX.Graphics
{
    public delegate Vector4 Fill2DTextureCallback(Vector2 texCoord, Vector2 texelSize);

    public interface ITextureFactory
    {
        ITexture2D CreateFromName(string name);
        ITextureCube CreateCubeFromFile(string name);
        IRenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality);
        IRenderTarget2D CreateFullsizeRenderTarget();
        IDepthStencilBuffer CreateFullsizeDepthStencil(DepthFormat format, MultiSampleType multiSampleType);
        ITexture2D CreateFromFunction(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format, Fill2DTextureCallback callbackFunction);
        ITexture2D CreateFromGenerator(string name, int width, int height, int numMipLevels, TextureUsage usage, SurfaceFormat format, ITextureGenerator generator);
        ITexture2D WhiteTexture { get; }
        List<Texture2DParameters> Texture2DParameters { get; }
    }
}
