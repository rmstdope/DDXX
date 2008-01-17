using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableVector2Test
    {
        private class Target
        {
            private Vector2 variable;
            public Vector2 Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableVector2 tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableVector2(target.GetType().GetProperty("Variable"), target);
            status = new TweakerStatus(1, 1);
        }

        [Test]
        public void Dimension()
        {
            // Verify
            Assert.AreEqual(2, tweakable.Dimension);
        }

        [Test]
        public void SetAllIndices()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.SetFromString("2, 1");
            // Verify
            Assert.AreEqual(new Vector2(2, 1), target.Variable);
        }

        [Test]
        public void SetFirstIndex()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.SetFromString(0, "23.4");
            // Verify
            Assert.AreEqual(new Vector2(23.4f, 2), target.Variable);
        }

        [Test]
        public void SetSecondIndex()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.SetFromString(1, "6.2");
            // Verify
            Assert.AreEqual(new Vector2(1, 6.2f), target.Variable);
        }

        [Test]
        public void IncreaseFirstIndex()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.IncreaseValue(0);
            // Verify
            Assert.AreEqual(new Vector2(2, 2), target.Variable);
        }

        [Test]
        public void IncreaseSecondIndex()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.IncreaseValue(1);
            // Verify
            Assert.AreEqual(new Vector2(1, 3), target.Variable);
        }

        [Test]
        public void DecreaseFirstIndex()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.DecreaseValue(0);
            // Verify
            Assert.AreEqual(new Vector2(0, 2), target.Variable);
        }

        [Test]
        public void DecreaseSecondIndex()
        {
            // Setup
            target.Variable = new Vector2(1, 2);
            // Exercise SUT
            tweakable.DecreaseValue(1);
            // Verify
            Assert.AreEqual(new Vector2(1, 1), target.Variable);
        }

    }
}
