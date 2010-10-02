using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;

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

            protected override Vector4 GetPixel()
            {
                return returnValue;
            }

            public Vector4 CallGetInput(int inputPin)
            {
                return GetInputPixel(inputPin, new Vector2(), Vector2.Zero);
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TooFewInputs()
        {
            new SimpleGenerator(-1);
        }

        [Test]
        public void MultipleInputs()
        {
            for (int i = 0; i < 10; i++)
                new SimpleGenerator(i);
        }

        [Test]
        public void ConnectToNull()
        {
            ITextureGenerator generator1 = new SimpleGenerator(1);
            generator1.ConnectToInput(0, null);
            generator1 = new SimpleGenerator(5);
            for (int i = 0; i < 5; i++)
                generator1.ConnectToInput(i, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConnectInputNegative()
        {
            ITextureGenerator generator1 = new SimpleGenerator(1);
            generator1.ConnectToInput(-1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConnectInvalidInput1()
        {
            ITextureGenerator generator1 = new SimpleGenerator(1);
            generator1.ConnectToInput(1, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConnectInvalidInput2()
        {
            ITextureGenerator generator1 = new SimpleGenerator(5);
            generator1.ConnectToInput(5, null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetInputNegative()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            generator1.CallGetInput(-1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetInputTooHigh1()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            generator1.CallGetInput(1);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetInputTooHigh2()
        {
            SimpleGenerator generator1 = new SimpleGenerator(5);
            generator1.CallGetInput(5);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void GetInputNotConnected()
        {
            SimpleGenerator generator1 = new SimpleGenerator(5);
            generator1.CallGetInput(2);
        }

        //[Test]
        //public void GetInputOk1()
        //{
        //    SimpleGenerator generator1 = new SimpleGenerator(1);
        //    SimpleGenerator generator2 = new SimpleGenerator(1, new Vector4(1, 2, 3, 4));
        //    generator1.ConnectToInput(0, generator2);
        //    Assert.AreEqual(new Vector4(1, 2, 3, 4), generator1.CallGetInput(0));
        //}

        //[Test]
        //public void GetInputOk2()
        //{
        //    SimpleGenerator generator1 = new SimpleGenerator(1);
        //    SimpleGenerator generator2 = new SimpleGenerator(1, new Vector4(5, 6, 7, 8));
        //    generator1.ConnectToInput(0, generator2);
        //    Assert.AreEqual(new Vector4(5, 6, 7, 8), generator1.CallGetInput(0));
        //}

        [Test]
        public void GetReverseConnectionOk()
        {
            SimpleGenerator generator1 = new SimpleGenerator(1);
            SimpleGenerator generator2 = new SimpleGenerator(0);
            generator1.ConnectToInput(0, generator2);
            Assert.AreEqual(generator1, generator2.Output);
        }

        [Test]
        public void TraceInput()
        {
            SimpleGenerator generator1 = new SimpleGenerator(3);
            SimpleGenerator generator2 = new SimpleGenerator(0);
            SimpleGenerator generator3 = new SimpleGenerator(0);
            SimpleGenerator generator4 = new SimpleGenerator(0);
            generator1.ConnectToInput(0, generator2);
            generator1.ConnectToInput(1, generator3);
            generator1.ConnectToInput(2, generator4);
            Assert.AreEqual(0, generator1.GetInputIndex(generator2));
            Assert.AreEqual(1, generator1.GetInputIndex(generator3));
            Assert.AreEqual(2, generator1.GetInputIndex(generator4));
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TraceInputInvalid()
        {
            SimpleGenerator generator1 = new SimpleGenerator(0);
            SimpleGenerator generator2 = new SimpleGenerator(0);
            generator1.GetInputIndex(generator2);
        }

        [Test]
        public void NumGeneratorsInSingleGenerator()
        {
            SimpleGenerator g1 = new SimpleGenerator(0);
            Assert.AreEqual(1, g1.NumGeneratorsInChain);
        }

        [Test]
        public void NumGeneratorsInChain()
        {
            SimpleGenerator g1 = new SimpleGenerator(2);
            SimpleGenerator g2 = new SimpleGenerator(1);
            SimpleGenerator g3 = new SimpleGenerator(0);
            SimpleGenerator g4 = new SimpleGenerator(0);
            g2.ConnectToInput(0, g4);
            g1.ConnectToInput(0, g2);
            g1.ConnectToInput(1, g3);
            Assert.AreEqual(4, g1.NumGeneratorsInChain);
        }
    }
}
