using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class CustomModelTest : D3DMockTest
    {
        private CustomModel model;
        private IModelMesh mesh;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            mesh = mockery.NewMock<IModelMesh>();
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void Constructor()
        {
            // Exercise SUT
            model = new CustomModel(mesh);
            // Verify
            Assert.AreEqual(1, model.Meshes.Count);
            Assert.AreSame(mesh, model.Meshes[0]);
        }
    }
}
