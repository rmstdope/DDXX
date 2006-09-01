using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    class LightNodeTest
    {
        [SetUp]
        public void SetUp()
        {
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void ConstructorTest()
        {
            Light light = new Light();
            LightNode node = new LightNode("Light", light);
            Assert.AreSame(light, node.Light);
        }

        [Test]
        public void TestSetLightState()
        {
            ColorValue diffuse1 = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            ColorValue specular1 = new ColorValue(0.12f, 0.22f, 0.32f, 0.42f);
            Vector3 position1 = new Vector3(0.13f, 0.23f, 0.33f);

            LightState state = new LightState();
            LightNode light = new LightNode("LightNode", new Light());

            light.WorldState.Position = position1;
            light.Light.DiffuseColor = diffuse1;
            light.Light.SpecularColor = specular1;

            light.SetLightState(state);

            Assert.AreEqual(1, state.NumLights);
            Assert.AreEqual(state.Positions, new Vector3[] { position1 });
            Assert.AreEqual(state.DiffuseColor, new ColorValue[] { diffuse1 });
            Assert.AreEqual(state.SpecularColor, new ColorValue[] { specular1 });
        }
    }
}
