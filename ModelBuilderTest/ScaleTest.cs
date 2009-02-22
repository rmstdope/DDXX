using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class ScaleTest : IModifier
    {
        private Scale scale;
        private IPrimitive primitive;

        [SetUp]
        public void SetUp()
        {
            primitive = new Primitive(null, null);
        }

        [Test]
        public void Getters()
        {
            CreatePrimitive(new Vector3[] { new Vector3() });
            Scale(1, 2, 3);
            Assert.AreEqual(1, scale.X);
            Assert.AreEqual(2, scale.Y);
            Assert.AreEqual(3, scale.Z);
        }

        [Test]
        public void TestScaleOrigo()
        {
            CreatePrimitive(new Vector3[] { new Vector3() });
            Scale(2, 3, 4);
            VerifyVertices(new Vector3[] { new Vector3() });
        }

        [Test]
        public void TestScaleOneVertex()
        {
            CreatePrimitive(new Vector3[] { new Vector3(1, 1, 1) });
            Scale(2, 3, 4);
            VerifyVertices(new Vector3[] { new Vector3(2, 3, 4) });
        }

        [Test]
        public void TestScaleMoreVertices()
        {
            CreatePrimitive(new Vector3[] { new Vector3(1, 2, 3), new Vector3(4, 5, 6) });
            Scale(3, 4, 5);
            VerifyVertices(new Vector3[] { new Vector3(3, 8, 15), new Vector3(12, 20, 30) });
        }

        private void VerifyVertices(Vector3[] positions)
        {
            Assert.AreEqual(positions.Length, primitive.Vertices.Length);
            for (int i = 0; i < positions.Length; i++)
                Assert.AreEqual(positions[i], primitive.Vertices[i].Position);
        }

        private void Scale(float x, float y, float z)
        {
            scale = new Scale();
            scale.X = x;
            scale.Y = y;
            scale.Z = z;
            scale.ConnectToInput(0, this);
            primitive = scale.Generate();
            Assert.IsNull(primitive.Body);
        }

        private void CreatePrimitive(Vector3[] positions)
        {
            primitive.Vertices = new Vertex[positions.Length];
            primitive.Indices = new short[0];
            for (int i = 0; i < positions.Length; i++)
            {
                primitive.Vertices[i] = new Vertex();
                primitive.Vertices[i].Position = positions[i];
            }
        }

        public void ConnectToInput(int inputPin, IModifier outputGenerator)
        {
        }

        public IPrimitive Generate()
        {
            return primitive;
        }

        #region IModifier Members


        public IModifier GetInputModifier(int index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
