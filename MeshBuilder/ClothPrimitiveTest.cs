using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class ClothPrimitiveTest : IBody
    {
        private List<IPhysicalParticle> particles;
        private List<Dope.DDXX.Physics.IConstraint> constraints;

        [SetUp]
        public void SetUp()
        {
            particles = new List<IPhysicalParticle>();
            constraints = new List<Dope.DDXX.Physics.IConstraint>();
        }

        /// <summary>
        /// Test that the body is set in the Primitive.
        /// </summary>
        [Test]
        public void TestBody()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 10, 30, 1, 1);
            Assert.AreSame(this, cloth.Body, "Body should have been set to this.");
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestNumParticlesInBody1()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 10, 30, 1, 1);
            Assert.AreEqual(4, particles.Count, "We should have four particles.");
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestNumParticlesInBody2()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 20, 40, 4, 2);
            Assert.AreEqual(15, particles.Count, "We should have 15 particles.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestNumConstraintsInBody1()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 10, 30, 1, 1);
            Assert.AreEqual(4, constraints.Count, "We should have four constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestNumConstraintsInBody2()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 20, 40, 4, 2);
            Assert.AreEqual(22, constraints.Count, "We should have 22 constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints in a pinned cloth.
        /// </summary>
        [Test]
        public void TestNumConstraintsInPinnedCloth1()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 10, 30, 1, 1, new int[] { });
            Assert.AreEqual(4, constraints.Count, "We should have four constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints in a pinned cloth.
        /// </summary>
        [Test]
        public void TestNumConstraintsInPinnedCloth2()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 10, 30, 1, 1, new int[] { 0, 1 });
            Assert.AreEqual(6, constraints.Count, "We should have six constraints.");
            // Now if we move particles 0 and 1 and then satisfy contraints 6 and 7, the two particles
            // shall be moved back.
            Vector3 original0 = particles[0].Position;
            Vector3 original1 = particles[1].Position;
            particles[0].Position = new Vector3(-100, -100, -100);
            particles[1].Position = new Vector3(-100, -100, -100);
            constraints[4].Satisfy();
            constraints[5].Satisfy();
            Assert.AreEqual(original0, particles[0].Position, 
                "Particle[0] should have been moved back.");
            Assert.AreEqual(original1, particles[1].Position,
                "Particle[0] should have been moved back.");
        }

        /// <summary>
        /// Test that all particles have the same positions are a plane of the same size.
        /// </summary>
        [Test]
        public void TestParticlePosition()
        {
            Primitive cloth = Primitive.ClothPrimitive(this, 56, 87, 7, 9);
            Primitive plane = Primitive.PlanePrimitive(56, 87, 7, 9);
            for (int i = 0; i < plane.Vertices.Length; i++)
            {
                Assert.IsInstanceOfType(typeof(PhysicalParticle), particles[i],
                    "All particles should be PhysicalParticles");
                Assert.AreEqual(plane.Vertices[i].Position, particles[i].Position,
                    "Position of particle " + i + " should be the same as the plane vertex.");
            }
        }


        #region IBody Members

        public void AddConstraint(Dope.DDXX.Physics.IConstraint constraint)
        {
            constraints.Add(constraint);
        }

        public void AddParticle(IPhysicalParticle particle)
        {
            particles.Add(particle);
        }

        public Vector3 Gravity
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public List<IPhysicalParticle> Particles
        {
            get { return particles; }
        }

        public void ApplyForce(Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
