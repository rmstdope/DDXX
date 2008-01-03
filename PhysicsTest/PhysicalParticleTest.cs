using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class PhysicalParticleTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void TestPosition()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 0);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(1, 2, 3), 1, 0);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
            Assert.AreEqual(new Vector3(1, 2, 3), particle2.Position, "Position should be (1, 2, 3).");
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
            Assert.AreEqual(new Vector3(1, 2, 3), particle2.Position, "Position should be (1, 2, 3).");
        }

        [Test]
        public void TestStepNoForceNoGravity()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 0);
            Time.Step(1.0f);
            particle1.Step(new Vector3(0, 0, 0));
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
        }

        [Test]
        public void TestStepGravity()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 0);
            Time.Step(3.0f);
            particle1.Step(new Vector3(2, 4, 6));
            Assert.AreEqual(new Vector3(9, 18, 27), particle1.Position, "Position should be a*t*t/2 (9, 18, 27).");
            Time.Step(1.0f);
            particle1.Step(new Vector3(2, 4, 8));
            Assert.AreEqual(new Vector3(9 + 3 + 1, 18 + 6 + 2, 27 + 9 + 4), particle1.Position, "Position should be s + v*t + a*t*t/2 (13, 26, 40).");
        }

        [Test]
        public void TestStepGravityForces()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 0);
            Time.Step(1.0f);
            particle1.ApplyForce(new Vector3(-1, -1, -1));
            particle1.ApplyForce(new Vector3(-1, -3, -5));
            particle1.Step(new Vector3(2, 4, 6));
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
            particle1.Step(new Vector3(2, 4, 6));
            Assert.AreEqual(new Vector3(1, 2, 3), particle1.Position, "Position should be (0, 0, 0).");
        }

        [Test]
        public void TestStepGravityForcesMassOfTwo()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 2, 0);
            Time.Step(1.0f);
            particle1.ApplyForce(new Vector3(-2, -2, -2));
            particle1.ApplyForce(new Vector3(-2, -6, -10));
            particle1.Step(new Vector3(2, 4, 6));
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
            particle1.Step(new Vector3(2, 4, 6));
            Assert.AreEqual(new Vector3(1, 2, 3), particle1.Position, "Position should be (0, 0, 0).");
        }

        [Test]
        public void TestStepDrag()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 0.5f);
            Time.Step(1.0f);
            particle1.Step(new Vector3(2, 4, 6));
            Assert.AreEqual(new Vector3(1, 2, 3), particle1.Position, "Position should be a*t*t/2 (1, 2, 3).");
            particle1.Step(new Vector3(0, 0, 0));
            Assert.AreEqual(new Vector3(1 + 0.5f, 2 + 1, 3 + 1.5f), particle1.Position, "Position should be s + v*t + a*t*t/2 (1.5, 3, 4.5).");
        }

    }
}
