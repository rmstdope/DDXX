using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFramework
{
    using NUnit.Framework;
    using NMock;

    [TestFixture]
    public class TrackTest
    {
        Track track;

        protected DynamicMock CreateMockEffect(float start, float end)
        {
            DynamicMock mockEffect = new DynamicMock(typeof(IEffect));
            mockEffect.SetupResult("StartTime", start);
            mockEffect.SetupResult("EndTime", end);
            return mockEffect;
        }

        [SetUp]
        public virtual void Setup()
        {
            track = new Track();
        }

        [Test]
        public void TestSingleRegistration()
        {
            DynamicMock mock1 = CreateMockEffect(0, 10);
            IEffect e1 = (IEffect)mock1.MockInstance;

            track.Register(e1);

            Assert.AreEqual(new IEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IEffect[] { }, track.GetEffects(15.0f));

            mock1.Verify();
        }

        [Test]
        public void TestMultipleRegistrations()
        {
            DynamicMock mock1 = CreateMockEffect(0, 10);
            IEffect e1 = (IEffect)mock1.MockInstance;
            DynamicMock mock2 = CreateMockEffect(5, 15);
            IEffect e2 = (IEffect)mock2.MockInstance;

            track.Register(e1);
            track.Register(e2);

            Assert.AreEqual(new IEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IEffect[] { e1, e2 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IEffect[] { e1, e2 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IEffect[] { e2 }, track.GetEffects(15.0f));
            Assert.AreEqual(new IEffect[] { }, track.GetEffects(20.0f));

            mock1.Verify();
            mock2.Verify();
        }

        [Test]
        public void TestSorting()
        {
            DynamicMock mock1 = CreateMockEffect(5, 10);
            IEffect e1 = (IEffect)mock1.MockInstance;
            DynamicMock mock2 = CreateMockEffect(0, 15);
            IEffect e2 = (IEffect)mock1.MockInstance;
            DynamicMock mock3 = CreateMockEffect(10, 15);
            IEffect e3 = (IEffect)mock1.MockInstance;
            DynamicMock mock4 = CreateMockEffect(0, 10);
            IEffect e4 = (IEffect)mock1.MockInstance;
            DynamicMock mock5 = CreateMockEffect(5, 15);
            IEffect e5 = (IEffect)mock1.MockInstance;

            track.Register(e1);
            track.Register(e2);
            track.Register(e3);
            track.Register(e4);
            track.Register(e5);

            Assert.AreEqual(new IEffect[] { e4, e2, e1, e5, e3 }, track.GetEffects(5.0f));
        }

    }
}
