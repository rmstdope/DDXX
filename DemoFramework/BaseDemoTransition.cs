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
        //private IGraphicsDevice device;

        public int DestinationTrack
        {
            get { return destinationTrack; }
            set { destinationTrack = value; }
        }

        public IPostProcessor PostProcessor
        {
            get { return postProcessor; }
        }

        protected BaseDemoTransition(string name, float startTime, float endTime)
            : base(name)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public void Initialize(/*IDevice device, */IPostProcessor postProcessor)
        {
            //this.device = device;
            this.postProcessor = postProcessor;
        }

        abstract public IRenderTarget2D Combine(IRenderTarget2D fromTexture, IRenderTarget2D toTexture);

        public IRenderTarget2D Render(IRenderTarget2D fromTexture, IRenderTarget2D toTexture)
        {
            return Combine(fromTexture, toTexture);
        }

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
