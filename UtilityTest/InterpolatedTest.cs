using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Utility
{
    [TestFixture]
    public class InterpolatedTest
    {
        [Test]
        public void TestFloatConstructor()
        {
            float float1 = 10.0f;
            float float2 = 20.0f;
            InterpolatedFloat f1 = new InterpolatedFloat(float1);
            InterpolatedFloat f2 = new InterpolatedFloat(float2);
            Assert.AreEqual((float)f1, float1);
            Assert.AreEqual((float)f2, float2);
        }

        [Test]
        public void TestFloatOperators()
        {
            float float1 = 10.0f;
            float float2 = 20.0f;
            InterpolatedFloat f1 = new InterpolatedFloat(float1);
            InterpolatedFloat f2 = new InterpolatedFloat(float2);
            InterpolatedFloat f3 = (InterpolatedFloat)f1.Sub(f2);
            InterpolatedFloat f4 = (InterpolatedFloat)f1.Add(f2);
            InterpolatedFloat f5 = (InterpolatedFloat)f1.Mul(4.0f);
            Assert.AreEqual((float)f3, float1 - float2);
            Assert.AreEqual((float)f4, float1 + float2);
            Assert.AreEqual((float)f5, float1 * 4.0f);
        }

        [Test]
        public void TestVector3Constructor()
        {
            Vector3 vec1 = new Vector3(1, 2, 3);
            Vector3 vec2 = new Vector3(10, 20, 30);
            InterpolatedVector3 v1 = new InterpolatedVector3(vec1);
            InterpolatedVector3 v2 = new InterpolatedVector3(vec2);
            Assert.AreEqual((Vector3)v1, vec1);
            Assert.AreEqual((Vector3)v2, vec2);
        }

        [Test]
        public void TestVector3Operators()
        {
            Vector3 vec1 = new Vector3(1, 2, 3);
            Vector3 vec2 = new Vector3(10, 20, 30);
            InterpolatedVector3 v1 = new InterpolatedVector3(vec1);
            InterpolatedVector3 v2 = new InterpolatedVector3(vec2);
            InterpolatedVector3 v3 = (InterpolatedVector3)v1.Sub(v2);
            InterpolatedVector3 v4 = (InterpolatedVector3)v1.Add(v2);
            InterpolatedVector3 v5 = (InterpolatedVector3)v1.Mul(4.0f);
            Assert.AreEqual((Vector3)v3, vec1 - vec2);
            Assert.AreEqual((Vector3)v4, vec1 + vec2);
            Assert.AreEqual((Vector3)v5, vec1 * 4.0f);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTypeMismatch1()
        {
            InterpolatedFloat f = new InterpolatedFloat();
            InterpolatedVector3 v = new InterpolatedVector3();
            f.Add(v);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddTypeMismatch2()
        {
            InterpolatedFloat f = new InterpolatedFloat();
            InterpolatedVector3 v = new InterpolatedVector3();
            v.Add(f);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SubTypeMismatch1()
        {
            InterpolatedFloat f = new InterpolatedFloat();
            InterpolatedVector3 v = new InterpolatedVector3();
            f.Sub(v);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void SubTypeMismatch2()
        {
            InterpolatedFloat f = new InterpolatedFloat();
            InterpolatedVector3 v = new InterpolatedVector3();
            v.Sub(f);
        }

        [Test]
        public void TestFloatZero()
        {
            InterpolatedFloat f = new InterpolatedFloat();
            f = f.Zero() as InterpolatedFloat;
            Assert.AreEqual(0.0f, (float)f);
        }

        [Test]
        public void TestVector3Zero()
        {
            InterpolatedVector3 v = new InterpolatedVector3(new Vector3(1, 2, 3));
            v = v.Zero() as InterpolatedVector3;
            Assert.AreEqual(new Vector3(0, 0, 0), (Vector3)v);
        }
    }
}
