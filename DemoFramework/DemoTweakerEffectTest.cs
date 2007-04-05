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
using Microsoft.DirectX.DirectInput;
using Dope.DDXX.Input;

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
        private IInputDriver inputDriver;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userInterface = mockery.NewMock<IUserInterface>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            inputDriver = mockery.NewMock<IInputDriver>();
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
            
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(1, tweaker.CurrentVariable);

            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(2, tweaker.CurrentVariable);
            
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(3, tweaker.CurrentVariable);
            
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(3, tweaker.CurrentVariable);
        }

        [Test]
        public void TestKeyUp()
        {
            TestKeyDown();
            Assert.AreEqual(3, tweaker.CurrentVariable);

            ExpectKey(Key.UpArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(2, tweaker.CurrentVariable);

            ExpectKey(Key.UpArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(1, tweaker.CurrentVariable);

            ExpectKey(Key.UpArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(0, tweaker.CurrentVariable);

            ExpectKey(Key.UpArrow);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(0, tweaker.CurrentVariable);
        }

        [Test]
        public void TestKeyPlus()
        {
            tester.SetStepSize(0, 5);
            tester.IntType = 10;
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(15, tester.IntType);

            // Goto next variable
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);

            tester.SetStepSize(1, 0.234f);
            tester.FloatType = 10;
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(10.234f, tester.FloatType);
        }

        [Test]
        public void TestKeyPgDn()
        {
            tester.SetStepSize(0, 6);
            tester.IntType = 10;
            ExpectKey(Key.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(4, tester.IntType);

            // Goto next variable
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);

            tester.SetStepSize(1, 0.2f);
            tester.FloatType = 10;
            ExpectKey(Key.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(9.8f, tester.FloatType);
        }

        [Test]
        public void TestVector()
        {
            // Tab doesn't affect int
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            tester.IntType = 10;
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(11, tester.IntType);

            // Goto vector
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);

            // Test x, y, z
            tester.Vector3Type = new Vector3(0, 0, 0);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(1, 0, 0), tester.Vector3Type);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(1, 1, 0), tester.Vector3Type);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(1, 1, 1), tester.Vector3Type);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(2, 1, 1), tester.Vector3Type);
        }

        [Test]
        public void TestColor()
        {
            // Goto Color
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);

            // Test r, g, b, a
            tester.ColorType = Color.FromArgb(0, 0, 0, 0);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(Color.FromArgb(0, 1, 0, 0), tester.ColorType);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(Color.FromArgb(0, 1, 1, 0), tester.ColorType);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(Color.FromArgb(0, 1, 1, 1), tester.ColorType);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(Color.FromArgb(1, 1, 1, 1), tester.ColorType);
            ExpectKey(Key.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(Color.FromArgb(1, 2, 1, 1), tester.ColorType);
        }

        [Test]
        public void TestInputReturn()
        {
            ExpectKey(Key.Unlabeled);
            Assert.IsFalse(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.UpArrow);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.DownArrow);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.PageUp);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.PageDown);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
            ExpectKey(Key.Tab);
            Assert.IsTrue(tweaker.HandleInput(inputDriver));
        }

        [Test]
        public void TestContainerChange()
        {
            // If we change container, effect number should be reset
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);
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

        [Test]
        public void TestInputInt()
        {
            tester.IntType = 0;

            // Set 98765
            Key[] keys = new Key[] { Key.NumPad9, Key.NumPad8, Key.NumPad7, Key.NumPad6, Key.NumPad5 };
            foreach (Key key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(98765, tester.IntType);

            // Set 43210
            keys = new Key[] { Key.NumPad4, Key.NumPad3, Key.NumPad2, Key.NumPad1, Key.NumPad0 };
            foreach (Key key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(43210, tester.IntType);

            // Set -43210
            keys = new Key[] { Key.NumPadMinus, Key.NumPad4, Key.NumPad3, Key.NumPad2, Key.NumPad1, Key.NumPad0 };
            foreach (Key key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(-43210, tester.IntType);
        }

        [Test]
        public void TestInvalidFormat()
        {
            tester.FloatType = 666;
            ExpectKey(Key.NumPadMinus);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.NumPadMinus);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(666, tester.FloatType);
        }

        [Test]
        public void TestInputFloat()
        {
            tester.FloatType = 0;
            ExpectKey(Key.DownArrow);
            tweaker.HandleInput(inputDriver);

            // Set .98765
            Key[] keys = new Key[] { Key.NumPadPeriod, Key.NumPad9, Key.NumPad8, Key.NumPad7, Key.NumPad6, Key.NumPad5 };
            foreach (Key key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(0.98765f, tester.FloatType);

            // Set 432.10
            keys = new Key[] { Key.NumPad4, Key.NumPad3, Key.NumPad2, Key.NumPadPeriod, Key.NumPad1, Key.NumPad0 };
            foreach (Key key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(432.10f, tester.FloatType);

            // Set -4321.0
            keys = new Key[] { Key.NumPadMinus, Key.NumPad4, Key.NumPad3, Key.NumPad2, Key.NumPad1, Key.NumPadPeriod, Key.NumPad0 };
            foreach (Key key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Key.NumPadEnter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(-4321, tester.FloatType);
        }

        private void ExpectKey(Key key)
        {
            Key[] noRepeatKeys = { Key.UpArrow, Key.DownArrow, Key.Tab, Key.NumPad0, Key.NumPad1, Key.NumPad2, Key.NumPad3, Key.NumPad4, Key.NumPad5, Key.NumPad6, Key.NumPad7, Key.NumPad8, Key.NumPad9, Key.NumPadPeriod, Key.NumPadMinus, Key.NumPadEnter };
            Key[] slowRepeatKeys = { Key.PageDown, Key.PageUp };
            for (int i = 0; i < slowRepeatKeys.Length; i++)
            {
                bool pressed = false;
                if (key == slowRepeatKeys[i])
                {
                    pressed = true;
                }
                Expect.Once.On(inputDriver).
                    Method("KeyPressedSlowRepeat").
                    With(slowRepeatKeys[i]).
                    Will(Return.Value(pressed));
            }
            for (int i = 0; i < noRepeatKeys.Length; i++)
            {
                bool pressed = false;
                if (key == noRepeatKeys[i])
                {
                    pressed = true;
                }
                Expect.Once.On(inputDriver).
                    Method("KeyPressedNoRepeat").
                    With(noRepeatKeys[i]).
                    Will(Return.Value(pressed));
                if (pressed && i >= 3)
                    return;
            }
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
