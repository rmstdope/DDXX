using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class PostProcessorTest : D3DMockTest
    {
        private IPostProcessor postProcessor;
        private IRenderTarget2D startTexture;
        private IRenderTarget2D fullsizeTexture1;
        private IRenderTarget2D fullsizeTexture2;
        private IRenderTarget2D fullsizeTexture3;

        private IEffectParameter sourceTextureParameter;
        private IEffectPass passHandle;
        private IEffectAnnotation scaleHandle;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            startTexture = mockery.NewMock<IRenderTarget2D>();
            fullsizeTexture1 = mockery.NewMock<IRenderTarget2D>();
            fullsizeTexture2 = mockery.NewMock<IRenderTarget2D>();
            fullsizeTexture3 = mockery.NewMock<IRenderTarget2D>();

            postProcessor = new PostProcessor();

            sourceTextureParameter = mockery.NewMock<IEffectParameter>();
            passHandle = mockery.NewMock<IEffectPass>();
            scaleHandle = mockery.NewMock<IEffectAnnotation>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitializeNoAnnotations()
        {
            InitializeSetup();
            ExpectForeachParameters(new IEffectParameter[] { });
            postProcessor.Initialize(device, graphicsFactory, textureFactory, effectFactory);
        }

        [Test]
        public void TestOneTemporaryTexture()
        {
            TestInitializeNoAnnotations();
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));

            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(1, false));
        }

        [Test]
        public void TestTwoTemporaryTextures()
        {
            TestInitializeNoAnnotations();
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(fullsizeTexture1);
            textures.Add(fullsizeTexture2);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture2));

            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(2, false));
        }

        [Test]
        public void TestTemporaryTexturesMultipleCalls()
        {
            TestInitializeNoAnnotations();
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(1, false));

            textures.Add(fullsizeTexture2);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture2));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(2, false));
        }

        [Test]
        public void TestOneTemporaryTextureSameAsOutput()
        {
            TestInitializeNoAnnotations();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            postProcessor.GetTemporaryTextures(1, false);
            postProcessor.StartFrame(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture2));
            Assert.AreSame(fullsizeTexture2, postProcessor.GetTemporaryTextures(1, false)[0]);
        }

        [Test]
        public void TestTwoTemporaryTexturesFirstSameAsOutput()
        {
            TestInitializeNoAnnotations();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture2));
            postProcessor.GetTemporaryTextures(2, false);
            postProcessor.StartFrame(fullsizeTexture1);
            List<IRenderTarget2D> textures = postProcessor.GetTemporaryTextures(2, false);
            Assert.AreSame(fullsizeTexture2, textures[0]);
            Assert.AreSame(fullsizeTexture1, textures[1]);
        }

        [Test]
        public void TestTwoTemporaryTexturesSkipOutput()
        {
            TestInitializeNoAnnotations();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture2));
            postProcessor.GetTemporaryTextures(2, false);
            postProcessor.StartFrame(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture3));
            List<IRenderTarget2D> textures = postProcessor.GetTemporaryTextures(2, true);
            Assert.AreSame(fullsizeTexture2, textures[0]);
            Assert.AreSame(fullsizeTexture3, textures[1]);
        }

        [Test]
        public void TestAllocateTexture()
        {
            TestInitializeNoAnnotations();
            List<IRenderTarget2D> textures = new List<IRenderTarget2D>();
            textures.Add(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(1, false));

            postProcessor.AllocateTexture(fullsizeTexture1);

            textures.Clear();
            textures.Add(fullsizeTexture2);
            textures.Add(fullsizeTexture2);
            Expect.Exactly(2).On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture2));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(2, false));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestFreeUnknownTexture()
        {
            postProcessor.FreeTexture(fullsizeTexture1);
        }

        [Test]
        public void TestFreeAllocatedTexture()
        {
            TestInitializeNoAnnotations();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            postProcessor.GetTemporaryTextures(1, false);
            postProcessor.AllocateTexture(fullsizeTexture1);
            postProcessor.FreeTexture(fullsizeTexture1);
            Assert.AreSame(fullsizeTexture1, postProcessor.GetTemporaryTextures(1, false)[0]);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAllocateTextureTwice()
        {
            TestInitializeNoAnnotations();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                Will(Return.Value(fullsizeTexture1));
            postProcessor.GetTemporaryTextures(1, false);
            postProcessor.AllocateTexture(fullsizeTexture1);
            postProcessor.AllocateTexture(fullsizeTexture1);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAllocateUnknownTexture()
        {
            TestInitializeNoAnnotations();
            postProcessor.AllocateTexture(fullsizeTexture1);
        }

        [Test]
        public void TestAnnotations()
        {
            presentParameters.BackBufferWidth = 2;
            presentParameters.BackBufferHeight = 4;
            IEffectParameter toParameter1 = mockery.NewMock<IEffectParameter>();
            IEffectParameter toParameter2 = mockery.NewMock<IEffectParameter>();
            IEffectParameter toParameter3 = mockery.NewMock<IEffectParameter>();
            IEffectAnnotation annotation1 = mockery.NewMock<IEffectAnnotation>();
            IEffectAnnotation annotation3 = mockery.NewMock<IEffectAnnotation>();
            IEffectParameter fromParameter1 = mockery.NewMock<IEffectParameter>();
            IEffectParameter fromParameter3 = mockery.NewMock<IEffectParameter>();
            IEffectParameter elements1 = mockery.NewMock<IEffectParameter>();
            IEffectParameter elements3 = mockery.NewMock<IEffectParameter>();

            InitializeSetup();
            ExpectForeachParameters(new IEffectParameter[] { toParameter1, toParameter2, toParameter3 });

            // First parameter
            ExpectGetAnnotation(toParameter1, "ConvertPixelsToTexels", annotation1);
            Expect.Once.On(annotation1).Method("GetValueString").Will(Return.Value("fromParameter1"));
            ExpectGetParameter("fromParameter1", fromParameter1);
            ExpectElementCount(fromParameter1, 1);
            Expect.Once.On(fromParameter1).Method("GetValueSingleArray").With(2).
                Will(Return.Value(new float[] { 1, 2 }));
            Expect.Once.On(toParameter1).Method("SetValue").
                With(new float[] { 1 / 2.0f, 2 / 4.0f });

            // Second parameter
            ExpectGetAnnotation(toParameter2, "ConvertPixelsToTexels", null);

            // Third parameter
            ExpectGetAnnotation(toParameter3, "ConvertPixelsToTexels", annotation3);
            Expect.Once.On(annotation3).Method("GetValueString").Will(Return.Value("fromParameter3"));
            ExpectGetParameter("fromParameter3", fromParameter3);
            ExpectElementCount(fromParameter3, 2);
            Expect.Once.On(fromParameter3).Method("GetValueSingleArray").With(4).
                Will(Return.Value(new float[] { 3, 4, 5, 6 }));
            Expect.Once.On(toParameter3).Method("SetValue").
                With(new float[] { 3 / 2.0f, 4 / 4.0f, 5 / 2.0f, 6 / 4.0f });

            postProcessor.Initialize(device, graphicsFactory, textureFactory, effectFactory);
        }

        [Test]
        public void TestNoPostEffect()
        {
            TestInitializeNoAnnotations();
            postProcessor.StartFrame(startTexture);
            Assert.AreSame(startTexture, postProcessor.OutputTexture);
        }

        [Test]
        public void TestProcess()
        {
            TestInitializeNoAnnotations();
            postProcessor.StartFrame(startTexture);
            CreateFullsize(new IRenderTarget2D[] { fullsizeTexture1 });

            SetupPostEffect(startTexture, fullsizeTexture1, "Monochrome", 400, 200, 1.0f, false);
            SetupBlend(BlendFunction.Max, Blend.SourceAlpha, Blend.SourceAlphaSaturation, Color.Fuchsia);
            postProcessor.Process("Monochrome", startTexture, fullsizeTexture1);
            Assert.AreSame(fullsizeTexture1, postProcessor.OutputTexture);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestProcessUnknownTexture()
        {
            postProcessor.Process("Monochrome", startTexture, startTexture);
        }

        [Test]
        public void TestDownSample()
        {
            TestInitializeNoAnnotations();
            postProcessor.StartFrame(startTexture);
            CreateFullsize(new IRenderTarget2D[] { fullsizeTexture1, fullsizeTexture2 });

            SetupPostEffect(startTexture, fullsizeTexture1, "DownSample4x", 400, 200, 0.5f, false);
            SetupBlend(BlendFunction.ReverseSubtract, Blend.DestinationColor, Blend.BothInverseSourceAlpha, Color.DodgerBlue);
            postProcessor.Process("DownSample4x", startTexture, fullsizeTexture1);
            Assert.AreSame(fullsizeTexture1, postProcessor.OutputTexture);

            SetupPostEffect(fullsizeTexture1, fullsizeTexture2, "DownSample4x", 200, 100, 0.5f, false);
            SetupBlend(BlendFunction.Add, Blend.One, Blend.Zero, Color.DodgerBlue);
            postProcessor.Process("DownSample4x", fullsizeTexture1, fullsizeTexture2);
            Assert.AreSame(fullsizeTexture2, postProcessor.OutputTexture);

            postProcessor.StartFrame(startTexture);
            SetupPostEffect(startTexture, fullsizeTexture1, "DownSample4x", 400, 200, 0.5f, false);
            SetupBlend(BlendFunction.Subtract, Blend.DestinationColor, Blend.BothInverseSourceAlpha, Color.DimGray);
            postProcessor.Process("DownSample4x", startTexture, fullsizeTexture1);
        }

        [Test]
        public void TestUpSample()
        {
            TestDownSample();
            mockery.VerifyAllExpectationsHaveBeenMet();

            SetupPostEffect(fullsizeTexture1, fullsizeTexture2, "UpSample4x", 200, 100, 2.0f, false);
            SetupBlend(BlendFunction.Subtract, Blend.DestinationColor, Blend.BothInverseSourceAlpha, Color.DimGray);
            postProcessor.Process("UpSample4x", fullsizeTexture1, fullsizeTexture2);
            Assert.AreEqual(fullsizeTexture2, postProcessor.OutputTexture);

            SetupPostEffect(fullsizeTexture2, fullsizeTexture1, "Monochrome", 400, 200, 1.0f, false);
            SetupBlend(BlendFunction.ReverseSubtract, Blend.DestinationColor, Blend.BothInverseSourceAlpha, Color.DimGray);
            postProcessor.Process("Monochrome", fullsizeTexture2, fullsizeTexture1);
            Assert.AreEqual(fullsizeTexture1, postProcessor.OutputTexture);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestUpSampleFail()
        {
            TestInitializeNoAnnotations();
            postProcessor.StartFrame(startTexture);
            CreateFullsize(new IRenderTarget2D[] { fullsizeTexture1 });
            SetupPostEffect(startTexture, fullsizeTexture1, "UpSample4x", 400, 200, 2.0f, true);
            SetupBlend(BlendFunction.ReverseSubtract, Blend.DestinationColor, Blend.BothInverseSourceAlpha, Color.DimGray);
            verifyExpectations = false;
            postProcessor.Process("UpSample4x", startTexture, fullsizeTexture1);
        }

        private void CreateFullsize(IRenderTarget2D[] textures)
        {
            foreach (IRenderTarget2D texture in textures)
                Expect.Once.On(textureFactory).Method("CreateFullsizeRenderTarget").
                    Will(Return.Value(texture));
            postProcessor.GetTemporaryTextures(textures.Length, false);
        }

        public void InitializeSetup()
        {
            effect = mockery.NewMock<IEffect>();
            Expect.Once.On(effectFactory).Method("CreateFromFile").
                With("Content\\effects\\PostEffects").Will(Return.Value(effect));
            Expect.Once.On(graphicsFactory).Method("CreateSpriteBatch").Will(Return.Value(spriteBatch));
        }

        private void SetupBlend(BlendFunction operation, Blend source, Blend destination, Color factor)
        {
            if (BlendFunction.Add == operation &&
                Blend.One == source &&
                Blend.Zero == destination)
            {
                Expect.Once.On(renderState).
                    SetProperty("AlphaBlendEnable").
                    To(false);
            }
            else
            {
                Expect.Once.On(renderState).
                    SetProperty("AlphaBlendEnable").
                    To(true);
                Expect.Once.On(renderState).
                    SetProperty("BlendFunction").
                    To(operation);
                Expect.Once.On(renderState).
                    SetProperty("SourceBlend").
                    To(source);
                Expect.Once.On(renderState).
                    SetProperty("DestinationBlend").
                    To(destination);
                Expect.Once.On(renderState).
                    SetProperty("BlendFactor").
                    To(factor);
            }
            postProcessor.SetBlendParameters(operation, source, destination, factor);
        }

        private void SetupPostEffect(IRenderTarget2D startTexture, IRenderTarget2D destTexture, string techniqueName, int width, int height, float scale, bool clear)
        {
            IEffectTechnique technique = mockery.NewMock<IEffectTechnique>();
            IEffectPass pass = mockery.NewMock<IEffectPass>();
            IEffectAnnotation annotation = mockery.NewMock<IEffectAnnotation>();
            Expect.Once.On(effect).GetProperty("CurrentTechnique").Will(Return.Value(technique));
            Expect.Once.On(device).Method("SetRenderTarget").
                With(0, destTexture);
            ExpectGetTechnique(techniqueName, technique);
            Expect.Once.On(effect).SetProperty("CurrentTechnique").To(technique);
            Expect.Once.On(spriteBatch).Method("Begin").
                With(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            Expect.Once.On(effect).Method("Begin");
            ExpectForeachPass(technique, new IEffectPass[] { pass });
            Expect.Once.On(pass).Method("Begin");
            ExpectGetAnnotation(pass, "Scale", annotation);
            Expect.Once.On(annotation).Method("GetValueSingle").Will(Return.Value(scale));
            if (clear)
            {
                Expect.Once.On(device).
                    Method("Clear").With(ClearOptions.Target, Color.Black, 0, 0);
            }
            Expect.Once.On(startTexture).Method("GetTexture").Will(Return.Value(texture));
            Expect.Once.On(spriteBatch).Method("Draw").With(texture,
                new Rectangle(0, 0, (int)(width * scale), (int)(height * scale)),
                new Rectangle(0, 0, width, height),
                Color.White);
            Expect.Once.On(spriteBatch).Method("End");
            Expect.Once.On(pass).Method("End");
            Expect.Once.On(effect).Method("End");
            Expect.Once.On(device).Method("ResolveRenderTarget").With(0);
        }

        private void ExpectForeachParameters(IEffectParameter[] parameters)
        {
            ICollectionAdapter<IEffectParameter> collection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            IEnumerator<IEffectParameter> enumerator = mockery.NewMock<IEnumerator<IEffectParameter>>();
            Expect.Once.On(effect).GetProperty("Parameters").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("GetEnumerator").
                Will(Return.Value(enumerator));
            foreach (IEffectParameter parameter in parameters)
            {
                Expect.Once.On(enumerator).Method("MoveNext").Will(Return.Value(true));
                Expect.Once.On(enumerator).GetProperty("Current").Will(Return.Value(parameter));
            }
            Expect.Once.On(enumerator).Method("MoveNext").Will(Return.Value(false));
            Expect.Once.On(enumerator).Method("Dispose");
        }

        private void ExpectForeachPass(IEffectTechnique technique, IEffectPass[] passes)
        {
            ICollectionAdapter<IEffectPass> collection = mockery.NewMock<ICollectionAdapter<IEffectPass>>();
            IEnumerator<IEffectPass> enumerator = mockery.NewMock<IEnumerator<IEffectPass>>();
            Expect.Once.On(technique).GetProperty("Passes").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("GetEnumerator").
                Will(Return.Value(enumerator));
            foreach (IEffectPass pass in passes)
            {
                Expect.Once.On(enumerator).Method("MoveNext").Will(Return.Value(true));
                Expect.Once.On(enumerator).GetProperty("Current").Will(Return.Value(pass));
            }
            Expect.Once.On(enumerator).Method("Dispose");
            Expect.Once.On(enumerator).Method("MoveNext").Will(Return.Value(false));
        }

        private void ExpectGetAnnotation(IEffectParameter parameter, string name, IEffectAnnotation annotation)
        {
            ICollectionAdapter<IEffectAnnotation> collection = mockery.NewMock<ICollectionAdapter<IEffectAnnotation>>();
            Expect.Once.On(parameter).GetProperty("Annotations").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("get_Item").With(name).Will(Return.Value(annotation));
        }

        private void ExpectGetAnnotation(IEffectPass pass, string name, IEffectAnnotation annotation)
        {
            ICollectionAdapter<IEffectAnnotation> collection = mockery.NewMock<ICollectionAdapter<IEffectAnnotation>>();
            Expect.Once.On(pass).GetProperty("Annotations").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("get_Item").With(name).Will(Return.Value(annotation));
        }

        private void ExpectGetParameter(string name, IEffectParameter parameter)
        {
            ICollectionAdapter<IEffectParameter> collection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            Expect.Once.On(effect).GetProperty("Parameters").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("get_Item").With(name).Will(Return.Value(parameter));
        }

        private void ExpectElementCount(IEffectParameter parameter, int num)
        {
            ICollectionAdapter<IEffectParameter> collection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            Expect.Once.On(parameter).GetProperty("Elements").
                Will(Return.Value(collection));
            Expect.Once.On(collection).GetProperty("Count").Will(Return.Value(num));
        }

        private void ExpectGetTechnique(string techniqueName, IEffectTechnique technique)
        {
            ICollectionAdapter<IEffectTechnique> collection = mockery.NewMock<ICollectionAdapter<IEffectTechnique>>();
            Expect.Once.On(effect).GetProperty("Techniques").
                Will(Return.Value(collection));
            Expect.Once.On(collection).Method("get_Item").With(techniqueName).Will(Return.Value(technique));
        }

    }
}
