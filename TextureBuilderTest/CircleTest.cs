using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class CircleTest
    {
        [Test]
        public void FilledCircleCornerAndCenter()
        {
            // Setup
            Circle circle = new Circle();
            circle.SolidRadius = 0.25f;
            circle.GradientRadius1 = 0.25f;
            // Exercise SUT and verify
            Vector4[,] data = circle.GenerateTexture(3, 3);
            Assert.AreEqual(new Vector4(1, 1, 1, 1), data[1, 1]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[0, 0]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[0, 2]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[2, 2]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[2, 0]);
        }

        [Test]
        public void FilledCircleRandValues()
        {
            // Setup
            Circle circle = new Circle();
            circle.SolidRadius = 0.5f;
            // Exercise SUT and verify
            Vector4[,] data = circle.GenerateTexture(121, 121);
            Assert.AreEqual(new Vector4(1, 1, 1, 1), data[0, 60]);
            Assert.AreEqual(new Vector4(1, 1, 1, 1), data[120, 60]);
            Assert.AreEqual(new Vector4(1, 1, 1, 1), data[60, 0]);
            Assert.AreEqual(new Vector4(1, 1, 1, 1), data[60, 120]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[0, 70]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[120, 70]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[70, 0]);
            Assert.AreEqual(new Vector4(0, 0, 0, 0), data[70, 120]);
        }

    }
}
