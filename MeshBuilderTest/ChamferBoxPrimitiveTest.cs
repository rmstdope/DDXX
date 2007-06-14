using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class ChamferBoxPrimitiveTest
    {
        private const float epsilon = 0.0001f;
        private Vertex[] vertices;
        private short[] indices;

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestChamferBoxTooLargeFillet()
        {
            CreateChamferBox(2, 2, 2, 1.1f, 2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestChamferBoxTooFewFilletSegments()
        {
            CreateChamferBox(10, 10, 10, 1, 1);
        }

        [Test]
        public void TestChamferBoxVertexCount()
        {
            int rings = 4;
            int filletSegments = (rings / 4) + 1;
            CreateChamferBox(10, 10, 10, 1, filletSegments);
            Vertex[] sphereVertices;
            short[] sphereIndices;
            IBody sphereBody;
            CreateSphere(1, rings, out sphereVertices, out sphereIndices, out sphereBody);
            Assert.AreEqual(sphereVertices.Length + rings + (rings + 2) + (rings + 4),
                vertices.Length);
            rings = 8;
            filletSegments = (rings / 4) + 1;
            CreateChamferBox(10, 10, 10, 1, filletSegments);
            CreateSphere(1, rings, out sphereVertices, out sphereIndices, out sphereBody);
            Assert.AreEqual(sphereVertices.Length + rings + (rings + 2) + (rings + 4),
                vertices.Length);
        }

        private static void CreateSphere(float radius, int rings, out Vertex[] sphereVertices, out short[] sphereIndices, out IBody sphereBody)
        {
            SpherePrimitive sphere = new SpherePrimitive();
            sphere.Radius = 1;
            sphere.Rings = rings;
            sphere.Generate(out sphereVertices, out sphereIndices, out sphereBody);
        }

        private void CreateChamferBox(float length, float width, float height, float fillet, int filletSegments)
        {
            IBody body;
            ChamferBoxPrimitive chamferBox = new ChamferBoxPrimitive();
            chamferBox.Length = length;
            chamferBox.Width = width;
            chamferBox.Height = height;
            chamferBox.Fillet = fillet;
            chamferBox.FilletSegments = filletSegments;
            chamferBox.Generate(out vertices, out indices, out body);
            Assert.IsNull(body);
        }
    }
}
