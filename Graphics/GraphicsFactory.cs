using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Dope.DDXX.Graphics
{
    public class GraphicsFactory : IGraphicsFactory
    {
        private IDeviceManager deviceManager;
        private IContentManager contentManager;
        private ITextureFactory textureFactory;
        private IModelFactory modelFactory;
        private IEffectFactory effectFactory;

        public GraphicsFactory(Game game, IServiceProvider serviceProvider)
        {
            deviceManager = new GraphicsDeviceManagerAdapter(new GraphicsDeviceManager(game));
            contentManager = new ContentManagerAdapter(new ContentManager(serviceProvider));
            textureFactory = new TextureFactory(this);
            modelFactory = new ModelFactory(this, textureFactory);
            effectFactory = new EffectFactory(this);
        }

        public void SetScreen(int width, int height, bool fullscreen)
        {
            deviceManager.PreferredBackBufferWidth = width;
            deviceManager.PreferredBackBufferHeight = height;
            deviceManager.IsFullScreen = fullscreen;
        }

        public IDeviceManager GraphicsDeviceManager
        {
            get { return deviceManager; }
        }

        public IContentManager ContentManager
        {
            get { return contentManager; }
        }

        public IGraphicsDevice GraphicsDevice
        {
            get { return deviceManager.GraphicsDevice; }
        }

        public ITextureFactory TextureFactory 
        {
            get { return textureFactory; } 
        }
        
        public IModelFactory ModelFactory 
        {
            get { return modelFactory; }
        }
        
        public IEffectFactory EffectFactory 
        {
            get { return effectFactory; }
        }

        private GraphicsDevice DxGraphicsDevice
        {
            get { return (deviceManager.GraphicsDevice as GraphicsDeviceAdapter).DxGraphicsDevice; }
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, int numLevels,
            SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
        {
            return new RenderTarget2DAdapter(new RenderTarget2D(DxGraphicsDevice, width, height, numLevels, 
                format, multiSampleType, multiSampleQuality, RenderTargetUsage.PreserveContents));
        }

        public ITexture2D CreateTexture2D(int width, int height, int numLevels,
            TextureUsage usage, SurfaceFormat format)
        {
            return new Texture2DAdapter(new Texture2D(DxGraphicsDevice, width, height, numLevels, usage, format));
        }

        public IDepthStencilBuffer CreateDepthStencilBuffer(int width, int height, DepthFormat format, MultiSampleType multiSampleType)
        {
            return new DepthStencilBufferAdapter(new DepthStencilBuffer(DxGraphicsDevice, width, height, format, multiSampleType, 0));
        }

        public ITexture2D Texture2DFromFile(string srcFile)
        {
            return new Texture2DAdapter(contentManager.Load<Texture2D>(srcFile));
        }

        public ITextureCube TextureCubeFromFile(string srcFile)
        {
            return new TextureCubeAdapter(contentManager.Load<TextureCube>(srcFile));
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, BufferUsage usage)
        {
            return new VertexBufferAdapter(new VertexBuffer(DxGraphicsDevice, typeVertexType, numVerts, usage));
        }

        public ISpriteBatch CreateSpriteBatch()
        {
            return new SpriteBatchAdapter(new SpriteBatch(DxGraphicsDevice));
        }

        public IEffect EffectFromFile(string filename)
        {
            Effect effect = contentManager.Load<Effect>(filename);
            return new EffectAdapter(effect.Clone(DxGraphicsDevice));
        }

        public ISpriteFont SpriteFontFromFile(string name)
        {
            return new SpriteFontAdapter(contentManager.Load<SpriteFont>(name));
        }

        public IModel ModelFromFile(string name)
        {
            return new ModelAdapter(contentManager.Load<Model>(name));
        }


        public IIndexBuffer CreateIndexBuffer(Type indexType, int elementCount, BufferUsage usage)
        {
            return new IndexBufferAdapter(new IndexBuffer(DxGraphicsDevice, indexType, elementCount, usage));
        }

        public IVertexDeclaration CreateVertexDeclaration(VertexElement[] elements)
        {
            return new VertexDeclarationAdapter(new VertexDeclaration(DxGraphicsDevice, elements));
        }

        public IBasicEffect CreateBasicEffect()
        {
            return new BasicEffectAdapter(new BasicEffect(DxGraphicsDevice, null));
        }

    }
}
