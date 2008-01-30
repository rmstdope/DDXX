using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoPostEffect : Registerable, IDemoPostEffect
    {
        private int drawOrder;
        private IPostProcessor postProcessor;
        private ITextureFactory textureFactory;
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

        public void Initialize(IGraphicsFactory graphicsFactory, IPostProcessor postProcessor, ITextureFactory textureFactory)
        {
            this.graphicsFactory = graphicsFactory;
            this.postProcessor = postProcessor;
            this.textureFactory = textureFactory;
            Initialize();
        }

        #endregion


    }
}
