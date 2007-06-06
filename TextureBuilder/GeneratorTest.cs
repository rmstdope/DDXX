using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    [TestFixture]
    public class GeneratorTest
    {
        private class SimpleGenerator : Generator
        {
            private Vector4 returnValue;

            public SimpleGenerator(int pins)
                : base(pins)
            {
            }

            public SimpleGenerator(int pins, Vector4 returnValue)
                : base(pins)
            {
                this.returnValue = returnValue;
            }

            public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
            {
                return returnValue;
            }

            public Vector4 CallGetInput(int inputPin)
            {
                return GetInput(inputPin, new Vector2());
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestTooFewInputs()
        {
            new SimpleGenerator(-1);
        }

        [Test]
        public void TestInputsOk()
        {
            for (int i = 0; i < 10; i++)
                new SimpleGenerator(i);
        }

        [Test]
        public void TestConnectOk()
        {
            IGenerator generator1 = new SimpleGenerator(1);
            generator1.ConnectToInput(0, null);
            generator1 = new SimpleGenerator(5);
            for (int i = 0; i < 5; i++)
                generator1.ConnectToInput(i, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConnectInputNegative()
        {
            IGenerator generator1 = new SimpleGenerator(1);
            generator1.ConnectToInput(-1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConnectInvalidInput1()
        {
            IGenerator generator1 = new SimpleGenerator(1);
            generator1.ConnectToInput(1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestConnectInvalidInput2()
        {
            IGenerator generator1 = new SimpleGenerator(5);
            generator1.ConnectToInput(5, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetInputNegative()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            generator1.CallGetInput(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetInputTooHigh1()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            generator1.CallGetInput(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestGetInputTooHigh2()
        {
            SimpleGenerator generator1 = new SimpleGenerator(5);
            generator1.CallGetInput(5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetInputNotConnected()
        {
            SimpleGenerator generator1 = new SimpleGenerator(5);
            generator1.CallGetInput(2);
        }

        [Test]
        public void TestGetInputOk1()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            SimpleGenerator generator2 = new SimpleGenerator(1, new Vector4(1, 2, 3, 4));
            generator1.ConnectToInput(0, generator2);
            Assert.AreEqual(new Vector4(1, 2, 3, 4), generator1.CallGetInput(0));
        }

        [Test]
        public void TestGetInputOk2()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            SimpleGenerator generator2 = new SimpleGenerator(1, new Vector4(5, 6, 7, 8));
            generator1.ConnectToInput(0, generator2);
            Assert.AreEqual(new Vector4(5, 6, 7, 8), generator1.CallGetInput(0));
        }

    }
}
