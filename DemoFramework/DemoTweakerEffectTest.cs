using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using System.IO;
using System.Drawing;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerEffectTest : D3DMockTest
    {
        private class TestClass : TweakableContainer
        {
            int intType;
            public int IntType
            {
                get { return intType; }
                set { intType = value; }
            }

            float floatType;
            public float FloatType
            {
                get { return floatType; }
                set { floatType = value; }
            }

            Vector3 vector3Type;
            public Vector3 Vector3Type
            {
                get { return vector3Type; }
                set { vector3Type = value; }
            }

            //string stringType;
            //public string StringType
            //{
            //    get { return stringType; }
            //    set { stringType = value; }
            //}

            Color colorType;
            public Color ColorType
            {
                get { return colorType; }
                set { colorType = value; }
            }
        }

        private DemoTweakerEffect tweaker;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private TestClass tester;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userInterface = mockery.NewMock<IUserInterface>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            Stub.On(registrator).GetProperty("StartTime").Will(Return.Value(0.5f));

            tester = new TestClass();
            tweaker = new DemoTweakerEffect();
            tweaker.IdentifierFromParent(tester);
            tweaker.UserInterface = userInterface;

            Time.Initialize();
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
        public void TestKeyDown()
        {
            Assert.AreEqual(0, tweaker.CurrentVariable);
            tweaker.KeyDown();
            Assert.AreEqual(1, tweaker.CurrentVariable);
            tweaker.KeyDown();
            Assert.AreEqual(2, tweaker.CurrentVariable);
            tweaker.KeyDown();
            Assert.AreEqual(3, tweaker.CurrentVariable);
            tweaker.KeyDown();
            Assert.AreEqual(3, tweaker.CurrentVariable);
        }

        [Test]
        public void TestKeyUp()
        {
            TestKeyDown();
            Assert.AreEqual(3, tweaker.CurrentVariable);
            tweaker.KeyUp();
            Assert.AreEqual(2, tweaker.CurrentVariable);
            tweaker.KeyUp();
            Assert.AreEqual(1, tweaker.CurrentVariable);
            tweaker.KeyUp();
            Assert.AreEqual(0, tweaker.CurrentVariable);
            tweaker.KeyUp();
            Assert.AreEqual(0, tweaker.CurrentVariable);
        }

        [Test]
        public void TestContainerChange()
        {
            // If we change container, effect number should be reset
            tweaker.KeyDown();
            Assert.AreEqual(1, tweaker.CurrentVariable);
            tweaker.CurrentContainer = tester;
            Assert.AreEqual(1, tweaker.CurrentVariable);
            tweaker.CurrentContainer = new TestClass();
            Assert.AreEqual(0, tweaker.CurrentVariable);
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();
            tweaker.CurrentContainer = new TweakableContainer();

            ExpectDraw("TestDraw");
            tweaker.Draw();
        }

        [Test]
        public void TestDraw5Variables()
        {
            TestInitialize();
            ExpectDraw("TestDraw5Variables1");
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
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        Assert.AreEqual(0, mainBox.Children[1].Children.Count);
                        break;
                    case "TestDraw5Variables1":
                        // mainWindow
                        // 0 titleWindow
                        // 1 tweakableWindow
                        //   0 Selection box
                        //   1 Float name
                        //   2 Float value
                        //   3 Integer name
                        //   4 Integer value
                        //   5 Vector3 name
                        //   6 Vector3 value 1
                        //   7 Vector3 value 2
                        //   8 Vector3 value 3
                        //   9 Color name
                        //  10 Color value 1
                        //  11 Color value 2
                        //  12 Color value 3
                        //  13 Color value 4
                        //  14 Color value 5
                        //  15 Color value 6
                        Assert.AreEqual(16, mainBox.Children[1].Children.Count);
                        //Assert.AreEqual(12 + 13, mainBox.Children[1].Children[0].Children.Count);
                        //Assert.AreEqual("<--MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[12].Children[0]).Text);
                        //Assert.AreEqual(Color.Crimson, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        //for (int i = 0; i < 11; i++)
                        //{
                        //    Assert.AreEqual("MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[13 + i].Children[0]).Text);
                        //    Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[0].Children[13 + i]).Color);
                        //}
                        //Assert.AreEqual("MockObject-->", ((TextControl)mainBox.Children[1].Children[0].Children[24].Children[0]).Text);
                        //Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[0].Children[24]).Color);
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
                        Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[0].Children[12]).Color);
                        for (int i = 0; i < 11; i++)
                        {
                            Assert.AreEqual("MockObject", ((TextControl)mainBox.Children[1].Children[0].Children[13 + i].Children[0]).Text);
                            Assert.AreEqual(Color.DarkBlue, ((BoxControl)mainBox.Children[1].Children[0].Children[13 + i]).Color);
                        }
                        Assert.AreEqual("MockObject-->", ((TextControl)mainBox.Children[1].Children[0].Children[24].Children[0]).Text);
                        Assert.AreEqual(Color.Crimson, ((BoxControl)mainBox.Children[1].Children[0].Children[24]).Color);
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
