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

        [Test]
        public void TestOneParticlesOneConstraint()
        {
            IPhysicalParticle particle1 = mockery.NewMock<IPhysicalParticle>();
            IConstraint constraint = mockery.NewMock<IConstraint>();
            Stub.On(constraint).GetProperty("Priority").Will(Return.Value(ConstraintPriority.PositionPriority));

            body.SetGravity(new Vector3(1, 2, 3));
            body.AddParticle(particle1);
            body.AddConstraint(constraint);

            Expect.Once.On(particle1).Method("Step").With(new Vector3(1, 2, 3));
            Expect.Exactly(4).On(constraint).Method("Satisfy");
            body.Step();

        }

        [Test]
        public void TestMoreParticlesMoreConstraint()
        {
            const int PARTICLES = 20;
            const int CONSTRAINTS = 10;
            IPhysicalParticle[] particles = new IPhysicalParticle[PARTICLES];
            IConstraint[] constraints = new IConstraint[CONSTRAINTS];
            body.SetGravity(new Vector3(5, 6, 7));
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
                Expect.Exactly(4).On(constraints[j]).Method("Satisfy");
            body.Step();
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
                for (int i = 0; i < 4; i++)
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
