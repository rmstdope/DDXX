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
    public class TextureDirectorTest : D3DMockTest, ITextureFactory
    {
        private TextureDirector director;
        private ITextureGenerator generatorUsed;
        private int textureWidth;
        private int textureHeight;
        private int textureMipMaps;
        private SurfaceFormat textureFormat;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            director = new TextureDirector(this);
        }

        [Test]
        public void PerlinNoise()
        {
            // Setup
            director.CreatePerlinNoise(2, 4, 0.5f);
            // Exercise SUT
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
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
            director.CreateCircle(0.1f, 0.2f, 0.2f, 0.5f, new Vector2(0.5f, 0.5f));
            // Exercise SUT
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Circle), generatorUsed);
            Circle circle = generatorUsed as Circle;
            Assert.AreEqual(0.1f, circle.SolidRadius);
            Assert.AreEqual(0.2f, circle.GradientRadius1);
        }

        [Test]
        public void Constant()
        {
            // Setup
            director.CreateConstant(new Vector4(1, 2, 3, 4));
            // Exercise SUT
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Constant), generatorUsed);
            Constant constant = generatorUsed as Constant;
            Assert.AreEqual(new Vector4(1, 2, 3, 4), constant.Color);
        }

        [Test]
        public void Modulate()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f, 0.2f, 0.5f, new Vector2(0.5f, 0.5f));
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Multiply();
            // Exercise SUT
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Multiply), generatorUsed);
            Multiply modulate = generatorUsed as Multiply;
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
            director.Multiply();
        }

        [Test]
        public void GeneratorParameters()
        {
            // Exercise SUT
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
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
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(FromFile), generatorUsed);
            FromFile fromFile = generatorUsed as FromFile;
            Assert.AreEqual("file", fromFile.Filename);
            Assert.AreSame(this, fromFile.TextureFactory);
        }

        [Test]
        public void NormalMap()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f, 0.2f, 0.5f, new Vector2(0.5f, 0.5f));
            director.NormalMap();
            // Exercise SUT
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(NormalMap), generatorUsed);
            NormalMap normalMap = generatorUsed as NormalMap;
            Assert.IsInstanceOfType(typeof(Circle), normalMap.GetInput(0));
        }

        [Test]
        public void Add()
        {
            // Setup
            director.CreateCircle(0.1f, 0.2f, 0.2f, 0.5f, new Vector2(0.5f, 0.5f));
            director.CreatePerlinNoise(2, 4, 0.5f);
            director.Add();
            // Exercise SUT
            director.Generate("name", 1, 2, 3, SurfaceFormat.Color);
            // Verify
            Assert.IsInstanceOfType(typeof(Add), generatorUsed);
            Add add = generatorUsed as Add;
            Assert.IsInstanceOfType(typeof(PerlinNoise), add.GetInput(0));
            Assert.IsInstanceOfType(typeof(Circle), add.GetInput(1));
        }

        #region ITextureFactory Members

        public ITexture2D CreateFromName(string name)
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

        public ITexture2D CreateFromFunction(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format, Generate2DTextureCallback callbackFunction)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture2D CreateFromGenerator(string name, int width, int height, int numMipLevels, TextureUsage usage, SurfaceFormat format, ITextureGenerator generator)
        {
            generatorUsed = generator;
            textureWidth = width;
            textureHeight = height;
            textureMipMaps = numMipLevels;
            textureFormat = format;
            return null;
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

        #endregion

        #region ITextureFactory Members


        public void Update(Texture2DParameters Target)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITextureFactory Members

        public IGraphicsDevice GraphicsDevice
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region ITextureFactory Members

        public bool TextureExists(string name)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
