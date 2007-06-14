using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class PositionConstraintTest
    {
        [Test]
        public void TestSatisfy()
        {
            PhysicalParticle particle = new PhysicalParticle(new Vector3(1, 1, 1), 1, 1);
            PositionConstraint constraint = new PositionConstraint(particle, new Vector3(2, 3, 4));
            Assert.AreEqual(new Vector3(1, 1, 1), particle.Position);
            constraint.Satisfy();
            Assert.AreEqual(new Vector3(2, 3, 4), particle.Position);
        }
    }
}
