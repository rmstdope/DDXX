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
using Dope.DDXX.Input;
using Microsoft.DirectX.DirectInput;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerMainTest : D3DMockTest
    {
        private DemoTweakerMain tweaker;
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
            tweaker = new DemoTweakerMain(context, tweakers, settings);
            input = mockery.NewMock<IInputDriver>();
            registrator = mockery.NewMock<IDemoRegistrator>();
            userInterface = mockery.NewMock<IUserInterface>();
            Stub.On(registrator).GetProperty("StartTime").Will(Return.Value(0.5f));
            tweaker.UserInterface = userInterface;

            Stub.On(settings).GetProperty("Alpha").Will(Return.Value(0.0f));
            Stub.On(settings).GetProperty("TitleColor").Will(Return.Value(Color.Wheat));
            Stub.On(settings).GetProperty("TextAlpha").Will(Return.Value(0.0f));
            Stub.On(settings).GetProperty("TimeColor").Will(Return.Value(Color.DarkGray));

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
            Expect.Once.On(tweakers[0]).
                Method("Initialize").
                With(registrator);
            Expect.Once.On(tweakers[1]).
                Method("Initialize").
                With(registrator);
            Expect.Once.On(userInterface).
                Method("Initialize");
            tweaker.Initialize(registrator);
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
        public void TestShouldSaveNoMovement()
        {
            TestInitialize();

            ExpectKeypresses(-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1);
            Assert.IsTrue(tweaker.ShouldSave(input));
        }

        [Test]
        public void TestShouldSaveMoveLeft()
        {
            TestInitialize();

            ExpectKeypresses(0, 1, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            ExpectKeypresses(-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectDraw("TestShouldSaveMoveLeft");
            Assert.IsTrue(tweaker.ShouldSave(input));
        }

        [Test]
        public void TestShouldSaveMoveRight()
        {
            TestInitialize();

            ExpectKeypresses(1, 0, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            ExpectKeypresses(-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectDraw("TestShouldSaveMoveRight");
            Assert.IsFalse(tweaker.ShouldSave(input));
        }

        [Test]
        public void TestShouldSaveMoveRightAndLeft()
        {
            TestInitialize();

            ExpectKeypresses(1, 0, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            ExpectKeypresses(0, 1, -1, 0, -1, -1, -1, -1, -1, -1, -1);
            ExpectDraw("TestShouldSaveMoveRightAndLeft1");
            ExpectKeypresses(-1, -1, -1, 1, -1, -1, -1, -1, -1, -1, -1);
            ExpectDraw("TestShouldSaveMoveRightAndLeft2");
            Assert.IsTrue(tweaker.ShouldSave(input));
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
            Expect.Once.On(device).
                Method("Present");
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
                    Method("KeyPressedNoRepeat").
                    With(Key.RightArrow).
                    Will(Return.Value(right != 0));
            if (left != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.LeftArrow).
                    Will(Return.Value(left != 0));
            if (space != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.Space).
                    Will(Return.Value(space != 0));
            if (enter != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.Return).
                    Will(Return.Value(enter != 0));
            if (escape != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.Escape).
                    Will(Return.Value(escape != 0));
            if (f1 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.F1).
                    Will(Return.Value(f1 != 0));
            if (f2 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.F2).
                    Will(Return.Value(f2 != 0));
            if (f3 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.F3).
                    Will(Return.Value(f3 != 0));
            if (f4 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.F4).
                    Will(Return.Value(f4 != 0));
            if (f5 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.F5).
                    Will(Return.Value(f5 != 0));
            if (f6 != -1)
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.F6).
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
