using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class PrimitiveBaseTest
    {
        private class SimplePrimitive : ModifierBase
        {
            public IPrimitive primitive;

            public SimplePrimitive(int pins)
                : base(pins)
            {
            }

            public SimplePrimitive(int pins, Vertex[] vertices, short[] indices, IBody body)
                : base(pins)
            {
                primitive = new Primitive(vertices, indices, body);
            }

            public override IPrimitive Generate()
            {
                return primitive;
            }

            public IPrimitive CallGetInput(int inputPin)
            {
                return GetInput(inputPin);
            }
        }

        private IPrimitive primitive;

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestTooFewInputs()
        {
            new SimplePrimitive(-1);
        }

        [Test]
        public void TestInputsOk()
        {
            for (int i = 0; i < 10; i++)
                new SimplePrimitive(i);
        }

        [Test]
        public void TestConnectOk()
        {
            IModifier generator1 = new SimplePrimitive(1);
            generator1.ConnectToInput(0, null);
            generator1 = new SimplePrimitive(5);
            for (int i = 0; i < 5; i++)
                generator1.ConnectToInput(i, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConnectInputNegative()
        {
            IModifier generator1 = new SimplePrimitive(1);
            generator1.ConnectToInput(-1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConnectInvalidInput1()
        {
            IModifier generator1 = new SimplePrimitive(1);
            generator1.ConnectToInput(1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConnectInvalidInput2()
        {
            IModifier generator1 = new SimplePrimitive(5);
            generator1.ConnectToInput(5, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetInputNegative()
        {
            SimplePrimitive generator1 = new SimplePrimitive(1);
            primitive = generator1.CallGetInput(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetInputTooHigh1()
        {
            SimplePrimitive generator1 = new SimplePrimitive(1);
            primitive = generator1.CallGetInput(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetInputTooHigh2()
        {
            SimplePrimitive generator1 = new SimplePrimitive(5);
            primitive = generator1.CallGetInput(5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetInputNotConnected()
        {
            SimplePrimitive generator1 = new SimplePrimitive(5);
            primitive = generator1.CallGetInput(2);
        }

        [Test]
        public void TestGetInputOk()
        {
            // Setup
            SimplePrimitive generator1 = new SimplePrimitive(1);
            SimplePrimitive generator2 = new SimplePrimitive(1, new Vertex[1], new short[1], null);
            generator1.ConnectToInput(0, generator2);
            // Exercise SUT
            primitive = generator1.CallGetInput(0);
            // Verify
            Assert.AreEqual(generator2.primitive.Vertices, primitive.Vertices);
            Assert.AreEqual(generator2.primitive.Indices, primitive.Indices);
            Assert.AreEqual(generator2.primitive.Body, primitive.Body);
        }

    }
}
