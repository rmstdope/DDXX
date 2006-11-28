using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;

namespace EngineTest
{
    class EmptyEffect : BaseDemoEffect
    {
        public EmptyEffect(float startTime, float endTime) 
            : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartTimeUpdated()
        {
        }

        public override void EndTimeUpdated()
        {
        }

        public override void Step()
        {
        }

        public override void Render()
        {
        }
    }
}
