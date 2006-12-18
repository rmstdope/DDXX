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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            context = mockery.NewMock<IDemoTweakerContext>();
            tweakers = new IDemoTweaker[] { mockery.NewMock<IDemoTweaker>(), mockery.NewMock<IDemoTweaker>() };
            tweaker = new DemoTweakerMain(context, tweakers);
            input = mockery.NewMock<IInputDriver>();

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
                With(Is.Null);
            Expect.Once.On(tweakers[1]).
                Method("Initialize").
                With(Is.Null);
            tweaker.Initialize(null);
        }

        [Test]
        public void TestInputNoPress()
        {
            TestInitialize();

            // Call twice with no key press
            // This should not case any context change
            ExpectKeypresses(0, 0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestPauseResume()
        {
            TestInitialize();

            ExpectKeypresses(0, 0, 1, 0, 0, 0);
            Expect.Once.On(context).Method("TogglePause");
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestJumpForward()
        {
            TestInitialize();

            ExpectKeypresses(1, 0, 0, 0, 0, 0);
            Expect.Once.On(context).Method("JumpInTime").With(5.0f);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestJumpBackward()
        {
            TestInitialize();

            ExpectKeypresses(0, 1, 0, 0, 0, 0);
            Expect.Once.On(context).Method("JumpInTime").With(-5.0f);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestInput()
        {
            TestInitialize();

            // First call once with Return pressed
            // Then call and see that context was changed
            ExpectKeypresses(0, 0, 0, 1, -1, -1);
            ExpectTweakerTransition(-1, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);

            ExpectTweakerInput(0, true);
            tweaker.HandleInput(input);

            // Now go to level 2
            ExpectKeypresses(0, 0, 0, 1, -1, -1);
            ExpectTweakerTransition(0, 1);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);

            ExpectTweakerInput(1, true);
            tweaker.HandleInput(input);

            // Go back to 1
            ExpectKeypresses(0, 0, 0, 0, 1, -1);
            ExpectTweakerTransition(1, 0);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0, 0);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);

            ExpectTweakerInput(0, true);
            tweaker.HandleInput(input);

            // And finally back to no selection
            ExpectKeypresses(0, 0, 0, 0, 1, -1);
            ExpectTweakerTransition(0, -1);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            // Draw 0
            ExpectKeypresses(0, 0, 0, 1, -1, -1);
            ExpectTweakerTransition(-1, 0);
            tweaker.HandleInput(input);
            ExpectTweakerDraw(0);
            tweaker.Draw();

            // Draw 1
            ExpectKeypresses(0, 0, 0, 1, -1, -1);
            ExpectTweakerTransition(0, 1);
            ExpectTweakerInput(0, false);
            tweaker.HandleInput(input);
            ExpectTweakerDraw(1);
            tweaker.Draw();

            // Turn GUI off
            ExpectKeypresses(0, 0, 0, 0, 0, 1);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // Turn GUI on
            ExpectKeypresses(0, 0, 0, 0, 0, 1);
            ExpectTweakerInput(1, false);
            tweaker.HandleInput(input);
            ExpectTweakerDraw(1);
            tweaker.Draw();

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

        private void ExpectKeypresses(int right, int left, int space, int enter, int escape, int f1)
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
        }

    }
}
