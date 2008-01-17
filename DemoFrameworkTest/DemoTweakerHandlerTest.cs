using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Reflection;
using NMock2;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class DemoTweakerHandlerTest
    {
        private class TestHelper
        {
            public bool B { set { } }
            public Color C { set { } }
            public int I { set { } }
            public float F { set { } }
            public string S { set { } }
            public Vector2 V2 { set { } }
            public Vector3 V3 { set { } }
            public Vector4 V4 { set { } }
        }

        private DemoTweakerHandler handler;
        private TestHelper helper;
        private Mockery mockery;
        //private ITweakableObject demoTweakable;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            //demoTweakable = mockery.NewMock<ITweakableObject>();
            //Stub.On(demoTweakable).GetProperty("NumVisableVariables").Will(Return.Value(1));
            handler = new DemoTweakerHandler(null, null);
            helper = new TestHelper();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void CreateTweakableBoolean()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("B");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableBoolean), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableBoolean).Property);
        }

        [Test]
        public void CreateTweakableColor()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("C");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableColor), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableColor).Property);
        }

        [Test]
        public void CreateTweakableInt32()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("I");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableInt32), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableInt32).Property);
        }

        [Test]
        public void CreateTweakableSingle()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("F");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableSingle), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableSingle).Property);
        }

        [Test]
        public void CreateTweakableString()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("S");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableString), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableString).Property);
        }

        [Test]
        public void CreateTweakableVector2()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("V2");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableVector2), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableVector2).Property);
        }

        [Test]
        public void CreateTweakableVector3()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("V3");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableVector3), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableVector3).Property);
        }

        [Test]
        public void CreateTweakableVector4()
        {
            // Exercise SUT
            PropertyInfo property = helper.GetType().GetProperty("V4");
            ITweakableValue tweakable = handler.CreateTweakableValue(property, helper);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableVector4), tweakable);
            Assert.AreEqual(property, (tweakable as TweakableVector4).Property);
        }

        [Test]
        public void CreateTweakableTrack()
        {
            // Exercise SUT
            Track track = new Track();
            ITweakableObject tweakable = handler.CreateTweakableObject(track);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableTrack), tweakable);
        }

        [Test]
        public void CreateTweakableRegisterable()
        {
            // Exercise SUT
            Registerable registerable = new Registerable("", 0, 0);
            ITweakableObject tweakable = handler.CreateTweakableObject(registerable);
            // Verify
            Assert.IsInstanceOfType(typeof(TweakableRegisterable), tweakable);
        }
    }
}
