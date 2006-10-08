using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using System.IO;

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
                        break;
                    case "TestDrawEffects":
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
                return true;
            }
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
        public void TestDrawEffects()
        {
            TestInitialize();
            tweaker.Enabled = true;
            Stub.On(registrator).
                GetProperty("Tracks").
                Will(Return.Value(new List<Track>()));
            ExpectDraw("TestDrawEffects");
            tweaker.Draw();
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
