using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.DemoFramework
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
            tweakable = new TweakableInt32(target.GetType().GetProperty("Variable"), target);
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
            // Setup
            status.InputString = "1234567890";
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.AreEqual(1234567890, target.Variable);
        }

        [Test]
        public void SetNegativeValue()
        {
            // Setup
            status.InputString = "-987654321";
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.AreEqual(-987654321, target.Variable);
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
