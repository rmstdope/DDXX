using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Direct3D;
using Utility;

namespace DemoFramework
{
    using NUnit.Framework;
    using NMock;

    [TestFixture]
    public class DemoExecuterTest : TrackTest
    {
        DemoExecuter executer;
        DynamicMock mockFactory;
        DynamicMock mockDevice;

        [SetUp]
        public override void Setup()
        {
            Time.Initialize();

            mockFactory = new DynamicMock(typeof(IFactory));
            mockDevice = new DynamicMock(typeof(IDevice));
            mockFactory.SetupResult("CreateDevice", (IDevice)mockDevice.MockInstance, typeof(int), typeof(DeviceType), typeof(Control), typeof(CreateFlags), typeof(PresentParameters));
            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            D3DDriver.SetFactory((IFactory)mockFactory.MockInstance);
            D3DDriver.GetInstance().Init(null, desc);
            mockDevice.Strict = true;
            mockFactory.Strict = true;

            base.Setup();
            executer = new DemoExecuter();
        }

        [TearDown]
        public void TearDown()
        {
            mockDevice.Expect("Dispose");
            D3DDriver.GetInstance().Reset();
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
            executer.Initialize();

            mockDevice.Expect("Clear", ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            mockDevice.Expect("Present");
            Time.CurrentTime = 2.0f;
            executer.Run();
            Assert.Greater(Time.StepTime, 2.0f);
            
            mockDevice.Expect("Clear", ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            mockDevice.Expect("Present");
            Time.CurrentTime = 2.1f;
            DynamicMock mock1 = CreateMockEffect(0, 0.1f);
            IEffect t0e1 = (IEffect)mock1.MockInstance;
            executer.Register(0, t0e1);
            executer.Run();
            Assert.Greater(Time.StepTime, 2.1f);
        }

    }
}
