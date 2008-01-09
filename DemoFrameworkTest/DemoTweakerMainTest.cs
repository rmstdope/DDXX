using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Dope.DDXX.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerMainTest : D3DMockTest
    {
        private DemoTweakerHandler tweaker;
        private IDemoTweaker[] tweakers;
        private IInputDriver input;
        private IDemoTweakerContext context;
        private ITweakerSettings settings;
        private IDemoRegistrator registrator;
        private IUserInterface userInterface;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            context = mockery.NewMock<IDemoTweakerContext>();
            settings = mockery.NewMock<ITweakerSettings>();
            tweakers = new IDemoTweaker[] { mockery.NewMock<IDemoTweaker>(), mockery.NewMock<IDemoTweaker>() };
            tweaker = new DemoTweakerHandler(context, tweakers, settings);
            input = mockery.NewMock<IInputDriver>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            userInterface = mockery.NewMock<IUserInterface>();
            Stub.On(registrator).GetProperty("StartTime").Will(Return.Value(0.5f));
            tweaker.UserInterface = userInterface;

            Stub.On(settings).GetProperty("Alpha").Will(Return.Value((byte)0));
            Stub.On(settings).GetProperty("TitleColor").Will(Return.Value(Color.Wheat));
            Stub.On(settings).GetProperty("TextAlpha").Will(Return.Value((byte)0));
            Stub.On(settings).GetProperty("TimeColor").Will(Return.Value(Color.DarkGray));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            Expect.Once.On(tweakers[0]).
                Method("Initialize").
                With(registrator, graphicsFactory, textureFactory);
            Expect.Once.On(tweakers[1]).
                Method("Initialize").
                With(registrator, graphicsFactory, textureFactory);
            Expect.Once.On(userInterface).
                Method("Initialize").
                With(graphicsFactory, textureFactory);
            tweaker.Initialize(registrator, graphicsFactory, textureFactory);
        }

        [Test]
        public void TestInputNoPress()
        {
            TestInitialize();

            // Call twice with no key press
            // This should not case any context change
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestPauseResume()
        {
            TestInitialize();

            ExpectKeypresses(0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0);
            Expect.Once.On(context).Method("TogglePause");
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestJumpForward()
        {
            TestInitialize();

            ExpectKeypresses(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Expect.Once.On(context).Method("JumpInTime").With(5.0f);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestJumpBackward()
        {
            TestInitialize();

            ExpectKeypresses(0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            Expect.Once.On(context).Method("JumpInTime").With(-5.0f);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestInput()
        {
            TestInitialize();

            // First call once with Return pressed
            // Then call and see that context was changed
            ExpectKeypresses(0, 0, 0, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectTweakerTransition(-1, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);

            ExpectTweakerInput(0, true);
            tweaker.HandleInput(input);

            // Now go to level 2
            ExpectKeypresses(0, 0, 0, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectTweakerTransition(0, 1);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);

            ExpectTweakerInput(1, true);
            tweaker.HandleInput(input);

            // Go back to 1
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            ExpectTweakerTransition(1, 0);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);

            ExpectTweakerInput(0, true);
            tweaker.HandleInput(input);

            // And finally back to no selection
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            ExpectTweakerTransition(0, -1);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestF2()
        {
            TestInitialize();

            // Press F2
            Expect.Once.On(settings).
                Method("SetTransparency").With(Transparency.Low);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestF3()
        {
            TestInitialize();

            // Press F3
            Expect.Once.On(settings).
                Method("SetTransparency").With(Transparency.Medium);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestF4()
        {
            TestInitialize();

            // Press F4
            Expect.Once.On(settings).
                Method("SetTransparency").With(Transparency.High);
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestF5()
        {
            TestInitialize();

            // Press F5
            Expect.Once.On(settings).
                Method("NextColorSchema");
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestF6()
        {
            TestInitialize();

            // Press F6
            Expect.Once.On(settings).
                Method("PreviousColorSchema");
            ExpectKeypresses(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            // Draw 0
            ExpectKeypresses(0, 0, 0, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectTweakerTransition(-1, 0);
            tweaker.HandleInput(input);
            ExpectTweakerDraw(0);
            tweaker.Draw();

            // Draw 1
            ExpectKeypresses(0, 0, 0, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectTweakerTransition(0, 1);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);
            ExpectTweakerDraw(1);
            tweaker.Draw();

            // Turn GUI off
            ExpectKeypresses(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // Turn GUI on
            ExpectKeypresses(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);
            ExpectTweakerDraw(1);
            tweaker.Draw();
        }

        [Test]
        public void TestShouldSaveWithoutAccess()
        {
            TestInitialize();
            Assert.IsFalse(tweaker.ShouldSave());
        }

        [Test]
        public void TestExitingFlag()
        {
            Assert.IsFalse(tweaker.Exiting);
            // Press Esc
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);
            Assert.IsTrue(tweaker.Exiting);
        }

        [Test]
        public void TestShouldSaveNoMovement()
        {
            TestInput();

            // Press Esc
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);

            // Press Enter
            ExpectKeypresses(0, 0, -1, 1, -1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);

            Assert.IsTrue(tweaker.ShouldSave());
        }

        [Test]
        public void TestShouldSaveMoveLeft()
        {
            TestInput();

            // Press Esc
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);

            // Press Left
            ExpectKeypresses(0, 1, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);
            ExpectDraw("TestShouldSaveMoveLeft");
            tweaker.Draw();
            Assert.IsTrue(tweaker.ShouldSave());
        }

        [Test]
        public void TestShouldSaveMoveRight()
        {
            TestInput();

            // Press Esc
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);

            // Press Right
            ExpectKeypresses(1, 0, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);
            ExpectDraw("TestShouldSaveMoveRight");
            tweaker.Draw();
            Assert.IsFalse(tweaker.ShouldSave());
        }

        [Test]
        public void TestShouldSaveMoveRightAndLeft()
        {
            TestInput();

            // Press Esc
            ExpectKeypresses(0, 0, 0, 0, 1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);

            // Press Right
            ExpectKeypresses(1, 0, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);
            ExpectDraw("TestShouldSaveMoveRightAndLeft1");
            tweaker.Draw();

            // Press Left
            ExpectKeypresses(0, 1, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            tweaker.HandleInput(input);
            ExpectDraw("TestShouldSaveMoveRightAndLeft2");
            tweaker.Draw();

            Assert.IsTrue(tweaker.ShouldSave());
        }

        private void ExpectDraw(string name)
        {
            Expect.Once.On(userInterface).
                Method("DrawControl").
                With(new ControlMatcher(name));
        }

        private void ExpectTweakerTransition(int from, int to)
        {
            if (from != -1 && from < to)
            {
                Expect.Once.On(tweakers[from]).
                    Method("IdentifierToChild").
                    Will(Return.Value(to));
                Expect.Once.On(tweakers[to]).
                    Method("IdentifierFromParent").
                    With(to);
            }
        }

        private void ExpectTweakerInput(int index, bool returnValue)
        {
            Expect.Once.On(tweakers[index]).
                Method("HandleInput").
                With(input).
                Will(Return.Value(returnValue));
        }

        private void ExpectTweakerDraw(int index)
        {
            Expect.Once.On(tweakers[index]).
                Method("Draw");
        }

        private void ExpectKeypresses(int right, int left, int space, int enter, int escape, int f1, int f2, int f3, int f4, int f5, int f6)
        {
            if (right != -1)
                Expect.Once.On(input).
                    Method("RightPressedNoRepeat").
                    Will(Return.Value(right != 0));
            if (left != -1)
                Expect.Once.On(input).
                    Method("LeftPressedNoRepeat").
                    Will(Return.Value(left != 0));
            if (space != -1)
                Expect.Once.On(input).
                    Method("PausePressedNoRepeat").
                    Will(Return.Value(space != 0));
            if (enter != -1)
                Expect.Once.On(input).
                    Method("OkPressedNoRepeat").
                    Will(Return.Value(enter != 0));
            if (escape != -1)
                Expect.Once.On(input).
                    Method("BackPressedNoRepeat").
                    Will(Return.Value(escape != 0));
            if (f1 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Keys.F1).
                    Will(Return.Value(f1 != 0));
            if (f2 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Keys.F2).
                    Will(Return.Value(f2 != 0));
            if (f3 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Keys.F3).
                    Will(Return.Value(f3 != 0));
            if (f4 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Keys.F4).
                    Will(Return.Value(f4 != 0));
            if (f5 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Keys.F5).
                    Will(Return.Value(f5 != 0));
            if (f6 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Keys.F6).
                    Will(Return.Value(f6 != 0));
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

                // mainWindow
                // 0 titleWindow
                // 1 tweakableWindow
                //   0 "Should..."
                //   1 "Yes..."
                //   2 "No..."
                Assert.AreEqual(3, mainBox.Children[1].Children.Count);
                TextControl yes = (TextControl)mainBox.Children[1].Children[1];
                TextControl no = (TextControl)mainBox.Children[1].Children[2];
                if (test == "TestShouldSaveMoveLeft" ||
                    test == "TestShouldSaveMoveRightAndLeft2")
                    {
                    Assert.AreEqual(Color.White, yes.Color);
                    Assert.AreEqual(Color.DarkGray, no.Color);
                }
                else
                {
                    Assert.AreEqual(Color.White, no.Color);
                    Assert.AreEqual(Color.DarkGray, yes.Color);
                }
                return true;
            }
        }

    }
}
