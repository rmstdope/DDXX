using System;
using System.Collections.Generic;
using System.Text;
using DemoFramework;
using Direct3D;

namespace EngineTest
{
    public class TestEffect : BaseEffect
    {
        public TestEffect(float startTime, float endTime) : base(startTime, endTime)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            IMesh mesh = Factory.CreateBoxMesh(Device, 10.0f, 10.0f, 10.0f);
        }

        public override void Step()
        {
        }

        public override void Render()
        {
        }
    }
}
