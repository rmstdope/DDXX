using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System.CodeDom.Compiler;
using System.IO;
using Microsoft.CSharp;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Sound;
using Dope.DDXX.Utility;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private ITextureBuilder textureBuilder;
        private ICue sound;
        private List<ITrack> tracks;
        private int numTracksRegistered;
        private IDemoEffectTypes effectTypes;
        private IRenderTarget2D renderTarget2;
        private IRenderTarget2D renderTarget3;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            Time.Initialize();

            sound = mockery.NewMock<ICue>();

            soundDriver = mockery.NewMock<ISoundDriver>();
            inputDriver = mockery.NewMock<IInputDriver>();
            effectTypes = mockery.NewMock<IDemoEffectTypes>();
            executer = new DemoExecuter(this, soundDriver, inputDriver, postProcessor, effectTypes);
            tweaker = mockery.NewMock<IDemoTweaker>();
            executer.Tweaker = tweaker;
            textureBuilder = mockery.NewMock<ITextureBuilder>();
            renderTarget2 = mockery.NewMock<IRenderTarget2D>();
            renderTarget3 = mockery.NewMock<IRenderTarget2D>();

            effectChangeListener = mockery.NewMock<IEffectChangeListener>();

            Stub.On(inputDriver).Method("KeyPressedNoRepeat").With(Keys.Space).Will(Return.Value(false));
            Stub.On(graphicsFactory).Method("CreateSpriteBatch").Will(Return.Value(spriteBatch));
            Stub.On(textureFactory).Method("CreateFullsizeRenderTarget").Will(Return.Value(renderTarget));

            numTracksRegistered = 0;
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
        public void TestTransitionsRegisterOk()
        {
            RegisterTransition("name1", 1, 0, 5);
            RegisterTransition("name2", 0, 5, 10);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTransitionsRegisterOverlap1()
        {
            RegisterTransition("name1", 1, 0, 5);
            RegisterTransition("name2", 0, 4, 10);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTransitionsRegisterOverlap2()
        {
            RegisterTransition("name1", 0, 4, 10);
            RegisterTransition("name2", 1, 0, 5);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestTransitionsNotUnique()
        {
            RegisterTransition("name1", 1, 0, 5);
            RegisterTransition("name1", 0, 5, 10);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestEffectsNotUnique()
        {
            EnsureNumTracks(2);
            Expect.Once.On(tracks[0]).Method("IsEffectRegistered").
                Will(Return.Value(false));
            Expect.Once.On(tracks[1]).Method("IsEffectRegistered").
                Will(Return.Value(true));
            IDemoEffect effect = CreateMockEffect("name1", 5, 10);
            executer.Register(0, effect);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestPostEffectsNotUnique()
        {
            EnsureNumTracks(2);
            Expect.Once.On(tracks[0]).Method("IsPostEffectRegistered").
                Will(Return.Value(false));
            Expect.Once.On(tracks[1]).Method("IsPostEffectRegistered").
                Will(Return.Value(true));
            IDemoPostEffect effect = CreateMockPostEffect("name1", 5, 10);
            executer.Register(0, effect);
        }

        [Test]
        public void TestTracks()
        {
            Assert.AreEqual(0, executer.NumTracks);

            EnsureNumTracks(1);
            Assert.AreEqual(1, executer.NumTracks);

            EnsureNumTracks(2);
            Assert.AreEqual(2, executer.NumTracks);
        }

        [Test]
        public void TestStep()
        {
            EnsureNumTracks(2);

            Expect.Once.On(inputDriver).
                Method("Step");
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

            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder);
        }

        [Test]
        public void TestInitializeOKNoSong2()
        {
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();

            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder);
        }

        [Test]
        public void TestInitializeOKSong()
        {
            executer.SetSong("Song");
            FileUtility.SetLoadPaths(new string[] { "./" });
            ExpectSoundInitialize();
            Expect.Once.On(soundDriver).
                Method("PlaySound").
                With("Song").
                Will(Return.Value(sound));
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();

            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder);
        }

        [Test]
        public void TestInitializeOKEffects()
        {
            EnsureNumTracks(50);

            for (int i = 0; i < 50; i++)
                Stub.On(tracks[i]).GetProperty("StartTime").Will(Return.Value(0.0f));

            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();

            for (int i = 0; i < 50; i++)
                Expect.Once.On(tracks[i]).
                    Method("Initialize").With(graphicsFactory, device, textureFactory, effectFactory,
                    textureBuilder, executer, postProcessor);

            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder);
        }

        [Test]
        public void TestInitializeOKTransitions()
        {
            IDemoTransition t1 = RegisterTransition("name1", 0, 1, 2);
            IDemoTransition t2 = RegisterTransition("name2", 49, 2, 3);

            for (int i = 0; i < 50; i++)
                Stub.On(tracks[i]).GetProperty("StartTime").Will(Return.Value(0.0f));

            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();

            Expect.Once.On(t1).
                Method("Initialize").With(postProcessor);
            Expect.Once.On(t2).
                Method("Initialize").With(postProcessor);

            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder);
        }

        [Test]
        public void TestRegister()
        {
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1.0f));
            Stub.On(tracks[1]).GetProperty("EndTime").Will(Return.Value(3.0f));
            Stub.On(tracks[2]).GetProperty("EndTime").Will(Return.Value(2.0f));
            Stub.On(tracks[0]).GetProperty("StartTime").Will(Return.Value(0.3f));
            Stub.On(tracks[1]).GetProperty("StartTime").Will(Return.Value(0.1f));
            Stub.On(tracks[2]).GetProperty("StartTime").Will(Return.Value(0.2f));
            Assert.AreEqual(0.0f, executer.StartTime);
            Assert.AreEqual(0.0f, executer.EndTime);

            EnsureNumTracks(1);
            Assert.AreEqual(0.3f, executer.StartTime);
            Assert.AreEqual(1.0f, executer.EndTime);

            EnsureNumTracks(2);
            Assert.AreEqual(0.1f, executer.StartTime);
            Assert.AreEqual(3.0f, executer.EndTime);

            EnsureNumTracks(3);
            Assert.AreEqual(0.1f, executer.StartTime);
            Assert.AreEqual(3.0f, executer.EndTime);
        }

        [Test]
        public void TestRenderNoEffect()
        {
            TestInitializeOKSong();

            Expect.Once.On(inputDriver).Method("KeyPressedNoRepeat").With(Keys.F12).Will(Return.Value(false));

            Expect.Once.On(tweaker).
                Method("Draw");
            Expect.Once.On(device).
                Method("Present");

            executer.Render();
        }

        private void ExpectCopyToBackBuffer(IRenderTarget2D sourceTexture)
        {
            Expect.Once.On(device).Method("SetRenderTarget").With(0, null);
            Expect.Once.On(spriteBatch).Method("Begin").
                With(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            Expect.Once.On(spriteBatch).Method("Draw").
                With(texture, new Rectangle(0, 0, presentParameters.BackBufferWidth, presentParameters.BackBufferHeight), Color.White);
            Expect.Once.On(spriteBatch).Method("End");
        }

        [Test]
        public void TestDraw1Track()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(inputDriver).Method("KeyPressedNoRepeat").With(Keys.F12).Will(Return.Value(false));

            EnsureNumTracks(1);
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1000.0f));

            executer.ClearColor = Color.DarkSlateBlue;

            Expect.Once.On(tracks[0]).Method("Render").
                With(device, renderTarget, Color.DarkSlateBlue).Will(Return.Value(renderTarget2));
            Expect.Once.On(renderTarget2).Method("GetTexture").Will(Return.Value(texture));
            ExpectCopyToBackBuffer(renderTarget2);
            Expect.Once.On(tweaker).
                Method("Draw");
            Expect.Once.On(device).
                Method("Present");

            executer.Render();
        }

        [Test]
        public void TestDraw2TracksNoTransition()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(inputDriver).Method("KeyPressedNoRepeat").With(Keys.F12).Will(Return.Value(false));

            EnsureNumTracks(2);
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1000.0f));
            Stub.On(tracks[1]).GetProperty("EndTime").Will(Return.Value(1000.0f));

            executer.ClearColor = Color.DarkSlateBlue;

            Expect.Once.On(tracks[0]).Method("Render").
                With(device, renderTarget, Color.DarkSlateBlue).Will(Return.Value(renderTarget2));
            Expect.Once.On(renderTarget2).Method("GetTexture").Will(Return.Value(texture));
            ExpectCopyToBackBuffer(renderTarget2);
            Expect.Once.On(tweaker).
                Method("Draw");
            Expect.Once.On(device).
                Method("Present");

            executer.Render();
        }

        [Test]
        public void TestRender2TracksTransition()
        {
            ITexture newTexture = mockery.NewMock<ITexture>();
            TestInitializeOKNoSong1();

            Expect.Once.On(inputDriver).Method("KeyPressedNoRepeat").With(Keys.F12).Will(Return.Value(false));

            EnsureNumTracks(2);
            IDemoTransition transition = RegisterTransition("name1", 1, 0, 10);
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1000.0f));
            Stub.On(tracks[1]).GetProperty("EndTime").Will(Return.Value(1000.0f));

            executer.ClearColor = Color.DarkSlateBlue;

            Expect.Once.On(tracks[0]).Method("Render").
                With(device, renderTarget, Color.DarkSlateBlue).Will(Return.Value(renderTarget2));
            List<IRenderTarget2D> targets = new List<IRenderTarget2D>();
            targets.Add(renderTarget3);
            Expect.Once.On(postProcessor).Method("GetTemporaryTextures").
                With(1, false).Will(Return.Value(targets));
            Expect.Once.On(postProcessor).Method("AllocateTexture").With(renderTarget2);
            Expect.Once.On(tracks[1]).Method("Render").
                With(device, renderTarget3, Color.DarkSlateBlue).Will(Return.Value(renderTarget));
            //Expect.Once.On(postProcessor).Method("AllocateTexture").With(backBuffer1);
            Expect.Once.On(transition).Method("Render").With(renderTarget2, renderTarget).
                Will(Return.Value(renderTarget3));
            Expect.Once.On(postProcessor).Method("FreeTexture").With(renderTarget2);
            //Expect.Once.On(postProcessor).Method("FreeTexture").With(backBuffer2);
            Expect.Once.On(renderTarget3).Method("GetTexture").Will(Return.Value(texture));
            ExpectCopyToBackBuffer(renderTarget3);
            Expect.Once.On(tweaker).
                Method("Draw");
            Expect.Once.On(device).
                Method("Present");

            executer.Render();
        }

        [Test]
        public void TestRender2TracksTransitionNotOnTime()
        {
            TestInitializeOKNoSong1();

            Expect.Once.On(inputDriver).Method("KeyPressedNoRepeat").With(Keys.F12).Will(Return.Value(false));

            EnsureNumTracks(2);
            IDemoTransition transition = RegisterTransition("name1", 1, 0, 10);
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(1000.0f));
            Stub.On(tracks[1]).GetProperty("EndTime").Will(Return.Value(1000.0f));
            Time.CurrentTime = 14;

            executer.ClearColor = Color.DarkSlateBlue;

            Expect.Once.On(tracks[1]).Method("Render").
                With(device, renderTarget, Color.DarkSlateBlue).Will(Return.Value(renderTarget2));
            Expect.Once.On(renderTarget2).Method("GetTexture").Will(Return.Value(texture));
            ExpectCopyToBackBuffer(renderTarget2);
            Expect.Once.On(tweaker).
                Method("Draw");
            Expect.Once.On(device).
                Method("Present");

            executer.Render();
        }

        private void ExpectStepCalled(int[] trackNum)
        {
            foreach (int track in trackNum)
            {
                Expect.Once.On(tracks[track]).
                    Method("Step");
            }
            Expect.Once.On(tweaker).
                Method("HandleInput").Will(Return.Value(true));
        }

        [Test]
        public void TestInitializeFromFile()
        {
            FooEffect fooEffect = new FooEffect("", 0, 0);
            BarEffect barEffect = new BarEffect("", 0, 0);
            FooGlow fooGlow = new FooGlow("", 0, 0);
            FooTransition fooTransition = new FooTransition("", 0, 0);
            List<object> param = new List<object>();

            string twoEffectContents =
@"<Demo>
<Effect class=""FooEffect"" name=""FooName"" track=""1"" endTime=""6.5"">
<Parameter name=""FooParam"" int=""3"" />
<Parameter name=""BarParam"" float=""4.3"" />
<Parameter name=""StrParam"" string=""foostr"" />
</Effect>
<Effect class=""BarEffect"" name=""BarName"" startTime=""2.5"" endTime=""5.2"">
<Parameter name=""Goo"" string=""string value"" />
<Parameter name=""VecParam"" Vector3=""5.4, 4.3, 3.2"" />
<Parameter name=""ColParam"" Color=""101, 102, 103, 250"" />
<Parameter name=""ColParamNamed"" Color=""1, 2, 3, 4"" />
<SetupCall name=""Setup"">
<Parameter string=""MethodCalled"" />
</SetupCall>
</Effect>
<PostEffect class=""FooGlow"" name=""GlowName"" track=""2"" startTime=""3.4"" endTime=""4.5"">
<Parameter name=""GlowParam"" float=""5.4"" />
</PostEffect>
<Transition class=""FooTransition"" name=""TransitionName"" destinationTrack=""1"" startTime=""4"" endTime=""5"">
<Parameter name=""TransParameter"" float=""3.4"" />
</Transition>
</Demo>
";
            StubTrackForRegisteredEffects();
            ExpectSoundInitialize();
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();

            DemoXMLReaderTest.TempFiles tempFiles = new DemoXMLReaderTest.TempFiles();
            FileUtility.SetLoadPaths(new string[] { "" });
            Expect.Once.On(effectTypes).Method("CreateInstance").With("FooEffect", "FooName", 0.0f, 6.5f).Will(Return.Value(fooEffect));
            Expect.Once.On(effectTypes).Method("SetProperty").With(fooEffect, "FooParam", 3);
            Expect.Once.On(effectTypes).Method("SetProperty").With(fooEffect, "BarParam", 4.3f);
            Expect.Once.On(effectTypes).Method("SetProperty").With(fooEffect, "StrParam", "foostr");
            Expect.Once.On(effectTypes).Method("CreateInstance").With("BarEffect", "BarName", 2.5f, 5.2f).Will(Return.Value(barEffect));
            Expect.Once.On(effectTypes).Method("SetProperty").With(barEffect, "Goo", "string value");
            Expect.Once.On(effectTypes).Method("SetProperty").With(barEffect, "VecParam", new Vector3(5.4f, 4.3f, 3.2f));
            Expect.Once.On(effectTypes).Method("SetProperty").With(barEffect, "ColParam", new Color(101, 102, 103, 250));
            Expect.Once.On(effectTypes).Method("SetProperty").With(barEffect, "ColParamNamed", new Color(1, 2, 3, 4));
            param.Add("MethodCalled");
            Expect.Once.On(effectTypes).Method("CallSetup").With(barEffect, "Setup", param);
            Expect.Once.On(effectTypes).Method("CreateInstance").With("FooGlow", "GlowName", 3.4f, 4.5f).Will(Return.Value(fooGlow));
            Expect.Once.On(effectTypes).Method("SetProperty").With(fooGlow, "GlowParam", 5.4f);
            Expect.Once.On(effectTypes).Method("CreateInstance").With("FooTransition", "TransitionName", 4.0f, 5.0f).Will(Return.Value(fooTransition));
            Expect.Once.On(effectTypes).Method("SetProperty").With(fooTransition, "TransParameter", 3.4f);

            for (int i = 0; i < 3; i++)
                Stub.On(tracks[i]).GetProperty("StartTime").Will(Return.Value(0.0f));
            Expect.Once.On(tracks[0]).Method("Register").With(barEffect);
            Expect.Once.On(tracks[1]).Method("Register").With(fooEffect);
            Expect.Once.On(tracks[2]).Method("Register").With(fooGlow);
            Expect.Once.On(tracks[0]).Method("Initialize").With(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, executer, postProcessor);
            Expect.Once.On(tracks[1]).Method("Initialize").With(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, executer, postProcessor);
            Expect.Once.On(tracks[2]).Method("Initialize").With(graphicsFactory, device, textureFactory, effectFactory, textureBuilder, executer, postProcessor);
            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder, tempFiles.New(twoEffectContents));
            Assert.AreEqual(3, executer.NumTracks);
        }

        [Test]
        public void TestInitializeSongFromFile()
        {
            string songXml =
@"<Demo song=""SongX""></Demo>";
            FileUtility.SetLoadPaths(new string[] { "./" });
            ExpectSoundInitialize();
            Expect.Once.On(soundDriver).
                Method("PlaySound").
                With("SongX").
                Will(Return.Value(sound));
            ExpectPostProcessorInitialize();
            ExpectTweakerInitialize();

            DemoXMLReaderTest.TempFiles tempFiles = new DemoXMLReaderTest.TempFiles();
            FileUtility.SetLoadPaths(new string[] { "" });
            executer.Initialize(device, graphicsFactory, textureFactory, effectFactory, textureBuilder, tempFiles.New(songXml));
            Assert.AreEqual(0, executer.NumTracks);
        }

        [Test]
        public void TestXMLUpdate()
        {
            TestInitializeFromFile();

            for (int i = 0; i < 3; i++)
                Expect.Once.On(tracks[i]).Method("UpdateListener").With(effectChangeListener);
            executer.Update(effectChangeListener);

        }

        //[Test]
        //public void TestResumeSong()
        //{
        //    TestInitializeOKSong();

        //    Expect.Once.On(soundDriver).Method("SetPosition");
        //    Expect.Once.On(soundDriver).Method("ResumeChannel");
        //    Time.Pause();
        //    executer.TogglePause();
        //    Assert.IsFalse(Time.IsPaused());
        //}

        [Test]
        public void TestResumeNoSong()
        {
            TestInitializeOKNoSong1();

            Time.Pause();
            executer.TogglePause();
            Assert.IsFalse(Time.IsPaused());
        }

        //[Test]
        //public void TestPauseSong()
        //{
        //    TestInitializeOKSong();

        //    Expect.Once.On(soundDriver).Method("PauseChannel");
        //    Time.Resume();
        //    executer.TogglePause();
        //    Assert.IsTrue(Time.IsPaused());
        //}

        [Test]
        public void TestPauseNoSong()
        {
            TestInitializeOKNoSong1();

            Time.Resume();
            executer.TogglePause();
            Assert.IsTrue(Time.IsPaused());
        }

        //[Test]
        //public void TestJumpSong()
        //{
        //    EnsureNumTracks(1);
        //    Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
        //    Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(100.0f));
        //    TestInitializeOKSong();

        //    Time.CurrentTime = 1.0f;
        //    Time.Pause();
        //    float t = Time.CurrentTime;
        //    Expect.Once.On(soundDriver).Method("SetPosition");
        //    executer.JumpInTime(5.0f);
        //    Assert.AreEqual(6.0f, Time.CurrentTime, 0.00001f);
        //}

        [Test]
        public void TestJumpNoSong()
        {
            EnsureNumTracks(1);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("StartTime").Will(Return.Value(0.0f));
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
            EnsureNumTracks(1);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("StartTime").Will(Return.Value(0.0f));
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(100.0f));
            TestInitializeOKNoSong1();

            Time.CurrentTime = 5.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            //Expect.Once.On(soundDriver).Method("SetPosition");
            executer.JumpInTime(-10.0f);
            Assert.AreEqual(0.0f, Time.CurrentTime, 0.00001f);
        }

        [Test]
        public void TestJumpPastEnd()
        {
            EnsureNumTracks(1);
            Expect.Once.On(tracks[0]).Method("Initialize").WithAnyArguments();
            Stub.On(tracks[0]).GetProperty("StartTime").Will(Return.Value(0.0f));
            Stub.On(tracks[0]).GetProperty("EndTime").Will(Return.Value(10.0f));
            TestInitializeOKNoSong1();

            Time.CurrentTime = 5.0f;
            Time.Pause();
            float t = Time.CurrentTime;
            //Expect.Once.On(soundDriver).Method("SetPosition");
            executer.JumpInTime(10.0f);
            Assert.AreEqual(10.0f, Time.CurrentTime, 0.00001f);
        }

        [Test]
        public void TestAddGeneratorNoParameter()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();

            Expect.Once.On(effectTypes).Method("CreateGenerator").With("className").Will(Return.Value(generator1));
            executer.AddGenerator("name", "className");
        }

        [Test]
        public void TestAddGeneratorWithParameter()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();

            Expect.Once.On(effectTypes).Method("CreateGenerator").With("className2").Will(Return.Value(generator1));
            executer.AddGenerator("gen1", "className2");
            Expect.Once.On(effectTypes).Method("SetProperty").With(generator1, "paramName", 10.0f);
            executer.AddFloatParameter("paramName", 10, -1);
        }

        [Test]
        public void TestAddTwoGeneratorWithParameters()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();
            TestGenerator generator2 = new TestGenerator();

            Expect.Once.On(effectTypes).Method("CreateGenerator").With("className3").Will(Return.Value(generator1));
            Expect.Once.On(effectTypes).Method("SetProperty").With(generator1, "paramName2", "hejsan");
            Expect.Once.On(effectTypes).Method("CreateGenerator").With("className4").Will(Return.Value(generator2));
            Expect.Once.On(effectTypes).Method("SetProperty").With(generator2, "paramName3", Color.Red);
            executer.AddGenerator("gen1", "className3");
            executer.AddStringParameter("paramName2", "hejsan");
            executer.AddGenerator("gen2", "className4");
            executer.AddColorParameter("paramName3", Color.Red);
        }

        [Test]
        public void TestAddTexture()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();
            ITexture2D texture = mockery.NewMock<ITexture2D>();

            Expect.Once.On(effectTypes).Method("CreateGenerator").With("className").Will(Return.Value(generator1));
            executer.AddGenerator("genName", "className");
            Expect.Once.On(textureBuilder).Method("Generate").
                With(generator1, 1, 2, 3, SurfaceFormat.Color).Will(Return.Value(texture));
            Expect.Once.On(textureFactory).Method("RegisterTexture").
                With("texName", texture);
            executer.AddTexture("texName", "genName", 1, 2, 3);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAddTextureNoGenerator()
        {
            TestInitializeOKSong();
            executer.AddTexture("texName", "genName", 0, 0, 0);
        }

        [Test]
        public void TestAddGeneratorOneInput()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();

            Expect.Once.On(effectTypes).Method("CreateGenerator").
                With("className2").Will(Return.Value(generator1));
            executer.AddGenerator("gen1", "className2");
            executer.AddGeneratorInput(0, "gen1");
            Assert.AreSame(generator1, generator1.Inputs[0]);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInputWithoutGenerator()
        {
            TestInitializeOKSong();
            executer.AddGeneratorInput(0, "gen1");
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInputWithUnknownGenerator()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();
            Expect.Once.On(effectTypes).Method("CreateGenerator").
                With("className1").Will(Return.Value(generator1));
            executer.AddGenerator("gen1", "className1");
            executer.AddGeneratorInput(0, "nogen");
        }

        [Test]
        public void TestAddGeneratorAnotherInput()
        {
            TestInitializeOKSong();
            TestGenerator generator1 = new TestGenerator();
            TestGenerator generator2 = new TestGenerator();
            Expect.Once.On(effectTypes).Method("CreateGenerator").
                With("className1").Will(Return.Value(generator1));
            executer.AddGenerator("gen1", "className1");

            Expect.Once.On(effectTypes).Method("CreateGenerator").
                With("className2").Will(Return.Value(generator2));
            executer.AddGenerator("gen2", "className2");
            
            executer.AddGeneratorInput(1, "gen1");
            Assert.AreSame(generator1, generator2.Inputs[1]);
        }

        private void StubTrackForRegisteredEffects()
        {
            foreach (ITrack track in tracks)
            {
                Stub.On(track).Method("IsEffectRegistered").Will(Return.Value(false));
                Stub.On(track).Method("IsPostEffectRegistered").Will(Return.Value(false));
            }
        }

        private void EnsureNumTracks(int numTracks)
        {
            for (int i = 0; i < numTracksRegistered; i++)
            {
                Stub.On(tracks[i]).Method("IsEffectRegistered").Will(Return.Value(false));
                Stub.On(tracks[i]).Method("IsPostEffectRegistered").Will(Return.Value(false));
            }
            IDemoEffect effect = CreateMockEffect("name", 0, 0);
            Expect.Once.On(tracks[numTracks - 1]).Method("Register").With(effect);
            executer.Register(numTracks - 1, effect);
            numTracksRegistered = numTracks;
        }

        private IDemoTransition RegisterTransition(string name, int destinationTrack, float startTime, float endTime)
        {
            IDemoTransition transition = CreateMockTransition(name, destinationTrack, startTime, endTime);
            executer.Register(transition);
            return transition;
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
                 Method("Initialize").With("Test");
        }

        private void ExpectPostProcessorInitialize()
        {
            Expect.Once.On(postProcessor).
                Method("Initialize").
                With(device, graphicsFactory, textureFactory, effectFactory);
        }

        private void ExpectTweakerInitialize()
        {
            Expect.Once.On(tweaker).
                Method("Initialize");
        }


        #region IDemoFactory Members

        public ITrack CreateTrack()
        {
            return tracks[numTracksRegistered++];
        }

        #endregion
    }

    public class FooEffect : BaseDemoEffect
    {
        public FooEffect(string name, float start, float end) : base(name, start, end) { }
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
        public BarEffect(string name, float start, float end) : base(name, start, end) { setupParam = "NotCalled"; }
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
        public FooGlow(string name, float start, float end) : base(name, start, end) { }
        private float glowParam;
        public float GlowParam
        {
            get { return glowParam; }
            set { glowParam = value; }
        }
        public override void Render() { }
        protected override void Initialize() { }
    }
    public class FooTransition : BaseDemoTransition
    {
        public FooTransition(string name, float start, float end) : base(name, start, end) { }
        public override IRenderTarget2D Combine(IRenderTarget2D fromTexture, IRenderTarget2D toTexture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    public class TestGenerator : IGenerator
    {
        public IGenerator[] Inputs = new IGenerator[100];

        public Vector4  GetPixel(Vector2 textureCoordinate)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public void  ConnectToInput(int inputPin, IGenerator outputGenerator)
        {
            Inputs[inputPin] = outputGenerator;
        }
    }
}
