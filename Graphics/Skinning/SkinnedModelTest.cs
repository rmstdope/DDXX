using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using System.IO;
using Microsoft.DirectX;

namespace Dope.DDXX.Graphics.Skinning
{
    [TestFixture]
    public class SkinnedModelTest
    {
        private Mockery mockery;
        private IAnimationRootFrame rootFrame;
        private IFrame frame1;
        private IFrame frame2;
        private IMeshContainer meshContainer;
        private IMeshData meshData;
        private IMesh mesh;
        private ITextureFactory textureFactory;
        private IEffectHandler effectHandler;
        private IEffect effect;
        private ExtendedMaterial[] materials;
        private SkinnedModel model;
        private Matrix world = Matrix.RotationX(1);
        private Matrix view = Matrix.RotationY(1);
        private Matrix projection = Matrix.RotationZ(1);
        private ColorValue sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f);

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            rootFrame = mockery.NewMock<IAnimationRootFrame>();
            frame1 = mockery.NewMock<IFrame>();
            frame2 = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            meshData = mockery.NewMock<IMeshData>();
            mesh = mockery.NewMock<IMesh>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            materials = new ExtendedMaterial[2];
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();

            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(frame1));
            Stub.On(meshContainer).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));
        }

        [Test]
        public void ConstructorTest()
        {
            materials[0].TextureFilename = "0";
            materials[1].TextureFilename = "1";

            // Find a real MeshContainer in second level 
            Expect.Once.On(frame1).GetProperty("MeshContainer").Will(Return.Value(null));
            Expect.Once.On(frame1).GetProperty("FrameFirstChild").Will(Return.Value(frame2));
            Stub.On(frame2).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Expect.Once.On(meshData).GetProperty("Mesh").Will(Return.Value(mesh));
            Expect.Once.On(meshContainer).Method("GetMaterials").Will(Return.Value(materials));

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("0");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("1");
            model = new SkinnedModel(rootFrame, textureFactory);
            Assert.AreSame(mesh, model.Mesh);
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
