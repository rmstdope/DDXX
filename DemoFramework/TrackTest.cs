using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TrackTest : D3DMockTest
    {
        private Track track;

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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            track = new Track();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestSingleEffectRegistration()
        {
            IDemoEffect e1 = CreateMockEffect(0, 10);

            track.Register(e1);

            Assert.AreEqual(new IDemoEffect[] { e1 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(15.0f));

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f, -0.1f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(-5.0f, 0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f, 5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(10.0f, 15.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(11.0f, 15.0f));
        }

        [Test]
        public void TestSinglePostEffectRegistration()
        {
            IDemoPostEffect e1 = CreateMockPostEffect(0, 10);

            track.Register(e1);

            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(0.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(10.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(15.0f));

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f, -0.1f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(-5.0f, 0.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(0.0f, 5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(10.0f, 15.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(11.0f, 15.0f));
        }

        [Test]
        public void TestMultipleEffectRegistrations()
        {
            IDemoEffect e1 = CreateMockEffect(0, 10);
            IDemoEffect e2 = CreateMockEffect(5, 15);

            track.Register(e1);
            track.Register(e2);

            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IDemoEffect[] { e2 }, track.GetEffects(15.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(20.0f));

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f, -1.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(-1.0f, 3.0f));
            Assert.AreEqual(new IDemoEffect[] { e1, e2 }, track.GetEffects(3.0f, 6.0f));
            Assert.AreEqual(new IDemoEffect[] { e2 }, track.GetEffects(11.0f, 16.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(16.0f, 17.0f));
        }
        [Test]
        public void TestMultiplePostEffectRegistrations()
        {
            IDemoPostEffect e1 = CreateMockPostEffect(0, 10);
            IDemoPostEffect e2 = CreateMockPostEffect(5, 15);

            track.Register(e1);
            track.Register(e2);

            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(0.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.GetPostEffects(5.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.GetPostEffects(10.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e2 }, track.GetPostEffects(15.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(20.0f));

            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(-5.0f, -1.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1 }, track.GetPostEffects(-1.0f, 3.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e1, e2 }, track.GetPostEffects(3.0f, 6.0f));
            Assert.AreEqual(new IDemoPostEffect[] { e2 }, track.GetPostEffects(11.0f, 16.0f));
            Assert.AreEqual(new IDemoPostEffect[] { }, track.GetPostEffects(16.0f, 17.0f));
        }

        [Test]
        public void TestEffectSorting()
        {
            IDemoEffect e1 = CreateMockEffect(5, 10);
            IDemoEffect e2 = CreateMockEffect(0, 15);
            IDemoEffect e3 = CreateMockEffect(10, 15);
            IDemoEffect e4 = CreateMockEffect(0, 10);
            IDemoEffect e5 = CreateMockEffect(5, 15);

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IDemoEffect[] { e4, e2, e1, e5, e3 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { e4, e2, e1, e5 }, track.GetEffects(5.0f));
        }
        [Test]
        public void TestPostEffectSorting()
        {
            IDemoPostEffect e1 = CreateMockPostEffect(5, 10);
            IDemoPostEffect e2 = CreateMockPostEffect(0, 15);
            IDemoPostEffect e3 = CreateMockPostEffect(10, 15);
            IDemoPostEffect e4 = CreateMockPostEffect(0, 10);
            IDemoPostEffect e5 = CreateMockPostEffect(5, 15);

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IDemoPostEffect[] { e4, e2, e1, e5, e3 }, track.PostEffects);

            Assert.AreEqual(new IDemoPostEffect[] { e4, e2, e1, e5 }, track.GetPostEffects(5.0f));
        }

    }
}
