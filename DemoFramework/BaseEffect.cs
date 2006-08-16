using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseEffect : IDemoEffect
    {
        private float startTime;
        private float endTime;
        private IDevice device;
        //private IFactory factory;
        private EffectFactory effectFactory;
        private MeshFactory meshFactory;
        private Scene scene;

        protected BaseEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;

            scene = new Scene();
        }

        protected IDevice Device
        {
            get { return device; }
        }

        //protected IFactory Factory
        //{
        //    get { return factory; }
        //}

        protected EffectFactory EffectFactory
        {
            get { return effectFactory; }
        }

        protected MeshFactory MeshFactory
        {
            get { return meshFactory; }
        }

        protected Scene Scene
        {
            get { return scene; }
        }

        #region IEffect Members

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

        public abstract void StartTimeUpdated();

        public abstract void EndTimeUpdated();

        public virtual void Initialize()
        {
            device = D3DDriver.GetInstance().GetDevice();
            effectFactory = D3DDriver.EffectFactory;
            meshFactory = D3DDriver.MeshFactory;
        }

        #endregion
    }
}
