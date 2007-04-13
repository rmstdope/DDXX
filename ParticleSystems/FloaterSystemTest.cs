using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.SceneGraph;

namespace Dope.DDXX.ParticleSystems
{
    [TestFixture]
    public class FloaterSystemTest : D3DMockTest
    {
        private FloaterSystem system;
        private IVertexBuffer vertexBuffer;
        private IGraphicsStream graphicsStream;
        private EffectHandle technique;
        private IEffectHandler effectHandler;
        private IScene scene;
        private IRenderableCamera camera;
        private Matrix worldMatrix = Matrix.Identity;
        private Matrix viewMatrix = Matrix.RotationX(4.0f);
        private Matrix projectionMatrix = Matrix.RotationY(1.3f);
        private ColorValue sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f);

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            SetupTime();
            
            system = new FloaterSystem("FS");
            
            vertexBuffer = mockery.NewMock<IVertexBuffer>();
            graphicsStream = mockery.NewMock<IGraphicsStream>();
            effect = mockery.NewMock<IEffect>();
            technique = EffectHandle.FromString("1");
            effectHandler = mockery.NewMock<IEffectHandler>();
            scene = mockery.NewMock<IScene>();
            camera = mockery.NewMock<IRenderableCamera>();

            Stub.On(scene).GetProperty("ActiveCamera").Will(Return.Value(camera));
            Stub.On(camera).GetProperty("ViewMatrix").Will(Return.Value(viewMatrix));
            Stub.On(camera).GetProperty("ProjectionMatrix").Will(Return.Value(projectionMatrix));
            Stub.On(scene).GetProperty("AmbientColor").Will(Return.Value(sceneAmbient));
            Stub.On(graphicsStream).Method("Dispose");
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize1()
        {
            ExpectEffect();
            ExpectVertexBuffer();
            system.Initialize(100, 50.0f, null);
            system.EffectHandler = effectHandler;
            Assert.AreEqual(50.0f, system.BoundaryRadius);
            Assert.AreEqual(100, system.NumParticles);
        }

        [Test]
        public void TestInitialize2()
        {
            ExpectEffect();
            ExpectVertexBuffer();
            Expect.Once.On(textureFactory).
                Method("CreateFromFile").
                WithAnyArguments().
                Will(Return.Value(texture));
            system.Initialize(100, 50.0f, "Texture");
            system.EffectHandler = effectHandler;
            Assert.AreEqual(50.0f, system.BoundaryRadius);
            Assert.AreEqual(100, system.NumParticles);
        }

        [Test]
        public void TestStep()
        {
            TestInitialize1();

            Expect.Once.On(vertexBuffer).
                Method("Lock").
                With(0, 0, LockFlags.Discard).
                Will(Return.Value(graphicsStream));
            Expect.Exactly(100).On(graphicsStream).
                Method("Write").
                With(new CheckVertex(500.0f, 10.0f));
            Expect.Once.On(vertexBuffer).
                Method("Unlock");
            system.Step();
        }

        [Test]
        public void TestRender()
        {
            TestInitialize1();

            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));

            using (mockery.Ordered)
            {
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With(worldMatrix, viewMatrix, projectionMatrix);
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").WithAnyArguments();
                
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);

                Expect.Once.On(renderStateManager).SetProperty("AlphaBlendEnable").To(true);
                Expect.Once.On(renderStateManager).SetProperty("BlendOperation").To(BlendOperation.Add);
                Expect.Once.On(renderStateManager).SetProperty("SourceBlend").To(Blend.One);
                Expect.Once.On(renderStateManager).SetProperty("DestinationBlend").To(Blend.One);
                Expect.Once.On(device).Method("SetStreamSource").With(0, vertexBuffer, 0);
                Expect.Once.On(device).SetProperty("VertexDeclaration").To(Is.Null);
                Expect.Once.On(device).Method("DrawPrimitives").With(PrimitiveType.PointList, 0, 100);

                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
            }

            system.Render(scene);
        }

        private void ExpectVertexBuffer()
        {
            Expect.Once.On(factory).
                Method("CreateVertexBuffer").
                With(Is.Anything, Is.EqualTo(100), Is.Anything, Is.EqualTo(Usage.WriteOnly | Usage.Dynamic), Is.EqualTo(VertexFormats.None), Is.EqualTo(Pool.Default)).
                Will(Return.Value(vertexBuffer));
            Expect.Once.On(factory).
                Method("CreateVertexDeclaration").
                WithAnyArguments().
                Will(Return.Value(null));
        }

        private void ExpectEffect()
        {
            Expect.Once.On(effectFactory).
                Method("CreateFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            Stub.On(effect).
                Method("GetParameter").
                WithAnyArguments().
                Will(Return.Value(EffectHandle.FromString("1")));
        }

        private class CheckVertex : Matcher
        {
            private float radius;
            private float maxSize;
            public CheckVertex(float radius, float maxSize)
            {
                this.radius = radius;
                this.maxSize = maxSize;
            }

            public override void DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write("Expecting a radius of " + radius);
            }

            public override bool Matches(object o)
            {
                if (!(o is VertexColorPoint))
                    return false;
                VertexColorPoint v = (VertexColorPoint)o;
                if (v.Position.Length() > radius)
                    return false;
                if (v.Size > maxSize)
                    return false;
                return true;
            }
        }

    }
}
