using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class ClothPrimitiveTest : PrimitiveTest
    {
        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void TestNumParticlesInBody1()
        {
            Primitive cloth = Primitive.ClothPrimitive(10, 30, 1, 1);
            //Assert.AreEqual(cloth.Body.Particles.Count, cloth.Vertices.Length, 
            //    "The cloth should have as many particles as vertices.");
        }
    }
}
