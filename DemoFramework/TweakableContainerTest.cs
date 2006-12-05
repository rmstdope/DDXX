using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableContainerTest
    {
        private class TestClass : TweakableContainer
        {
            int intType;
            public int IntType
            {
                get { return intType; }
                set { intType = value; }
            }

            float floatType;
            public float FloatType
            {
                get { return floatType; }
                set { floatType = value; }
            }

            Vector3 vector3Type;
            public Vector3 Vector3Type
            {
                get { return vector3Type; }
                set { vector3Type = value; }
            }

            string stringType;
            public string StringType
            {
                get { return stringType; }
                set { stringType = value; }
            }

            Color colorType;
            public Color ColorType
            {
                get { return colorType; }
                set { colorType = value; }
            }

            int noSetter = 1;
            public int NoSetter
            {
                get { return noSetter; }
            }

            float noGetter;
            public float NoGetter
            {
                set { noGetter = value; }
            }

            DDXXException invalidType;
            public DDXXException InvalidType
            {
                get { return invalidType; }
                set { invalidType = value; }
            }

        }

        private TweakableContainer container;
        private enum Types
        {
            INT_TYPE,
            FLOAT_TYPE,
            VECTOR3_TYPE,
            STRING_TYPE,
            COLOR_TYPE
        }

        [SetUp]
        public void SetUp()
        {
            container = new TestClass();
        }

        [Test]
        public void TestEnumeration()
        {
            // Check that only properties with get _and_ set is returned
            Assert.AreEqual(5, container.GetNumTweakables());
        }

        [Test]
        public void TestNames()
        {
            Assert.AreEqual("IntType", container.GetTweakableName((int)Types.INT_TYPE));
            Assert.AreEqual("FloatType", container.GetTweakableName((int)Types.FLOAT_TYPE));
            Assert.AreEqual("Vector3Type", container.GetTweakableName((int)Types.VECTOR3_TYPE));
            Assert.AreEqual("StringType", container.GetTweakableName((int)Types.STRING_TYPE));
            Assert.AreEqual("ColorType", container.GetTweakableName((int)Types.COLOR_TYPE));
        }

        [Test]
        public void TestTypes()
        {
            Assert.AreEqual(TweakableType.Integer, container.GetTweakableType((int)Types.INT_TYPE));
            Assert.AreEqual(TweakableType.Float, container.GetTweakableType((int)Types.FLOAT_TYPE));
            Assert.AreEqual(TweakableType.Vector3, container.GetTweakableType((int)Types.VECTOR3_TYPE));
            Assert.AreEqual(TweakableType.String, container.GetTweakableType((int)Types.STRING_TYPE));
            Assert.AreEqual(TweakableType.Color, container.GetTweakableType((int)Types.COLOR_TYPE));
        }

        [Test]
        public void TestGetSetInt()
        {
            container.SetValue((int)Types.INT_TYPE, 10);
            Assert.AreEqual(10, container.GetIntValue((int)Types.INT_TYPE));
            container.SetValue((int)Types.INT_TYPE, -10);
            Assert.AreEqual(-10, container.GetIntValue((int)Types.INT_TYPE));
        }

        [Test]
        public void TestGetSetFloat()
        {
            container.SetValue((int)Types.FLOAT_TYPE, 10.0f);
            Assert.AreEqual(10.0f, container.GetFloatValue((int)Types.FLOAT_TYPE));
            container.SetValue((int)Types.FLOAT_TYPE, -10.0f);
            Assert.AreEqual(-10.0f, container.GetFloatValue((int)Types.FLOAT_TYPE));
        }

        [Test]
        public void TestGetSetVector3()
        {
            container.SetValue((int)Types.VECTOR3_TYPE, new Vector3(1, 2, 3));
            Assert.AreEqual(new Vector3(1, 2, 3), container.GetVector3Value((int)Types.VECTOR3_TYPE));
            container.SetValue((int)Types.VECTOR3_TYPE, new Vector3(-1, -2, -3));
            Assert.AreEqual(new Vector3(-1, -2, -3), container.GetVector3Value((int)Types.VECTOR3_TYPE));
        }

        [Test]
        public void TestGetSetString()
        {
            container.SetValue((int)Types.STRING_TYPE, "string1");
            Assert.AreEqual("string1", container.GetStringValue((int)Types.STRING_TYPE));
            container.SetValue((int)Types.STRING_TYPE, "string2");
            Assert.AreEqual("string2", container.GetStringValue((int)Types.STRING_TYPE));
        }

        [Test]
        public void TestGetSetColor()
        {
            container.SetValue((int)Types.COLOR_TYPE, Color.DarkBlue);
            Assert.AreEqual(Color.DarkBlue, container.GetColorValue((int)Types.COLOR_TYPE));
            container.SetValue((int)Types.COLOR_TYPE, Color.Cornsilk);
            Assert.AreEqual(Color.Cornsilk, container.GetColorValue((int)Types.COLOR_TYPE));
        }

        [Test]
        public void TestStepSize()
        {
            Assert.AreEqual(1.0f, container.GetStepSize((int)Types.INT_TYPE));
            Assert.AreEqual(1.0f, container.GetStepSize((int)Types.FLOAT_TYPE));
            Assert.AreEqual(1.0f, container.GetStepSize((int)Types.VECTOR3_TYPE));
            Assert.AreEqual(1.0f, container.GetStepSize((int)Types.STRING_TYPE));
            Assert.AreEqual(1.0f, container.GetStepSize((int)Types.COLOR_TYPE));
            container.SetStepSize((int)Types.FLOAT_TYPE, -0.1f);
            container.SetStepSize((int)Types.INT_TYPE, -0.2f);
            container.SetStepSize((int)Types.VECTOR3_TYPE, -0.3f);
            container.SetStepSize((int)Types.STRING_TYPE, -0.4f);
            container.SetStepSize((int)Types.COLOR_TYPE, -0.5f);
            Assert.AreEqual(-0.1f, container.GetStepSize((int)Types.FLOAT_TYPE));
            Assert.AreEqual(-0.2f, container.GetStepSize((int)Types.INT_TYPE));
            Assert.AreEqual(-0.3f, container.GetStepSize((int)Types.VECTOR3_TYPE));
            Assert.AreEqual(-0.4f, container.GetStepSize((int)Types.STRING_TYPE));
            Assert.AreEqual(-0.5f, container.GetStepSize((int)Types.COLOR_TYPE));
        }

        [Test]
        public void TestNameToNum()
        {
            Assert.AreEqual((int)Types.FLOAT_TYPE, container.GetTweakableNumber("FloatType"));
            Assert.AreEqual((int)Types.INT_TYPE, container.GetTweakableNumber("IntType"));
            Assert.AreEqual((int)Types.VECTOR3_TYPE, container.GetTweakableNumber("Vector3Type"));
            Assert.AreEqual((int)Types.STRING_TYPE, container.GetTweakableNumber("StringType"));
            Assert.AreEqual((int)Types.COLOR_TYPE, container.GetTweakableNumber("ColorType"));
        }
    }
}
