using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoPostEffect : Registerable, IDemoPostEffect
    {
        private int drawOrder;
        private PostProcessor postProcessor;
        private IGraphicsFactory graphicsFactory;

        protected IGraphicsFactory GraphicsFactory
        {
            get { return graphicsFactory; }
        }

        protected GraphicsDevice GraphicsDevice
        {
            get { return graphicsFactory.GraphicsDevice; }
        }

        protected PostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected TextureFactory TextureFactory
        {
            get { return graphicsFactory.TextureFactory; }
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

        public abstract void Render();

        public void Initialize(IGraphicsFactory graphicsFactory, PostProcessor postProcessor)
        {
            this.graphicsFactory = graphicsFactory;
            this.postProcessor = postProcessor;
            Initialize();
        }

    }
}
