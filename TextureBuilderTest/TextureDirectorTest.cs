using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class TextureDirectorTest : D3DMockTest, ITextureBuilder
    {
        private TextureDirector director;
        private IGenerator generatorUsed;
        private int textureWidth;
        private int textureHeight;
        private int textureMipMaps;
        private SurfaceFormat textureFormat;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            director = new TextureDirector(this, textureFactory);
        }

        [Test]
        public void PerlinNoise()
        {
            // Setup
            director.CreatePerlinNoise(2, 4, 0.5f);
            // Exercise SUT
            director.Generate(1, 2, 3, SurfaceFormat.Color);
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
            director.Generate(1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Circle), generatorUsed);
            Circle circle = generatorUsed as Circle;
            Assert.AreEqual(0.1f, circle.InnerRadius);
            Assert.AreEqual(0.2f, circle.OuterRadius);
        }

        [Test]
        public void Constant()
        {
            // Setup
            director.CreateConstant(new Vector4(1, 2, 3, 4));
            // Exercise SUT
            director.Generate(1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Constant), generatorUsed);
            Constant constant = generatorUsed as Constant;
            Assert.AreEqual(new Vector4(1, 2, 3, 4), constant.Color);
        }

        [Test]
        public void Modulate()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f);
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Modulate();
            // Exercise SUT
            director.Generate(1, 2, 3, SurfaceFormat.Color);
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
            director.Generate(1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.AreEqual(1, textureWidth);
            Assert.AreEqual(2, textureHeight);
            Assert.AreEqual(3, textureMipMaps);
            Assert.AreEqual(SurfaceFormat.Color, textureFormat);
        }

        [Test]
        public void FromFile()
        {
            // Setup
            director.FromFile("file");
            // Exercise SUT
            director.Generate(1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(FromFile), generatorUsed);
            FromFile fromFile = generatorUsed as FromFile;
            Assert.AreEqual("file", fromFile.Filename);
            Assert.AreEqual(textureFactory, fromFile.TextureFactory);
        }

        [Test]
        public void NormalMap()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f);
            director.NormalMap();
            // Exercise SUT
            director.Generate(1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(NormalMap), generatorUsed);
            NormalMap normalMap = generatorUsed as NormalMap;
            Assert.IsInstanceOfType(typeof(Circle), normalMap.GetInput(0));
        }

        [Test]
        public void Add()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f);
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Add();
            // Exercise SUT
            director.Generate(1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Add), generatorUsed);
            Add add = generatorUsed as Add;
            Assert.IsInstanceOfType(typeof(PerlinNoise), add.GetInput(0));
            Assert.IsInstanceOfType(typeof(Circle), add.GetInput(1));
        }

        #region ITextureBuilder Members

        public ITexture2D Generate(IGenerator generator, int width, int height, int numMipLevels, SurfaceFormat format)
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
