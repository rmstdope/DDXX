using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoEffect : TweakableContainer, IDemoEffect
    {
        private float startTime;
        private float endTime;
        private IDevice device;
        private IEffectFactory effectFactory;
        private ModelFactory meshFactory;

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

        protected ModelFactory ModelFactory
        {
            get { return meshFactory; }
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
            meshFactory = D3DDriver.ModelFactory;
        }

        #endregion

    }
}
