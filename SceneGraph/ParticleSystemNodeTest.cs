using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class ParticleSystemNodeTest
    {
        private ParticleSystemNode system;
        private Mockery mockery;
        private IDevice device;
        private IGraphicsFactory graphicsFactory;
        private IEffectFactory effectFactory;
        private IEffect effect;
        private IVertexBuffer vertexBuffer;
        private ITexture texture;
        private IGraphicsStream graphicsStream;
        private ISystemParticleSpawner spawner;
        private List<ISystemParticle> particles;

        [SetUp]
        public void SetUp()
        {
            system = new ParticleSystemNode("FSS");
            mockery = new Mockery();
            device = mockery.NewMock<IDevice>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            effectFactory = mockery.NewMock<IEffectFactory>();
            effect = mockery.NewMock<IEffect>();
            vertexBuffer = mockery.NewMock<IVertexBuffer>();
            texture = mockery.NewMock<ITexture>();
            graphicsStream = mockery.NewMock<IGraphicsStream>();
            spawner = mockery.NewMock<ISystemParticleSpawner>();
            particles = new List<ISystemParticle>();

            Stub.On(graphicsStream).Method("Dispose");
            Time.Initialize();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestInitializeNoTexture()
        {
            ExpectEffect();
            ExpectVertexBuffer(100, typeof(float));
            ExpectSpawner(2, 100, typeof(float));
            system.Initialize(spawner, device, graphicsFactory, effectFactory, null);
        }

        [Test]
        public void TestInitializeWithTexture()
        {
            ExpectEffect();
            ExpectVertexBuffer(20, typeof(int));
            ExpectSpawner(5, 20, typeof(int));
            system.Initialize(spawner, device, graphicsFactory, effectFactory, texture);
        }

        [Test]
        public void TestStep()
        {
            TestInitializeNoTexture();

            Expect.Once.On(vertexBuffer).
                Method("Lock").
                With(0, 0, LockFlags.Discard).
                Will(Return.Value(graphicsStream));
            foreach (ISystemParticle particle in particles)
                Expect.Once.On(particle).Method("StepAndWrite").With(graphicsStream);
            Expect.Once.On(vertexBuffer).
                Method("Unlock");
            system.Step();
        }

        private void ExpectSpawner(int numInitial, int num, Type type)
        {
            Stub.On(spawner).GetProperty("MaxNumParticles").Will(Return.Value(num));
            Stub.On(spawner).GetProperty("VertexType").Will(Return.Value(type));
            Stub.On(spawner).GetProperty("NumInitialSpawns").Will(Return.Value(numInitial));
            for (int i = 0; i < numInitial; i++)
            {
                particles.Add(mockery.NewMock<ISystemParticle>());
                Expect.Once.On(spawner).Method("Spawn").Will(Return.Value(particles[i]));
            }
        }

        private void ExpectEffect()
        {
            Expect.Once.On(effectFactory).Method("CreateFromFile").
                With("ParticleSystem.fxo").Will(Return.Value(effect));
            Stub.On(effect).Method("GetParameter").
                Will(Return.Value(EffectHandle.FromString("X")));
        }

        private void ExpectVertexBuffer(int num, Type type)
        {
            Expect.Once.On(graphicsFactory).Method("CreateVertexBuffer").
                With(type, num, device, Usage.Dynamic | Usage.WriteOnly, VertexFormats.None, Pool.Default).
                Will(Return.Value(vertexBuffer));
        }

    }
}
