using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableSingleTest
    {
        private class Target
        {
            private float variable;
            public float Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableSingle tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableSingle(target.GetType().GetProperty("Variable"), target);
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
            tweakable.SetFromString(0, "123456.7890");
            // Verify
            Assert.AreEqual(123456.7890, target.Variable, 0.01f);
        }

        [Test]
        public void SetNegativeValue()
        {
            // Exercise SUT
            tweakable.SetFromString("-987.654321");
            // Verify
            Assert.AreEqual(-987.654321, target.Variable, 0.0001f);
        }

        [Test]
        public void GetValue()
        {
            // Setup
            target.Variable = -45.34f;
            // Exercise SUT and verify
            Assert.AreEqual("-45.34", tweakable.GetToString());
        }

        [Test]
        public void IncreaseValue()
        {
            // Setup
            target.Variable = -1.5f;
            // Exercise SUT
            tweakable.IncreaseValue(0);
            // Verify
            Assert.AreEqual(-0.5f, target.Variable);
        }

        [Test]
        public void DecreaseValue()
        {
            // Setup
            target.Variable = 17.2f;
            // Exercise SUT
            tweakable.DecreaseValue(0);
            // Verify
            Assert.AreEqual(16.2f, target.Variable);
        }
    }
}
