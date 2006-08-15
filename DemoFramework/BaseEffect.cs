using System;
using System.Collections.Generic;
using System.Text;
using Graphics;
using SceneGraph;

namespace DemoFramework
{
    public abstract class BaseEffect : IDemoEffect
    {
        private float startTime;
        private float endTime;
        private IDevice device;
        private IFactory factory;
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

        protected IFactory Factory
        {
            get { return factory; }
        }

        protected Scene Scene
        {
            get { return scene; }
        }

        public virtual void Initialize()
        {
            device = D3DDriver.GetInstance().GetDevice();
            factory = D3DDriver.Factory;
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

        public virtual void StartTimeUpdated() { }

        public virtual void EndTimeUpdated() { }

        #endregion
    }
}
