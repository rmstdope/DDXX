using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using FMOD;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Dope.DDXX.Utility;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.DemoFramework
{

    [TestFixture]
    public class DemoExecuterTest : TrackTest
    {
        DemoExecuter executer;
        private Dope.DDXX.Input.IInputFactory iFactory;
        private Dope.DDXX.Sound.ISoundFactory sFactory;
        private ISoundSystem system;

        private void ExpectPostProcessorInitialize()
        {
            IEffect effect = mockery.NewMock<IEffect>();
            Expect.Once.On(factory).
                Method("EffectFromFile").
                With(Is.EqualTo(device), Is.EqualTo("../../../Effects/PostEffects.fxo"), Is.Null, Is.EqualTo(""), Is.EqualTo(ShaderFlags.None), Is.Anything).
                Will(Return.Value(effect));
            Stub.On(effect).
                Method("GetTechnique").
                Will(Return.Value(EffectHandle.FromString("")));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "SourceTexture").
                Will(Return.Value(EffectHandle.FromString("")));
        }

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Time.Initialize();

            iFactory = mockery.NewMock<Dope.DDXX.Input.IInputFactory>();
            InputDriver.Factory = iFactory;

            sFactory = mockery.NewMock<Dope.DDXX.Sound.ISoundFactory>();
            system = mockery.NewMock<ISoundSystem>();
            Stub.On(sFactory).
                Method("CreateSystem").
                Will(Return.Value(system));
            SoundDriver.Factory = sFactory;

            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = DeviceType.Hardware;
            desc.width = displayMode.Width;
            desc.height = displayMode.Height;
            desc.colorFormat = displayMode.Format;
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
            IDemoEffect t0e1 = CreateMockEffect(0, 10);
            IDemoPostEffect t1e1 = CreateMockPostEffect(5, 15);

            Assert.AreEqual(0, executer.NumTracks);
            executer.Register(0, t0e1);
            Assert.AreEqual(1, executer.NumTracks);
            executer.Register(1, t1e1);
            Assert.AreEqual(2, executer.NumTracks);
        }

        [Test]
        public void TestStep()
        {
            IDemoEffect t0e1 = CreateMockEffect(0, 10);
            IDemoEffect t0e2 = CreateMockEffect(10, 15);
            IDemoEffect t1e1 = CreateMockEffect(3, 12);
            IDemoEffect t1e2 = CreateMockEffect(1, 2);

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
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail1()
        {
            Expect.Once.On(system).
                Method("GetVersion").
                Will(Return.Value(FMOD.RESULT.ERR_VERSION));
            executer.Initialize("");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
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
            executer.Initialize("test.mp3");
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
            ExpectPostProcessorInitialize();

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
            ExpectPostProcessorInitialize();

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
            ExpectPostProcessorInitialize();

            executer.Initialize("test.mp3");
        }

        [Test]
        public void TestInitializeOKEffects()
        {
            IDemoEffect effect1 = CreateMockEffect(1, 2);
            executer.Register(0, effect1);
            IDemoEffect effect2 = CreateMockEffect(1, 2);
            executer.Register(100, effect2);
            IDemoPostEffect postEffect = CreateMockPostEffect(1, 2);
            executer.Register(1, postEffect);

            // SoundDriver
            Expect.Once.On(system).
                Method("GetVersion").
                Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                Will(Return.Value(FMOD.RESULT.OK));

            ExpectPostProcessorInitialize();

            // Effects
            Expect.Once.On(effect1).
                Method("Initialize");
            Expect.Once.On(effect2).
                Method("Initialize");
            Expect.Once.On(postEffect).
                Method("Initialize").
                With(Is.NotNull);

            executer.Initialize(null);
        }

        [Test]
        public void TestRegister()
        {
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(0.0f, executer.EndTime);

            IDemoEffect t0e1 = CreateMockEffect(1, 2);
            executer.Register(0, t0e1);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(2.0f, executer.EndTime);

            IDemoEffect t0e2 = CreateMockEffect(5, 100);
            executer.Register(0, t0e2);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(100.0f, executer.EndTime);

            IDemoPostEffect t0e3 = CreateMockPostEffect(99, 101);
            executer.Register(0, t0e3);

            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(101.0f, executer.EndTime);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestRunSongFail()
        {
            TestInitializeOKSong();

            Expect.Once.On(system).
                Method("PlaySound").
                Will(Return.Value(FMOD.RESULT.ERR_VERSION));

            executer.Run();
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
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Blue, 1.0f, 0);
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
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Blue, 1.0f, 0);
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
            IDemoEffect effect = CreateMockEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Step");
            Expect.Once.On(effect).
                Method("Render");
            executer.Register(0, effect);
            executer.Run();
        }

        [Test]
        public void TestRenderOneTrack1()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Blue, 1.0f, 0);
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
            IDemoEffect effect = CreateMockEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Render");
            executer.Register(0, effect);
            executer.Render();
        }

        [Test]
        public void TestRenderOneTrack2()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Blue, 1.0f, 0);
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
            IDemoEffect effect = CreateMockEffect(0, 10.0f);
            IDemoPostEffect postEffect = CreateMockPostEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Render");
            Expect.Once.On(postEffect).
                Method("Render");
            executer.Register(0, effect);
            executer.Register(0, postEffect);
            executer.Render();
        }

    }
}
