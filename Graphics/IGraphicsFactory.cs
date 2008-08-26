using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public interface IGraphicsFactory
    {
        IDeviceManager GraphicsDeviceManager { get; }
        IGraphicsDevice GraphicsDevice { get; }
        IRenderTarget2D CreateRenderTarget2D(int width, int height, int numLevels, SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality);
        ITexture2D CreateTexture2D(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format);
        IDepthStencilBuffer CreateDepthStencilBuffer(int width, int height, DepthFormat format, MultiSampleType multiSampleType);
        ITexture2D Texture2DFromFile(string name);
        ITextureCube TextureCubeFromFile(string name);
        IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, BufferUsage usage);
        ISpriteBatch CreateSpriteBatch();
        IEffect EffectFromFile(string name);
        ISpriteFont SpriteFontFromFile(string name);
        IModel ModelFromFile(string name);
        IIndexBuffer CreateIndexBuffer(Type indexType, int elementCount, BufferUsage usage);
        IVertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElement);
        IBasicEffect CreateBasicEffect();
    }
}
