using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class CustomModelMeshPartTest : D3DMockTest
    {
        private CustomModelMeshPart part;
        private IMaterialHandler material;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            material = mockery.NewMock<IMaterialHandler>();
            Stub.On(material).GetProperty("Effect").Will(Return.Value(effect));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void VertexVariables()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(material, 42, 43, 0, 0);
            // Verify
            Assert.AreEqual(42, part.BaseVertex);
            Assert.AreEqual(43, part.NumVertices);
        }

        [Test]
        public void IndexVariables()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(material, 0, 0, 54, 55);
            // Verify
            Assert.AreEqual(54, part.StartIndex);
            Assert.AreEqual(55, part.PrimitiveCount);
        }

        [Test]
        public void Effect()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(material, 0, 0, 0, 0);
            // Verify
            Assert.AreSame(effect, part.Effect);
        }

        [Test]
        public void MaterialHandler()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(material, 0, 0, 0, 0);
            // Verify
            Assert.AreSame(material, part.MaterialHandler);
        }

    }
}
