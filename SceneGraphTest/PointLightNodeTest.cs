using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class PointLightNodeTest
    {
        PointLightNode light;

        [SetUp]
        public void SetUp()
        {
            light = new PointLightNode("Light");
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
            Assert.AreEqual(new Vector3(0, 0, 0), light.Position);
            light.Position = new Vector3(1, 2, 3);
            Assert.AreEqual(new Vector3(1, 2, 3), light.Position);
        }

        [Test]
        public void DiffuseTest()
        {
            Assert.AreEqual(new Color(255, 255, 255, 255), light.Color);
            light.Color = new Color(1, 2, 3, 4);
            Assert.AreEqual(new Color(1, 2, 3, 4), light.Color);
        }

        [Test]
        public void TestSetLightState()
        {
            Color diffuse1 = new Color(1, 2, 3, 4);
            Vector3 position1 = new Vector3(0.13f, 0.23f, 0.33f);
            Color diffuse2 = new Color(8, 7, 6, 5);
            Vector3 position2 = new Vector3(0.16f, 0.26f, 0.36f);

            LightState state = new LightState();
            LightNode light = new PointLightNode("LightNode");

            light.Position = position1;
            light.Color = diffuse1;
            light.SetLightState(state);
            Assert.AreEqual(1, state.NumLights);
            Assert.AreEqual(state.Positions, new Vector3[] { position1 });
            Assert.AreEqual(state.Color, new Vector3[] { diffuse1.ToVector3() });

            light.Position = position2;
            light.Color = diffuse2;
            light.SetLightState(state);
            Assert.AreEqual(2, state.NumLights);
            Assert.AreEqual(state.Positions, 
                new Vector3[] { position1, position2 });
            Assert.AreEqual(state.Color, new Vector3[] { diffuse1.ToVector3(), diffuse2.ToVector3() });
        }
    }
}
