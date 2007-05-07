using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.CodeDom.Compiler;
using System.IO;
using System.Drawing;
using Microsoft.CSharp;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.Direct3D;
using FMOD;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{

    [TestFixture]
    public class DemoExecuterTest : DemoMockTest, IDemoFactory
    {
        private DemoExecuter executer;
        private IDemoTweaker tweaker;
        private ISoundDriver soundDriver;
        private IInputDriver inputDriver;
        private IEffectChangeListener effectChangeListener;
        private FMOD.Sound sound;
        private FMOD.Channel channel;
        private List<ITrack> tracks;
        private int trackNum;

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

            soundDriver = mockery.NewMock<ISoundDriver>();
            inputDriver = mockery.NewMock<IInputDriver>();
            executer = new DemoExecuter(this, soundDriver, inputDriver, postProcessor);
            tweaker = mockery.NewMock<IDemoTweaker>();
            executer.Tweaker = tweaker;

            effectChangeListener = mockery.NewMock<IEffectChangeListener>();

            Stub.On(inputDriver).Method("KeyPressedNoRepeat").With(Key.Space).Will(Return.Value(false));

            trackNum = 0;
            tracks = new List<ITrack>();
            for (int i = 0; i < 100; i++)
                tracks.Add(mockery.NewMock<ITrack>());
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestTracks()
        {
            Assert.AreEqual(0, executer.NumTracks);

            RegisterEffect(0, 0, 10);
            Assert.AreEqual(1, executer.NumTracks);

            RegisterEffect(1, 5, 15);
            Assert.AreEqual(2, executer.NumTracks);
        }

        [Test]
        public void TestStep()
        {
            RegisterEffect(0, 0, 10);
            RegisterEffect(1, 3, 12);

            Expect.Once.On(tracks[0]).
                Method("Step");
            Expect.Once.On(tracks[1]).
                Method("Step");
            Expect.Once.On(tweaker).
                Method("HandleInput").
                Will(Return.Value(true));
            Time.CurrentTime = 5;
            Time.SetDeltaTimeForTest(1.0f);
            executer.Step();
            Time.UnSetDeltaTimeForTest();
            Assert.Greater(Time.StepTime, 5.0f);
        }

        [Test]
        public void TestInitializeOKNoSong1()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            executer.Initialize(device, graphicsFactory, textureFactory, "");
        }

        [Test]
        public void TestInitializeOKNoSong2()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            executer.Initialize(device, graphicsFactory, textureFactory, null);
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

            executer.Initialize(device, graphicsFactory, textureFactory, "Dope.DDXX.DemoFramework.dll");
        }

        [Test]
        public void TestInitializeOKEffects()
        {
            RegisterEffect(0, 1, 2);
            RegisterEffect(49, 1, 2);
            RegisterPostEffect(1, 1, 2);

            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();
            ExpectGraphicsInitialize();

            for (int i = 0; i < 50; i++)
                Expect.Once.On(tracks[i]).
                    Method("Initialize").With(graphicsFactory, device, postProcessor);

            executer.Initialize(device, graphicsFactory, textureFactory, null);
        }

        [Test]
        public void TestRegister()
        {
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1.0f));
            Stub.On(tracks[1]).GetProperty("EndTime").Will(Return.Value(3.0f));
            Stub.On(tracks[2]).GetProperty("EndTime").Will(Return.Value(2.0f));
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(0.0f, executer.EndTime);

            RegisterEffect(0, 1, 2);
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(1.0f, executer.EndTime);

            RegisterEffect(1, 1, 2);
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(3.0f, executer.EndTime);

            RegisterEffect(2, 1, 2);
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(3.0f, executer.EndTime);
        }

        [Test]
        public void TestRunNoEffect()
        {
            TestInitializeOKSong();

            Expect.Once.On(tweaker).
                Method("HandleInput").Will(Return.Value(true));
            Expect.Once.On(soundDriver).
                Method("PlaySound").Will(Return.Value(channel));
            Expect.Once.On(soundDriver).
                Method("SetPosition").With(Is.EqualTo(channel), Is.Anything);
            Expect.Exactly(2).On(device).
                Method("GetRenderTarget").With(0).Will(Return.Value(surface));
            Expect.Exactly(2).On(device).
                Method("SetRenderTarget").With(0, surface);
            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            Expect.Once.On(postProcessor).
                Method("StartFrame").With(texture);
            Expect.Once.On(postProcessor).
                GetProperty("OutputTexture").Will(Return.Value(texture));
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");
            Time.CurrentTime = 2.0f;
            Time.SetDeltaTimeForTest(1.0f);

            Expect.Once.On(tweaker).
                Method("Draw");
            Stub.On(tweaker).
                GetProperty("Quit").Will(Return.Value(false));
            executer.Run();
            Assert.Greater(Time.StepTime, 2.0f);
            Time.UnSetDeltaTimeForTest();
        }

        [Test]
        public void TestRun1Track()
        {
            TestInitializeOKNoSong1();

            RegisterEffect(0, 0, 0);
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1000.0f));

            Expect.Once.On(device).
                Method("Clear").
                With(ClearFlags.Target | ClearFlags.ZBuffer, System.Drawing.Color.Black, 1.0f, 0);
            Expect.Once.On(postProcessor).
                Method("StartFrame").With(texture);
            Expect.Exactly(2).On(device).
                Method("GetRenderTarget").With(0).Will(Return.Value(surface));
            Expect.Exactly(2).On(device).
                Method("SetRenderTarget").With(0, surface);
            Expect.Once.On(postProcessor).
                GetProperty("OutputTexture").Will(Return.Value(texture));
            Expect.Once.On(device).
                Method("StretchRectangle");
            Expect.Once.On(device).
                Method("Present");
            Expect.Once.On(tracks[0]).
                Method("Step");
            Expect.Once.On(tweaker).
                Method("HandleInput").Will(Return.Value(true));
            Expect.Once.On(tracks[0]).
                Method("Render").With(device);
            Expect.Once.On(tweaker).
                Method("Draw");

            LimitRunLoop(1);

            executer.Run();
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
            Expect.Once.On(tracks[0]).Method("Register").WithAnyArguments();
            Expect.Once.On(tracks[0]).Method("Initialize").With(graphicsFactory, device, postProcessor);
            Expect.Once.On(tracks[1]).Method("Register").WithAnyArguments();
            Expect.Once.On(tracks[1]).Method("Initialize").With(graphicsFactory, device, postProcessor);
            Expect.Once.On(tracks[2]).Method("Register").WithAnyArguments();
            Expect.Once.On(tracks[2]).Method("Initialize").With(graphicsFactory, device, postProcessor);
            executer.Initialize(device, graphicsFactory, textureFactory, "", tempFiles.New(twoEffectContents));
            Assert.AreEqual(3, executer.NumTracks);
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
            for (int i = 0; i < 3; i++)
            {
                Expect.Once.On(tracks[i]).Method("Register").WithAnyArguments();
                Expect.Once.On(tracks[i]).Method("Initialize").With(graphicsFactory, device, postProcessor);
            }
            executer.Initialize(device, graphicsFactory, textureFactory, "", tempFiles.New(twoEffectContents));

            for (int i = 0; i < 3; i++)
                Expect.Once.On(tracks[i]).Method("UpdateListener").With(effectChangeListener);
            executer.Update(effectChangeListener);

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
            RegisterEffect(0, 0, 0);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(100.0f));
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
            RegisterEffect(0, 0, 0);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(100.0f));
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
            RegisterEffect(0, 0, 0);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(100.0f));
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
            RegisterEffect(0, 0, 0);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(10.0f));
            TestInitializeOKSong();

            Time.CurrentTime = 5.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            Expect.Once.On(soundDriver).Method("SetPosition");
            executer.JumpInTime(10.0f);
            Assert.AreEqual(10.0f, Time.CurrentTime, 0.00001f);
        }

        private IDemoEffect RegisterEffect(int track, float startTime, float endTime)
        {
            IDemoEffect effect = CreateMockEffect(startTime, endTime);
            Expect.Once.On(tracks[track]).Method("Register").With(effect);
            executer.Register(track, effect);
            return effect;
        }

        private IDemoPostEffect RegisterPostEffect(int track, float startTime, float endTime)
        {
            IDemoPostEffect effect = CreateMockPostEffect(startTime, endTime);
            Expect.Once.On(tracks[track]).Method("Register").With(effect);
            executer.Register(track, effect);
            return effect;
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
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(texture));
        }


        #region IDemoFactory Members

        public ITrack CreateTrack()
        {
            return tracks[trackNum++];
        }

        #endregion
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

        protected override void Initialize()
        {
        }
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
        protected override void Initialize() { }
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
