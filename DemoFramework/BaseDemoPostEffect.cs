using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoPostEffect : IDemoPostEffect
    {
        private float startTime;
        private float endTime;
        private PostProcessor postProcessor;

        protected PostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected BaseDemoPostEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        #region IDemoPostEffect Members

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; StartTimeUpdated(); }
        }

        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; EndTimeUpdated(); }
        }

        public abstract void Render();

        public virtual void StartTimeUpdated()
        {
        }

        public virtual void EndTimeUpdated()
        {
        }

        public virtual void Initialize(PostProcessor postProcessor)
        {
            this.postProcessor = postProcessor;
        }

        #endregion
    }
}
