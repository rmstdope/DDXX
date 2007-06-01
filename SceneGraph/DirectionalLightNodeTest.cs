using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class DirectionalLightNodeTest
    {
        DirectionalLightNode light;

        [SetUp]
        public void SetUp()
        {
            light = new DirectionalLightNode("Light");
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.AreEqual("Light", light.Name);
        }

        [Test]
        public void PositionTest()
        {
            Vector3 v = new Vector3(1, 1, 1);
            v.Normalize();
            Assert.AreEqual(v, light.Direction);
            light.Direction = new Vector3(1, 2, 3);
            v = new Vector3(1, 2, 3);
            v.Normalize();
            Assert.AreEqual(v, light.Direction);
        }

        [Test]
        public void DiffuseTest()
        {
            Assert.AreEqual(new ColorValue(1, 1, 1, 1), light.DiffuseColor);
            light.DiffuseColor = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            Assert.AreEqual(new ColorValue(0.11f, 0.21f, 0.31f, 0.41f), light.DiffuseColor);
        }

        [Test]
        public void SpecularTest()
        {
            Assert.AreEqual(new ColorValue(1, 1, 1, 1), light.SpecularColor);
            light.SpecularColor = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            Assert.AreEqual(new ColorValue(0.11f, 0.21f, 0.31f, 0.41f), light.SpecularColor);
        }

        [Test]
        public void TestSetLightState()
        {
            ColorValue diffuse1 = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            ColorValue specular1 = new ColorValue(0.12f, 0.22f, 0.32f, 0.42f);
            Vector3 direction1 = new Vector3(0.13f, 0.23f, 0.33f);
            ColorValue diffuse2 = new ColorValue(0.14f, 0.24f, 0.34f, 0.44f);
            ColorValue specular2 = new ColorValue(0.15f, 0.25f, 0.35f, 0.45f);
            Vector3 direction2 = new Vector3(0.16f, 0.26f, 0.36f);

            LightState state = new LightState();
            DirectionalLightNode light = new DirectionalLightNode("LightNode");

            light.Direction = direction1;
            light.DiffuseColor = diffuse1;
            light.SpecularColor = specular1;
            light.SetLightState(state);
            direction1.Normalize();
            Assert.AreEqual(1, state.NumLights);
            Assert.AreEqual(state.Directions, new Vector4[] { new Vector4(direction1.X, direction1.Y, direction1.Z, 1.0f) });
            Assert.AreEqual(state.DiffuseColor, new ColorValue[] { diffuse1 });
            Assert.AreEqual(state.SpecularColor, new ColorValue[] { specular1 });

            light.Direction = direction2;
            light.DiffuseColor = diffuse2;
            light.SpecularColor = specular2;
            light.SetLightState(state);
            direction2.Normalize();
            Assert.AreEqual(2, state.NumLights);
            Assert.AreEqual(state.Directions, new Vector4[] { 
                new Vector4(direction1.X, direction1.Y, direction1.Z, 1.0f),
                new Vector4(direction2.X, direction2.Y, direction2.Z, 1.0f) });
            Assert.AreEqual(state.DiffuseColor, new ColorValue[] { diffuse1, diffuse2 });
            Assert.AreEqual(state.SpecularColor, new ColorValue[] { specular1, specular2 });
        }
    }
}
