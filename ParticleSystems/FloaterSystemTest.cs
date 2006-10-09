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
        private FloaterSystem floaterSystem;
        private IVertexBuffer vertexBuffer;
        private IGraphicsStream graphicsStream;
        private IEffect effect;
        private EffectHandle technique;
        private IEffectHandler effectHandler;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            SetupD3DDriver();
            SetupTime();
            
            floaterSystem = new FloaterSystem("FS");
            
            vertexBuffer = mockery.NewMock<IVertexBuffer>();
            graphicsStream = mockery.NewMock<IGraphicsStream>();
            effect = mockery.NewMock<IEffect>();
            technique = EffectHandle.FromString("1");
            effectHandler = mockery.NewMock<IEffectHandler>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void TestInitialize()
        {
            // Effect creation
            Expect.Once.On(factory).
                Method("EffectFromFile").
                WithAnyArguments().
                Will(Return.Value(effect));
            // EffectHandler
            Expect.Once.On(effect).
                Method("FindNextValidTechnique").
                WithAnyArguments().
                Will(Return.Value(technique));
            Stub.On(effect).
                Method("GetParameter").
                WithAnyArguments().
                Will(Return.Value(EffectHandle.FromString("1")));
            // Create
            Expect.Once.On(factory).
                Method("CreateVertexBuffer").
                With(Is.Anything, Is.EqualTo(100), Is.Anything, Is.EqualTo(Usage.WriteOnly | Usage.Dynamic), Is.EqualTo(VertexFormats.None), Is.EqualTo(Pool.Default)).
                Will(Return.Value(vertexBuffer));
            //Expect.Once.On(vertexBuffer).
            //    Method("Lock").
            //    With(0, 0, LockFlags.Discard).
            //    Will(Return.Value(graphicsStream));
            //Expect.Exactly(100).On(graphicsStream).
            //    Method("Write").
            //    With(new CheckVertex(50.0f, 1.0f));
            //Expect.Once.On(vertexBuffer).
            //    Method("Unlock");
            // Create declaration
            Expect.Once.On(factory).
                Method("CreateVertexDeclaration").
                WithAnyArguments().
                Will(Return.Value(null));
            floaterSystem.Initialize(100, 50.0f);
            floaterSystem.EffectHandler = effectHandler;
            Assert.AreEqual(50.0f, floaterSystem.BoundaryRadius);
            Assert.AreEqual(100, floaterSystem.NumParticles);
        }

        [Test]
        public void TestStep()
        {
            TestInitialize();

            Expect.Once.On(vertexBuffer).
                Method("Lock").
                With(0, 0, LockFlags.Discard).
                Will(Return.Value(graphicsStream));
            Expect.Exactly(100).On(graphicsStream).
                Method("Write").
                With(new CheckVertex(500.0f, 10.0f));
            Expect.Once.On(vertexBuffer).
                Method("Unlock");
            floaterSystem.Step();
        }

        [Test]
        public void TestRender()
        {
            TestInitialize();

            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));

            using (mockery.Ordered)
            {
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With(null, floaterSystem);
                
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);

                Expect.Once.On(device).Method("SetStreamSource").With(0, vertexBuffer, 0);
                Expect.Once.On(device).SetProperty("VertexDeclaration").To(Is.Null);
                Expect.Once.On(device).Method("DrawPrimitives").With(PrimitiveType.PointList, 0, 100);

                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
            }

            floaterSystem.Render(null);
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
                if (!(o is FloaterSystem.FloaterVertex))
                    return false;
                FloaterSystem.FloaterVertex v = (FloaterSystem.FloaterVertex)o;
                if (v.Position.Length() > radius)
                    return false;
                if (v.Size > maxSize)
                    return false;
                return true;
            }
        }

    }
}
