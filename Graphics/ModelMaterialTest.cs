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
        private Material material;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            texture = mockery.NewMock<ITexture>();
            material = new Material();
        }

        [TearDown]
        public void TearDown()
        {
        }

        bool CompareMaterial(Material m1, Material m2)
        {
            if (m1.AmbientColor.Equals(m2.AmbientColor) &&
                m1.DiffuseColor.Equals(m2.DiffuseColor) &&
                m1.EmissiveColor.Equals(m2.EmissiveColor) &&
                m1.SpecularColor.Equals(m2.SpecularColor) &&
                m1.SpecularSharpness.Equals(m2.SpecularSharpness))
                return true;

            return false;
        }

        [Test]
        public void TestConstructor()
        {
            ModelMaterial modelMaterial = new ModelMaterial(material);
            CompareMaterial(material, modelMaterial.Material);
            Assert.IsNull(modelMaterial.DiffuseTexture);

            modelMaterial = new ModelMaterial(material, texture);
            CompareMaterial(material, modelMaterial.Material);
            Assert.AreEqual(texture, modelMaterial.DiffuseTexture);
        }
    }
}
