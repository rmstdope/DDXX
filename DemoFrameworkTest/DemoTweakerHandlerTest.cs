using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using NMock2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerHandlerTest
    {
        private class TestHelper
        {
            public bool B { set { } }
            public Color C { set { } }
            public int I { set { } }
            public float F { set { } }
            public string S { set { } }
            public Vector2 V2 { set { } }
            public Vector3 V3 { set { } }
            public Vector4 V4 { set { } }
        }
        private class TextureFactoryStub : ITextureFactory
        {
            public ITexture2D CreateFromFile(string name)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public ITextureCube CreateCubeFromFile(string name)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public IRenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public IRenderTarget2D CreateFullsizeRenderTarget()
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public IDepthStencilBuffer CreateFullsizeDepthStencil(DepthFormat format, MultiSampleType multiSampleType)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public ITexture2D CreateFromFunction(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format, Fill2DTextureCallback callbackFunction)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public void RegisterTexture(string name, ITexture2D texture)
            {
                throw new Exception("The method or operation is not implemented.");
            }
            public ITexture2D WhiteTexture
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }
            public List<Texture2DParameters> Texture2DParameters
            {
                get { throw new Exception("The method or operation is not implemented."); }
            }

            #region ITextureFactory Members


            public ITexture2D CreateFromGenerator(string name, int width, int height, int numMipLevels, TextureUsage usage, SurfaceFormat format, ITextureGenerator generator)
            {
                throw new Exception("The method or operation is not implemented.");
            }

            #endregion
        }

        private DemoTweakerHandler handler;
        private TestHelper helper;
        private Mockery mockery;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            handler = new DemoTweakerHandler(null, null);
            helper = new TestHelper();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateTweakableBoolean()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("B");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableBoolean), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableBoolean).Property);
        }

        [Test]
        public void CreateTweakableColor()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("C");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableColor), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableColor).Property);
        }

        [Test]
        public void CreateTweakableInt32()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("I");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableInt32), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableInt32).Property);
        }

        [Test]
        public void CreateTweakableSingle()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("F");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableSingle), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableSingle).Property);
        }

        [Test]
        public void CreateTweakableString()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("S");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableString), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableString).Property);
        }

        [Test]
        public void CreateTweakableVector2()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("V2");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableVector2), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableVector2).Property);
        }

        [Test]
        public void CreateTweakableVector3()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("V3");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableVector3), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableVector3).Property);
        }

        [Test]
        public void CreateTweakableVector4()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("V4");
            ITweakableProperty tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableVector4), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableVector4).Property);
        }

        [Test]
        public void CreateTweakableRegisterable()
        {
            // Exercise SUT
            Registerable registerable = new Registerable("", 0, 0);
            ITweakable tweakable = handler.CreateTweakableObject(registerable);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableRegisterable), tweakable);
        }

        [Test]
        public void CreateTweakableTextureFactory()
        {
            // Exercise SUT
            ITextureFactory textureFactory = new TextureFactoryStub();
            ITweakable tweakable = handler.CreateTweakableObject(textureFactory);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableTextureFactory), tweakable);
        }

    }
}
