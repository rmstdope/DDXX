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

        [Test]
        public void TestOneParticlesOneConstraint()
        {
            IPhysicalParticle particle1 = mockery.NewMock<IPhysicalParticle>();
            IConstraint constraint = mockery.NewMock<IConstraint>();

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
            using (mockery.Ordered)
            {
                for (int i = 0; i < PARTICLES; i++)
                {
                    particles[i] = mockery.NewMock<IPhysicalParticle>();
                    body.AddParticle(particles[i]);
                    Expect.Once.On(particles[i]).Method("Step").With(new Vector3(5, 6, 7));
                }
                for (int i = 0; i < CONSTRAINTS; i++)
                {
                    constraints[i] = mockery.NewMock<IConstraint>();
                    body.AddConstraint(constraints[i]);
                }
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < CONSTRAINTS; j++)
                        Expect.Once.On(constraints[j]).Method("Satisfy");
            }
            body.Step();
        }

    }
}
