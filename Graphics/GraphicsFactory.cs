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
        private GraphicsDeviceManager deviceManager;
        private ContentManager contentManager;
        private TextureFactory textureFactory;
        private ModelFactory modelFactory;
        private EffectFactory effectFactory;

        public GraphicsFactory(Game game, IServiceProvider serviceProvider)
        {
            deviceManager = new GraphicsDeviceManager(game);
            contentManager = new ContentManager(serviceProvider);
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

        public GraphicsDeviceManager GraphicsDeviceManager
        {
            get { return deviceManager; }
        }

        public ContentManager ContentManager
        {
            get { return contentManager; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get { return deviceManager.GraphicsDevice; }
        }

        public TextureFactory TextureFactory 
        {
            get { return textureFactory; } 
        }
        
        public ModelFactory ModelFactory 
        {
            get { return modelFactory; }
        }
        
        public EffectFactory EffectFactory 
        {
            get { return effectFactory; }
        }

        public Texture2D Texture2DFromFile(string srcFile)
        {
            return contentManager.Load<Texture2D>(srcFile);
        }

        public TextureCube TextureCubeFromFile(string srcFile)
        {
            return contentManager.Load<TextureCube>(srcFile);
        }

        public Effect EffectFromFile(string filename)
        {
            Effect effect = contentManager.Load<Effect>(filename);
            return effect.Clone();
        }

        public SpriteFont SpriteFontFromFile(string name)
        {
            return contentManager.Load<SpriteFont>(name);
        }

        public CustomModel ModelFromFile(string name)
        {
            return contentManager.Load<CustomModel>(name);
        }

    }
}
