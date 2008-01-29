using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableTextureFactoryTest
    {
        private TweakableTextureFactory tweakable;
        private Mockery mockery;
        private ITextureFactory target;
        private ITweakableFactory factory;
        List<Texture2DParameters> list;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            target = mockery.NewMock<ITextureFactory>();
            factory = mockery.NewMock<ITweakableFactory>();
            tweakable = new TweakableTextureFactory(target, factory);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void EmptyFunctions()
        {
            // Exercise SUT
            //tweakable.DecreaseValue(5);
            //tweakable.IncreaseValue(10);
            //tweakable.NextIndex(null);
            //tweakable.SetFromString("x");
            //tweakable.SetValue(null);
        }

        [Test]
        public void NumVisable()
        {
            // Exercise SUT and verify
            Assert.AreEqual(13, tweakable.NumVisableVariables);
        }

        [Test]
        public void NumVariables()
        {
            // Setup
            SetupTextures(2);
            // Exercise SUT and verify
            Assert.AreEqual(2, tweakable.NumVariables);
        }

        [Test]
        public void ChildVariables()
        {
            // Setup
            SetupTextures(2);
            ITweakable expectedChild = mockery.NewMock<ITweakable>();
            Expect.Once.On(factory).Method("CreateTweakableObject").With(list[0]).Will(Return.Value(expectedChild));
            // Exercise SUT
            ITweakable child = tweakable.GetChild(0);
            // Verify
            Assert.AreSame(expectedChild, child);
        }

        private void SetupTextures(int num)
        {
            list = new List<Texture2DParameters>();
            for (int i = 0; i < num; i++)
            {
                ITexture2D texture = mockery.NewMock<ITexture2D>();
                list.Add(new Texture2DParameters(i.ToString(), texture));
            }
            Stub.On(target).GetProperty("Texture2DParameters").Will(Return.Value(list));
        }

    }
}
