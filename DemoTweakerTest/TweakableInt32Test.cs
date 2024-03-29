using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.DemoFramework;

namespace Dope.DDXX.DemoTweaker
{
    [TestFixture]
    public class TweakableInt32Test
    {
        private class Target
        {
            private int variable;
            public int Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableInt32 tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableInt32(target.GetType().GetProperty("Variable"), target, null);
            status = new TweakerStatus(1, 1);
        }

        [Test]
        public void Dimension()
        {
            // Verify
            Assert.AreEqual(1, tweakable.Dimension);
        }

        [Test]
        public void SetPositiveValue()
        {
            // Exercise SUT
            tweakable.SetFromString(0, "1234567890");
            // Verify
            Assert.AreEqual(1234567890, target.Variable);
        }

        [Test]
        public void SetNegativeValue()
        {
            // Exercise SUT
            tweakable.SetFromString("-987654321");
            // Verify
            Assert.AreEqual(-987654321, target.Variable);
        }

        [Test]
        public void GetValue()
        {
            // Setup
            target.Variable = -45;
            // Exercise SUT and verify
            Assert.AreEqual("-45", tweakable.GetToString());
        }

        [Test]
        public void IncreaseValue()
        {
            // Setup
            target.Variable = -1;
            // Exercise SUT
            tweakable.IncreaseValue(0);
            // Verify
            Assert.AreEqual(0, target.Variable);
        }

        [Test]
        public void DecreaseValue()
        {
            // Setup
            target.Variable = 17;
            // Exercise SUT
            tweakable.DecreaseValue(0);
            // Verify
            Assert.AreEqual(16, target.Variable);
        }
    }
}
