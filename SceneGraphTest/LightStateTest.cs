using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            Color diffuse1 = new Color(1, 2, 3, 4);
            Color specular1 = new Color(5, 6, 7, 8);
            Vector3 position2 = new Vector3(1.1f, 1.2f, 1.3f);
            Vector3 direction2 = new Vector3(1.14f, 1.24f, 1.34f);
            Color diffuse2 = new Color(8, 7, 6, 5);
            Color specular2 = new Color(4, 3, 2, 1);

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
            Assert.AreEqual(lightState.DiffuseColor, new Vector4[] { diffuse1.ToVector4(), diffuse2.ToVector4() });
            Assert.AreEqual(lightState.SpecularColor, new Vector4[] { specular1.ToVector4(), specular2.ToVector4() });

        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestBuildupTooMany()
        {
            Vector3 position1 = new Vector3(0.1f, 0.2f, 0.3f);
            Vector3 direction1 = new Vector3(0.1f, 0.2f, 0.3f);
            Color diffuse1 = new Color(1, 2, 3, 4);
            Color specular1 = new Color(5, 6, 7, 8);

            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
            lightState.NewState(position1, direction1, diffuse1, specular1);
        }
    }
}
