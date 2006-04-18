using System;
using System.Collections.Generic;
using System.Text;

namespace DemoFramework
{
    using NUnit.Framework;
    using NMock;

    [TestFixture]
    public class DemoExecuterTest : TrackTest
    {
        DemoExecuter executer;

        [SetUp]
        public override void Setup()
        {
            base.Setup();
            executer = new DemoExecuter();
        }

        [Test]
        public void TestTracks()
        {
            DynamicMock mock1 = CreateMockEffect(0, 10);
            IEffect t0e1 = (IEffect)mock1.MockInstance;
            DynamicMock mock2 = CreateMockEffect(5, 15);
            IEffect t1e1 = (IEffect)mock2.MockInstance;

            //mock1.Expect("Step");

            Assert.AreEqual(0, executer.NumTracks);
            executer.Register(0, t0e1);
            Assert.AreEqual(1, executer.NumTracks);
            executer.Register(1, t1e1);
            Assert.AreEqual(2, executer.NumTracks);

            mock1.Verify();
            //Assert.AreEqual(new IEffect[] { }, executer.GetEffects(0, -5.0f));
            ////Assert.AreEqual(new IEffect[] { }, executer.GetEffects(1, -5.0f));
            //Assert.AreEqual(new IEffect[] { t0e1 }, executer.GetEffects(0, 0.0f));
            ////Assert.AreEqual(new IEffect[] {      }, executer.GetEffects(1, 0.0f));
            //Assert.AreEqual(new IEffect[] { t0e1 }, executer.GetEffects(0, 5.0f));
            ////Assert.AreEqual(new IEffect[] { t1e1 }, executer.GetEffects(1, 5.0f));
            //Assert.AreEqual(new IEffect[] { t0e1 }, executer.GetEffects(0, 10.0f));
            ////Assert.AreEqual(new IEffect[] { t1e1 }, executer.GetEffects(1, 10.0f));
            //Assert.AreEqual(new IEffect[] {      }, executer.GetEffects(0, 15.0f));
            ////Assert.AreEqual(new IEffect[] { t1e1 }, executer.GetEffects(1, 15.0f));
            //Assert.AreEqual(new IEffect[] { }, executer.GetEffects(0, 20.0f));
            ////Assert.AreEqual(new IEffect[] { }, executer.GetEffects(1, 20.0f));
        }

        [Test]
        public void TestExecution()
        {
        }
    }
}
