using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class PostProcessorTest : D3DMockTest
    {
        private PostProcessor postProcessor;
        private ITexture startTexture;
        private ITexture fullsizeTexture1;
        private PresentParameters presentParameters;

        private IEffect effect;
        private EffectHandle monochromeHandle;
        private EffectHandle sourceTextureParameter;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            startTexture = mockery.NewMock<ITexture>();
            fullsizeTexture1 = mockery.NewMock<ITexture>();

            presentParameters = new PresentParameters();
            presentParameters.BackBufferFormat = Format.R32F;
            presentParameters.BackBufferHeight = 200;
            presentParameters.BackBufferWidth = 201;
            D3DDriver.TextureFactory = new TextureFactory(device, factory, presentParameters);
            D3DDriver.EffectFactory = new EffectFactory(device, factory);
            postProcessor = new PostProcessor();

            monochromeHandle = EffectHandle.FromString("1");
            sourceTextureParameter = EffectHandle.FromString("2");
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitializeOK()
        {
            effect = mockery.NewMock<IEffect>();
            Expect.Once.On(factory).
                Method("EffectFromFile").
                With(Is.EqualTo(device), Is.EqualTo("../../../Effects/PostEffects.fxo"), Is.Null, Is.EqualTo(""), Is.EqualTo(ShaderFlags.None), Is.Anything).
                Will(Return.Value(effect));
            Expect.Once.On(effect).
                Method("GetTechnique").
                With("Monochrome").
                Will(Return.Value(monochromeHandle));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "SourceTexture").
                Will(Return.Value(sourceTextureParameter));
            Expect.Exactly(2).On(factory).
                Method("CreateTexture").
                With(device, presentParameters.BackBufferWidth, presentParameters.BackBufferHeight, 1, Usage.RenderTarget, presentParameters.BackBufferFormat, Pool.Default).
                Will(Return.Value(fullsizeTexture1));
            postProcessor.Initialize(device);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInitializeFail()
        {
            effect = mockery.NewMock<IEffect>();
            Expect.Once.On(factory).
                Method("EffectFromFile").
                With(Is.EqualTo(device), Is.EqualTo("../../../Effects/PostEffects.fxo"), Is.Null, Is.EqualTo(""), Is.EqualTo(ShaderFlags.None), Is.Anything).
                Will(Return.Value(effect));
            Expect.Once.On(effect).
                Method("GetTechnique").
                With("Monochrome").
                Will(Return.Value(null));
            postProcessor.Initialize(device);
        }

        [Test]
        public void TestNoPostEffect()
        {
            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            Assert.AreSame(startTexture, postProcessor.OutputTexture);
            Assert.AreEqual(PostProcessor.TextureID.INPUT_TEXTURE, postProcessor.OutputTextureID);
        }

        [Test]
        public void TestMonocromeEffect()
        {
            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            Expect.Once.On(fullsizeTexture1).
                Method("GetSurfaceLevel").
                With(0).
                Will(Return.Value(surface));
            Expect.Once.On(device).
                Method("SetRenderTarget").
                With(0, surface);
            Expect.Once.On(effect).
                Method("SetValue").
                With(sourceTextureParameter, startTexture);
            Expect.Once.On(effect).
                SetProperty("Technique").
                To(monochromeHandle);
            Expect.Once.On(device).
                SetProperty("VertexFormat").
                To(CustomVertex.TransformedTextured.Format);
            Expect.Once.On(device).
                Method("BeginScene");
            Expect.Once.On(effect).
                Method("Begin").
                With(FX.None).
                Will(Return.Value(1));
            Expect.Once.On(effect).
                Method("BeginPass").With(0);
            Expect.Once.On(device).
                Method("DrawUserPrimitives").
                WithAnyArguments();
            Expect.Once.On(effect).
                Method("EndPass");
            Expect.Once.On(effect).
                Method("End");
            Expect.Once.On(device).
                Method("EndScene");
            postProcessor.Monochrome(PostProcessor.TextureID.INPUT_TEXTURE, PostProcessor.TextureID.FULLSIZE_TEXTURE_1);
            Assert.AreSame(fullsizeTexture1, postProcessor.OutputTexture);
            Assert.AreEqual(PostProcessor.TextureID.FULLSIZE_TEXTURE_1, postProcessor.OutputTextureID);
        }
    }
}
