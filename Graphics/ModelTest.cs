using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelTest
    {
        private Mockery mockery;
        private IMesh mesh;
        private ITextureFactory textureFactory;
        private ITexture texture;
        private IEffectHandler effectHandler;
        private IEffect effect;
        private ExtendedMaterial[] materials;
        private Model model;
        private Matrix world = Matrix.RotationX(1);
        private Matrix view = Matrix.RotationY(1);
        private Matrix projection = Matrix.RotationZ(1);

        private ColorValue sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f);

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            texture = mockery.NewMock<ITexture>();
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();

            Material material;
            materials = new ExtendedMaterial[2];
            materials[0] = new ExtendedMaterial();
            material = new Material();
            material.Ambient = Color.AliceBlue;
            material.Diffuse = Color.Aquamarine;
            materials[0].Material3D = material;
            materials[1] = new ExtendedMaterial();
            materials[1].TextureFilename = "TextureFileName";

            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));
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
        public void TestDrawSubset()
        {
            ConstructorTest();

            Expect.Once.On(mesh).Method("DrawSubset").With(17);
            model.DrawSubset(17);
        }

        class MaterialMatcher : Matcher
        {
            private ExtendedMaterial material;

            public MaterialMatcher(ExtendedMaterial material)
            {
                this.material = material;
            }

            public override bool Matches(object o)
            {
                if (!(o is ModelMaterial)) return false;
                ModelMaterial m = (ModelMaterial)o;

                if (m.Ambient == material.Material3D.Diffuse &&
                    m.Diffuse == material.Material3D.Diffuse)
                    return true;

                return false;
            }

            public override void DescribeTo(TextWriter writer)
            {
                writer.Write(material.ToString());
            }
        }

        [Test]
        public void TestDraw()
        {
            ConstructorTest();

            using (mockery.Ordered)
            {
                //Subset 1
            //    Expect.Once.On(effectHandler).Method("SetNodeConstants").With(scene, node);
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials[0]), Is.EqualTo(0));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(0);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
                // Subset 2
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials[1]), Is.EqualTo(1));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(2));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("BeginPass").With(1);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
            }

            model.Draw(effectHandler, sceneAmbient, world, view, projection);
        }
    }
}
