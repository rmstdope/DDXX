using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoPostEffect : TweakableContainer, IDemoPostEffect
    {
        private float startTime;
        private float endTime;
        private IPostProcessor postProcessor;
        List<ITweakable> tweakables;

        protected IPostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected BaseDemoPostEffect(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
            tweakables = new List<ITweakable>();
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

        public virtual void Initialize(IPostProcessor postProcessor)
        {
            this.postProcessor = postProcessor;
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
