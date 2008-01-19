using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableVector3Test
    {
        private class Target
        {
            private Vector3 variable;
            public Vector3 Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableVector3 tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableVector3(target.GetType().GetProperty("Variable"), target);
            status = new TweakerStatus(1, 1);
        }

        [Test]
        public void Dimension()
        {
            // Verify
            Assert.AreEqual(3, tweakable.Dimension);
        }

        [Test]
        public void SetAllIndices()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.SetFromString("3, 2, 1");
            // Verify
            Assert.AreEqual(new Vector3(3, 2, 1), target.Variable);
        }

        [Test]
        public void GetValue()
        {
            // Setup
            target.Variable = new Vector3(1.2f, 3.4f, 5.6f);
            // Exercise SUT and verify
            Assert.AreEqual("1.2, 3.4, 5.6", tweakable.GetToString());
        }

        [Test]
        public void SetFirstIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.SetFromString(0, "23.4");
            // Verify
            Assert.AreEqual(new Vector3(23.4f, 2, 3), target.Variable);
        }

        [Test]
        public void SetSecondIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.SetFromString(1, "6.2");
            // Verify
            Assert.AreEqual(new Vector3(1, 6.2f, 3), target.Variable);
        }

        [Test]
        public void SetThirdIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.SetFromString(2, "42.1");
            // Verify
            Assert.AreEqual(new Vector3(1, 2, 42.1f), target.Variable);
        }

        [Test]
        public void IncreaseFirstIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.IncreaseValue(0);
            // Verify
            Assert.AreEqual(new Vector3(2, 2, 3), target.Variable);
        }

        [Test]
        public void IncreaseSecondIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.IncreaseValue(1);
            // Verify
            Assert.AreEqual(new Vector3(1, 3, 3), target.Variable);
        }

        [Test]
        public void IncreaseThirdIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.IncreaseValue(2);
            // Verify
            Assert.AreEqual(new Vector3(1, 2, 4), target.Variable);
        }

        [Test]
        public void DecreaseFirstIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.DecreaseValue(0);
            // Verify
            Assert.AreEqual(new Vector3(0, 2, 3), target.Variable);
        }

        [Test]
        public void DecreaseSecondIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.DecreaseValue(1);
            // Verify
            Assert.AreEqual(new Vector3(1, 1, 3), target.Variable);
        }

        [Test]
        public void DecreaseThirdIndex()
        {
            // Setup
            target.Variable = new Vector3(1, 2, 3);
            // Exercise SUT
            tweakable.DecreaseValue(2);
            // Verify
            Assert.AreEqual(new Vector3(1, 2, 2), target.Variable);
        }

    }
}
