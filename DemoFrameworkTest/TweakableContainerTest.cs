using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using System.Drawing;
using NMock2;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class TweakableContainerTest : IEffectChangeListener
    {
        private class EmptyContainer : TweakableContainer
        {
        };

        private class TestClass : TweakableContainer
        {
            private int intType;
            public int IntType
            {
                get { return intType; }
                set { intType = value; }
            }

            private float floatType;
            public float FloatType
            {
                get { return floatType; }
                set { floatType = value; }
            }

            private Vector3 vector3Type;
            public Vector3 Vector3Type
            {
                get { return vector3Type; }
                set { vector3Type = value; }
            }

            private string stringType;
            public string StringType
            {
                get { return stringType; }
                set { stringType = value; }
            }

            private Color colorType;
            public Color ColorType
            {
                get { return colorType; }
                set { colorType = value; }
            }

            private bool boolType;
            public bool BoolType
            {
                get { return boolType; }
                set { boolType = value; }
            }

            private int noSetter = 1;
            public int NoSetter
            {
                get { return noSetter; }
            }

            private float noGetter;
            public float NoGetter
            {
                set { noGetter = value; }
            }

            private DDXXException invalidType;
            public DDXXException InvalidType
            {
                get { return invalidType; }
                set { invalidType = value; }
            }

            private float startTime;
            public float StartTime
            {
                get { return startTime; }
                set { startTime = value; }
            }

            private float endTime;
            public float EndTime
            {
                get { return endTime; }
                set { endTime = value; }
            }
        }

        private TestClass container;

        private float startTime;
        private float endTime;
        private int intType;
        private float floatType;
        private Vector3 vector3Type;
        private string stringType;
        private Color colorType;
        private bool boolType;

        private enum Types
        {
            INT_TYPE,
            FLOAT_TYPE,
            VECTOR3_TYPE,
            STRING_TYPE,
            COLOR_TYPE,
            BOOL_TYPE
        }

        [SetUp]
        public void SetUp()
        {
            container = new TestClass();
            startTime = -1;
            endTime = -1;
            intType = -1;
            floatType = -1;
            vector3Type = new Vector3();
            stringType = null;
            colorType = new Color();
            boolType = false;
        }

        [Test]
        public void TestEnumeration()
        {
            // Check that only properties with get _and_ set is returned
            Assert.AreEqual(8, container.GetNumTweakables());
        }

        [Test]
        public void TestNames()
        {
            Assert.AreEqual("IntType", container.GetTweakableName((int)Types.INT_TYPE));
            Assert.AreEqual("FloatType", container.GetTweakableName((int)Types.FLOAT_TYPE));
            Assert.AreEqual("Vector3Type", container.GetTweakableName((int)Types.VECTOR3_TYPE));
            Assert.AreEqual("StringType", container.GetTweakableName((int)Types.STRING_TYPE));
            Assert.AreEqual("ColorType", container.GetTweakableName((int)Types.COLOR_TYPE));
            Assert.AreEqual("BoolType", container.GetTweakableName((int)Types.BOOL_TYPE));
        }

        [Test]
        public void TestTypes()
        {
            Assert.AreEqual(TweakableType.Integer, container.GetTweakableType((int)Types.INT_TYPE));
            Assert.AreEqual(TweakableType.Float, container.GetTweakableType((int)Types.FLOAT_TYPE));
            Assert.AreEqual(TweakableType.Vector3, container.GetTweakableType((int)Types.VECTOR3_TYPE));
            Assert.AreEqual(TweakableType.String, container.GetTweakableType((int)Types.STRING_TYPE));
            Assert.AreEqual(TweakableType.Color, container.GetTweakableType((int)Types.COLOR_TYPE));
            Assert.AreEqual(TweakableType.Bool, container.GetTweakableType((int)Types.BOOL_TYPE));
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
        public void TestGetSetBool()
        {
            container.SetValue((int)Types.BOOL_TYPE, true);
            Assert.IsTrue(container.GetBoolValue((int)Types.BOOL_TYPE));
            container.SetValue((int)Types.BOOL_TYPE, false);
            Assert.IsFalse(container.GetBoolValue((int)Types.BOOL_TYPE));
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
            Assert.AreEqual((int)Types.BOOL_TYPE, container.GetTweakableNumber("BoolType"));
        }

        [Test]
        public void TestUpdateStartTime()
        {
            container.StartTime = 1.0f;
            container.UpdateListener(this);
            Assert.AreEqual(1.0f, startTime);

            container.StartTime = 2.0f;
            container.UpdateListener(this);
            Assert.AreEqual(2.0f, startTime);
        }

        [Test]
        public void TestUpdateEndTime()
        {
            container.EndTime = 5.0f;
            container.UpdateListener(this);
            Assert.AreEqual(5.0f, endTime);

            container.EndTime = 2.0f;
            container.UpdateListener(this);
            Assert.AreEqual(2.0f, endTime);
        }

        [Test]
        public void TestUpdateIntParameter()
        {
            container.IntType = 2;
            container.UpdateListener(this);
            Assert.AreEqual(2, intType);

            container.IntType = -500;
            container.UpdateListener(this);
            Assert.AreEqual(-500, intType);
        }

        [Test]
        public void TestUpdateFloatParameter()
        {
            container.FloatType = 2.3f;
            container.UpdateListener(this);
            Assert.AreEqual(2.3f, floatType);

            container.FloatType = -5.34f;
            container.UpdateListener(this);
            Assert.AreEqual(-5.34f, floatType);
        }

        [Test]
        public void TestUpdateStringParameter()
        {
            container.StringType = "hejsan";
            container.UpdateListener(this);
            Assert.AreEqual("hejsan", stringType);

            container.StringType = "svejsan";
            container.UpdateListener(this);
            Assert.AreEqual("svejsan", stringType);
        }

        [Test]
        public void TestUpdateVector3Parameter()
        {
            container.Vector3Type = new Vector3(1, 2, 3);
            container.UpdateListener(this);
            Assert.AreEqual(new Vector3(1, 2, 3), vector3Type);

            container.Vector3Type = new Vector3(-1, -2, -3);
            container.UpdateListener(this);
            Assert.AreEqual(new Vector3(-1, -2, -3), vector3Type);
        }

        [Test]
        public void TestUpdateColorParameter()
        {
            container.ColorType = Color.Yellow;
            container.UpdateListener(this);
            Assert.AreEqual(Color.Yellow, colorType);

            container.ColorType = Color.Black;
            container.UpdateListener(this);
            Assert.AreEqual(Color.Black, colorType);
        }

        [Test]
        public void TestUpdateBoolParameter()
        {
            container.BoolType = true;
            container.UpdateListener(this);
            Assert.IsTrue(boolType);

            container.BoolType = false;
            container.UpdateListener(this);
            Assert.IsFalse(boolType);
        }

        #region IEffectChangeListener Members

        public void SetStartTime(string effectName, float value)
        {
            Assert.AreEqual("TestClass", effectName);
            startTime = value;
        }

        public void SetEndTime(string effectName, float value)
        {
            Assert.AreEqual("TestClass", effectName);
            endTime = value;
        }

        public void SetColorParam(string effectName, string param, Color value)
        {
            Assert.AreEqual("TestClass", effectName);
            Assert.AreEqual("ColorType", param);
            colorType = value;
        }

        public void SetFloatParam(string effectName, string param, float value)
        {
            Assert.AreEqual("TestClass", effectName);
            Assert.AreEqual("FloatType", param);
            floatType = value;
        }

        public void SetIntParam(string effectName, string param, int value)
        {
            Assert.AreEqual("TestClass", effectName);
            Assert.AreEqual("IntType", param);
            intType = value;
        }

        public void SetStringParam(string effectName, string param, string value)
        {
            Assert.AreEqual("TestClass", effectName);
            Assert.AreEqual("StringType", param);
            stringType = value;
        }

        public void SetVector3Param(string effectName, string param, Vector3 value)
        {
            Assert.AreEqual("TestClass", effectName);
            Assert.AreEqual("Vector3Type", param);
            vector3Type = value;
        }

        public void SetBoolParam(string effectName, string param, bool value)
        {
            Assert.AreEqual("TestClass", effectName);
            Assert.AreEqual("BoolType", param);
            boolType = value;
        }

        #endregion
    }
}
