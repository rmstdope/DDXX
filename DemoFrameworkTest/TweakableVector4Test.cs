using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableVector4Test
    {
        private class Target
        {
            private Vector4 variable;
            public Vector4 Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableVector4 tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableVector4(target.GetType().GetProperty("Variable"), target);
            status = new TweakerStatus(1, 1);
        }

        [Test]
        public void Dimension()
        {
            // Verify
            Assert.AreEqual(4, tweakable.Dimension);
        }

        [Test]
        public void SetFirstIndex()
        {
            // Setup
            status.InputString = "23.4";
            status.Index = 0;
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.AreEqual(new Vector4(23.4f, 2, 3, 4), target.Variable);
        }

        [Test]
        public void SetSecondIndex()
        {
            // Setup
            status.InputString = "6.2";
            status.Index = 1;
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.AreEqual(new Vector4(1, 6.2f, 3, 4), target.Variable);
        }

        [Test]
        public void SetThirdIndex()
        {
            // Setup
            status.InputString = "42.1";
            status.Index = 2;
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 42.1f, 4), target.Variable);
        }

        [Test]
        public void SetFourthIndex()
        {
            // Setup
            status.InputString = "17.7";
            status.Index = 3;
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.SetFromInputString(status);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 3, 17.7f), target.Variable);
        }

        [Test]
        public void IncreaseFirstIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.IncreaseValue(0);
            // Verify
            Assert.AreEqual(new Vector4(2, 2, 3, 4), target.Variable);
        }

        [Test]
        public void IncreaseSecondIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.IncreaseValue(1);
            // Verify
            Assert.AreEqual(new Vector4(1, 3, 3, 4), target.Variable);
        }

        [Test]
        public void IncreaseThirdIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.IncreaseValue(2);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 4, 4), target.Variable);
        }

        [Test]
        public void IncreaseFourthIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.IncreaseValue(3);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 3, 5), target.Variable);
        }

        [Test]
        public void DecreaseFirstIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.DecreaseValue(0);
            // Verify
            Assert.AreEqual(new Vector4(0, 2, 3, 4), target.Variable);
        }

        [Test]
        public void DecreaseSecondIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.DecreaseValue(1);
            // Verify
            Assert.AreEqual(new Vector4(1, 1, 3, 4), target.Variable);
        }

        [Test]
        public void DecreaseThirdIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.DecreaseValue(2);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 2, 4), target.Variable);
        }

        [Test]
        public void DecreaseFourthIndex()
        {
            // Setup
            target.Variable = new Vector4(1, 2, 3, 4);
            // Exercise SUT
            tweakable.DecreaseValue(3);
            // Verify
            Assert.AreEqual(new Vector4(1, 2, 3, 3), target.Variable);
        }

    }
}
