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
using System.Reflection;
using System.Reflection.Emit;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{

    [TestFixture]
    public class DemoExecuterTest : TrackTest
    {
        DemoExecuter executer;
        private Dope.DDXX.Input.IInputFactory iFactory;
        private Dope.DDXX.Sound.ISoundFactory sFactory;
        private ISoundSystem system;
        private IDemoTweaker tweaker;
        private IPostProcessor postProcessor;

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
            desc.width = presentParameters.BackBufferWidth;
            desc.height = presentParameters.BackBufferHeight;
            desc.colorFormat = presentParameters.BackBufferFormat;
            Expect.Once.On(prerequisits).Method("CheckPrerequisits").WithAnyArguments();
            D3DDriver.GetInstance().Initialize(null, desc, prerequisits);

            postProcessor = mockery.NewMock<IPostProcessor>();
            executer = new DemoExecuter(postProcessor);
            tweaker = mockery.NewMock<IDemoTweaker>();
            executer.Tweaker = tweaker;

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
            Expect.Once.On(tweaker).
                Method("HandleInput");
            Time.CurrentTime = 5;
            executer.Step();

            Expect.Once.On(t0e1).
                Method("Step");
            Expect.Once.On(t1e2).
                Method("Step");
            Expect.Once.On(tweaker).
                Method("HandleInput");
            Time.CurrentTime = 1;
            executer.Step();

            Time.CurrentTime = 20;
            Expect.Once.On(tweaker).
                Method("HandleInput");
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
            ExpectGraphicsInitialize();
            executer.Initialize("");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail2()
        {
            FileUtility.SetLoadPaths(new string[] { "./" });
            ExpectSoundInitialize();
            Expect.Once.On(system).
                Method("CreateSound").
                Will(Return.Value(FMOD.RESULT.ERR_VERSION));
            ExpectGraphicsInitialize();

            executer.Initialize("Dope.DDXX.DemoFramework.dll");
        }

        [Test]
        public void TestInitializeOKNoSong1()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            executer.Initialize("");
        }

        [Test]
        public void TestInitializeOKNoSong2()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            executer.Initialize(null);
        }

        [Test]
        public void TestInitializeOKSong()
        {
            FileUtility.SetLoadPaths(new string[] { "./" });
            ExpectSoundInitialize();
            Expect.Once.On(system).
                Method("CreateSound").
                Will(Return.Value(FMOD.RESULT.OK));
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            executer.Initialize("Dope.DDXX.DemoFramework.dll");
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

            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

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

            Expect.Once.On(tweaker).
                Method("HandleInput");
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
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            Expect.Once.On(postProcessor).
                Method("StartFrame").
                With(texture);
            Expect.Once.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(texture));
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");
            Time.CurrentTime = 2.0f;

            Expect.Once.On(tweaker).
                Method("Draw");
            Stub.On(tweaker).
                GetProperty("Quit").
                Will(Return.Value(false));
            executer.Run();
            Assert.Greater(Time.StepTime, 2.0f);
        }

        [Test]
        public void TestRun1Effect()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            Expect.Once.On(postProcessor).
                Method("StartFrame").
                With(texture);
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
            Expect.Once.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(texture));
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");
            IDemoEffect effect = CreateMockEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Step");
            Expect.Once.On(tweaker).
                Method("HandleInput");
            Expect.Once.On(effect).
                Method("Render");
            Expect.Once.On(tweaker).
                Method("Draw");

            LimitRunLoop(1);

            executer.Register(0, effect);
            executer.Run();
        }

        [Test]
        public void TestRenderOneTrack1()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            Expect.Once.On(postProcessor).
                Method("StartFrame").
                With(texture);
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
            Expect.Once.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(texture));
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");

            Time.CurrentTime = 5.0f;
            IDemoEffect effect = CreateMockEffect(0, 10.0f);
            Expect.Once.On(effect).
                Method("Render");
            Expect.Once.On(tweaker).
                Method("Draw");
            executer.Register(0, effect);
            executer.Render();
        }

        [Test]
        public void TestRenderOneTrack2()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            Expect.Once.On(postProcessor).
                Method("StartFrame").
                With(texture);
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
            Expect.Once.On(postProcessor).
                GetProperty("OutputTexture").
                Will(Return.Value(texture));
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
            Expect.Once.On(tweaker).
                Method("Draw");
            executer.Register(0, effect);
            executer.Register(0, postEffect);
            executer.Render();
        }

        [Test]
        public void TestInitializeFromFile()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();
            ExpectBaseDemoEffects(2);
            string twoEffectContents =
