using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

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
        public void DiffuseTest()
        {
            Assert.AreEqual(new Color(255, 255, 255, 255), lightNode.DiffuseColor);
            lightNode.DiffuseColor = new Color(1, 2, 3);
            Assert.AreEqual(new Color(1, 2, 3), lightNode.DiffuseColor);
        }

        [Test]
        public void SpecularTest()
        {
            Assert.AreEqual(new Color(255, 255, 255, 255), lightNode.SpecularColor);
            lightNode.SpecularColor = new Color(4, 5, 6);
            Assert.AreEqual(new Color(4, 5, 6), lightNode.SpecularColor);
        }

        [Test]
        public void TestSetLightState()
        {
            Color diffuse1 = new Color(1, 2, 3);
            Color specular1 = new Color(4, 5, 6);
            Vector3 direction1 = new Vector3(0.13f, 0.23f, 0.33f);
            Color diffuse2 = new Color(7, 8, 9);
            Color specular2 = new Color(10, 11, 12);
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
            Assert.AreEqual(state.DiffuseColor, new Vector4[] { diffuse1.ToVector4() });
            Assert.AreEqual(state.SpecularColor, new Vector4[] { specular1.ToVector4() });

            light.Direction = direction2;
            light.DiffuseColor = diffuse2;
            light.SpecularColor = specular2;
            light.SetLightState(state);
            direction2.Normalize();
            Assert.AreEqual(2, state.NumLights);
            Assert.AreEqual(state.Directions, new Vector4[] { 
                new Vector4(direction1.X, direction1.Y, direction1.Z, 1.0f),
                new Vector4(direction2.X, direction2.Y, direction2.Z, 1.0f) });
            Assert.AreEqual(state.DiffuseColor, new Vector4[] { diffuse1.ToVector4(), diffuse2.ToVector4() });
            Assert.AreEqual(state.SpecularColor, new Vector4[] { specular1.ToVector4(), specular2.ToVector4() });
        }
    }
}
