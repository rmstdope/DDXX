using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoTransition : Registerable, IDemoTransition
    {
        private int destinationTrack;
        private IPostProcessor postProcessor;
        private IGraphicsFactory graphicsFactory;

        public int DestinationTrack
        {
            get { return destinationTrack; }
            set { destinationTrack = value; }
        }

        public IPostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected ITextureFactory TextureFactory
        {
            get { return graphicsFactory.TextureFactory; }
        }

        protected BaseDemoTransition(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected virtual void Initialize() { }
        public void Initialize(IPostProcessor postProcessor, IGraphicsFactory graphicsFactory)
        {
            this.postProcessor = postProcessor;
            this.graphicsFactory = graphicsFactory;
            Initialize();
        }

        abstract public IRenderTarget2D Combine(IRenderTarget2D fromTexture, IRenderTarget2D toTexture);

        public IRenderTarget2D Render(IRenderTarget2D fromTexture, IRenderTarget2D toTexture)
        {
            return Combine(fromTexture, toTexture);
        }

    }
}
