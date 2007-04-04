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
        private ICubeTexture reflectiveTexture;
        private ITextureFactory textureFactory;
        private Material material;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            texture = mockery.NewMock<ITexture>();
            normalTexture = mockery.NewMock<ITexture>();
            reflectiveTexture = mockery.NewMock<ICubeTexture>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            material = new Material();
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestConstructorNoMaps()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.DiffuseTexture);
            Assert.IsNull(modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuse()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, texture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormal()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, texture, normalTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormalReflective()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, texture, normalTexture, reflectiveTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.AreEqual(reflectiveTexture, modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseFromString()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(texture));
            ModelMaterial modelMaterial = new ModelMaterial(material, "Texture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormalFromString()
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
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormalReflectiveFromString()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(texture));
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("NormalTexture").Will(Return.Value(normalTexture));
            Expect.Once.On(textureFactory).Method("CreateCubeFromFile").
                With("ReflectiveTexture").Will(Return.Value(reflectiveTexture));
            ModelMaterial modelMaterial =
                new ModelMaterial(material, "Texture", "NormalTexture", "ReflectiveTexture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.AreEqual(reflectiveTexture, modelMaterial.ReflectiveTexture);
        }

    }
}
