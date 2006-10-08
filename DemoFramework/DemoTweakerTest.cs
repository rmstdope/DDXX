using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerTest : D3DMockTest
    {
        private DemoTweaker tweaker;
        private IUserInterface userInterface;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            userInterface = mockery.NewMock<IUserInterface>();

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
            tweaker.Initialize();
        }

        [Test]
        public void TestDraw()
        {
            TestInitialize();

            Assert.IsFalse(tweaker.Enabled);
            tweaker.Draw();

            tweaker.Enabled = true;
            Expect.Once.On(device).
                Method("BeginScene");
            Expect.Once.On(userInterface).
                Method("DrawControl");
            Expect.Once.On(device).
                Method("EndScene");
            tweaker.Draw();

            tweaker.Enabled = false;
            Assert.IsFalse(tweaker.Enabled);
            tweaker.Draw();
        }
    }
}
