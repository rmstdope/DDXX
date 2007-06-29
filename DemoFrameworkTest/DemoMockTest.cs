using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.DemoFramework
{
    public class DemoMockTest : D3DMockTest
    {
        protected IPostProcessor postProcessor;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            postProcessor = mockery.NewMock<IPostProcessor>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        protected IDemoEffect CreateMockEffect(float start, float end)
        {
            IDemoEffect e = mockery.NewMock<IDemoEffect>();
            Stub.On(e).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(e).
                GetProperty("EndTime").
                Will(Return.Value(end));
            return e;
        }

        protected IDemoPostEffect CreateMockPostEffect(float start, float end)
        {
            IDemoPostEffect e = mockery.NewMock<IDemoPostEffect>();
            Stub.On(e).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(e).
                GetProperty("EndTime").
                Will(Return.Value(end));
            return e;
        }

        protected IDemoTransition CreateMockTransition(int destinationTrack, float start, float end)
        {
            IDemoTransition t = mockery.NewMock<IDemoTransition>();
            Stub.On(t).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(t).
                GetProperty("EndTime").
                Will(Return.Value(end));
            Stub.On(t).
                GetProperty("DestinationTrack").
                Will(Return.Value(destinationTrack));
            return t;
        }

    }
}
