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
        private IDevice device;
        private IEffectFactory effectFactory;
        private IModelFactory modelFactory;
        private ITextureFactory textureFactory;
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

        protected IEffectFactory EffectFactory
        {
            get { return effectFactory; }
        }

        protected IModelFactory ModelFactory
        {
            get { return modelFactory; }
        }

        protected ITextureFactory TextureFactory
        {
            get { return textureFactory; }
        }

        protected IXLoader XLoader
        {
            get { return xLoader; }
        }

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

        public virtual void Initialize()
        {
            device = D3DDriver.GetInstance().Device;
            effectFactory = D3DDriver.EffectFactory;
            modelFactory = D3DDriver.ModelFactory;
            textureFactory = D3DDriver.TextureFactory;
            xLoader = new XLoader(D3DDriver.GraphicsFactory, new NodeFactory(textureFactory), device);
        }

        #endregion

    }
}
