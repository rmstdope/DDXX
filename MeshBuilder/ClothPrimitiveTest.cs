using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

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

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestNumParticlesInBody1()
        {
            Expect.Exactly(4).On(body).Method("AddParticle").With(Is.NotNull);
            Primitive cloth = Primitive.ClothPrimitive(body, 10, 30, 1, 1);
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestNumParticlesInBody2()
        {
            Expect.Exactly(15).On(body).Method("AddParticle").With(Is.NotNull);
            Primitive cloth = Primitive.ClothPrimitive(body, 20, 40, 4, 2);
        }

    }
}
