using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using System.Drawing;
using NUnit.Framework.SyntaxHelpers;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class PostProcessorTest : D3DMockTest
    {
        private IPostProcessor postProcessor;
        private ITexture startTexture;
        private ITexture fullsizeTexture1;
        private ITexture fullsizeTexture2;
        private ITexture fullsizeTexture3;

        private EffectHandle sourceTextureParameter;
        private EffectHandle passHandle;
        private EffectHandle scaleHandle;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            startTexture = mockery.NewMock<ITexture>();
            fullsizeTexture1 = mockery.NewMock<ITexture>();
            fullsizeTexture2 = mockery.NewMock<ITexture>();
            fullsizeTexture3 = mockery.NewMock<ITexture>();

            postProcessor = new PostProcessor();

            sourceTextureParameter = EffectHandle.FromString("A");
            passHandle = EffectHandle.FromString("a");
            scaleHandle = EffectHandle.FromString("b");
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitializeOK()
        {
            InitializeSetup();
            Stub.On(effect).
                GetProperty("Description_Parameters").
                Will(Return.Value(0));
            postProcessor.Initialize(device, textureFactory, effectFactory);
        }

        [Test]
        public void TestOneTemporaryTexture()
        {
            TestInitializeOK();
            List<ITexture> textures = new List<ITexture>();
            textures.Add(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));

            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(1, false));
        }

        [Test]
        public void TestTwoTemporaryTextures()
        {
            TestInitializeOK();
            List<ITexture> textures = new List<ITexture>();
            textures.Add(fullsizeTexture1);
            textures.Add(fullsizeTexture2);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture2));

            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(2, false));
        }

        [Test]
        public void TestTemporaryTexturesMultipleCalls()
        {
            TestInitializeOK();
            List<ITexture> textures = new List<ITexture>();
            textures.Add(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(1, false));

            textures.Add(fullsizeTexture2);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture2));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(2, false));
        }

        [Test]
        public void TestOneTemporaryTextureSameAsOutput()
        {
            TestInitializeOK();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            postProcessor.GetTemporaryTextures(1, false);
            postProcessor.StartFrame(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture2));
            Assert.AreSame(fullsizeTexture2, postProcessor.GetTemporaryTextures(1, false)[0]);
        }

        [Test]
        public void TestTwoTemporaryTexturesFirstSameAsOutput()
        {
            TestInitializeOK();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture2));
            postProcessor.GetTemporaryTextures(2, false);
            postProcessor.StartFrame(fullsizeTexture1);
            List<ITexture> textures = postProcessor.GetTemporaryTextures(2, false);
            Assert.AreSame(fullsizeTexture2, textures[0]);
            Assert.AreSame(fullsizeTexture1, textures[1]);
        }

        [Test]
        public void TestTwoTemporaryTexturesSkipOutput()
        {
            TestInitializeOK();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture2));
            postProcessor.GetTemporaryTextures(2, false);
            postProcessor.StartFrame(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture3));
            List<ITexture> textures = postProcessor.GetTemporaryTextures(2, true);
            Assert.AreSame(fullsizeTexture2, textures[0]);
            Assert.AreSame(fullsizeTexture3, textures[1]);
        }

        [Test]
        public void TestAllocateTexture()
        {
            TestInitializeOK();
            List<ITexture> textures = new List<ITexture>();
            textures.Add(fullsizeTexture1);
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(1, false));

            postProcessor.AllocateTexture(fullsizeTexture1);

            textures.Clear();
            textures.Add(fullsizeTexture2);
            textures.Add(fullsizeTexture2);
            Expect.Exactly(2).On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture2));
            Assert.AreEqual(textures, postProcessor.GetTemporaryTextures(2, false));
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAllocateTextureTwice()
        {
            TestInitializeOK();
            Expect.Once.On(textureFactory).
                Method("CreateFullsizeRenderTarget").
                With(Format.A8R8G8B8).
                Will(Return.Value(fullsizeTexture1));
            postProcessor.GetTemporaryTextures(1, false);
            postProcessor.AllocateTexture(fullsizeTexture1);
            postProcessor.AllocateTexture(fullsizeTexture1);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestAllocateUnknownTexture()
        {
            TestInitializeOK();
            postProcessor.AllocateTexture(fullsizeTexture1);
        }

        [Test]
        public void TestAnnotations()
        {
            EffectHandle toParameter1 = EffectHandle.FromString("1");
            EffectHandle toParameter2 = EffectHandle.FromString("2");
            EffectHandle toParameter3 = EffectHandle.FromString("3");
            EffectHandle annotation1 = EffectHandle.FromString("11");
            EffectHandle annotation3 = EffectHandle.FromString("33");
            EffectHandle fromParameter1 = EffectHandle.FromString("111");
            EffectHandle fromParameter3 = EffectHandle.FromString("333");
            EffectDescription effectDescription = new EffectDescription();
            ParameterDescription pDescription1 = new ParameterDescription();
            ParameterDescription pDescription2 = new ParameterDescription();

            InitializeSetup();

            Stub.On(effect).
                GetProperty("Description_Parameters").
                Will(Return.Value(3));
            Stub.On(effect).
                Method("GetParameter").
                With(null, 0).
                Will(Return.Value(toParameter1));
            Stub.On(effect).
                Method("GetParameter").
                With(null, 1).
                Will(Return.Value(toParameter2));
            Stub.On(effect).
                Method("GetParameter").
                With(null, 2).
                Will(Return.Value(toParameter3));
            Stub.On(effect).
                Method("GetAnnotation").
                With(toParameter1, "ConvertPixelsToTexels").
                Will(Return.Value(annotation1));
            Stub.On(effect).
                Method("GetAnnotation").
                With(toParameter2, "ConvertPixelsToTexels").
                Will(Return.Value(null));
            Stub.On(effect).
                Method("GetAnnotation").
                With(toParameter3, "ConvertPixelsToTexels").
                Will(Return.Value(annotation3));
            Stub.On(effect).
                Method("GetValueString").
                With(annotation1).
                Will(Return.Value("Convert1"));
            Stub.On(effect).
                Method("GetValueString").
                With(annotation3).
                Will(Return.Value("Convert3"));
            Stub.On(effect).
                Method("GetParameter").
                With(null, "Convert1").
                Will(Return.Value(fromParameter1));
            Stub.On(effect).
                Method("GetParameter").
                With(null, "Convert3").
                Will(Return.Value(fromParameter3));
            Stub.On(effect).
                Method("GetParameterDescription_Elements").
                With(fromParameter1).
                Will(Return.Value(1));
            Stub.On(effect).
                Method("GetParameterDescription_Elements").
                With(fromParameter3).
                Will(Return.Value(2));
            Expect.Once.On(effect).
                Method("GetValueFloatArray").
                With(fromParameter1, 2).
                Will(Return.Value(new float[] { 1, 2 }));
            Expect.Once.On(effect).
                Method("GetValueFloatArray").
                With(fromParameter3, 4).
                Will(Return.Value(new float[] { 3, 4, 5, 6 }));
            Expect.Once.On(effect).
                Method("SetValue").
                With(toParameter1, new float[] { 1.0f / device.PresentationParameters.BackBufferWidth, 
                                                 2.0f / device.PresentationParameters.BackBufferHeight });
            Expect.Once.On(effect).
                Method("SetValue").
                With(toParameter3, new float[] { 3.0f / device.PresentationParameters.BackBufferWidth, 
                                                 4.0f / device.PresentationParameters.BackBufferHeight,
                                                 5.0f / device.PresentationParameters.BackBufferWidth, 
                                                 6.0f / device.PresentationParameters.BackBufferHeight});

            postProcessor.Initialize(device, textureFactory, effectFactory);
        }

        [Test]
        public void TestNoPostEffect()
        {
            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            Assert.AreSame(startTexture, postProcessor.OutputTexture);
        }

        [Test]
        public void TestProcess()
        {
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, -0.5f, 1.0f, 1.0f), 1, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 200 - 0.5f, 1.0f, 1.0f), 0, 1);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, 200 - 0.5f, 1.0f, 1.0f), 1, 1);

            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            CreateFullsize(new ITexture[] { fullsizeTexture1 });

            SetupPostEffect(startTexture, fullsizeTexture1, "Monochrome", vertices, 1.0f, false);
            SetupBlend(BlendOperation.Max, Blend.SourceAlpha, Blend.SourceAlphaSat, Color.Fuchsia);
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
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(200 - 0.5f, -0.5f, 1.0f, 1.0f), 1, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 100 - 0.5f, 1.0f, 1.0f), 0, 1);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(200 - 0.5f, 100 - 0.5f, 1.0f, 1.0f), 1, 1);

            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            CreateFullsize(new ITexture[] { fullsizeTexture1, fullsizeTexture2 });

            SetupPostEffect(startTexture, fullsizeTexture1, "DownSample4x", vertices, 0.5f, true);
            SetupBlend(BlendOperation.RevSubtract, Blend.DestinationColor, Blend.BothInvSourceAlpha, Color.DodgerBlue);
            postProcessor.Process("DownSample4x", startTexture, fullsizeTexture1);
            Assert.AreSame(fullsizeTexture1, postProcessor.OutputTexture);

            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(100 - 0.5f, -0.5f, 1.0f, 1.0f), 0.5f, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 50 - 0.5f, 1.0f, 1.0f), 0, 0.5f);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(100 - 0.5f, 50 - 0.5f, 1.0f, 1.0f), 0.5f, 0.5f);
            SetupPostEffect(fullsizeTexture1, fullsizeTexture2, "DownSample4x", vertices, 0.5f, true);
            SetupBlend(BlendOperation.Add, Blend.One, Blend.Zero, Color.DodgerBlue);
            postProcessor.Process("DownSample4x", fullsizeTexture1, fullsizeTexture2);
            Assert.AreSame(fullsizeTexture2, postProcessor.OutputTexture);

            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(200 - 0.5f, -0.5f, 1.0f, 1.0f), 1, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 100 - 0.5f, 1.0f, 1.0f), 0, 1);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(200 - 0.5f, 100 - 0.5f, 1.0f, 1.0f), 1, 1);
            postProcessor.StartFrame(startTexture);
            SetupPostEffect(startTexture, fullsizeTexture1, "DownSample4x", vertices, 0.5f, false);
            SetupBlend(BlendOperation.Subtract, Blend.DestinationColor, Blend.BothInvSourceAlpha, Color.DimGray);
            postProcessor.Process("DownSample4x", startTexture, fullsizeTexture1);
        }

        [Test]
        public void TestUpSample()
        {
            TestDownSample();
            mockery.VerifyAllExpectationsHaveBeenMet();

            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];
            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, -0.5f, 1.0f, 1.0f), 0.5f, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 200 - 0.5f, 1.0f, 1.0f), 0, 0.5f);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, 200 - 0.5f, 1.0f, 1.0f), 0.5f, 0.5f);
            SetupPostEffect(fullsizeTexture1, fullsizeTexture2, "UpSample4x", vertices, 2.0f, false);
            SetupBlend(BlendOperation.Subtract, Blend.DestinationColor, Blend.BothInvSourceAlpha, Color.DimGray);
            postProcessor.Process("UpSample4x", fullsizeTexture1, fullsizeTexture2);
            Assert.AreEqual(fullsizeTexture2, postProcessor.OutputTexture);

            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, -0.5f, 1.0f, 1.0f), 1, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 200 - 0.5f, 1.0f, 1.0f), 0, 1);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, 200 - 0.5f, 1.0f, 1.0f), 1, 1);
            SetupPostEffect(fullsizeTexture2, fullsizeTexture1, "Monochrome", vertices, 1.0f, false);
            SetupBlend(BlendOperation.RevSubtract, Blend.DestinationColor, Blend.BothInvSourceAlpha, Color.DimGray);
            postProcessor.Process("Monochrome", fullsizeTexture2, fullsizeTexture1);
            Assert.AreEqual(fullsizeTexture1, postProcessor.OutputTexture);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestUpSampleFail()
        {
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];

            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            CreateFullsize(new ITexture[] { fullsizeTexture1 });
            SetupPostEffect(startTexture, fullsizeTexture1, "UpSample4x", vertices, 2.0f, false);
            SetupBlend(BlendOperation.RevSubtract, Blend.DestinationColor, Blend.BothInvSourceAlpha, Color.DimGray);
            postProcessor.Process("UpSample4x", startTexture, fullsizeTexture1);
        }

        private void CreateFullsize(ITexture[] textures)
        {
            foreach (ITexture texture in textures)
                Expect.Once.On(textureFactory).Method("CreateFullsizeRenderTarget").
                    With(Format.A8R8G8B8).Will(Return.Value(texture));
            postProcessor.GetTemporaryTextures(textures.Length, false);
        }

        public void InitializeSetup()
        {
            effect = mockery.NewMock<IEffect>();
            Expect.Once.On(effectFactory).
                Method("CreateFromFile").
                With("PostEffects.fxo").
                Will(Return.Value(effect));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "SourceTexture").
                Will(Return.Value(sourceTextureParameter));
        }

        private void SetupBlend(BlendOperation operation, Blend source, Blend destination, Color factor)
        {
            if (BlendOperation.Add == operation &&
                Blend.One == source &&
                Blend.Zero == destination)
            {
                Expect.Once.On(renderStateManager).
                    SetProperty("AlphaBlendEnable").
                    To(false);
            }
            else
            {
                Expect.Once.On(renderStateManager).
                    SetProperty("AlphaBlendEnable").
                    To(true);
                Expect.Once.On(renderStateManager).
                    SetProperty("BlendOperation").
                    To(operation);
                Expect.Once.On(renderStateManager).
                    SetProperty("SourceBlend").
                    To(source);
                Expect.Once.On(renderStateManager).
                    SetProperty("DestinationBlend").
                    To(destination);
                Expect.Once.On(renderStateManager).
                    SetProperty("BlendFactor").
                    To(factor);
            }
            postProcessor.SetBlendParameters(operation, source, destination, factor);
        }

        private void SetupPostEffect(ITexture startTexture, ITexture destTexture, string technique, CustomVertex.TransformedTextured[] vertices, float scale, bool clear)
        {
            SetupStub(technique, scale);

            SetupExpect(startTexture, destTexture, technique, vertices, clear);
        }

        private void SetupExpect(ITexture startTexture, ITexture destTexture, string technique, CustomVertex.TransformedTextured[] vertices, bool clear)
        {
            using (mockery.Ordered)
            {
                Expect.Once.On(destTexture).
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
                    To(NMock2.Is.Anything);//(EffectHandle)technique);
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
                if (clear)
                {
                    Expect.Once.On(device).
                        Method("Clear");
                }
                Expect.Once.On(device).
                    Method("DrawUserPrimitives").
                    With(NMock2.Is.EqualTo(PrimitiveType.TriangleStrip), NMock2.Is.EqualTo(2), new VertexMatcher(vertices));
                Expect.Once.On(effect).
                    Method("EndPass");
                Expect.Once.On(effect).
                    Method("End");
                Expect.Once.On(device).
                    Method("EndScene");
            }
        }

        private void SetupStub(EffectHandle technique, float scale)
        {
            Stub.On(effect).
                Method("GetPass").
                With(NMock2.Is.Anything /*technique*/, NMock2.Is.EqualTo(0)).
                Will(Return.Value(passHandle));
            Stub.On(effect).
                Method("GetAnnotation").
                With(passHandle, "Scale").
                Will(Return.Value(scaleHandle));
            Expect.Once.On(effect).
                Method("GetValueFloat").
                With(scaleHandle).
                Will(Return.Value(scale));
        }

        private class VertexMatcher : Matcher
        {
            private CustomVertex.TransformedTextured[] vertices;

            public VertexMatcher(CustomVertex.TransformedTextured[] vertices)
            {
                this.vertices = vertices;
            }

            public override void DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write(vertices.ToString());
            }

            public override bool Matches(object o)
            {
                if (!(o is CustomVertex.TransformedTextured[]))
                    return false;
                CustomVertex.TransformedTextured[] cmpVertices = (CustomVertex.TransformedTextured[])o;
                if (vertices.Length != cmpVertices.Length)
                    return false;
                for (int i = 0; i < vertices.Length; i++)
                {
                    if (vertices[i].Position != cmpVertices[i].Position)
                        return false;
                    if (vertices[i].Tu != cmpVertices[i].Tu)
                        return false;
                    if (vertices[i].Tv != cmpVertices[i].Tv)
                        return false;
                }
                return true;
            }
        }
    }
}
