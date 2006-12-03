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
        private EffectFactory effectFactory;
        private ModelFactory meshFactory;
        private Scene scene;
        List<ITweakable> tweakables;

        protected BaseDemoEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            tweakables = new List<ITweakable>();
        }

        protected IDevice Device
        {
            get { return device; }
        }

        protected EffectFactory EffectFactory
        {
            get { return effectFactory; }
        }

        protected ModelFactory ModelFactory
        {
            get { return meshFactory; }
        }

        protected Scene Scene
        {
            get { return scene; }
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
            device = D3DDriver.GetInstance().GetDevice();
            effectFactory = D3DDriver.EffectFactory;
            meshFactory = D3DDriver.ModelFactory;
            scene = new Scene();
        }

        #endregion

        #region ITweakableContainer Members

        public List<ITweakable> Tweakables
        {
            get { return tweakables; }
        }

        public void Add(ITweakable tweakable)
        {
            tweakables.Add(tweakable);
        }

        #endregion
    }
}
