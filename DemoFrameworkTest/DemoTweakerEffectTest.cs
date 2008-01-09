using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerEffectTest : D3DMockTest
    {
        private class TestClass : TweakableContainer
        {
            public TestClass()
                : base("")
            {
            }

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

            Vector2 vector2Type;
            public Vector2 Vector2Type
            {
                get { return vector2Type; }
                set { vector2Type = value; }
            }

            Vector3 vector3Type;
            public Vector3 Vector3Type
            {
                get { return vector3Type; }
                set { vector3Type = value; }
            }

            Vector4 vector4Type;
            public Vector4 Vector4Type
            {
                get { return vector4Type; }
                set { vector4Type = value; }
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

            bool boolType;
            public bool BoolType
            {
                get { return boolType; }
                set { boolType = value; }
            }
        }

        private DemoTweakerEffect tweaker;
        private IUserInterface userInterface;
        private IDemoRegistrator registrator;
        private TestClass tester;
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

            tester = new TestClass();
            tweaker = new DemoTweakerEffect(settings);
            tweaker.IdentifierFromParent(tester);
            tweaker.UserInterface = userInterface;
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
            tweaker.Initialize(registrator, graphicsFactory, textureFactory);
        }

        [Test]
        public void TestKeyDown()
        {
            for (int i = 0; i < 7; i++)
            {
                Assert.AreEqual(i, tweaker.CurrentVariable);
                ExpectKey(Keys.Down);
                tweaker.HandleInput(inputDriver);
            }
            Assert.AreEqual(6, tweaker.CurrentVariable);
        }

        [Test]
        public void TestKeyUp()
        {
            TestKeyDown();

            for (int i = 6; i >= 0; i--)
            {
                Assert.AreEqual(i, tweaker.CurrentVariable);
                ExpectKey(Keys.Up);
                tweaker.HandleInput(inputDriver);
            }

            Assert.AreEqual(0, tweaker.CurrentVariable);
        }

        [Test]
        public void TestKeyPgUpInt()
        {
            tester.SetStepSize(0, 5);
            tester.IntType = 10;
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(15, tester.IntType);
        }

        [Test]
        public void TestKeyPgUpFloat()
        {
            // Goto float variable
            GotoVariable(1);

            tester.SetStepSize(1, 0.234f);
            tester.FloatType = 10;
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(10.234f, tester.FloatType);
        }

        [Test]
        public void TestKeyPgUpVector2()
        {
            // Goto Vector2 variable
            GotoVariable(2);

            tester.SetStepSize(1, 1);
            tester.Vector2Type = new Vector2(1, 2);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector2(2, 2), tester.Vector2Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector2(2, 3), tester.Vector2Type);
        }

        [Test]
        public void TestKeyPgUpVector3()
        {
            // Goto Vector3 variable
            GotoVariable(3);

            tester.SetStepSize(2, 1);
            tester.Vector3Type = new Vector3(1, 2, 3);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(2, 2, 3), tester.Vector3Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(2, 3, 3), tester.Vector3Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(2, 3, 4), tester.Vector3Type);
        }

        [Test]
        public void TestKeyPgUpVector4()
        {
            // Goto Vector4 variable
            GotoVariable(4);

            tester.SetStepSize(2, 1);
            tester.Vector4Type = new Vector4(1, 2, 3, 4);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector4(2, 2, 3, 4), tester.Vector4Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector4(2, 3, 3, 4), tester.Vector4Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector4(2, 3, 4, 4), tester.Vector4Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector4(2, 3, 4, 5), tester.Vector4Type);
        }

        [Test]
        public void TestKeyPgUpColor()
        {
            // Goto Color variable
            GotoVariable(5);

            tester.ColorType = new Color(10, 20, 30, 40);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(11, 20, 30, 40), tester.ColorType);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(11, 21, 30, 40), tester.ColorType);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(11, 21, 31, 40), tester.ColorType);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(11, 21, 31, 41), tester.ColorType);
        }

        [Test]
        public void TestKeyPgUpBool()
        {
            // Goto float variable
            GotoVariable(6);

            tester.BoolType = false;
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.IsTrue(tester.BoolType);
            ExpectKey(Keys.PageUp);
            tweaker.HandleInput(inputDriver);
            Assert.IsFalse(tester.BoolType);
        }

        [Test]
        public void TestKeyPgDownInt()
        {
            tester.SetStepSize(0, 5);
            tester.IntType = 10;
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(5, tester.IntType);
        }

        [Test]
        public void TestKeyPgDownFloat()
        {
            // Goto float variable
            GotoVariable(1);

            tester.SetStepSize(1, 0.2f);
            tester.FloatType = 10;
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(9.8f, tester.FloatType);
        }

        [Test]
        public void TestKeyPgDownVector3()
        {
            // Goto Vector3 variable
            GotoVariable(3);

            tester.SetStepSize(2, 1);
            tester.Vector3Type = new Vector3(1, 2, 3);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(0, 2, 3), tester.Vector3Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(0, 1, 3), tester.Vector3Type);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Vector3(0, 1, 2), tester.Vector3Type);
        }

        [Test]
        public void TestKeyPgDownColor()
        {
            // Goto Color variable
            GotoVariable(5);

            tester.ColorType = new Color(10, 20, 30, 40);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(9, 20, 30, 40), tester.ColorType);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(9, 19, 30, 40), tester.ColorType);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(9, 19, 29, 40), tester.ColorType);
            ExpectKey(Keys.Tab);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(new Color(9, 19, 29, 39), tester.ColorType);
        }

        [Test]
        public void TestKeyPgDownBool()
        {
            // Goto float variable
            GotoVariable(6);

            tester.BoolType = false;
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.IsTrue(tester.BoolType);
            ExpectKey(Keys.PageDown);
            tweaker.HandleInput(inputDriver);
            Assert.IsFalse(tester.BoolType);
        }

        //[Test]
        //public void TestInputReturn()
        //{
        //    ExpectKey(Keys.Zoom);
        //    Assert.IsFalse(tweaker.HandleInput(inputDriver));
        //    ExpectKey(Keys.Up);
        //    Assert.IsTrue(tweaker.HandleInput(inputDriver));
        //    ExpectKey(Keys.Down);
        //    Assert.IsTrue(tweaker.HandleInput(inputDriver));
        //    ExpectKey(Keys.PageUp);
        //    Assert.IsTrue(tweaker.HandleInput(inputDriver));
        //    ExpectKey(Keys.PageDown);
        //    Assert.IsTrue(tweaker.HandleInput(inputDriver));
        //    ExpectKey(Keys.Tab);
        //    Assert.IsTrue(tweaker.HandleInput(inputDriver));
        //}

        [Test]
        public void TestContainerChange()
        {
            // If we change container, effect number should be reset
            ExpectKey(Keys.Down);
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
            tweaker.CurrentContainer = new TweakableContainer("name");

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
            Keys[] keys = new Keys[] { Keys.NumPad9, Keys.NumPad8, Keys.NumPad7, Keys.NumPad6, Keys.NumPad5 };
            foreach (Keys key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(98765, tester.IntType);

            // Set 43210
            keys = new Keys[] { Keys.NumPad4, Keys.NumPad3, Keys.NumPad2, Keys.NumPad1, Keys.NumPad0 };
            foreach (Keys key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(43210, tester.IntType);

            // Set -43210
            keys = new Keys[] { Keys.Subtract, Keys.NumPad4, Keys.NumPad3, Keys.NumPad2, Keys.NumPad1, Keys.NumPad0 };
            foreach (Keys key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(-43210, tester.IntType);
        }

        [Test]
        public void TestInvalidFormat()
        {
            tester.FloatType = 666;
            ExpectKey(Keys.Subtract);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.Subtract);
            tweaker.HandleInput(inputDriver);
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(666, tester.FloatType);
        }

        [Test]
        public void TestInputFloat()
        {
            tester.FloatType = 0;
            ExpectKey(Keys.Down);
            tweaker.HandleInput(inputDriver);

            // Set .98765
            Keys[] keys = new Keys[] { Keys.Decimal, Keys.D9, Keys.D8, Keys.NumPad7, Keys.D6, Keys.D5 };
            foreach (Keys key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(0.98765f, tester.FloatType);

            // Set 432.10
            keys = new Keys[] { Keys.D4, Keys.D3, Keys.D2, Keys.Decimal, Keys.D1, Keys.D0 };
            foreach (Keys key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(432.10f, tester.FloatType);

            // Set -4321.0
            keys = new Keys[] { Keys.OemMinus, Keys.D4, Keys.D3, Keys.D2, Keys.D1, Keys.OemPeriod, Keys.D0 };
            foreach (Keys key in keys)
            {
                ExpectKey(key);
                tweaker.HandleInput(inputDriver);
            }
            ExpectKey(Keys.Enter);
            tweaker.HandleInput(inputDriver);
            Assert.AreEqual(-4321, tester.FloatType);
        }

        private void ExpectKey(Keys key)
        {
            Keys[] noRepeatKeys = { Keys.Up, Keys.Down, Keys.Tab, Keys.NumPad0, Keys.D0, Keys.NumPad1, Keys.D1, Keys.NumPad2, Keys.D2, Keys.NumPad3, Keys.D3, Keys.NumPad4, Keys.D4, Keys.NumPad5, Keys.D5, Keys.NumPad6, Keys.D6, Keys.NumPad7, Keys.D7, Keys.NumPad8, Keys.D8, Keys.NumPad9, Keys.D9, Keys.Decimal, Keys.OemPeriod, Keys.Subtract, Keys.OemMinus, Keys.Enter };
            Keys[] slowRepeatKeys = { Keys.PageDown, Keys.PageUp };
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
                switch (noRepeatKeys[i])
                {
                    case Keys.Up:
                        Expect.Once.On(inputDriver).
                            Method("UpPressedNoRepeat").
                            Will(Return.Value(pressed));
                        break;
                    case Keys.Down:
                        Expect.Once.On(inputDriver).
                            Method("DownPressedNoRepeat").
                            Will(Return.Value(pressed));
                        break;
                    default:
                        Expect.Once.On(inputDriver).
                            Method("KeyPressedNoRepeat").
                            With(noRepeatKeys[i]).
                            Will(Return.Value(pressed));
                        break;
                }
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
                        //   5 Vector2 name
                        //   6 Vector2 value 1
                        //   7 Vector2 value 2
                        //   8 Vector3 name
                        //   9 Vector3 value 1
                        //  10 Vector3 value 2
                        //  11 Vector3 value 3
                        //  12 Vector4 name
                        //  13 Vector4 value 1
                        //  14 Vector4 value 2
                        //  15 Vector4 value 3
                        //  16 Vector4 value 4
                        //  17 Color name
                        //  18 Color value 1
                        //  19 Color value 2
                        //  20 Color value 3
                        //  21 Color value 4
                        //  22 Color value 5
                        //  23 Color value 6
                        //  24 Float name
                        //  25 Float value
                        Assert.AreEqual(26, mainBox.Children[1].Children.Count);
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
            Expect.Once.On(userInterface).
                Method("DrawControl").
                With(new ControlMatcher(name));
        }

        private void GotoVariable(int variable)
        {
            for (int i = 0; i < variable; i++)
            {
                ExpectKey(Keys.Down);
                tweaker.HandleInput(inputDriver);
            }
        }

    }
}
