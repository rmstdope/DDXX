using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Dope.DDXX.Graphics
{
    public delegate Vector4 OldFill2DTextureCallback(Vector2 texCoord, Vector2 texelSize);
    public delegate Vector4[,] Generate2DTextureCallback(int width, int height);

    public interface ITextureFactory
    {
        bool TextureExists(string name);
        IGraphicsDevice GraphicsDevice { get; }
        ITexture2D CreateFromName(string name);
        ITextureCube CreateCubeFromFile(string name);
        IRenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality);
        IRenderTarget2D CreateFullsizeRenderTarget();
        IDepthStencilBuffer CreateFullsizeDepthStencil(DepthFormat format, MultiSampleType multiSampleType);
        ITexture2D CreateFromFunction(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format, Generate2DTextureCallback callbackFunction);
        ITexture2D CreateFromGenerator(string name, int width, int height, int numMipLevels, TextureUsage usage, SurfaceFormat format, ITextureGenerator generator);
        ITexture2D WhiteTexture { get; }
        List<Texture2DParameters> Texture2DParameters { get; }
        void Update(Texture2DParameters Target);
    }
}
