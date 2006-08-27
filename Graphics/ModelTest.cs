using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelTest
    {
        Mockery mockery;
        IMesh mesh;
        ModelMaterial[] materials;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            materials = new ModelMaterial[2];
            materials[0] = new ModelMaterial(new Material());
            materials[1] = new ModelMaterial(new Material(), mockery.NewMock<ITexture>());
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ConstructorTest()
        {
            Model model = new Model(mesh, materials);
            Assert.AreSame(mesh, model.IMesh);
            Assert.AreEqual(materials.Length, model.Materials.Length);
            Assert.AreEqual(materials[0].Material.ToString(), model.Materials[0].Material.ToString());
            Assert.AreEqual(materials[1].Material.ToString(), model.Materials[1].Material.ToString());
            Assert.AreEqual(materials[0].DiffuseTexture, model.Materials[0].DiffuseTexture);
            Assert.AreEqual(materials[1].DiffuseTexture, model.Materials[1].DiffuseTexture);
        }
    }
}
