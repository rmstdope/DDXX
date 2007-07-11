using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;

namespace EngineTest
{
    class EmptyEffect : BaseDemoEffect
    {
        public EmptyEffect(string name, float startTime, float endTime) 
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
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
