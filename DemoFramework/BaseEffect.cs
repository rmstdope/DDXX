using System;
using System.Collections.Generic;
using System.Text;
using Direct3D;

namespace DemoFramework
{
    public abstract class BaseEffect : IEffect
    {
        private float startTime;
        private float endTime;
        private IDevice device;

        protected BaseEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        protected IDevice Device
        {
            get { return device; }
        }

        public virtual void Initialize()
        {
            device = D3DDriver.GetInstance().GetDevice();
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
