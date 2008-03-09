using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableStringTest
    {
        private class Target
        {
            private string variable;
            public string Variable
            {
                get { return variable; }
                set { variable = value; }
            }
        }

        private Target target;
        private TweakableString tweakable;
        private TweakerStatus status;

        [SetUp]
        public void SetUp()
        {
            target = new Target();
            tweakable = new TweakableString(target.GetType().GetProperty("Variable"), target, null);
            status = new TweakerStatus(1, 1);
        }

        [Test]
        public void Dimension()
        {
            // Verify
            Assert.AreEqual(1, tweakable.Dimension);
        }

        [Test]
        public void SetValue()
        {
            // Exercise SUT
            tweakable.SetFromString(0, "abba");
            // Verify
            Assert.AreEqual("abba", target.Variable);
        }

        [Test]
        public void GetValue()
        {
            // Setup
            target.Variable = "hesa";
            // Exercise SUT and verify
            Assert.AreEqual("hesa", tweakable.GetToString());
        }

        [Test]
        public void SetValueNoIndex()
        {
            // Exercise SUT
            tweakable.SetFromString("-987.654321");
            // Verify
            Assert.AreEqual("-987.654321", target.Variable);
        }

    }
}
