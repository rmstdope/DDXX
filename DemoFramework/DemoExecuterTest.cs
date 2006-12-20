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
using Microsoft.DirectX.DirectInput;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{

    [TestFixture]
    public class DemoExecuterTest : TrackTest
    {
        DemoExecuter executer;
        private IDemoTweaker tweaker;
        private ISoundDriver soundDriver;
        private IInputDriver inputDriver;
        private IPostProcessor postProcessor;
        private IEffectChangeListener effectChangeListener;
        private FMOD.Sound sound;
        private FMOD.Channel channel;

        private string twoEffectContents =
@"<Effects>
<Effect name=""FooEffect"" track=""1"" endTime=""6.5"">
<Parameter name=""FooParam"" int=""3"" />
<Parameter name=""BarParam"" float=""4.3"" />
<Parameter name=""StrParam"" string=""foostr"" />
</Effect>
<Effect name=""BarEffect"" startTime=""2.5"" endTime=""5.2"">
<Parameter name=""Goo"" string=""string value"" />
<Parameter name=""VecParam"" Vector3=""5.4, 4.3, 3.2"" />
<Parameter name=""ColParam"" Color=""250, 101, 102, 103"" />
<Parameter name=""ColParamNamed"" Color=""SlateBlue"" />
<SetupCall name=""Setup"">
<Parameter string=""MethodCalled"" />
</SetupCall>
</Effect>
<PostEffect name=""FooGlow"" track=""2"" startTime=""3.4"" endTime=""4.5"">
<Parameter name=""GlowParam"" float=""5.4"" />
</PostEffect>
</Effects>
";
        //<Transition name=""footrans"" destinationTrack=""1"">
        //<Parameter name=""transparam"" string=""tranny"" />
        //</Transition>

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Time.Initialize();

            sound = new FMOD.Sound();
            channel = new FMOD.Channel();

            DeviceDescription desc = new DeviceDescription();
            desc.deviceType = Microsoft.DirectX.Direct3D.DeviceType.Hardware;
            desc.width = presentParameters.BackBufferWidth;
            desc.height = presentParameters.BackBufferHeight;
            desc.colorFormat = presentParameters.BackBufferFormat;
            Expect.Once.On(prerequisits).Method("CheckPrerequisits").WithAnyArguments();
            D3DDriver.GetInstance().Initialize(null, desc, prerequisits);

            soundDriver = mockery.NewMock<ISoundDriver>();
            postProcessor = mockery.NewMock<IPostProcessor>();
            inputDriver = mockery.NewMock<IInputDriver>();
            executer = new DemoExecuter(soundDriver, inputDriver, postProcessor);
            tweaker = mockery.NewMock<IDemoTweaker>();
            executer.Tweaker = tweaker;

            effectChangeListener = mockery.NewMock<IEffectChangeListener>();

            Stub.On(inputDriver).Method("KeyPressedNoRepeat").With(Key.Space).Will(Return.Value(false));
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
                Method("HandleInput").
                Will(Return.Value(true));
            Time.CurrentTime = 5;
            executer.Step();

            Expect.Once.On(t0e1).
                Method("Step");
            Expect.Once.On(t1e2).
                Method("Step");
            Expect.Once.On(tweaker).
                Method("HandleInput").
                Will(Return.Value(true));
            Time.CurrentTime = 1;
            executer.Step();

            Time.CurrentTime = 20;
            Expect.Once.On(tweaker).
                Method("HandleInput").
            Will(Return.Value(true));
            executer.Step();

            Time.Resume();
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
            Expect.Once.On(soundDriver).
                Method("CreateSound").
                With("Dope.DDXX.DemoFramework.dll").
                Will(Return.Value(sound));
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
        public void TestRunNoEffect()
        {
            TestInitializeOKSong();

            Expect.Once.On(tweaker).
                Method("HandleInput").
                Will(Return.Value(true));
            Expect.Once.On(soundDriver).
                Method("PlaySound").
                Will(Return.Value(channel));
            Expect.Once.On(soundDriver).
                Method("SetPosition").
                With(Is.EqualTo(channel), Is.Anything);
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
                Method("HandleInput").
                Will(Return.Value(true));
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

        [Test]
        public void TestXMLUpdate()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            DemoXMLReaderTest.TempFiles tempFiles = new DemoXMLReaderTest.TempFiles();
            FileUtility.SetLoadPaths(new string[] { "" });
            executer.Initialize("", tempFiles.New(twoEffectContents));


            expectIntParam("FooEffect", "FooParam", 3);
            expectFloatParam("FooEffect", "BarParam", 4.3f);
            expectStringParam("FooEffect", "StrParam", "foostr");

            expectStringParam("BarEffect", "Goo", "string value");
            expectVector3Param("BarEffect", "VecParam", new Vector3(5.4f, 4.3f, 3.2f));
            expectColorParam("BarEffect", "ColParam", Color.FromArgb(250, 101, 102, 103));
            expectColorParam("BarEffect", "ColParamNamed", Color.SlateBlue);

            expectFloatParam("FooGlow", "GlowParam", 5.4f);

            expectStartEndTime("FooEffect", 0.0f, 6.5f);
            expectStartEndTime("BarEffect", 2.5f, 5.2f);
            expectStartEndTime("FooGlow", 3.4f, 4.5f);

            executer.Update(effectChangeListener);

        }

        private void expectStartEndTime(string effectName, float start, float end)
        {
            Expect.Once.On(effectChangeListener).
                Method("SetStartTime").With(effectName, start);
            Expect.Once.On(effectChangeListener).
                Method("SetEndTime").With(effectName, end);
        }

        private void expectStringParam(string effectName, string paramName, string value)
        {
            Expect.Once.On(effectChangeListener).
                Method("SetStringParam").With(effectName, paramName, value);
        }

        private void expectIntParam(string effectName, string paramName, int value)
        {
            Expect.Once.On(effectChangeListener).
                Method("SetIntParam").With(effectName, paramName, value);
        }

        private void expectVector3Param(string effectName, string paramName, Vector3 value)
        {
            Expect.Once.On(effectChangeListener).
                Method("SetVector3Param").With(effectName, paramName, value);
        }

        private void expectColorParam(string effectName, string paramName, Color value)
        {
            Expect.Once.On(effectChangeListener).
                Method("SetColorParam").With(effectName, paramName, value);
        }

        private void expectFloatParam(string effectName, string paramName, float value)
        {
            Expect.Once.On(effectChangeListener).
                Method("SetFloatParam").With(effectName, paramName, value);
        }

        [Test]
        public void TestResumeSong()
        {
            TestInitializeOKSong();

            Expect.Once.On(soundDriver).Method("SetPosition");
            Expect.Once.On(soundDriver).Method("ResumeChannel");
            Time.Pause();
            executer.TogglePause();
            Assert.IsFalse(Time.IsPaused());
        }

        [Test]
        public void TestResumeNoSong()
        {
            TestInitializeOKNoSong1();

            Time.Pause();
            executer.TogglePause();
            Assert.IsFalse(Time.IsPaused());
        }

        [Test]
        public void TestPauseSong()
        {
            TestInitializeOKSong();

            Expect.Once.On(soundDriver).Method("PauseChannel");
            Time.Resume();
            executer.TogglePause();
            Assert.IsTrue(Time.IsPaused());
        }

        [Test]
        public void TestPauseNoSong()
        {
            TestInitializeOKNoSong1();

            Time.Resume();
            executer.TogglePause();
            Assert.IsTrue(Time.IsPaused());
        }

        [Test]
        public void TestJumpSong()
        {
            IDemoEffect effect = CreateMockEffect(0, 10);
            Expect.Once.On(effect).Method("Initialize");
            executer.Register(0, effect);
            TestInitializeOKSong();

            Time.CurrentTime = 1.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            Expect.Once.On(soundDriver).Method("SetPosition");
            executer.JumpInTime(5.0f);
            Assert.AreEqual(6.0f, Time.CurrentTime, 0.00001f);
        }

        [Test]
        public void TestJumpNoSong()
        {
            IDemoEffect effect = CreateMockEffect(0, 10);
            Expect.Once.On(effect).Method("Initialize");
            executer.Register(0, effect);
            TestInitializeOKNoSong1();

            Time.CurrentTime = 1.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            executer.JumpInTime(5.0f);
            Assert.AreEqual(6.0f, Time.CurrentTime, 0.00001f);
        }

        [Test]
        public void TestJumpBeforeStart()
        {
            IDemoEffect effect = CreateMockEffect(0, 10);
            Expect.Once.On(effect).Method("Initialize");
            executer.Register(0, effect);
            TestInitializeOKSong();

            Time.CurrentTime = 5.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            Expect.Once.On(soundDriver).Method("SetPosition");
            executer.JumpInTime(-10.0f);
            Assert.AreEqual(0.0f, Time.CurrentTime, 0.00001f);
        }

        [Test]
        public void TestJumpPastEnd()
        {
            IDemoEffect effect = CreateMockEffect(0, 10);
            Expect.Once.On(effect).Method("Initialize");
            executer.Register(0, effect);
            TestInitializeOKSong();

            Time.CurrentTime = 5.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            Expect.Once.On(soundDriver).Method("SetPosition");
            executer.JumpInTime(10.0f);
            Assert.AreEqual(10.0f, Time.CurrentTime, 0.00001f);
        }

        private void LimitRunLoop(int numCalls)
        {
            if (numCalls > 0)
                Expect.Exactly(numCalls).On(tweaker).GetProperty("Quit").Will(Return.Value(false));
            Expect.Once.On(tweaker).GetProperty("Quit").Will(Return.Value(true));
        }

        private void ExpectSoundInitialize()
        {
            Expect.Once.On(soundDriver).
                 Method("Initialize");
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
        private Color colParam;
        private Color colParamNamed;
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
        public Color ColParam
        {
            get { return colParam; }
            set { colParam = value; }
        }
        public Color ColParamNamed
        {
            get { return colParamNamed; }
            set { colParamNamed = value; }
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
