using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Graphics
{
    public interface IGraphicsFactory
    {
        GraphicsDeviceManager GraphicsDeviceManager { get; }
        ContentManager ContentManager { get; }
        GraphicsDevice GraphicsDevice { get; }
        TextureFactory TextureFactory { get; }
        ModelFactory ModelFactory { get; }
        EffectFactory EffectFactory { get; }

        //RenderTarget2D CreateRenderTarget2D(int width, int height, SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality);
        //Texture2D CreateTexture2D(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format);
        Texture2D Texture2DFromFile(string name);
        TextureCube TextureCubeFromFile(string name);
        //VertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, BufferUsage usage);
        //SpriteBatch CreateSpriteBatch();
        Effect EffectFromFile(string name);
        SpriteFont SpriteFontFromFile(string name);
        CustomModel ModelFromFile(string name);
        //IndexBuffer CreateIndexBuffer(Type indexType, int elementCount, BufferUsage usage);
        //VertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElement);
        //BasicEffect CreateBasicEffect();
        void SetScreen(int width, int height, bool fullscreen);
    }
}
