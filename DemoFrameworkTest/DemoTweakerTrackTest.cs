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

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerTrackTest : DemoMockTest
    {
        private DemoTweakerTrack tweaker;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private Track track;
        private TweakerSettings settings = new TweakerSettings();

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userInterface = mockery.NewMock<IUserInterface>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            Stub.On(registrator).GetProperty("StartTime").Will(Return.Value(0.5f));

            tweaker = new DemoTweakerTrack(settings);
            tweaker.UserInterface = userInterface;

            Time.Initialize();

            track = new Track();
            tweaker.IdentifierFromParent(track);
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

        private void InitializeEffects(int numEffects, int numPostEffects)
        {
            for (int i = 0; i < numEffects; i++)
                track.Register(CreateMockEffect(0.0f, 10.0f));
            for (int i = 0; i < numPostEffects; i++)
                track.Register(CreateMockPostEffect(0.0f, 10.0f));
            Expect.Once.On(userInterface).
                Method("Initialize");
            tweaker.Initialize(registrator);
        }

        [Test]
        public void TestLeaveForChild()
        {
            InitializeEffects(2, 2);
            Assert.AreSame(track.Effects[0], tweaker.IdentifierToChild());
            tweaker.KeyDown();
            Assert.AreSame(track.Effects[1], tweaker.IdentifierToChild());
            tweaker.KeyDown();
            Assert.AreSame(track.PostEffects[0], tweaker.IdentifierToChild());
            tweaker.KeyDown();
            Assert.AreSame(track.PostEffects[1], tweaker.IdentifierToChild());
        }

        [Test]
        public void TestKeyDown()
        {
            InitializeEffects(2, 1);
            Assert.AreEqual(0, tweaker.CurrentEffect);
            tweaker.KeyDown();
            Assert.AreEqual(1, tweaker.CurrentEffect);
            tweaker.KeyDown();
            Assert.AreEqual(2, tweaker.CurrentEffect);
            tweaker.KeyDown();
            Assert.AreEqual(2, tweaker.CurrentEffect);
        }

        [Test]
        public void TestKeyUp()
        {
            TestKeyDown();
            Assert.AreEqual(2, tweaker.CurrentEffect);
            tweaker.KeyUp();
            Assert.AreEqual(1, tweaker.CurrentEffect);
            tweaker.KeyUp();
            Assert.AreEqual(0, tweaker.CurrentEffect);
            tweaker.KeyUp();
            Assert.AreEqual(0, tweaker.CurrentEffect);
        }

        [Test]
        public void TestTrackChange()
        {
            // If we change track, effect number should be reset
            InitializeEffects(1, 1);
            tweaker.KeyDown();
            Assert.AreEqual(1, tweaker.CurrentEffect);
            tweaker.CurrentTrack = track;
            Assert.AreEqual(1, tweaker.CurrentEffect);
            tweaker.CurrentTrack = new Track();
            Assert.AreEqual(0, tweaker.CurrentEffect);
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            ExpectDraw("TestDraw");
            tweaker.Draw();
        }

        [Test]
        public void TestDraw13Effects()
        {
            track.Register(CreateMockEffect(-10.0f, -5.0f));
            track.Register(CreateMockPostEffect(100.0f, 110.0f));
            InitializeEffects(5, 6);

            ExpectDraw("TestDraw13Effects1");
            tweaker.Draw();

            for (int i = 0; i < 12; i++)
                tweaker.KeyDown();
            ExpectDraw("TestDraw13Effects2");
            tweaker.Draw();
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
                    case "TestDraw13Effects1":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        //     12 effect 0 (selected)
                        //       0 "<--MockObject"
                        //     ...
                        //     24 effect 12
                        //       0 "MockObject-->"
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(12 + 13, mainBox.Children[1].Children[0].Children.Count);
                        Assert.AreEqual("<--MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Text);
                        Assert.AreEqual(settings.SelectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        for (int i = 0; i < 11; i++)
                        {
                            Assert.AreEqual("MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[13 + i].Children[0]).Text);
                            Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[13 + i]).Color);
                        }
                        Assert.AreEqual("MockObject-->", ((TextControl)mainBox.Children[1].Children[0].Children[24].Children[0]).Text);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[24]).Color);
                        break;
                    case "TestDraw13Effects2":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 timelineWindow
                        //     0..11 multiple controls (12)
                        //     12 effect 0
                        //       0 "<--MockObject"
                        //     ...
                        //     24 effect 12 (selected)
                        //       0 "MockObject-->"
                        Assert.AreEqual(1, mainBox.Children[1].Children.Count);
                        Assert.AreEqual(12 + 13, mainBox.Children[1].Children[0].Children.Count);
                        Assert.AreEqual("<--MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Text);
                        Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        for (int i = 0; i < 11; i++)
                        {
                            Assert.AreEqual("MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[13 + i].Children[0]).Text);
                            Assert.AreEqual(settings.UnselectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[13 + i]).Color);
                        }
                        Assert.AreEqual("MockObject-->", ((TextControl)mainBox.Children[1].Children[0].Children[24].Children[0]).Text);
                        Assert.AreEqual(settings.SelectedColor, ((BoxControl)mainBox.Children[1].Children[0].Children[24]).Color);
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
