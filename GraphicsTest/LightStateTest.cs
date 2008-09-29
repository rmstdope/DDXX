using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
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
            Color specular1 = new Color(0, 1, 2, 3);
            Vector3 position2 = new Vector3(1.1f, 1.2f, 1.3f);
            Vector3 direction2 = new Vector3(1.14f, 1.24f, 1.34f);
            Color diffuse2 = new Color(8, 7, 6, 5);
            Color specular2 = new Color(9, 8, 7, 6);

            Assert.AreEqual(0, lightState.NumLights);

            lightState.NewState(position1, direction1, diffuse1, specular1);
            Assert.AreEqual(1, lightState.NumLights);

            lightState.NewState(position2, direction2, diffuse2, specular2);
            Assert.AreEqual(2, lightState.NumLights);
            Assert.AreEqual(lightState.Positions.Length, 2);
            Assert.AreEqual(lightState.Positions, new Vector3[] { position1, position2 });
            Assert.AreEqual(lightState.Directions, new Vector3[] { direction1, direction2 });
            Assert.AreEqual(lightState.DiffuseColor, new Vector3[] { diffuse1.ToVector3(), diffuse2.ToVector3() });
            Assert.AreEqual(lightState.SpecularColor, new Vector3[] { specular1.ToVector3(), specular2.ToVector3() });
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestBuildupTooMany()
        {
            Vector3 position1 = new Vector3(0.1f, 0.2f, 0.3f);
            Vector3 direction1 = new Vector3(0.1f, 0.2f, 0.3f);
            Color diffuse1 = new Color(1, 2, 3, 4);
            
            lightState.NewState(position1, direction1, diffuse1, diffuse1);
            lightState.NewState(position1, direction1, diffuse1, diffuse1);
            lightState.NewState(position1, direction1, diffuse1, diffuse1);
            lightState.NewState(position1, direction1, diffuse1, diffuse1);
            lightState.NewState(position1, direction1, diffuse1, diffuse1);
        }
    }
}
