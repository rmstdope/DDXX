using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class ChamferBoxPrimitiveTest
    {
        private const float epsilon = 0.0001f;
        private ChamferBoxPrimitive chamferBox;
        private IPrimitive primitive;

        [Test]
        public void Getters()
        {
            CreateChamferBox(10, 20, 30, 1, 4);
            Assert.AreEqual(10, chamferBox.Length);
            Assert.AreEqual(20, chamferBox.Width);
            Assert.AreEqual(30, chamferBox.Height);
            Assert.AreEqual(1, chamferBox.Fillet);
            Assert.AreEqual(4, chamferBox.FilletSegments);
        }

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
            IPrimitive spherePrimitive;
            spherePrimitive = CreateSphere(1, rings);
            Assert.AreEqual(spherePrimitive.Vertices.Length + rings + (rings + 2) + (rings + 4),
                primitive.Vertices.Length);
            rings = 8;
            filletSegments = (rings / 4) + 1;
            CreateChamferBox(10, 10, 10, 1, filletSegments);
            spherePrimitive = CreateSphere(1, rings);
            Assert.AreEqual(spherePrimitive.Vertices.Length + rings + (rings + 2) + (rings + 4),
                primitive.Vertices.Length);
        }

        private IPrimitive CreateSphere(float radius, int rings)
        {
            SpherePrimitive sphere = new SpherePrimitive();
            sphere.Radius = 1;
            sphere.Rings = rings;
            return sphere.Generate();
        }

        private void CreateChamferBox(float length, float width, float height, float fillet, int filletSegments)
        {
            chamferBox = new ChamferBoxPrimitive();
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
