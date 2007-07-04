using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class ClothPrimitiveTest
    {
        private Primitive primitive;
        private List<int> pinned;

        [SetUp]
        public void SetUp()
        {
            pinned = new List<int>();
        }

        /// <summary>
        /// Test that as many particles are added to the primitive.Body as there is vertices.
        /// </summary>
        [Test]
        public void TestClothNumParticlesInBody1()
        {
            CreateCloth(10, 30, 1, 1, false);
            Assert.AreEqual(4, primitive.Body.Particles.Count, "We should have four particles.");
        }

        /// <summary>
        /// Test that as many particles are added to the primitive.Body as there is vertices.
        /// </summary>
        [Test]
        public void TestClothNumParticlesInBody2()
        {
            CreateCloth(20, 40, 4, 2, false);
            Assert.AreEqual(15, primitive.Body.Particles.Count, "We should have 15 particles.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInBody1()
        {
            CreateCloth(10, 30, 1, 1, false);
            Assert.AreEqual(6, primitive.Body.Constraints.Count, "We should have six constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInBody2()
        {
            CreateCloth(20, 40, 4, 2, false);
            Assert.AreEqual(38, primitive.Body.Constraints.Count, "We should have 38 constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints in a pinned cloth.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInPinnedCloth2()
        {
            pinned.Add(0);
            pinned.Add(1);
            CreateCloth(10, 30, 1, 1, false);
            Assert.AreEqual(8, primitive.Body.Constraints.Count, "We should have eight constraints.");
            // Now if we move particles 0 and 1 and then satisfy contraints 6 and 7, the two particles
            // shall be moved back.
            Vector3 original0 = primitive.Body.Particles[0].Position;
            Vector3 original1 = primitive.Body.Particles[1].Position;
            primitive.Body.Particles[0].Position = new Vector3(-100, -100, -100);
            primitive.Body.Particles[1].Position = new Vector3(-100, -100, -100);
            primitive.Body.Constraints[primitive.Body.Constraints.Count - 2].Satisfy();
            primitive.Body.Constraints[primitive.Body.Constraints.Count - 1].Satisfy();
            Assert.AreEqual(original0, primitive.Body.Particles[0].Position,
                "Particle[0] should have been moved back.");
            Assert.AreEqual(original1, primitive.Body.Particles[1].Position,
                "Particle[1] should have been moved back.");
        }

        private void CreateCloth(float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            ClothPrimitive cloth = new ClothPrimitive();
            foreach(int index in pinned)
                cloth.PinParticle(index);
            cloth.Width = width;
            cloth.Height = height;
            cloth.WidthSegments = widthSegments;
            cloth.HeightSegments = heightSegments;
            cloth.Textured = textured;
            primitive = cloth.Generate();
            Assert.IsNotNull(primitive.Body);
        }

    }
}
