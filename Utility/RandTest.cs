using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class RandTest : Random
    {
        private double nextDoubleReturn = 0;
        private int lastMinValue = -1;
        private int lastMaxValue = -1;
        private int nextIntReturn = 0;

        [Test]
        public void TestFloat()
        {
            Rand.Random = this;
            Assert.AreEqual(0, Rand.Float(0, 0));
            Assert.AreEqual(1, Rand.Float(1, 1));

            nextDoubleReturn = 0;
            Assert.AreEqual(0, Rand.Float(0, 100));
            nextDoubleReturn = 1;
            Assert.AreEqual(100, Rand.Float(0, 100));
            nextDoubleReturn = 0.5;
            Assert.AreEqual(50, Rand.Float(0, 100));

            nextDoubleReturn = 0;
            Assert.AreEqual(0.050, Rand.Float(0.050f, 0.500f));
            nextDoubleReturn = 1;
            Assert.AreEqual(0.500, Rand.Float(0.050f, 0.500f));
            nextDoubleReturn = 0.5;
            Assert.AreEqual(0.275, Rand.Float(0.050f, 0.500f));
        }

        [Test]
        public void TestInt()
        {
            Rand.Random = this;

            nextIntReturn = 10;
            Assert.AreEqual(10, Rand.Int(1, 2));
            Assert.AreEqual(1, lastMinValue);
            Assert.AreEqual(2, lastMaxValue);

            nextIntReturn = -10;
            Assert.AreEqual(-10, Rand.Int(-1, -2));
            Assert.AreEqual(-1, lastMinValue);
            Assert.AreEqual(-2, lastMaxValue);
        }

        public override int Next()
        {
            throw new Exception("");
        }

        public override int Next(int maxValue)
        {
            throw new Exception("");
        }

        public override int Next(int minValue, int maxValue)
        {
            lastMinValue = minValue;
            lastMaxValue = maxValue;
            return nextIntReturn;
        }

        public override void NextBytes(byte[] buffer)
        {
            throw new Exception("");
        }

        public override double NextDouble()
        {
            return nextDoubleReturn;
        }

        protected override double Sample()
        {
            throw new Exception("");
        }
    }
}
