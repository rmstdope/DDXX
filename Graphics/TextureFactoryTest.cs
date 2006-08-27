using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class TextureFactoryTest
    {
        TextureFactory textureFactory;

        Mockery mockery;
        IGraphicsFactory factory;
        ITexture texture1;
        ITexture texture2;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            texture1 = mockery.NewMock<ITexture>();
            texture2 = mockery.NewMock<ITexture>();
            factory = mockery.NewMock<IGraphicsFactory>();
            textureFactory = new TextureFactory(null, factory);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
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
    }
}
