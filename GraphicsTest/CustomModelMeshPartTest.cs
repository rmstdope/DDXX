using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class CustomModelMeshPartTest : D3DMockTest
    {
        private CustomModelMeshPart part;

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

        [Test]
        public void VertexVariables()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(effect, 42, 43, 0, 0);
            // Verify
            Assert.AreEqual(42, part.BaseVertex);
            Assert.AreEqual(43, part.NumVertices);
        }

        [Test]
        public void IndexVariables()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(effect, 0, 0, 54, 55);
            // Verify
            Assert.AreEqual(54, part.StartIndex);
            Assert.AreEqual(55, part.PrimitiveCount);
        }

        [Test]
        public void Effect()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(effect, 0, 0, 0, 0);
            // Verify
            Assert.AreSame(effect, part.Effect);
            part.Effect = null;
            Assert.IsNull(part.Effect);
        }

        [Test]
        public void MaterialHandler()
        {
            // Exercise SUT
            part = new CustomModelMeshPart(effect, 0, 0, 0, 0);
            // Verify
            Assert.IsNotNull(part.MaterialHandler);
        }

    }
}
