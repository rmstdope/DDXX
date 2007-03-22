using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class StickConstraintTest
    {
        [Test]
        public void TestSatisfySameMass1()
        {
            // Distance between from start is 6
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(2, 4, 4), 1, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 12);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2, 4, 4), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(-1, -2, -2), particle1.Position);
            Assert.AreEqual(new Vector3(3, 6, 6), particle2.Position);
        }

        [Test]
        public void TestSatisfySameMass2()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(2, 0, 0), 1, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 1);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2, 0, 0), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(0.5f, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(1.5f, 0, 0), particle2.Position);
        }

        [Test]
        public void TestSatisfyDifferentMass1()
        {
            // Mass is 1 and 4
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(20, 40, 40), 0.25f, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 120);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(20, 40, 40), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(-16, -32, -32), particle1.Position);
            Assert.AreEqual(new Vector3(24, 48, 48), particle2.Position);
        }

        [Test]
        public void TestSatisfyDifferentMass2()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(3, 0, 0), 0.5f, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 0);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(3, 0, 0), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(2, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2, 0, 0), particle2.Position);
        }

    }
}
