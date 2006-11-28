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

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            tweakers = new IDemoTweaker[] { mockery.NewMock<IDemoTweaker>(), mockery.NewMock<IDemoTweaker>() };
            tweaker = new DemoTweakerMain(tweakers);
            input = mockery.NewMock<IInputDriver>();

            Time.Initialize();

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
            for (int i = 0; i < 2; i++)
            {
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.Return).
                    Will(Return.Value(false));
                Expect.Once.On(input).
                    Method("KeyPressedNoRepeat").
                    With(Key.Escape).
                    Will(Return.Value(false));
                tweaker.HandleInput(input);
            }
        }

        [Test]
        public void TestInputAndDraw()
        {
            TestInitialize();

            // First call once with Return pressed
            // Then call and see that context was changed
            ExpectKeypresses(1, -1);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0);
            ExpectSelectedTweaker(0);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // Now go to level 2
            ExpectKeypresses(1, -1);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0);
            ExpectSelectedTweaker(1);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // Go back to 1
            ExpectKeypresses(0, 1);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0);
            ExpectSelectedTweaker(0);
            tweaker.HandleInput(input);
            tweaker.Draw();

            // And finally back to no selection
            ExpectKeypresses(0, 1);
            tweaker.HandleInput(input);
            ExpectKeypresses(0, 0);
            tweaker.HandleInput(input);
            tweaker.Draw();
        }

        private void ExpectSelectedTweaker(int index)
        {
            Expect.Once.On(tweakers[index]).
                Method("HandleInput").
                With(input);
            Expect.Once.On(tweakers[index]).
                Method("Draw");
        }

        private void ExpectKeypresses(int enter, int escape)
        {
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