@"<Effects>
<Effect name=""FooEffect"" track=""1"">
<Parameter name=""FooParam"" int=""3"" />
<Parameter name=""BarParam"" float=""4.3"" />
<Parameter name=""StrParam"" string=""foostr"" />
</Effect>
<Effect name=""BarEffect"">
<Parameter name=""Goo"" string=""string value"" />
<Parameter name=""VecParam"" Vector3=""5.4, 4.3, 3.2"" />
<SetupCall name=""Setup"">
<Parameter string=""MethodCalled"" />
</SetupCall>
</Effect>
<PostEffect name=""FooGlow"" track=""2"">
<Parameter name=""GlowParam"" float=""5.4"" />
</PostEffect>
</Effects>
";
//<Transition name=""footrans"" destinationTrack=""1"">
//<Parameter name=""transparam"" string=""tranny"" />
//</Transition>
            DemoXMLReaderTest.TempFiles tempFiles = new DemoXMLReaderTest.TempFiles();
            FileUtility.SetLoadPaths(new string[] { "" });
            executer.Initialize("", tempFiles.New(twoEffectContents));
            Assert.AreEqual(3, executer.NumTracks);
            Assert.AreEqual(1, executer.Tracks[0].Effects.Length);
            Assert.AreEqual(0, executer.Tracks[0].PostEffects.Length);
            Assert.AreEqual(1, executer.Tracks[1].Effects.Length);
            Assert.AreEqual(0, executer.Tracks[1].PostEffects.Length);
            Assert.AreEqual(0, executer.Tracks[2].Effects.Length);
            Assert.AreEqual(1, executer.Tracks[2].PostEffects.Length);

            Assert.AreEqual(typeof(BarEffect), executer.Tracks[0].Effects[0].GetType());
            Assert.AreEqual(typeof(FooEffect), executer.Tracks[1].Effects[0].GetType());
            Assert.AreEqual(typeof(FooGlow), executer.Tracks[2].PostEffects[0].GetType());

            Assert.AreEqual("string value", ((BarEffect)executer.Tracks[0].Effects[0]).Goo);
            Assert.AreEqual(new Vector3(5.4f, 4.3f, 3.2f), ((BarEffect)executer.Tracks[0].Effects[0]).VecParam);
            Assert.AreEqual("MethodCalled", ((BarEffect)executer.Tracks[0].Effects[0]).SetupParam);

            Assert.AreEqual(3, ((FooEffect)executer.Tracks[1].Effects[0]).FooParam);
            Assert.AreEqual(4.3f, ((FooEffect)executer.Tracks[1].Effects[0]).BarParam);
            Assert.AreEqual("foostr", ((FooEffect)executer.Tracks[1].Effects[0]).StrParam);

            Assert.AreEqual(5.4f, ((FooGlow)executer.Tracks[2].PostEffects[0]).GlowParam);
        }

        private void LimitRunLoop(int numCalls)
        {
            if (numCalls > 0)
                Expect.Exactly(numCalls).On(tweaker).GetProperty("Quit").Will(Return.Value(false));
            Expect.Once.On(tweaker).GetProperty("Quit").Will(Return.Value(true));
        }

        private void ExpectSoundInitialize()
        {
            Expect.Once.On(system).
                 Method("GetVersion").
                 Will(Return.Value(FMOD.RESULT.OK));
            Expect.Once.On(system).
                Method("Init").
                Will(Return.Value(FMOD.RESULT.OK));
        }

        private void ExpectPostProcessorInitialize()
        {
            Expect.Once.On(postProcessor).
                Method("Initialize").
                With(device);
        }

        private void ExpectTweakerInitialize()
        {
            Expect.Once.On(tweaker).
                Method("Initialize");
        }

        private void ExpectGraphicsInitialize()
        {
            Expect.Once.On(factory).
                Method("CreateTexture").
                With(device, presentParameters.BackBufferWidth, presentParameters.BackBufferHeight, 1, Usage.RenderTarget, presentParameters.BackBufferFormat, Pool.Default).
                Will(Return.Value(texture));
        }

    }

    public class FooEffect : BaseDemoEffect
    {
        public FooEffect(float start, float end) : base(start, end) { }
        private int fooParam;
        private float barParam;
        private string strParam;
        public string StrParam
        {
            get { return strParam; }
            set { strParam = value; }
        }
        public float BarParam
        {
            get { return barParam; }
            set { barParam = value; }
        }
        public int FooParam
        {
            get { return fooParam; }
            set { fooParam = value; }
        }
        public override void Step() { }
        public override void Render() { }
    }
    public class BarEffect : BaseDemoEffect
    {
        public BarEffect(float start, float end) : base(start, end) { setupParam = "NotCalled"; }
        private string goo;
        private Vector3 vecParam;
        private string setupParam;
        public string SetupParam
        {
            get { return setupParam; }
        }
        public string Goo
        {
            get { return goo; }
            set { goo = value; }
        }
        public Vector3 VecParam
        {
            get { return vecParam; }
            set { vecParam = value; }
        }
        public void Setup(string param) { setupParam = param; }
        public override void Step() { }
        public override void Render() { }
    }
    public class FooGlow : BaseDemoPostEffect
    {
        public FooGlow(float start, float end) : base(start, end) { }
        private float glowParam;
        public float GlowParam
        {
            get { return glowParam; }
            set { glowParam = value; }
        }
        public override void Render() { }
    }
}
