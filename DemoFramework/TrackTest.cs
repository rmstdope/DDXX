using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace DemoFramework
{
    [TestFixture]
    public class TrackTest
    {
        protected Mockery mockery;
        private Track track;

        protected IEffect CreateMockEffect(float start, float end)
        {
            IEffect e = mockery.NewMock<IEffect>();
            Stub.On(e).
                GetProperty("StartTime").
                Will(Return.Value(start));
            Stub.On(e).
                GetProperty("EndTime").
                Will(Return.Value(end));
            return e;
        }

        [SetUp]
        public virtual void Setup()
        {
            mockery = new Mockery();
            track = new Track();
        }

        [Test]
        public void TestSingleRegistration()
        {
            IEffect e1 = CreateMockEffect(0, 10);

            track.Register(e1);

            Assert.AreEqual(new IEffect[] { e1 }, track.Effects);

            Assert.AreEqual(new IEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IEffect[] { }, track.GetEffects(15.0f));
        }

        [Test]
        public void TestMultipleRegistrations()
        {
            IEffect e1 = CreateMockEffect(0, 10);
            IEffect e2 = CreateMockEffect(5, 15);

            track.Register(e1);
            track.Register(e2);

            Assert.AreEqual(new IEffect[] { e1, e2 }, track.Effects);

            Assert.AreEqual(new IEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IEffect[] { e1, e2 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IEffect[] { e1, e2 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IEffect[] { e2 }, track.GetEffects(15.0f));
            Assert.AreEqual(new IEffect[] { }, track.GetEffects(20.0f));
        }

        [Test]
        public void TestSorting()
        {
            IEffect e1 = CreateMockEffect(5, 10);
            IEffect e2 = CreateMockEffect(0, 15);
            IEffect e3 = CreateMockEffect(10, 15);
            IEffect e4 = CreateMockEffect(0, 10);
            IEffect e5 = CreateMockEffect(5, 15);

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IEffect[] { e4, e2, e1, e5, e3 }, track.Effects);

            Assert.AreEqual(new IEffect[] { e4, e2, e1, e5 }, track.GetEffects(5.0f));
        }

    }
}
