using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class PhysicalParticleTest
    {
        [SetUp]
        public void SetUp()
        {
            Time.Initialize();
            Time.Pause();
        }

        [Test]
        public void TestPosition()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            PhysicalParticle particle2 = new PhysicalParticle(new Vector3(1, 2, 3), 1, 1);
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
            Assert.AreEqual(new Vector3(1, 2, 3), particle2.Position, "Position should be (1, 2, 3).");
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
            Assert.AreEqual(new Vector3(1, 2, 3), particle2.Position, "Position should be (1, 2, 3).");
        }

        [Test]
        public void TestStepNoForceNoGravity()
        {
            PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
            Time.SetDeltaTimeForTest(1.0f);
            particle1.Step(new Vector3(0, 0, 0));
            Assert.AreEqual(new Vector3(0, 0, 0), particle1.Position, "Position should be (0, 0, 0).");
        }

        //[Test]
        //public void TestStepGravity()
        //{
        //    PhysicalParticle particle1 = new PhysicalParticle(new Vector3(0, 0, 0), 1, 1);
        //    Time.SetDeltaTimeForTest(1.0f);
        //    particle1.Step(new Vector3(2, 4, 6));
        //    Assert.AreEqual(new Vector3(1, 2, 3), particle1.Position, "Position should be (1, 2, 4).");
        //    Time.SetDeltaTimeForTest(2.0f);
        //    particle1.Step(new Vector3(1, 1, 1));
        //    Assert.AreEqual(new Vector3(4, 7, 10), particle1.Position, "Position should be (4, 7, 10).");
        //}
    }
}
