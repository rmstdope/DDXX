using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

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
            // Exercise SUT and verify
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.5f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0, 0), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0, 1), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(1, 1), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(1, 0), new Vector2()));
        }

        [Test]
        public void FilledCircleRandValues()
        {
            // Setup
            Circle circle = new Circle();
            // Exercise SUT and verify
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(1, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.5f, 0), new Vector2()));
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.5f, 1), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0, 0.55f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(1, 0.55f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.55f, 0), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.55f, 1), new Vector2()));
        }

        [Test]
        public void InterpolatedCircle()
        {
            // Setup
            Circle circle = new Circle();
            circle.InnerRadius = 0;
            // Exercise SUT and verify
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.5f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.75f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(1.0f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.5f, 0.75f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.5f, 1.0f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.25f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.0f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.5f, 0.25f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.5f, 0.0f), new Vector2()));
        }

        [Test]
        public void InterpolatedHalfCircle()
        {
            // Setup
            Circle circle = new Circle();
            circle.InnerRadius = 0;
            circle.OuterRadius = 0.25f;
            // Exercise SUT and verify
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.5f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.625f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(1.0f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.5f, 0.625f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.5f, 1.0f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.375f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.0f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.5f, 0.375f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(0.5f, 0.0f), new Vector2()));
        }

        [Test]
        public void FilledAndInterpolated()
        {
            // Setup
            Circle circle = new Circle();
            circle.InnerRadius = 0.25f;
            circle.OuterRadius = 0.5f;
            // Exercise SUT and verify
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.5f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(1, 1, 1, 1), circle.GetPixel(new Vector2(0.75f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0.5f, 0.5f, 0.5f, 0.5f), circle.GetPixel(new Vector2(0.875f, 0.5f), new Vector2()));
            Assert.AreEqual(new Vector4(0, 0, 0, 0), circle.GetPixel(new Vector2(1.0f, 0.5f), new Vector2()));
        }

        [Test]
        public void OuterRadiusSetSmallerThanInnerRadius()
        {
            // Setup
            Circle circle = new Circle();
            circle.InnerRadius = 0.2f;
            // Exercise SUT
            circle.OuterRadius = 0.1f;
            // Verify
            Assert.AreEqual(0.1f, circle.InnerRadius);
        }

        [Test]
        public void InnerRadiusSetLargerThanOuterRadius()
        {
            // Setup
            Circle circle = new Circle();
            circle.OuterRadius = 0.1f;
            // Exercise SUT
            circle.InnerRadius = 0.2f;
            // Verify
            Assert.AreEqual(0.2f, circle.OuterRadius);
        }

    }
}
