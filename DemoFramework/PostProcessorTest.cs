using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX;
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

        private IEffect effect;
        private EffectHandle sourceTextureParameter;
        private EffectHandle passHandle;
        private EffectHandle scaleHandle;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            startTexture = mockery.NewMock<ITexture>();
            fullsizeTexture1 = mockery.NewMock<ITexture>();

            D3DDriver.TextureFactory = new TextureFactory(device, factory, presentParameters);
            D3DDriver.EffectFactory = new EffectFactory(device, factory);
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
            effect = mockery.NewMock<IEffect>();
            Expect.Once.On(factory).
                Method("EffectFromFile").
                With(Is.EqualTo(device), Is.EqualTo("../../../Effects/PostEffects.fxo"), Is.Null, Is.EqualTo(""), Is.EqualTo(ShaderFlags.None), Is.Anything).
                Will(Return.Value(effect));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "SourceTexture").
                Will(Return.Value(sourceTextureParameter));
            Stub.On(factory).
                Method("CreateTexture").
                With(device, presentParameters.BackBufferWidth, presentParameters.BackBufferHeight, 1, Usage.RenderTarget, presentParameters.BackBufferFormat, Pool.Default).
                Will(Return.Value(fullsizeTexture1));
            postProcessor.Initialize(device);
        }

        [Test]
        public void TestNoPostEffect()
        {
            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            Assert.AreSame(startTexture, postProcessor.OutputTexture);
            Assert.AreEqual(TextureID.INPUT_TEXTURE, postProcessor.OutputTextureID);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInvalidSource()
        {
            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            Assert.AreSame(startTexture, postProcessor.OutputTexture);
            postProcessor.Process("Monochrome", TextureID.FULLSIZE_TEXTURE_1, TextureID.INPUT_TEXTURE);
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
            SetupPostEffect(startTexture, "Monochrome", vertices, 1.0f);
            postProcessor.Process("Monochrome", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
            Assert.AreSame(fullsizeTexture1, postProcessor.OutputTexture);
            Assert.AreEqual(TextureID.FULLSIZE_TEXTURE_1, postProcessor.OutputTextureID);
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
            SetupPostEffect(startTexture, "DownSample4x", vertices, 0.5f);
            postProcessor.Process("DownSample4x", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
            Assert.AreSame(fullsizeTexture1, postProcessor.OutputTexture);
            Assert.AreEqual(TextureID.FULLSIZE_TEXTURE_1, postProcessor.OutputTextureID);

            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(100 - 0.5f, -0.5f, 1.0f, 1.0f), 0.5f, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 50 - 0.5f, 1.0f, 1.0f), 0, 0.5f);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(100 - 0.5f, 50 - 0.5f, 1.0f, 1.0f), 0.5f, 0.5f);
            SetupPostEffect(fullsizeTexture1, "DownSample4x", vertices, 0.5f);
            postProcessor.Process("DownSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);

            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(200 - 0.5f, -0.5f, 1.0f, 1.0f), 1, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 100 - 0.5f, 1.0f, 1.0f), 0, 1);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(200 - 0.5f, 100 - 0.5f, 1.0f, 1.0f), 1, 1);
            postProcessor.StartFrame(startTexture);
            SetupPostEffect(startTexture, "DownSample4x", vertices, 0.5f);
            postProcessor.Process("DownSample4x", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
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
            SetupPostEffect(fullsizeTexture1, "UpSample4x", vertices, 2.0f);
            postProcessor.Process("UpSample4x", TextureID.FULLSIZE_TEXTURE_1, TextureID.FULLSIZE_TEXTURE_2);
            Assert.AreEqual(TextureID.FULLSIZE_TEXTURE_2, postProcessor.OutputTextureID);

            vertices[0] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, -0.5f, 1.0f, 1.0f), 0, 0);
            vertices[1] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, -0.5f, 1.0f, 1.0f), 1, 0);
            vertices[2] = new CustomVertex.TransformedTextured(new Vector4(-0.5f, 200 - 0.5f, 1.0f, 1.0f), 0, 1);
            vertices[3] = new CustomVertex.TransformedTextured(new Vector4(400 - 0.5f, 200 - 0.5f, 1.0f, 1.0f), 1, 1);
            SetupPostEffect(fullsizeTexture1, "Monochrome", vertices, 1.0f);
            postProcessor.Process("Monochrome", TextureID.FULLSIZE_TEXTURE_2, TextureID.FULLSIZE_TEXTURE_1);
            Assert.AreEqual(TextureID.FULLSIZE_TEXTURE_1, postProcessor.OutputTextureID);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestUpSampleFail()
        {
            CustomVertex.TransformedTextured[] vertices = new CustomVertex.TransformedTextured[4];

            TestInitializeOK();
            postProcessor.StartFrame(startTexture);
            SetupPostEffect(startTexture, "UpSample4x", vertices, 2.0f);
            postProcessor.Process("UpSample4x", TextureID.INPUT_TEXTURE, TextureID.FULLSIZE_TEXTURE_1);
        }

        private void SetupPostEffect(ITexture startTexture, string technique, CustomVertex.TransformedTextured[] vertices, float scale)
        {
            SetupStub(technique, scale);

            SetupExpect(startTexture, technique, vertices);
        }

        private void SetupExpect(ITexture startTexture, string technique, CustomVertex.TransformedTextured[] vertices)
        {
            using (mockery.Ordered)
            {
                Expect.Once.On(device).
                    Method("SetRenderTarget").
                    With(0, surface);
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(sourceTextureParameter, startTexture);
                Expect.Once.On(effect).
                    SetProperty("Technique").
                    To(Is.Anything);//(EffectHandle)technique);
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
                    With(Is.EqualTo(PrimitiveType.TriangleStrip), Is.EqualTo(2), new VertexMatcher(vertices));
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
                With(Is.Anything /*technique*/, Is.EqualTo(0)).
                Will(Return.Value(passHandle));
            Stub.On(effect).
                Method("GetAnnotation").
                With(passHandle, "Scale").
                Will(Return.Value(scaleHandle));
            Stub.On(fullsizeTexture1).
                Method("GetSurfaceLevel").
                With(0).
                Will(Return.Value(surface));
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
