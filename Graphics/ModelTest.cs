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
    public class ModelTest
    {
        Mockery mockery;
        IMesh mesh;
        ITextureFactory textureFactory;
        ITexture texture;
        ExtendedMaterial[] materials;
        Model model;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            texture = mockery.NewMock<ITexture>();

            Material material;
            materials = new ExtendedMaterial[2];
            materials[0] = new ExtendedMaterial();
            material = new Material();
            material.Ambient = Color.AliceBlue;
            material.Diffuse = Color.Aquamarine;
            materials[0].Material3D = material;
            materials[1] = new ExtendedMaterial();
            materials[1].TextureFilename = "TextureFileName";
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ConstructorTest()
        {
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("TextureFileName").Will(Return.Value(texture));
            model = new Model(mesh, textureFactory, materials);
            Assert.AreEqual(materials.Length, model.Materials.Length);
            // Check that ambient is set to diffuse
            Material material = materials[0].Material3D;
            material.Ambient = material.Diffuse;
            Assert.AreEqual(materials[0].Material3D.Diffuse, model.Materials[0].Ambient);
            Assert.AreEqual(materials[0].Material3D.Diffuse, model.Materials[0].Diffuse);
            Assert.AreEqual(materials[1].Material3D.Diffuse, model.Materials[1].Ambient);
            Assert.AreEqual(materials[1].Material3D.Diffuse, model.Materials[1].Diffuse);
            Assert.AreEqual(null, model.Materials[0].DiffuseTexture);
            Assert.AreEqual(texture, model.Materials[1].DiffuseTexture);
        }

        [Test]
        public void TestDraw()
        {
            ConstructorTest();

            Expect.Once.On(mesh).Method("DrawSubset").With(17);
            model.DrawSubset(17);
        }
    }
}
