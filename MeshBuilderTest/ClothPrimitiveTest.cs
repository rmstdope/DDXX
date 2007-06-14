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
        private Vertex[] vertices;
        private short[] indices;
        private IBody body;
        private List<int> pinned;

        [SetUp]
        public void SetUp()
        {
            pinned = new List<int>();
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestClothNumParticlesInBody1()
        {
            CreateCloth(10, 30, 1, 1, false);
            Assert.AreEqual(4, body.Particles.Count, "We should have four particles.");
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestClothNumParticlesInBody2()
        {
            CreateCloth(20, 40, 4, 2, false);
            Assert.AreEqual(15, body.Particles.Count, "We should have 15 particles.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInBody1()
        {
            CreateCloth(10, 30, 1, 1, false);
            Assert.AreEqual(6, body.Constraints.Count, "We should have six constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInBody2()
        {
            CreateCloth(20, 40, 4, 2, false);
            Assert.AreEqual(38, body.Constraints.Count, "We should have 38 constraints.");
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
            Assert.AreEqual(8, body.Constraints.Count, "We should have eight constraints.");
            // Now if we move particles 0 and 1 and then satisfy contraints 6 and 7, the two particles
            // shall be moved back.
            Vector3 original0 = body.Particles[0].Position;
            Vector3 original1 = body.Particles[1].Position;
            body.Particles[0].Position = new Vector3(-100, -100, -100);
            body.Particles[1].Position = new Vector3(-100, -100, -100);
            body.Constraints[body.Constraints.Count - 2].Satisfy();
            body.Constraints[body.Constraints.Count - 1].Satisfy();
            Assert.AreEqual(original0, body.Particles[0].Position,
                "Particle[0] should have been moved back.");
            Assert.AreEqual(original1, body.Particles[1].Position,
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
            cloth.Generate(out vertices, out indices, out body);
            Assert.IsNotNull(body);
        }

    }
}
