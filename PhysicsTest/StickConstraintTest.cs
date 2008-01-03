using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class StickConstraintTest
    {
        [Test]
        public void TestSatisfySameMassNoStiffness1()
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
        public void TestSatisfySameMassStiffness1()
        {
            // Distance between from start is 6
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(2, 4, 4), 1, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 12, 0.5f);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2, 4, 4), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(-0.5f, -1, -1), particle1.Position);
            Assert.AreEqual(new Vector3(2.5f, 5, 5), particle2.Position);
        }

        [Test]
        public void TestSatisfySameMassNoStiffness2()
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
        public void TestSatisfySameMassStiffness2()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(2, 0, 0), 1, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 1, 2);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2, 0, 0), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(1, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(1, 0, 0), particle2.Position);
        }

        [Test]
        public void TestSatisfyDifferentMassNoStiffness1()
        {
            // Mass is 1 and 4
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(20, 40, 40), 4, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 120);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(20, 40, 40), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(-16, -32, -32), particle1.Position);
            Assert.AreEqual(new Vector3(24, 48, 48), particle2.Position);
        }

        [Test]
        public void TestSatisfyDifferentMassStiffness1()
        {
            // Mass is 1 and 4
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(20, 40, 40), 4, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 120, 0.5f);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(20, 40, 40), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(-8, -16, -16), particle1.Position);
            Assert.AreEqual(new Vector3(22, 44, 44), particle2.Position);
        }

        [Test]
        public void TestSatisfyDifferentMassNoStiffness2()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(3, 0, 0), 2, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 0);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(3, 0, 0), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(2, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2, 0, 0), particle2.Position);
        }

        [Test]
        public void TestSatisfyDifferentMassStiffness2()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(3, 0, 0), 2, 1);
            StickConstraint constraint = new StickConstraint(particle1, particle2, 0, 0.5f);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(3, 0, 0), particle2.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(1, 0, 0), particle1.Position);
            Assert.AreEqual(new Vector3(2.5f, 0, 0), particle2.Position);
        }

    }
}
