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
        private IFrame firstFrame;
        private Matrix firstMatrix = Matrix.RotationX(1);
        private IFrame childFrame;
        private Matrix childMatrix = Matrix.RotationX(2);
        private IFrame childChildFrame;
        private Matrix childChildMatrix = Matrix.RotationX(3);
        private IFrame childChildSiblingFrame;
        private Matrix childChildSiblingMatrix = Matrix.RotationX(4);
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
            firstFrame = mockery.NewMock<IFrame>();
            childFrame = mockery.NewMock<IFrame>();
            childChildFrame = mockery.NewMock<IFrame>();
            childChildSiblingFrame = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            meshData = mockery.NewMock<IMeshData>();
            mesh = mockery.NewMock<IMesh>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            materials = new ExtendedMaterial[2];
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();

            // firstFrame
            //  |-childFrame
            //     |-childChildFrame
            //     |-childChildSiblingFrame - MeshContainer
            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(firstFrame));

            Stub.On(firstFrame).GetProperty("FrameFirstChild").Will(Return.Value(childFrame));
            Stub.On(firstFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(firstFrame).GetProperty("MeshContainer").Will(Return.Value(null));
            Stub.On(firstFrame).GetProperty("TransformationMatrix").Will(Return.Value(firstMatrix));
            
            Stub.On(childFrame).GetProperty("FrameFirstChild").Will(Return.Value(childChildFrame));
            Stub.On(childFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(childFrame).GetProperty("MeshContainer").Will(Return.Value(null));
            Stub.On(childFrame).GetProperty("TransformationMatrix").Will(Return.Value(childMatrix));

            Stub.On(childChildFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(childChildFrame).GetProperty("FrameSibling").Will(Return.Value(childChildSiblingFrame));
            Stub.On(childChildFrame).GetProperty("MeshContainer").Will(Return.Value(null));
            Stub.On(childChildFrame).GetProperty("TransformationMatrix").Will(Return.Value(childChildMatrix));

            Stub.On(childChildSiblingFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(childChildSiblingFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(childChildSiblingFrame).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Stub.On(childChildSiblingFrame).GetProperty("TransformationMatrix").Will(Return.Value(childChildSiblingMatrix));

            Stub.On(meshContainer).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));
            Stub.On(meshData).GetProperty("Mesh").Will(Return.Value(mesh));
            Stub.On(meshContainer).Method("GetMaterials").Will(Return.Value(materials));

        }

        [Test]
        public void ConstructorTest()
        {
            materials[0].TextureFilename = "0";
            materials[1].TextureFilename = "1";

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("0");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("1");
            model = new SkinnedModel(rootFrame, textureFactory);
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
            ModelMaterial[] modelMaterials = new ModelMaterial[] { new ModelMaterial(materials[0].Material3D), new ModelMaterial(materials[1].Material3D) };
            ConstructorTest();

            using (mockery.Ordered)
            {
                // Node
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With((childChildSiblingMatrix * (childMatrix * firstMatrix)) * world, view, projection);

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

            model.Draw(effectHandler, sceneAmbient, world, view, projection);
        }
    }
}
