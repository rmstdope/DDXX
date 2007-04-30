using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoEffect : TweakableContainer, IDemoEffect
    {
        private float startTime;
        private float endTime;
        private IGraphicsFactory graphicsFactory;
        private IDevice device;
        private IXLoader xLoader;

        protected BaseDemoEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        protected IDevice Device
        {
            get { return device; }
        }

        protected IGraphicsFactory GraphicsFactory
        {
            get { return graphicsFactory; }
        }

        protected IEffectFactory EffectFactory
        {
            get { return D3DDriver.EffectFactory; }
        }

        protected IModelFactory ModelFactory
        {
            get { return D3DDriver.ModelFactory; }
        }

        protected ITextureFactory TextureFactory
        {
            get { return D3DDriver.TextureFactory; }
        }

        protected IXLoader XLoader
        {
            get { return xLoader; }
        }

        protected abstract void Initialize();

        #region IDemoEffect Members

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; StartTimeUpdated();  }
        }

        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; EndTimeUpdated();  }
        }

        public abstract void Step();

        public abstract void Render();

        public virtual void StartTimeUpdated()
        {
        }

        public virtual void EndTimeUpdated()
        {
        }

        public void Initialize(IGraphicsFactory graphicsFactory, IDevice device)
        {
            this.graphicsFactory = graphicsFactory;
            this.device = device;

            xLoader = new XLoader(GraphicsFactory, new NodeFactory(TextureFactory), Device);
            
            Initialize();
        }

        #endregion

    }
}
