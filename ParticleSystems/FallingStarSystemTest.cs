using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using NMock2;
using Dope.DDXX.Utility;

namespace Dope.DDXX.ParticleSystems
{
    [TestFixture]
    public class FallingStarSystemTest
    {
        private FallingStarSystem system;
        private Mockery mockery;
        private IDevice device;
        private IGraphicsFactory graphicsFactory;
        private IEffectFactory effectFactory;
        private IEffect effect;
        private IVertexBuffer vertexBuffer;
        private ITexture texture;
        private static VertexColorPoint[] vertexData;

        [SetUp]
        public void SetUp()
        {
            system = new FallingStarSystem("FSS");
            mockery = new Mockery();
            device = mockery.NewMock<IDevice>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            effectFactory = mockery.NewMock<IEffectFactory>();
            effect = mockery.NewMock<IEffect>();
            vertexBuffer = mockery.NewMock<IVertexBuffer>();
            texture = mockery.NewMock<ITexture>();
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
            ExpectVertexBuffer(100);
            system.Initialize(100, device, graphicsFactory, effectFactory, null);
            Assert.AreEqual(100, system.NumParticles);
        }

        [Test]
        public void TestInitializeWithTexture()
        {
            ExpectEffect();
            ExpectVertexBuffer(20);
            system.Initialize(20, device, graphicsFactory, effectFactory, texture);
            Assert.AreEqual(20, system.NumParticles);
        }

        [Test]
        public void TestStep()
        {
            TestInitializeNoTexture();
            Expect.Once.On(vertexBuffer).
                Method("SetData").
                With(new SaveData(), Is.EqualTo(0), Is.EqualTo(LockFlags.Discard));
            system.Step();
            Assert.AreEqual(100, vertexData.Length);
        }

        private void ExpectEffect()
        {
            Expect.Once.On(effectFactory).Method("CreateFromFile").
                With("ParticleSystem.fxo").Will(Return.Value(effect));
            Stub.On(effect).Method("GetParameter").
                Will(Return.Value(EffectHandle.FromString("X")));
        }

        private void ExpectVertexBuffer(int num)
        {
            Expect.Once.On(graphicsFactory).Method("CreateVertexBuffer").
                With(Is.Anything, Is.EqualTo(num), Is.EqualTo(device), Is.EqualTo(Usage.Dynamic | Usage.WriteOnly),
                Is.EqualTo(VertexFormats.None), Is.EqualTo(Pool.Default)).
                Will(Return.Value(vertexBuffer));
            Expect.Once.On(graphicsFactory).
                Method("CreateVertexDeclaration").
                With(device, new VertexElement[] 
                {
                    new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                    new VertexElement(0, 12, DeclarationType.Float1, DeclarationMethod.Default, DeclarationUsage.PointSize, 0),
                    new VertexElement(0, 16, DeclarationType.Color, DeclarationMethod.Default, DeclarationUsage.Color, 0),
                    VertexElement.VertexDeclarationEnd
                }).
                Will(Return.Value(null));
        }

        private class SaveData: Matcher
        {
            public override void DescribeTo(System.IO.TextWriter writer)
            {
                writer.Write("Saving data from SetData method call.");
            }

            public override bool Matches(object o)
            {
                vertexData = (VertexColorPoint[])o;
                return true;
            }
        }

    }
}
