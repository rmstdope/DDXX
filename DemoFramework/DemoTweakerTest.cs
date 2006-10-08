using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.IO;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerTest : D3DMockTest
    {
        private DemoTweaker tweaker;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userInterface = mockery.NewMock<IUserInterface>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            Stub.On(registrator).GetProperty("StartTime").Will(Return.Value(0.5f));

            tweaker = new DemoTweaker();
            tweaker.UserInterface = userInterface;

            SetupD3DDriver();
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
            List<Track> tracks = new List<Track>();
            for (int i = 0; i < 5; i++)
                tracks.Add(new Track());
            TestInitialize();
            tweaker.Enabled = true;
            Stub.On(registrator).
                GetProperty("Tracks").
                Will(Return.Value(tracks));
            Assert.AreEqual(0, tweaker.CurrentTrack);
            tweaker.IncreaseTrack();
            Assert.AreEqual(1, tweaker.CurrentTrack);
            tweaker.IncreaseTrack();
            tweaker.IncreaseTrack();
            tweaker.IncreaseTrack();
            Assert.AreEqual(4, tweaker.CurrentTrack);
            tweaker.IncreaseTrack();
            Assert.AreEqual(4, tweaker.CurrentTrack);
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            Assert.IsFalse(tweaker.Enabled);
            tweaker.Draw();

            tweaker.Enabled = true;
            Stub.On(registrator).
                GetProperty("Tracks").
                Will(Return.Value(new List<Track>()));
            ExpectDraw("TestDraw");
            tweaker.Draw();

            tweaker.Enabled = false;
            Assert.IsFalse(tweaker.Enabled);
            tweaker.Draw();
        }

        [Test]
        public void TestDraw5Effects()
        {
            List<Track> tracks = new List<Track>();
            for (int i = 0; i < 5; i++)
                tracks.Add(new Track());
            TestInitialize();
            tweaker.Enabled = true;
            Stub.On(registrator).
                GetProperty("Tracks").
                Will(Return.Value(tracks));

            ExpectDraw("TestDraw5Effects1");
            tweaker.Draw();

            tweaker.IncreaseTrack();
            ExpectDraw("TestDraw5Effects2");
            tweaker.Draw();

            tweaker.IncreaseTrack();
            tweaker.IncreaseTrack();
            tweaker.IncreaseTrack();
            ExpectDraw("TestDraw5Effects3");
            tweaker.Draw();
        }

        class ControlMatcher : Matcher
        {
            private string test;
            public ControlMatcher(string test)
            {
                this.test = test;
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
                        Assert.AreEqual(0, mainBox.Children[1].Children.Count);
                        break;
                    case "TestDraw5Effects1":
                        Assert.AreEqual(3, mainBox.Children[1].Children.Count);
                        Assert.AreEqual("Track 0", ((TextControl)mainBox.Children[1].Children[0].Children[0]).Text);
                        Assert.AreEqual(Color.Crimson, ((BoxControl)mainBox.Children[1].Children[0]).Color);
                        Assert.AreEqual("Track 1", ((TextControl)mainBox.Children[1].Children[1].Children[0]).Text);
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[1]).Color);
                        Assert.AreEqual("Track 2", ((TextControl)mainBox.Children[1].Children[2].Children[0]).Text);
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[2]).Color);
                        break;
                    case "TestDraw5Effects2":
                        Assert.AreEqual(3, mainBox.Children[1].Children.Count);
                        Assert.AreEqual("Track 0", ((TextControl)mainBox.Children[1].Children[0].Children[0]).Text);
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[0]).Color);
                        Assert.AreEqual("Track 1", ((TextControl)mainBox.Children[1].Children[1].Children[0]).Text);
                        Assert.AreEqual(Color.Crimson, ((BoxControl)mainBox.Children[1].Children[1]).Color);
                        Assert.AreEqual("Track 2", ((TextControl)mainBox.Children[1].Children[2].Children[0]).Text);
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[2]).Color);
                        break;
                    case "TestDraw5Effects3":
                        Assert.AreEqual(3, mainBox.Children[1].Children.Count);
                        Assert.AreEqual("Track 2", ((TextControl)mainBox.Children[1].Children[0].Children[0]).Text);
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[0]).Color);
                        Assert.AreEqual("Track 3", ((TextControl)mainBox.Children[1].Children[1].Children[0]).Text);
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[1]).Color);
                        Assert.AreEqual("Track 4", ((TextControl)mainBox.Children[1].Children[2].Children[0]).Text);
                        Assert.AreEqual(Color.Crimson, ((BoxControl)mainBox.Children[1].Children[2]).Color);
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
                With(new ControlMatcher(name));
            Expect.Once.On(device).
                Method("EndScene");
        }

    }
}
