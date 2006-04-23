using System;
using System.Collections.Generic;
using System.Text;
using Utility;

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

            Assert.AreEqual(0, executer.NumTracks);
            executer.Register(0, t0e1);
            Assert.AreEqual(1, executer.NumTracks);
            executer.Register(1, t1e1);
            Assert.AreEqual(2, executer.NumTracks);

            //mock1.Expect("Step");
            mock1.Verify();
        }

        [Test]
        public void TestStep()
        {
            DynamicMock mock1 = CreateMockEffect(0, 10);
            IEffect t0e1 = (IEffect)mock1.MockInstance;
            DynamicMock mock2 = CreateMockEffect(10, 15);
            IEffect t0e2 = (IEffect)mock2.MockInstance;
            DynamicMock mock3 = CreateMockEffect(3, 12);
            IEffect t1e1 = (IEffect)mock3.MockInstance;
            DynamicMock mock4 = CreateMockEffect(1, 2);
            IEffect t1e2 = (IEffect)mock4.MockInstance;

            executer.Register(0, t0e1);
            executer.Register(0, t0e2);
            executer.Register(1, t1e1);
            executer.Register(1, t1e2);


            Time.Initialize();
            Time.Pause();

            mock1.Expect("Step");
            mock3.Expect("Step");
            Time.CurrentTime = 5;
            executer.Step();
            mock1.Verify();
            mock2.Verify();
            mock3.Verify();
            mock4.Verify();

            mock1.Expect("Step");
            mock4.Expect("Step");
            Time.CurrentTime = 1;
            executer.Step();
            mock1.Verify();
            mock2.Verify();
            mock3.Verify();
            mock4.Verify();

            Time.CurrentTime = 20;
            executer.Step();
            mock1.Verify();
            mock2.Verify();
            mock3.Verify();
            mock4.Verify();
        }

        [Test]
        public void TestRegister()
        {
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(0.0f, executer.EndTime);

            DynamicMock mock1 = CreateMockEffect(1, 2);
            IEffect t0e1 = (IEffect)mock1.MockInstance;
            executer.Register(0, t0e1);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(2.0f, executer.EndTime);

            DynamicMock mock2 = CreateMockEffect(5, 100);
            IEffect t10e1 = (IEffect)mock2.MockInstance;
            executer.Register(0, t10e1);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(100.0f, executer.EndTime);
        }


        [Test]
        public void TestEmptyRun()
        {
            executer.Run();
            Assert.AreEqual(2.0f, Time.StepTime, 0.001f);

            DynamicMock mock1 = CreateMockEffect(0, 0.1f);
            IEffect t0e1 = (IEffect)mock1.MockInstance;
            executer.Register(0, t0e1);
            executer.Run();
            Assert.AreEqual(2.1f, Time.StepTime, 0.001f);
        }
    }
}
