using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelMaterialTest
    {
        private Mockery mockery;
        private ITexture diffuseTexture;
        private ITexture normalTexture;
        private ICubeTexture reflectiveTexture;
        private ITextureFactory textureFactory;
        private Material material;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            diffuseTexture = mockery.NewMock<ITexture>();
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

        /// <summary>
        /// Test the constructor where no textures are given.
        /// </summary>
        [Test]
        public void TestConstructorNoMaps()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(0.0f, modelMaterial.ReflectiveFactor);
            Assert.IsNull(modelMaterial.DiffuseTexture);
            Assert.IsNull(modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestReflectiveFactorFail1()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            modelMaterial.ReflectiveFactor = -0.1f;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestReflectiveFactorFail2()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            modelMaterial.ReflectiveFactor = 1.1f;
        }

        [Test]
        public void TestReflectiveFactor()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            modelMaterial.ReflectiveFactor = 0.3f;
            Assert.AreEqual(0.3f, modelMaterial.ReflectiveFactor);
        }

        [Test]
        public void TestConstructorDiffuse()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, diffuseTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(diffuseTexture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormal()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, diffuseTexture, normalTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(diffuseTexture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormalReflective()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material, diffuseTexture, normalTexture, reflectiveTexture);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(diffuseTexture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.AreEqual(reflectiveTexture, modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseFromString()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(diffuseTexture));
            ModelMaterial modelMaterial = new ModelMaterial(material, "Texture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(diffuseTexture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.IsNull(modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormalFromString()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(diffuseTexture));
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("NormalTexture").Will(Return.Value(normalTexture));
            ModelMaterial modelMaterial = 
                new ModelMaterial(material, "Texture", "NormalTexture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(diffuseTexture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.IsNull(modelMaterial.ReflectiveTexture);
        }

        [Test]
        public void TestConstructorDiffuseNormalReflectiveFromString()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("Texture").Will(Return.Value(diffuseTexture));
            Expect.Once.On(textureFactory).Method("CreateFromFile").
                With("NormalTexture").Will(Return.Value(normalTexture));
            Expect.Once.On(textureFactory).Method("CreateCubeFromFile").
                With("ReflectiveTexture").Will(Return.Value(reflectiveTexture));
            ModelMaterial modelMaterial =
                new ModelMaterial(material, "Texture", "NormalTexture", "ReflectiveTexture", textureFactory);
            Assert.AreEqual(material.Ambient, modelMaterial.Ambient);
            Assert.AreEqual(material.Diffuse, modelMaterial.Diffuse);
            Assert.AreEqual(diffuseTexture, modelMaterial.DiffuseTexture);
            Assert.AreEqual(material.Specular, modelMaterial.Specular);
            Assert.AreEqual(normalTexture, modelMaterial.NormalTexture);
            Assert.AreEqual(reflectiveTexture, modelMaterial.ReflectiveTexture);
        }

        /// <summary>
        /// Test the clone function for a material without maps.
        /// </summary>
        [Test]
        public void TestCloneNoMaps()
        {
            material.Ambient = Color.AliceBlue;
            material.Diffuse = Color.AntiqueWhite;
            material.Specular = Color.Aqua;
            material.SpecularSharpness = 0.2f;
            ModelMaterial modelMaterial1 = new ModelMaterial(material);
            modelMaterial1.ReflectiveFactor = 0.1f;
            ModelMaterial modelMaterial2 = modelMaterial1.Clone();
            CompareModelMaterials(modelMaterial1, modelMaterial2);
        }

        /// <summary>
        /// Test the clone function for a material with maps.
        /// </summary>
        [Test]
        public void TestCloneMaps()
        {
            material.Ambient = Color.Aquamarine;
            material.Diffuse = Color.Azure;
            material.Specular = Color.Beige;
            material.SpecularSharpness = 0.3f;
            ModelMaterial modelMaterial1 = new ModelMaterial(material, diffuseTexture, normalTexture, reflectiveTexture);
            modelMaterial1.ReflectiveFactor = 0.2f;
            ModelMaterial modelMaterial2 = modelMaterial1.Clone();
            CompareModelMaterials(modelMaterial1, modelMaterial2);
        }

        private static void CompareModelMaterials(ModelMaterial modelMaterial1, ModelMaterial modelMaterial2)
        {
            Assert.AreEqual(modelMaterial1.Ambient, modelMaterial2.Ambient);
            Assert.AreEqual(modelMaterial1.Diffuse, modelMaterial2.Diffuse);
            Assert.AreEqual(modelMaterial1.Specular, modelMaterial2.Specular);
            Assert.AreEqual(modelMaterial1.ReflectiveFactor, modelMaterial2.ReflectiveFactor);
            Assert.AreEqual(modelMaterial1.DiffuseTexture, modelMaterial2.DiffuseTexture);
            Assert.AreEqual(modelMaterial1.NormalTexture, modelMaterial2.NormalTexture);
            Assert.AreEqual(modelMaterial1.ReflectiveTexture, modelMaterial2.ReflectiveTexture);
        }

    }
}
