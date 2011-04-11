using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoTransition : Registerable, IDemoTransition
    {
        private int destinationTrack;
        private PostProcessor postProcessor;
        private IGraphicsFactory graphicsFactory;

        public int DestinationTrack
        {
            get { return destinationTrack; }
            set { destinationTrack = value; }
        }

        public PostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected TextureFactory TextureFactory
        {
            get { return graphicsFactory.TextureFactory; }
        }

        protected BaseDemoTransition(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected virtual void Initialize() { }
        public void Initialize(PostProcessor postProcessor, IGraphicsFactory graphicsFactory)
        {
            this.postProcessor = postProcessor;
            this.graphicsFactory = graphicsFactory;
            Initialize();
        }

        abstract public RenderTarget2D Combine(RenderTarget2D fromTexture, RenderTarget2D toTexture);

        public RenderTarget2D Render(RenderTarget2D fromTexture, RenderTarget2D toTexture)
        {
            return Combine(fromTexture, toTexture);
        }

    }
}
