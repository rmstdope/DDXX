using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Direct3D;
using FMOD;
using Input;
using Sound;
using Utility;
using NUnit.Framework;
using NMock2;

namespace DemoFramework
{

    [TestFixture]
    public class DemoExecuterTest : TrackTest
    {
        DemoExecuter executer;
        private Input.IFactory iFactory;
        private Sound.IFactory sFactory;
        private ISystem system;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Time.Initialize();

            iFactory = mockery.NewMock<Input.IFactory>();
            InputDriver.Factory = iFactory;

            sFactory = mockery.NewMock<Sound.IFactory>();
            system = mockery.NewMock<ISystem>();
            Stub.On(sFactory).
                Method("CreateSystem").
                Will(Return.Value(system));
            SoundDriver.Factory = sFactory;

            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            D3DDriver.GetInstance().Initialize(null, desc);

            executer = new DemoExecuter();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestTracks()
        {
            IEffect t0e1 = CreateMockEffect(0, 10);
            IEffect t1e1 = CreateMockEffect(5, 15);

            Assert.AreEqual(0, executer.NumTracks);
            executer.Register(0, t0e1);
            Assert.AreEqual(1, executer.NumTracks);
            executer.Register(1, t1e1);
            Assert.AreEqual(2, executer.NumTracks);
        }

        [Test]
        public void TestStep()
        {
            IEffect t0e1 = CreateMockEffect(0, 10);
            IEffect t0e2 = CreateMockEffect(10, 15);
            IEffect t1e1 = CreateMockEffect(3, 12);
            IEffect t1e2 = CreateMockEffect(1, 2);

            executer.Register(0, t0e1);
            executer.Register(0, t0e2);
            executer.Register(1, t1e1);
            executer.Register(1, t1e2);


            Time.Initialize();
            Time.Pause();

            Expect.Once.On(t0e1).
                Method("Step");
            Expect.Once.On(t1e1).
                Method("Step");
            Time.CurrentTime = 5;
            executer.Step();

            Expect.Once.On(t0e1).
                Method("Step");
            Expect.Once.On(t1e2).
                Method("Step");
            Time.CurrentTime = 1;
            executer.Step();

            Time.CurrentTime = 20;
            executer.Step();

            Time.Resume();
        }

        [Test]
        public void TestInitializeFail1()
        {
            Expect.Once.On(system).
                Method("GetVersion").
                Will(Return.Value(FMOD.RESULT.ERR_VERSION));
            try
            {
                executer.Initialize("");
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitializeFail2()
        {
            Expect.Once.On(system).
                 Method("GetVersion").
                 Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("CreateSound").
                Will(Return.Value(FMOD.RESULT.ERR_VERSION));
            try
            {
                executer.Initialize("test.mp3");
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestInitializeOKNoSong1()
        {
            Expect.Once.On(system).
                Method("GetVersion").
                Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                Will(Return.Value(FMOD.RESULT.OK));
            executer.Initialize("");
        }

        [Test]
        public void TestInitializeOKNoSong2()
        {
            Expect.Once.On(system).
                Method("GetVersion").
                Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                Will(Return.Value(FMOD.RESULT.OK));
            executer.Initialize(null);
        }

        [Test]
        public void TestInitializeOKSong()
        {
            Expect.Once.On(system).
                 Method("GetVersion").
                 Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("CreateSound").
                Will(Return.Value(FMOD.RESULT.OK));
            executer.Initialize("test.mp3");
        }

        [Test]
        public void TestRegister()
        {
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(0.0f, executer.EndTime);

            IEffect t0e1 = CreateMockEffect(1, 2);
            executer.Register(0, t0e1);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(2.0f, executer.EndTime);

            IEffect t10e1 = CreateMockEffect(5, 100);
            executer.Register(0, t10e1);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(100.0f, executer.EndTime);
        }

        [Test]
        public void TestRunSongFail()
        {
            TestInitializeOKSong();

            Expect.Once.On(system).
                Method("PlaySound").
                Will(Return.Value(FMOD.RESULT.ERR_VERSION));

            try
            {
                executer.Run();
                Assert.Fail();
            }
            catch (DDXXException) { }
        }

        [Test]
        public void TestRunNoEffect()
        {
            TestInitializeOKSong();

            Stub.On(iFactory).
                Method("KeyPressed").
                WithAnyArguments().
                Will(Return.Value(false));
            Expect.Once.On(system).
                Method("PlaySound").
                Will(Return.Value(FMOD.RESULT.OK));
            Expect.Exactly(2).On(device).
                Method("GetRenderTarget").
                With(0).
                Will(Return.Value(surface));
            Expect.Exactly(2).On(device).
                Method("SetRenderTarget").
                With(0, surface);
            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");
            Time.CurrentTime = 2.0f;
            executer.Run();
            Assert.Greater(Time.StepTime, 2.0f);
        }

        [Test]
        public void TestRun1Effect()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(iFactory).
                Method("KeyPressed").
                WithAnyArguments().
                Will(Return.Value(false));
            Expect.Once.On(iFactory).
                Method("KeyPressed").
                WithAnyArguments().
                Will(Return.Value(true));
            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            Expect.Once.On(device).
                Method("BeginScene");
            Expect.Exactly(2).On(device).
                Method("GetRenderTarget").
                With(0).
                Will(Return.Value(surface));
            Expect.Exactly(2).On(device).
                Method("SetRenderTarget").
                With(0, surface);
            Expect.Once.On(device).
                Method("EndScene");
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");
            IEffect effect = CreateMockEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Step");
            Expect.Once.On(effect).
                Method("Render");
            executer.Register(0, effect);
            executer.Run();
        }

        [Test]
        public void TestRenderOneTrack()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target, System.Drawing.Color.Blue, 1.0f, 0);
            Expect.Once.On(device).
                Method("BeginScene");
            Expect.Exactly(2).On(device).
                Method("GetRenderTarget").
                With(0).
                Will(Return.Value(surface));
            Expect.Exactly(2).On(device).
                Method("SetRenderTarget").
                With(0, surface);
            Expect.Once.On(device).
                Method("EndScene");
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");

            Time.CurrentTime = 5.0f;
            IEffect effect = CreateMockEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Render");
            executer.Register(0, effect);
            executer.Render();
        }

    }
}
