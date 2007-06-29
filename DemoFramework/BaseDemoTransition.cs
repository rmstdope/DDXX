using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseDemoTransition : TweakableContainer, IDemoTransition
    {
        private float startTime;
        private float endTime;
        private int destinationTrack;
        private IPostProcessor postProcessor;

        public int DestinationTrack
        {
            get { return destinationTrack; }
            set { destinationTrack = value; }
        }

        public IPostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected BaseDemoTransition(float startTime, float endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public void Initialize(IPostProcessor postProcessor)
        {
            this.postProcessor = postProcessor;
        }

        abstract public ITexture Combine(ITexture fromTexture, ITexture toTexture);

        public float StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }

        public float EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }

    }
}
