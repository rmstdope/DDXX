using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoPostEffect : TweakableContainer, IDemoPostEffect
    {
        private int drawOrder;
        private float startTime;
        private float endTime;
        private IPostProcessor postProcessor;
        private ITextureFactory textureFactory;
        private ITextureBuilder textureBuilder;
        //private IDevice device;
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
            : base(name)
        {
            StartTime = startTime;
            EndTime = endTime;
            drawOrder = 0;
        }

        protected abstract void Initialize();

        #region IDemoPostEffect Members

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; StartTimeUpdated(); }
        }

        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; EndTimeUpdated(); }
        }

        public abstract void Render();

        public virtual void StartTimeUpdated()
        {
        }

        public virtual void EndTimeUpdated()
        {
        }

        public void Initialize(IGraphicsFactory graphicsFactory, IPostProcessor postProcessor, ITextureFactory textureFactory, ITextureBuilder textureBuilder/*, IDevice device*/)
        {
            this.graphicsFactory = graphicsFactory;
            this.postProcessor = postProcessor;
            this.textureFactory = textureFactory;
            this.textureBuilder = textureBuilder;
            //this.device = device;
            Initialize();
        }

        #endregion

    }
}
