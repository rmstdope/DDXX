using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class SmoothIntervalTest
    {
        private float epsilon = 0.000001f;

        [Test]
        public void SmoothMaxima()
        {
            Assert.AreEqual(0, SmoothInterval.ScaledSine(10, 20, 10, 5), epsilon);
            Assert.AreEqual(0, SmoothInterval.ScaledSine(5, 25, 25, 10), epsilon);
            Assert.AreEqual(5, SmoothInterval.ScaledSine(10, 20, 15, 5), epsilon);
            Assert.AreEqual(10, SmoothInterval.ScaledSine(5, 25, 15, 10), epsilon);
        }

        [Test]
        public void Interpolation()
        {
            Assert.AreEqual(new Vector3(1, 2, 3),
                SmoothInterval.SineInterpolation(10, 20, 10, new Vector3(1, 2, 3), new Vector3(4, 5, 6)));
            Assert.AreEqual(new Vector3(4, 5, 6),
                SmoothInterval.SineInterpolation(10, 20, 20, new Vector3(1, 2, 3), new Vector3(4, 5, 6)));
            Assert.AreEqual(new Vector3(2.5f, 3.5f, 4.5f),
                SmoothInterval.SineInterpolation(10, 20, 15, new Vector3(1, 2, 3), new Vector3(4, 5, 6)));
            Assert.AreEqual(new Vector3(0.5f, 0.5f, 0.5f),
                SmoothInterval.SineInterpolation(1, 3, 2, new Vector3(0, 0, 0), new Vector3(1, 1, 1)));
        }
    }
}
