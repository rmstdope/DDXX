using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class BodyTest
    {
        private Body body;
        private Mockery mockery;
        private const int NUM_ITERATIONS = 3;

        [SetUp]
        public void SetUp()
        {
            body = new Body();
            mockery = new Mockery();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test that the gravity is sett correctly.
        /// </summary>
        [Test]
        public void TestGravity()
        {
            Assert.AreEqual(new Vector3(0, -9.82f, 0), body.Gravity,
                "Standard gravity should be (0, -9.82, 0).");
            body.Gravity = new Vector3(1, 2, 3);
            Assert.AreEqual(new Vector3(1, 2, 3), body.Gravity,
                "Standard gravity should be (0, -9.82, 0).");
        }

        /// <summary>
        /// Test stepping with one particle and one constraint.
        /// </summary>
        [Test]
        public void TestOneParticleOneConstraint()
        {
            IPhysicalParticle particle1 = mockery.NewMock<IPhysicalParticle>();
            IConstraint constraint = mockery.NewMock<IConstraint>();
            Stub.On(constraint).GetProperty("Priority").Will(Return.Value(ConstraintPriority.PositionPriority));

            body.Gravity = new Vector3(1, 2, 3);
            body.AddParticle(particle1);
            body.AddConstraint(constraint);

            Expect.Once.On(particle1).Method("Step").With(new Vector3(1, 2, 3));
            Expect.Exactly(NUM_ITERATIONS).On(constraint).Method("Satisfy");
            body.Step();

        }

        /// <summary>
        /// Test stepping with more particles and constraints.
        /// </summary>
        [Test]
        public void TestMoreParticlesMoreConstraint()
        {
            const int PARTICLES = 20;
            const int CONSTRAINTS = 10;
            IPhysicalParticle[] particles = new IPhysicalParticle[PARTICLES];
            IConstraint[] constraints = new IConstraint[CONSTRAINTS];
            body.Gravity = new Vector3(5, 6, 7);
            for (int i = 0; i < PARTICLES; i++)
            {
                particles[i] = mockery.NewMock<IPhysicalParticle>();
                body.AddParticle(particles[i]);
            }
            for (int i = 0; i < CONSTRAINTS; i++)
            {
                constraints[i] = mockery.NewMock<IConstraint>();
                Stub.On(constraints[i]).GetProperty("Priority").Will(Return.Value(ConstraintPriority.PositionPriority));
                body.AddConstraint(constraints[i]);
            }
            using (mockery.Ordered)
            {
                for (int i = 0; i < PARTICLES; i++)
                {
                    Expect.Once.On(particles[i]).Method("Step").With(new Vector3(5, 6, 7));
                }
            }
            for (int j = 0; j < CONSTRAINTS; j++)
                Expect.Exactly(NUM_ITERATIONS).On(constraints[j]).Method("Satisfy");
            body.Step();
        }

        /// <summary>
        /// Test applying a constant force to a body.
        /// </summary>
        [Test]
        public void TestApplyForce()
        {
            IPhysicalParticle particle1 = mockery.NewMock<IPhysicalParticle>();
            IPhysicalParticle particle2 = mockery.NewMock<IPhysicalParticle>();

            body.AddParticle(particle1);
            body.AddParticle(particle2);

            Expect.Once.On(particle1).Method("ApplyForce").With(new Vector3(1, 2, 3));
            Expect.Once.On(particle2).Method("ApplyForce").With(new Vector3(1, 2, 3));
            body.ApplyForce(new Vector3(1, 2, 3));

            Expect.Once.On(particle1).Method("ApplyForce").With(new Vector3(5, 6, 7));
            Expect.Once.On(particle2).Method("ApplyForce").With(new Vector3(5, 6, 7));
            body.ApplyForce(new Vector3(5, 6, 7));
        }


        /// <summary>
        /// Test three different priorities. 
        /// First add a constraint will low priority and then two with higher priority.
        /// Both high priority constraints shall be sorted in before the low priority.
        /// </summary>
        [Test]
        public void TestConstraintPriorities()
        {
            IConstraint constraint1 = mockery.NewMock<IConstraint>();
            Stub.On(constraint1).GetProperty("Priority").Will(Return.Value(ConstraintPriority.PositionPriority));
            body.AddConstraint(constraint1);
            IConstraint constraint2 = mockery.NewMock<IConstraint>();
            Stub.On(constraint2).GetProperty("Priority").Will(Return.Value(ConstraintPriority.StickPriority));
            body.AddConstraint(constraint2);
            IConstraint constraint3 = mockery.NewMock<IConstraint>();
            Stub.On(constraint3).GetProperty("Priority").Will(Return.Value(ConstraintPriority.StickPriority));
            body.AddConstraint(constraint3);
            using (mockery.Ordered)
            {
                for (int i = 0; i < NUM_ITERATIONS; i++)
                {
                    Expect.Once.On(constraint3).Method("Satisfy");
                    Expect.Once.On(constraint2).Method("Satisfy");
                    Expect.Once.On(constraint1).Method("Satisfy");
                }
            }
            body.Step();
        }

    }
}
