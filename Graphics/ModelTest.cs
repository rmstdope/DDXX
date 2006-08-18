using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelTest
    {
        Mockery mockery;
        IMesh mesh;
        ExtendedMaterial[] materials;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            materials = new ExtendedMaterial[2];
            materials[0] = new ExtendedMaterial();
            materials[1] = new ExtendedMaterial();
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
            Assert.AreEqual(materials.Length, model.GetMaterials().Length);
            Assert.AreEqual(materials[0].ToString(), model.GetMaterials()[0].ToString());
            Assert.AreEqual(materials[1].ToString(), model.GetMaterials()[1].ToString());
        }
    }
}
