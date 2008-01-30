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
        protected IDemoMixer mixer;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            postProcessor = mockery.NewMock<IPostProcessor>();
            mixer = mockery.NewMock<IDemoMixer>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        protected IDemoEffect CreateMockEffect(string name, float start, float end)
        {
            IDemoEffect e = mockery.NewMock<IDemoEffect>();
            Stub.On(e).
                GetProperty("Name").
                Will(Return.Value(name));
            Stub.On(e).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(e).
                GetProperty("EndTime").
                Will(Return.Value(end));
            return e;
        }

        protected IDemoPostEffect CreateMockPostEffect(string name, float start, float end)
        {
            IDemoPostEffect e = mockery.NewMock<IDemoPostEffect>();
            Stub.On(e).
                GetProperty("Name").
                Will(Return.Value(name));
            Stub.On(e).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(e).
                GetProperty("EndTime").
                Will(Return.Value(end));
            return e;
        }

        protected IDemoTransition CreateMockTransition(string name, int destinationTrack, float start, float end)
        {
            IDemoTransition t = mockery.NewMock<IDemoTransition>();
            Stub.On(t).
                GetProperty("Name").
                Will(Return.Value(name));
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
