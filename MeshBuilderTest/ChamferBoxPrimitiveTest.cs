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
        private Primitive primitive;

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
            Primitive spherePrimitive = CreateSphere(1, rings);
            Assert.AreEqual(spherePrimitive.Vertices.Length + rings + (rings + 2) + (rings + 4),
                primitive.Vertices.Length);
            rings = 8;
            filletSegments = (rings / 4) + 1;
            CreateChamferBox(10, 10, 10, 1, filletSegments);
            spherePrimitive = CreateSphere(1, rings);
            Assert.AreEqual(spherePrimitive.Vertices.Length + rings + (rings + 2) + (rings + 4),
                primitive.Vertices.Length);
        }

        private static Primitive CreateSphere(float radius, int rings)
        {
            SpherePrimitive sphere = new SpherePrimitive();
            sphere.Radius = 1;
            sphere.Rings = rings;
            return sphere.Generate();
        }

        private void CreateChamferBox(float length, float width, float height, float fillet, int filletSegments)
        {
            ChamferBoxPrimitive chamferBox = new ChamferBoxPrimitive();
            chamferBox.Length = length;
            chamferBox.Width = width;
            chamferBox.Height = height;
            chamferBox.Fillet = fillet;
            chamferBox.FilletSegments = filletSegments;
            primitive = chamferBox.Generate();
            Assert.IsNull(primitive.Body);
        }
    }
}
