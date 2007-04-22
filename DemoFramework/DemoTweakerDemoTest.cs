using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Drawing;
using Dope.DDXX.Utility;
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Input;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerDemoTest : DemoMockTest
    {
        private DemoTweakerDemo tweaker;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private List<Track> tracks;
        private IInputDriver inputDriver;
        private TweakerSettings settings = new TweakerSettings();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userInterface = mockery.NewMock<IUserInterface>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            inputDriver = mockery.NewMock<IInputDriver>();
            Stub.On(registrator).GetProperty("StartTime").Will(Return.Value(0.5f));

            tweaker = new DemoTweakerDemo(settings);
            tweaker.UserInterface = userInterface;

            Time.Initialize();

            tracks = new List<Track>();
            Stub.On(registrator).
                GetProperty("Tracks").
                Will(Return.Value(tracks));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            Expect.Once.On(userInterface).
                Method("Initialize");
            tweaker.Initialize(registrator);
        }

        [Test]
        public void TestTrack()
        {
            for (int i = 0; i < 5; i++)
                tracks.Add(new Track());
            TestInitialize();
            Assert.AreEqual(0, tweaker.CurrentTrack);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(1, tweaker.CurrentTrack);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(4, tweaker.CurrentTrack);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(4, tweaker.CurrentTrack);
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            ExpectDraw("TestDraw");
            tweaker.Draw();
        }

        [Test]
        public void TestDraw5Tracks()
        {
            for (int i = 0; i < 5; i++)
                tracks.Add(new Track());
            TestInitialize();


            ExpectDraw("TestDraw5Tracks1");
            tweaker.Draw();

            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectDraw("TestDraw5Tracks2");
            tweaker.Draw();

            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectDraw("TestDraw5Tracks3");
            tweaker.Draw();
        }

        [Test]
        public void TestDrawEffects()
        {
            Time.CurrentTime = 0.0f;
            tracks.Add(new Track());
            TestInitialize();

            tracks[0].Register(CreateMockEffect(0.0f, 1.0f));
            tracks[0].Register(CreateMockEffect(1.0f, 2.0f));
            tracks[0].Register(CreateMockEffect(2.0f, 3.0f));
            tracks[0].Register(CreateMockPostEffect(0.0f, 3.0f));
            tracks[0].Register(CreateMockPostEffect(0.0f, 4.0f));

            ExpectDraw("TestDrawEffects");
            tweaker.Draw();
        }

        [Test]
        public void TestInputReturn()
        {
            tracks.Add(new Track());
            TestInitialize();

            ExpectKey(Key.Unlabeled);
            Assert.IsFalse(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.UpArrow);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.DownArrow);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
        }

        private void ExpectKey(Key key)
        {
            Key[] keys = { Key.UpArrow, Key.DownArrow };
            foreach (Key currentKey in keys)
            {
                bool pressed = false;
                if (key == currentKey)
                {
                    pressed = true;
                }
                Expect.Once.On(inputDriver).
                    Method("KeyPressedNoRepeat").
                    With(currentKey).
                    Will(Return.Value(pressed));
            }
        }

        class ControlMatcher : Matcher
        {
            private string test;
            private TweakerSettings settings;
            public ControlMatcher(string test, TweakerSettings settings)
            {
                this.test = test;
                this.settings = settings;
            }

            public override void DescribeTo(TextWriter writer)
            {
                writer.WriteLine("Matcher");
            }

            public override bool Matches(object o)
            {
                if (!(o is BoxControl))
                    Assert.Fail();
                BoxControl mainBox = (BoxControl)o;
                switch (test)
                {
                    case "TestDraw":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(12, mainBox.Children[1].Children[0].Children.Count);
                        break;
                    case "TestDraw5Tracks1":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        //     12 track 0 (selected)
                        //       0 "Track 0"
                        //     13 track 1
                        //       0 "Track 1"
                        //     14 track 2
                        //       0 "Track 2"
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(15, mainBox.Children[1].Children[0].Children.Count);
                        Assert.AreEqual("Track 0", ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Alpha);
                        Assert.AreEqual(settings.SelectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Alpha);
                        Assert.AreEqual("Track 1", ((TextControl)mainBox.Children[1].Children[0].Children[13].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[13].Children[0]).Alpha);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[13]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[13]).Alpha);
                        Assert.AreEqual("Track 2", ((TextControl)mainBox.Children[1].Children[0].Children[14].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[14].Children[0]).Alpha);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[14]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[14]).Alpha);
                        break;
                    case "TestDraw5Tracks2":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        //     12 track 0
                        //       0 "Track 0"
                        //     13 track 1 (selected)
                        //       0 "Track 1"
                        //     14 track 2
                        //       0 "Track 2"
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(15, mainBox.Children[1].Children[0].Children.Count);
                        Assert.AreEqual("Track 0", ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Alpha);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Alpha);
                        Assert.AreEqual("Track 1", ((TextControl)mainBox.Children[1].Children[0].Children[13].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[13].Children[0]).Alpha);
                        Assert.AreEqual(settings.SelectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[13]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[13]).Alpha);
                        Assert.AreEqual("Track 2", ((TextControl)mainBox.Children[1].Children[0].Children[14].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[14].Children[0]).Alpha);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[14]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[14]).Alpha);
                        break;
                    case "TestDraw5Tracks3":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        //     12 track 2
                        //       0 "Track 2"
                        //     13 track 3
                        //       0 "Track 3"
                        //     14 track 4 (selected)
                        //       0 "Track 4"
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(15, mainBox.Children[1].Children[0].Children.Count);
                        Assert.AreEqual("Track 2", ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Alpha);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Alpha);
                        Assert.AreEqual("Track 3", ((TextControl)mainBox.Children[1].Children[0].Children[13].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[13].Children[0]).Alpha);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[13]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[13]).Alpha);
                        Assert.AreEqual("Track 4", ((TextControl)mainBox.Children[1].Children[0].Children[14].Children[0]).Text);
                        Assert.AreEqual(settings.TextAlpha, ((TextControl)mainBox.Children[1].Children[0].Children[14].Children[0]).Alpha);
                        Assert.AreEqual(settings.SelectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[14]).Color);
                        Assert.AreEqual(settings.Alpha, ((BoxControl)mainBox.Children[1].Children[0].Children[14]).Alpha);
                        break;
                    case "TestDrawEffects":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        //     12 track 0
                        //       0 "Track 0"
                        //       1 Effect 0 text
                        //       2 Effect 0 line
                        //       3 Effect 1 text
                        //       4 Effect 1 line
                        //       5 Effect 2 text
                        //       6 Effect 2 line
                        //       7 PostEffect 0 text
                        //       8 PostEffect 0 line
                        //       9 PostEffect 1 text
                        //       10 PostEffect 1 line
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(13, mainBox.Children[1].Children[0].Children.Count);
                        Assert.AreEqual(11, mainBox.Children[1].Children[0].Children[12].Children.Count);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
                return true;
            }
        }

        private void ExpectDraw(string name)
        {
            Expect.Once.On(device).
                Method("BeginScene");
            Expect.Once.On(userInterface).
                Method("DrawControl").
                With(new ControlMatcher(name, settings));
            Expect.Once.On(device).
                Method("EndScene");
        }

    }
}
