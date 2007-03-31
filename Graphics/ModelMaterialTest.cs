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
        private ITextureFactory textureFactory;
        private Material material;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            texture = mockery.NewMock<ITexture>();
            normalTexture = mockery.NewMock<ITexture>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            material = new Material();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestConstructor1()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.DiffuseTexture);
            Assert.IsNull(modelMaterial.NormalTexture);
        }

        [Test]
        public void TestConstructor2()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, texture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);
        }

        [Test]
        public void TestConstructor3()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, texture, normalTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
        }

        [Test]
        public void TestConstructor4()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(texture));
            ModelMaterial modelMaterial = new ModelMaterial(material, "Texture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);
        }

        [Test]
        public void TestConstructor5()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(texture));
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("NormalTexture").Will(Return.Value(normalTexture));
            ModelMaterial modelMaterial = 
                new ModelMaterial(material, "Texture", "NormalTexture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
        }

    }
}
