using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.TextureBuilder
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
            ITexture2D texture = mockery.NewMock<ITexture2D>();
            Expect.Once.On(textureFactory).Method("CreateFromFunction").
                With(Is.EqualTo(10), Is.EqualTo(20), Is.EqualTo(30), Is.EqualTo(TextureUsage.None),
                Is.EqualTo(SurfaceFormat.Color), Is.NotNull).
                Will(Return.Value(texture));
            TextureBuilder builder = new TextureBuilder(textureFactory);
            Assert.AreEqual(texture, builder.Generate(generator, 10, 20, 30, SurfaceFormat.Color));
        }

        [Test]
        public void GenerateTexture2()
        {
            mockery = new Mockery();
            textureFactory = mockery.NewMock<ITextureFactory>();
            IGenerator generator = mockery.NewMock<IGenerator>();
            ITexture2D texture = mockery.NewMock<ITexture2D>();
            Expect.Once.On(textureFactory).Method("CreateFromFunction").
                With(Is.EqualTo(20), Is.EqualTo(30), Is.EqualTo(40), Is.EqualTo(TextureUsage.None),
                Is.EqualTo(SurfaceFormat.Depth24), Is.NotNull).
                Will(Return.Value(texture));
            TextureBuilder builder = new TextureBuilder(textureFactory);
            Assert.AreEqual(texture, builder.Generate(generator, 20, 30, 40, SurfaceFormat.Depth24));
        }

    }
}
