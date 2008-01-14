using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoPostEffect : Registerable, IDemoPostEffect
    {
        private int drawOrder;
        private IPostProcessor postProcessor;
        private ITextureFactory textureFactory;
        private ITextureBuilder textureBuilder;
        private IGraphicsFactory graphicsFactory;

        protected IGraphicsFactory GraphicsFactory
        {
            get { return graphicsFactory; }
        }

        protected IGraphicsDevice GraphicsDevice
        {
            get { return graphicsFactory.GraphicsDeviceManager.GraphicsDevice; }
        }

        protected IPostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected ITextureFactory TextureFactory
        {
            get { return textureFactory; }
        }

        protected ITextureBuilder TextureBuilder
        {
            get { return textureBuilder; }
        }

        public int DrawOrder
        {
            set { drawOrder = value; }
            get { return drawOrder; }
        }

        protected BaseDemoPostEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
            drawOrder = 0;
        }

        protected abstract void Initialize();

        #region IDemoPostEffect Members

        public abstract void Render();

        public void Initialize(IGraphicsFactory graphicsFactory, IPostProcessor postProcessor, ITextureFactory textureFactory, ITextureBuilder textureBuilder/*, IDevice device*/)
        {
            this.graphicsFactory = graphicsFactory;
            this.postProcessor = postProcessor;
            this.textureFactory = textureFactory;
            this.textureBuilder = textureBuilder;
            Initialize();
        }

        #endregion


    }
}
