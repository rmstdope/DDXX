using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using NMock2;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class NormalMapTest : D3DMockTest
    {
        private NormalMap normalMap;
        private IGenerator generator;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            normalMap = new NormalMap();
            generator = mockery.NewMock<IGenerator>();
            normalMap.ConnectToInput(0, generator);
        }

        [Test]
        public void SourceIs2x2TargetIs0()
        {
            // Setup
            const float delta = 1 / 256.0f;
            Stub.On(generator).Method("GetPixel").With(new Vector2(-delta, 0), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(1, 0, 0, 0)));
            Stub.On(generator).Method("GetPixel").With(new Vector2(0, -delta), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(2, 0, 0, 0)));
            Stub.On(generator).Method("GetPixel").With(new Vector2(delta, 0), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(3, 0, 0, 0)));
            Stub.On(generator).Method("GetPixel").With(new Vector2(0, delta), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(5, 0, 0, 0)));
            // Exercuse SUT
            Vector4 p1 = normalMap.GetPixel(new Vector2(0, 0), new Vector2(delta, delta));
            // Verify
            Vector3 normal = Vector3.Normalize(new Vector3(-2, 1, -3));
            Assert.AreEqual(new Vector4(normal.X, normal.Y, normal.Z, 0), p1);
        }

        [Test]
        public void SourceIs2x2TargetIs1()
        {
            // Setup
            const float delta = 100.1f;
            Stub.On(generator).Method("GetPixel").With(new Vector2(1 - delta, 1), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(0.4f, 0, 0, 0)));
            Stub.On(generator).Method("GetPixel").With(new Vector2(1, 1 - delta), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(0.4f, 0, 0, 0)));
            Stub.On(generator).Method("GetPixel").With(new Vector2(1 + delta, 1), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(0.1f, 0, 0, 0)));
            Stub.On(generator).Method("GetPixel").With(new Vector2(1, 1 + delta), new Vector2(delta, delta)).
                Will(Return.Value(new Vector4(0.2f, 0, 0, 0)));
            // Exercuse SUT
            Vector4 p1 = normalMap.GetPixel(new Vector2(1, 1), new Vector2(delta, delta));
            // Verify
            Vector3 normal = Vector3.Normalize(new Vector3(0.3f, 1, 0.2f));
            Assert.AreEqual(new Vector4(normal.X, normal.Y, normal.Z, 0), p1);
        }

    }
}
