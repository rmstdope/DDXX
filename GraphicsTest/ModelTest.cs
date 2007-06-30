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
        private IDevice device;
        private IRenderStateManager renderState;
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
            device = mockery.NewMock<IDevice>();
            renderState = mockery.NewMock<IRenderStateManager>();

            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));
            Stub.On(device).GetProperty("RenderState").Will(Return.Value(renderState));
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void TestConstructorOneMaterial()
        {
            Material material;
            materials = new ExtendedMaterial[1];
            materials[0] = new ExtendedMaterial();
            material = new Material();
            material.Ambient = Color.AliceBlue;
            material.Diffuse = Color.Aquamarine;
            materials[0].Material3D = material;
            materials[0].TextureFilename = "TextureFileName2";

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("TextureFileName2").Will(Return.Value(texture));
            model = new Model(mesh, textureFactory, materials);
            Assert.AreEqual(materials.Length, model.Materials.Length);
            // Check that ambient is set to diffuse
            material = materials[0].Material3D;
            material.Ambient = material.Diffuse;
            Assert.AreEqual(materials[0].Material3D.Diffuse, model.Materials[0].Ambient);
            Assert.AreEqual(materials[0].Material3D.Diffuse, model.Materials[0].Diffuse);
            Assert.AreEqual(texture, model.Materials[0].DiffuseTexture);
        }

        [Test]
        public void TestConstructorTwoMaterials()
        {
            Material material;
            materials = new ExtendedMaterial[2];
            materials[0] = new ExtendedMaterial();
            material = new Material();
            material.Ambient = Color.AliceBlue;
            material.Diffuse = Color.Aquamarine;
            materials[0].Material3D = material;
            materials[1] = new ExtendedMaterial();
            materials[1].TextureFilename = "TextureFileName";

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("TextureFileName").Will(Return.Value(texture));
            model = new Model(mesh, textureFactory, materials);
            Assert.AreEqual(materials.Length, model.Materials.Length);
            // Check that ambient is set to diffuse
            material = materials[0].Material3D;
            material.Ambient = material.Diffuse;
            Assert.AreEqual(materials[0].Material3D.Diffuse, model.Materials[0].Ambient);
            Assert.AreEqual(materials[0].Material3D.Diffuse, model.Materials[0].Diffuse);
            Assert.AreEqual(materials[1].Material3D.Diffuse, model.Materials[1].Ambient);
            Assert.AreEqual(materials[1].Material3D.Diffuse, model.Materials[1].Diffuse);
            Assert.AreEqual(null, model.Materials[0].DiffuseTexture);
            Assert.AreEqual(texture, model.Materials[1].DiffuseTexture);
        }

        /// <summary>
        /// Test that a model can be cloned, or rather that the materials get cloned.
        /// </summary>
        [Test]
        public void CloneTest()
        {
            TestConstructorTwoMaterials();

            IModel newModel = model.Clone();
            Assert.AreNotSame(model, newModel);
            CompareModelMaterials(newModel, model);
            Assert.AreSame(model.Mesh, newModel.Mesh);
        }

        private void CompareModelMaterials(IModel model1, IModel model2)
        {
            Assert.AreEqual(model2.Materials.Length, model1.Materials.Length);
            Assert.AreNotSame(model2.Materials, model1.Materials);
            for (int i = 0; i < model2.Materials.Length; i++)
            {
                Assert.AreNotSame(model2.Materials[i], model1.Materials[i]);
                Assert.AreEqual(model2.Materials[i].Ambient, model1.Materials[i].Ambient);
                Assert.AreEqual(model2.Materials[i].Diffuse, model1.Materials[i].Diffuse);
                Assert.AreEqual(model2.Materials[i].Specular, model1.Materials[i].Specular);
                Assert.AreEqual(model2.Materials[i].DiffuseTexture, model1.Materials[i].DiffuseTexture);
                Assert.AreEqual(model2.Materials[i].NormalTexture, model1.Materials[i].NormalTexture);
                Assert.AreEqual(model2.Materials[i].ReflectiveTexture, model1.Materials[i].ReflectiveTexture);
                Assert.AreEqual(model2.Materials[i].ReflectiveFactor, model1.Materials[i].ReflectiveFactor);
            }
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
        public void TestRenderOneMaterial()
        {
            TestConstructorOneMaterial();

            using (mockery.Ordered)
            {
                // Node
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With(world, view, projection);
                Expect.Once.On(renderState).SetProperty("CullMode").To(Cull.CounterClockwise);

                //Subset 1
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials[0]), Is.EqualTo(0));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(0);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
            }

            model.Render(device, effectHandler, sceneAmbient, world, view, projection);
        }

        [Test]
        public void TestRenderTwoMaterials()
        {
            TestConstructorTwoMaterials();

            model.CullMode = Cull.None;

            using (mockery.Ordered)
            {
                // Node
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With(world, view, projection);
                Expect.Once.On(renderState).SetProperty("CullMode").To(Cull.None);

                //Subset 1
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

            model.Render(device, effectHandler, sceneAmbient, world, view, projection);
        }
    }
}
