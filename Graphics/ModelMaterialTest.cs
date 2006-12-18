using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelMaterialTest
    {
        private Mockery mockery;
        private ITexture texture;
        private ITexture normalTexture;
        private Material material;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            texture = mockery.NewMock<ITexture>();
            normalTexture = mockery.NewMock<ITexture>();
            material = new Material();
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void TestConstructor()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.DiffuseTexture);
            Assert.IsNull(modelMaterial.NormalTexture);

            modelMaterial = new ModelMaterial(material, texture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);

            modelMaterial = new ModelMaterial(material, texture, normalTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
        }
    }
}
