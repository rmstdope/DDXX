using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class LightStateTest
    {
        private LightState lightState;

        [SetUp]
        public void SetUp()
        {
            lightState = new LightState();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestBuildup()
        {
            Vector3 position1 = new Vector3(0.1f, 0.2f, 0.3f);
            Vector3 direction1 = new Vector3(0.13f, 0.23f, 0.33f);
            ColorValue diffuse1 = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            ColorValue specular1 = new ColorValue(0.12f, 0.22f, 0.32f, 0.42f);
            Vector3 position2 = new Vector3(1.1f, 1.2f, 1.3f);
            Vector3 direction2 = new Vector3(1.14f, 1.24f, 1.34f);
            ColorValue diffuse2 = new ColorValue(1.11f, 1.21f, 1.31f, 1.41f);
            ColorValue specular2 = new ColorValue(1.12f, 1.22f, 1.32f, 1.42f);

            Assert.AreEqual(0, lightState.NumLights);

            lightState.NewState(position1, direction1, diffuse1, specular1);
            Assert.AreEqual(1, lightState.NumLights);

            lightState.NewState(position2, direction2, diffuse2, specular2);
            Assert.AreEqual(2, lightState.NumLights);
            Assert.AreEqual(lightState.Positions.Length, 2);
            Assert.AreEqual(lightState.Positions[0], new Vector4(position1.X, position1.Y, position1.Z, 1.0f));
            Assert.AreEqual(lightState.Positions[1], new Vector4(position2.X, position2.Y, position2.Z, 1.0f));
            Assert.AreEqual(lightState.Directions[0], new Vector4(direction1.X, direction1.Y, direction1.Z, 1.0f));
            Assert.AreEqual(lightState.Directions[1], new Vector4(direction2.X, direction2.Y, direction2.Z, 1.0f));
            Assert.AreEqual(lightState.DiffuseColor, new ColorValue[] { diffuse1, diffuse2 });
            Assert.AreEqual(lightState.SpecularColor, new ColorValue[] { specular1, specular2 });

        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestBuildupTooMany()
        {
            Vector3 position1 = new Vector3(0.1f, 0.2f, 0.3f);
            Vector3 direction1 = new Vector3(0.1f, 0.2f, 0.3f);
            ColorValue diffuse1 = new ColorValue(0.11f, 0.21f, 0.31f, 0.41f);
            ColorValue specular1 = new ColorValue(0.12f, 0.22f, 0.32f, 0.42f);

            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
        }
    }
}
