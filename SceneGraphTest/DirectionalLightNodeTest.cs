using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class DirectionalLightNodeTest
    {
        DirectionalLightNode lightNode;

        [SetUp]
        public void SetUp()
        {
            lightNode = new DirectionalLightNode("Light");
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void ConstructorTest()
        {
            Assert.AreEqual("Light", lightNode.Name);
        }

        [Test]
        public void DirectionTest()
        {
            Assert.AreEqual(new Vector3(), lightNode.Direction);
            lightNode.Direction = new Vector3(1, 1, 1);
            Vector3 v = new Vector3(1, 1, 1);
            v.Normalize();
            Assert.AreEqual(v, lightNode.Direction);
            lightNode.Direction = new Vector3(1, 2, 3);
            v = new Vector3(1, 2, 3);
            v.Normalize();
            Assert.AreEqual(v, lightNode.Direction);
        }

        [Test]
        public void ColorTest()
        {
            Assert.AreEqual(new Color(255, 255, 255, 255), lightNode.Color);
            lightNode.Color = new Color(1, 2, 3);
            Assert.AreEqual(new Color(1, 2, 3), lightNode.Color);
        }

        [Test]
        public void TestSetLightState()
        {
            Color diffuse1 = new Color(1, 2, 3);
            Vector3 direction1 = new Vector3(0.13f, 0.23f, 0.33f);
            Color diffuse2 = new Color(7, 8, 9);
            Vector3 direction2 = new Vector3(0.16f, 0.26f, 0.36f);

            LightState state = new LightState();

            DirectionalLightNode light = new DirectionalLightNode("LightNode");

            light.Direction = direction1;
            light.Color = diffuse1;
            light.SetLightState(state);
            direction1.Normalize();
            Assert.AreEqual(1, state.NumLights);
            Assert.AreEqual(state.Directions, new Vector3[] { direction1 });

            light.Direction = direction2;
            light.Color = diffuse2;
            light.SetLightState(state);
            direction2.Normalize();
            Assert.AreEqual(2, state.NumLights);
            Assert.AreEqual(state.Directions, new Vector3[] { direction1, direction2 });
            Assert.AreEqual(state.Color, new Vector3[] { diffuse1.ToVector3(), diffuse2.ToVector3() });
        }
    }
}
