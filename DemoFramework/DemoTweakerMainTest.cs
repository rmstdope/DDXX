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
            ExpectKeypresses(0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestPauseResume()
        {
            TestInitialize();

            ExpectKeypresses(0, 0, 1, 0, 0);
            Expect.Once.On(context).Method("TogglePause");
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestJumpForward()
        {
            TestInitialize();

            ExpectKeypresses(1, 0, 0, 0, 0);
            Expect.Once.On(context).Method("JumpInTime").With(5.0f);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestJumpBackward()
        {
            TestInitialize();

            ExpectKeypresses(0, 1, 0, 0, 0);
            Expect.Once.On(context).Method("JumpInTime").With(-5.0f);
            tweaker.HandleInput(input);
        }

        [Test]
        public void TestInputAndDraw()
        {
            TestInitialize();

            // First call once with Return pressed
            // Then call and see that context was changed
            ExpectKeypresses(0, 0, 0, 1, -1);
            ExpectTweakerTransition(-1, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0);
            ExpectSelectedTweaker(0);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // Now go to level 2
            ExpectKeypresses(0, 0, 0, 1, -1);
            ExpectTweakerTransition(0, 1);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0);
            ExpectSelectedTweaker(1);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // Go back to 1
            ExpectKeypresses(0, 0, 0, 0, 1);
            ExpectTweakerTransition(1, 0);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0);
            ExpectSelectedTweaker(0);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // And finally back to no selection
            ExpectKeypresses(0, 0, 0, 0, 1);
            ExpectTweakerTransition(0, -1);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0, 0, 0, 0);
            tweaker.HandleInput(input);
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

        private void ExpectSelectedTweaker(int index)
        {
            Expect.Once.On(tweakers[index]).
                Method("HandleInput").
                With(input);
            Expect.Once.On(tweakers[index]).
                Method("Draw");
        }

        private void ExpectKeypresses(int right, int left, int space, int enter, int escape)
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
        }

    }
}
