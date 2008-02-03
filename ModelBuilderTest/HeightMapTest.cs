using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using NMock2;
using Dope.DDXX.NUnitExtension;
using NUnitExtension;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class HeightMapTest : IModifier
    {
        private HeightMap heightMap;
        private IPrimitive primitive;
        private Mockery mockery;
        private ITextureGenerator textureGenerator;

        [SetUp]
        public void SetUp()
        {
            heightMap = new HeightMap();
            mockery = new Mockery();
            textureGenerator = mockery.NewMock<ITextureGenerator>();
            heightMap.HeightMapGenerator = textureGenerator;
            heightMap.ConnectToInput(0, this);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ZeroHeightOneVertex()
        {
            // Setup
            Stub.On(textureGenerator).Method("GetPixel").
                WithAnyArguments().Will(Return.Value(Vector4.Zero));
            primitive = new Primitive(new Vertex[] { new Vertex(new Vector3(1, 2, 3), Vector3.Zero) }, new short[] { });
            // Exercise SUT
            IPrimitive generatedPrimitive = heightMap.Generate();
            // Verify
            Assert.AreEqual(1, generatedPrimitive.Vertices.Length);
            Assert.AreEqual(new Vector3(1, 2, 3), generatedPrimitive.Vertices[0].Position);
        }

        [Test]
        public void NonZeroHeightOneVertex()
        {
            // Setup
            Stub.On(textureGenerator).Method("GetPixel").WithAnyArguments().
                Will(Return.Value(new Vector4(2,3,4,5)));
            primitive = new Primitive(new Vertex[] { new Vertex(new Vector3(1, 2, 3), new Vector3(3, 4, 5)) }, new short[] { });
            // Exercise SUT
            IPrimitive generatedPrimitive = heightMap.Generate();
            // Verify
            Assert.AreEqual(1, generatedPrimitive.Vertices.Length);
            Assert.AreEqual(new Vector3(1 + 2 * 3, 2 + 2 * 4, 3 + 2 * 5), generatedPrimitive.Vertices[0].Position);
        }

        [Test]
        public void ConstantHeightMultipleVertices()
        {
            // Setup
            Stub.On(textureGenerator).Method("GetPixel").WithAnyArguments().
                Will(Return.Value(new Vector4(5, 3, 4, 5)));
            primitive = new Primitive(new Vertex[] { 
                new Vertex(new Vector3(1, 2, 3), new Vector3(3, 4, 5)), 
                new Vertex(new Vector3(4, 5, 6), new Vector3(7, 8, 9)) 
            }, new short[] { });
            // Exercise SUT
            IPrimitive generatedPrimitive = heightMap.Generate();
            // Verify
            Assert.AreEqual(2, generatedPrimitive.Vertices.Length);
            Assert.AreEqual(new Vector3(1 + 5 * 3, 2 + 5 * 4, 3 + 5 * 5), generatedPrimitive.Vertices[0].Position);
            Assert.AreEqual(new Vector3(4 + 5 * 7, 5 + 5 * 8, 6 + 5 * 9), generatedPrimitive.Vertices[1].Position);
        }

        [Test]
        public void NonConstantHeightMultipleVertices()
        {
            // Setup
            Stub.On(textureGenerator).Method("GetPixel").With(Is.EqualTo(new Vector2(0.2f, 0.4f)), Is.Anything).
                Will(Return.Value(new Vector4(2, 0, 0, 0)));
            Stub.On(textureGenerator).Method("GetPixel").With(Is.EqualTo(new Vector2(0.6f, 0.8f)), Is.Anything).
                Will(Return.Value(new Vector4(3, 0, 0, 0)));
            primitive = new Primitive(new Vertex[] { 
                new Vertex(new Vector3(1, 2, 3), new Vector3(3, 4, 5), new Vector2(0.2f, 0.4f)), 
                new Vertex(new Vector3(4, 5, 6), new Vector3(7, 8, 9), new Vector2(0.6f, 0.8f)) 
            }, new short[] { });
            // Exercise SUT
            IPrimitive generatedPrimitive = heightMap.Generate();
            // Verify
            Assert.AreEqual(2, generatedPrimitive.Vertices.Length);
            Assert.AreEqual(new Vector3(1 + 2 * 3, 2 + 2 * 4, 3 + 2 * 5), generatedPrimitive.Vertices[0].Position);
            Assert.AreEqual(new Vector3(4 + 3 * 7, 5 + 3 * 8, 6 + 3 * 9), generatedPrimitive.Vertices[1].Position);
        }

        //[Test]
        //public void TilingFix()
        //{
        //    // Setup
        //    Stub.On(textureGenerator).Method("GetPixel").With(new IsCloseEnough(new Vector2(0.2f, 0.8f), 0.0001f), Is.Anything).
        //        Will(Return.Value(new Vector4(2, 0, 0, 0)));
        //    Stub.On(textureGenerator).Method("GetPixel").With(new IsCloseEnough(new Vector2(1.0f, 0.8f), 0.0001f), Is.Anything).
        //        Will(Return.Value(new Vector4(3, 0, 0, 0)));
        //    primitive = new Primitive(new Vertex[] { 
        //        new Vertex(new Vector3(1, 2, 3), new Vector3(3, 4, 5), new Vector2(-0.1f, 0.6f)), 
        //        new Vertex(new Vector3(4, 5, 6), new Vector3(7, 8, 9), new Vector2(0.5f, 1.4f)) 
        //    }, new short[] { });
        //    // Exercise SUT
        //    IPrimitive generatedPrimitive = heightMap.Generate();
        //    // Verify
        //    Assert.AreEqual(2, generatedPrimitive.Vertices.Length);
        //    Assert.AreEqual(new Vector3(1 + 2 * 3, 2 + 2 * 4, 3 + 2 * 5), generatedPrimitive.Vertices[0].Position);
        //    Assert.AreEqual(new Vector3(4 + 3 * 7, 5 + 3 * 8, 6 + 3 * 9), generatedPrimitive.Vertices[1].Position);
        //}

        #region IModifier Members

        public void ConnectToInput(int inputPin, IModifier outputGenerator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IPrimitive Generate()
        {
            return primitive;
        }

        #endregion
    }
}
