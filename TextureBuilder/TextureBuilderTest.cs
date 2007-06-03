using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace TextureBuilder
{
    [TestFixture]
    public class TextureBuilderTest
    {
        private Mockery mockery;
        private ITextureFactory textureFactory;

        [Test]
        public void GenerateTexture1()
        {
            mockery = new Mockery();
            textureFactory = mockery.NewMock<ITextureFactory>();
            IGenerator generator = mockery.NewMock<IGenerator>();
            ITexture texture = mockery.NewMock<ITexture>();
            Expect.Once.On(textureFactory).Method("CreateFromFunction").
                With(Is.EqualTo(10), Is.EqualTo(20), Is.EqualTo(30), Is.EqualTo(Usage.None),
                Is.EqualTo(Format.A8R8G8B8), Is.EqualTo(Pool.Managed), Is.NotNull).
                Will(Return.Value(texture));
            TextureBuilder builder = new TextureBuilder(textureFactory);
            Assert.AreEqual(texture, builder.Generate(generator, 10, 20, 30, Format.A8R8G8B8));
        }

        [Test]
        public void GenerateTexture2()
        {
            mockery = new Mockery();
            textureFactory = mockery.NewMock<ITextureFactory>();
            IGenerator generator = mockery.NewMock<IGenerator>();
            ITexture texture = mockery.NewMock<ITexture>();
            Expect.Once.On(textureFactory).Method("CreateFromFunction").
                With(Is.EqualTo(20), Is.EqualTo(30), Is.EqualTo(40), Is.EqualTo(Usage.None),
                Is.EqualTo(Format.L8), Is.EqualTo(Pool.Managed), Is.NotNull).
                Will(Return.Value(texture));
            TextureBuilder builder = new TextureBuilder(textureFactory);
            Assert.AreEqual(texture, builder.Generate(generator, 20, 30, 40, Format.L8));
        }

    }
}
