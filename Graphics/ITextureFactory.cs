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
        GraphicsDevice GraphicsDevice { get; }
        Texture2D CreateFromName(string name);
        TextureCube CreateCubeFromFile(string name);
        RenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, DepthFormat depthformat, int preferredMultiSampleCount);
        RenderTarget2D CreateFullsizeRenderTarget();
        Texture2D CreateFromFunction(int width, int height, bool mipMap, SurfaceFormat format, Generate2DTextureCallback callbackFunction);
        Texture2D CreateFromGenerator(string name, int width, int height, bool mipMap, SurfaceFormat format, ITextureGenerator generator);
        Texture2D WhiteTexture { get; }
        List<Texture2DParameters> Texture2DParameters { get; }
        void Update(Texture2DParameters Target);
    }
}
