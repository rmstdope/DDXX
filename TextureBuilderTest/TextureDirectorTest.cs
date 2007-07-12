using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class TextureDirectorTest : ITextureBuilder
    {
        private TextureDirector director;
        private IGenerator generatorUsed;
        private int textureWidth;
        private int textureHeight;
        private int textureMipMaps;
        private Format textureFormat;

        [SetUp]
        public void SetUp()
        {
            director = new TextureDirector(this);
        }

        [Test]
        public void PerlinNoise()
        {
            // Setup
            director.CreatePerlinNoise(2, 4, 0.5f);
            // Exercise SUT
            director.Generate(1, 2, 3, Format.A8R8G8B8);
            // Verify
            Assert.IsInstanceOfType(typeof(PerlinNoise), generatorUsed);
            PerlinNoise noise = generatorUsed as PerlinNoise;
            Assert.AreEqual(2, noise.BaseFrequency);
            Assert.AreEqual(4, noise.NumOctaves);
            Assert.AreEqual(0.5f, noise.Persistence);
        }

        [Test]
        public void Circle()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f);
            // Exercise SUT
            director.Generate(1, 2, 3, Format.A8R8G8B8);
            // Verify
            Assert.IsInstanceOfType(typeof(Circle), generatorUsed);
            Circle circle = generatorUsed as Circle;
            Assert.AreEqual(0.1f, circle.InnerRadius);
            Assert.AreEqual(0.2f, circle.OuterRadius);
        }

        [Test]
        public void Modulate()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f);
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Modulate();
            // Exercise SUT
            director.Generate(1, 2, 3, Format.A8R8G8B8);
            // Verify
            Assert.IsInstanceOfType(typeof(Modulate), generatorUsed);
            Modulate modulate = generatorUsed as Modulate;
            Assert.IsInstanceOfType(typeof(PerlinNoise), modulate.GetInput(0));
            Assert.IsInstanceOfType(typeof(Circle), modulate.GetInput(1));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ModulateTooFewParameters()
        {
            // Setup
            director.CreatePerlinNoise(2, 4, 0.5f);
            // Exercise SUT
            director.Modulate();
        }

        [Test]
        public void GeneratorParameters()
        {
            // Exercise SUT
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Generate(1, 2, 3, Format.A8R8G8B8);
            // Verify
            Assert.AreEqual(1, textureWidth);
            Assert.AreEqual(2, textureHeight);
            Assert.AreEqual(3, textureMipMaps);
            Assert.AreEqual(Format.A8R8G8B8, textureFormat);
        }

        #region ITextureBuilder Members

        public ITexture Generate(IGenerator generator, int width, int height, int numMipLevels, Format format)
        {
            generatorUsed = generator;
            textureWidth = width;
            textureHeight = height;
            textureMipMaps = numMipLevels;
            textureFormat = format;
            return null;
        }

        #endregion
    }
}
