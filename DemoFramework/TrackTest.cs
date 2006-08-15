using System;
using System.Collections.Generic;
using System.Text;
using Graphics;
using NUnit.Framework;
using NMock2;

namespace DemoFramework
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
        public void TestSingleRegistration()
        {
            IDemoEffect e1 = CreateMockEffect(0, 10);

            track.Register(e1);

            Assert.AreEqual(new IDemoEffect[] { e1 }, track.Effects);

            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(-5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(0.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(5.0f));
            Assert.AreEqual(new IDemoEffect[] { e1 }, track.GetEffects(10.0f));
            Assert.AreEqual(new IDemoEffect[] { }, track.GetEffects(15.0f));
        }

        [Test]
        public void TestMultipleRegistrations()
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
        }

        [Test]
        public void TestSorting()
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

    }
}
