using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class TextureFactoryTest
    {
        private TextureFactory textureFactory;
        private PresentParameters presentParameters;

        private Mockery mockery;
        private IGraphicsFactory factory;
        private ITexture texture1;
        private ITexture texture2;
        private ICubeTexture cubeTexture1;
        private ICubeTexture cubeTexture2;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            texture1 = mockery.NewMock<ITexture>();
            texture2 = mockery.NewMock<ITexture>();
            cubeTexture1 = mockery.NewMock<ICubeTexture>();
            cubeTexture2 = mockery.NewMock<ICubeTexture>();
            factory = mockery.NewMock<IGraphicsFactory>();
            presentParameters = new PresentParameters();
            presentParameters.BackBufferWidth = 100;
            presentParameters.BackBufferHeight = 200;
            presentParameters.BackBufferFormat = Format.Q16W16V16U16;
            textureFactory = new TextureFactory(null, factory, presentParameters);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestCreateFullsizeRenderTarget1()
        {
            Expect.Once.On(factory).
                Method("CreateTexture").
                With(null, 100, 200, 1, Usage.RenderTarget, Format.Q16W16V16U16, Pool.Default).
                Will(Return.Value(texture1));
            ITexture newTexture1 = textureFactory.CreateFullsizeRenderTarget();
            Assert.AreSame(texture1, newTexture1);
        }

        [Test]
        public void TestCreateFullsizeRenderTarget2()
        {
            Expect.Once.On(factory).
                Method("CreateTexture").
                With(null, 100, 200, 1, Usage.RenderTarget, Format.R16F, Pool.Default).
                Will(Return.Value(texture1));
            ITexture newTexture1 = textureFactory.CreateFullsizeRenderTarget(Format.R16F);
            Assert.AreSame(texture1, newTexture1);
        }

        [Test]
        public void CreateFromFileTest()
        {
            Expect.Once.On(factory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(texture1));
            ITexture newTexture1 = textureFactory.CreateFromFile("fileName");
            Assert.AreSame(texture1, newTexture1);
            ITexture newTexture2 = textureFactory.CreateFromFile("fileName");
            Assert.AreSame(newTexture1, newTexture2);

            Expect.Once.On(factory).
                Method("TextureFromFile").
                WithAnyArguments().
                Will(Return.Value(texture2));
            ITexture newTexture3 = textureFactory.CreateFromFile("fileName2");
            Assert.AreSame(texture2, newTexture3);
        }

        [Test]
        public void CreateCubeFromFileTest()
        {
            Expect.Once.On(factory).
                Method("CubeTextureFromFile").
                WithAnyArguments().
                Will(Return.Value(cubeTexture1));
            ICubeTexture newTexture1 = textureFactory.CreateCubeFromFile("fileName");
            Assert.AreSame(cubeTexture1, newTexture1);
            ICubeTexture newTexture2 = textureFactory.CreateCubeFromFile("fileName");
            Assert.AreSame(newTexture1, newTexture2);

            Expect.Once.On(factory).
                Method("CubeTextureFromFile").
                WithAnyArguments().
                Will(Return.Value(cubeTexture2));
            ICubeTexture newTexture3 = textureFactory.CreateCubeFromFile("fileName2");
            Assert.AreSame(cubeTexture2, newTexture3);
        }

        [Test]
        public void RegisterTextureX()
        {
            textureFactory.RegisterTexture("x", texture1);
            Assert.AreEqual(texture1, textureFactory.CreateFromFile("x"));
        }

        [Test]
        public void RegisterTextureY()
        {
            textureFactory.RegisterTexture("y", texture2);
            Assert.AreEqual(texture2, textureFactory.CreateFromFile("y"));
        }
    }
}
