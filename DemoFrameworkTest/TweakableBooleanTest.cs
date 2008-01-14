using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableBooleanTest
    {
        private class Target
        {
            private bool variable;
            public bool Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableBoolean tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableBoolean(target.GetType().GetProperty("Variable"), target);
            status = new TweakerStatus(1, 1);
        }

        [Test]
        public void Dimension()
        {
            // Verify
            Assert.AreEqual(1, tweakable.Dimension);
        }

        [Test]
        public void SetToTrue()
        {
            // Setup
            status.InputString = "true";
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.IsTrue(target.Variable);
        }

        [Test]
        public void SetToFalse()
        {
            // Setup
            status.InputString = "false";
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.IsFalse(target.Variable);
        }

        [Test]
        public void FlipFromFalse()
        {
            // Setup
            target.Variable = false;
            // Exercise SUT
            tweakable.IncreaseValue(0);
            // Verify
            Assert.IsTrue(target.Variable);
        }

        [Test]
        public void FlipFromTrue()
        {
            // Setup
            target.Variable = true;
            // Exercise SUT
            tweakable.DecreaseValue(0);
            // Verify
            Assert.IsFalse(target.Variable);
        }
    }
}
