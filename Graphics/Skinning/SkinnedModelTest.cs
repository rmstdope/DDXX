using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using System.IO;
using Microsoft.DirectX;
using Dope.DDXX.Utility;
using System.Drawing;
using NMock2.Actions;

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
        private IMeshContainer meshContainer1;
        private IMeshContainer meshContainer2;
        private MeshDataAdapter meshData;
        private IMesh mesh;
        private ITextureFactory textureFactory;
        private IEffectHandler effectHandler;
        private IEffect effect;
        private ExtendedMaterial[] materials1;
        private ExtendedMaterial[] materials2;
        private SkinnedModel model;
        private Matrix world = Matrix.RotationX(1);
        private Matrix view = Matrix.RotationY(1);
        private Matrix projection = Matrix.RotationZ(1);
        private ColorValue sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f);
        private IAnimationController animationController;
        private ISkinInformation skinInformation;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            rootFrame = mockery.NewMock<IAnimationRootFrame>();
            firstFrame = mockery.NewMock<IFrame>();
            childFrame = mockery.NewMock<IFrame>();
            childChildFrame = mockery.NewMock<IFrame>();
            childChildSiblingFrame = mockery.NewMock<IFrame>();
            meshContainer1 = mockery.NewMock<IMeshContainer>();
            meshContainer2 = mockery.NewMock<IMeshContainer>();
            mesh = mockery.NewMock<IMesh>();
            meshData = new MeshDataAdapter();
            meshData.Mesh = mesh;
            textureFactory = mockery.NewMock<ITextureFactory>();
            materials1 = new ExtendedMaterial[1];
            materials2 = new ExtendedMaterial[2];
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();
            animationController = mockery.NewMock<IAnimationController>();
            skinInformation = mockery.NewMock<ISkinInformation>();

            // firstFrame
            //  |-childFrame - meshContainer1
            //     |-childChildFrame
            //     |-childChildSiblingFrame - MeshContainer2
            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(firstFrame));

            Stub.On(firstFrame).GetProperty("FrameFirstChild").Will(Return.Value(childFrame));
            Stub.On(firstFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(firstFrame).GetProperty("MeshContainer").Will(Return.Value(null));
            Stub.On(firstFrame).GetProperty("TransformationMatrix").Will(Return.Value(firstMatrix));
            
            Stub.On(childFrame).GetProperty("FrameFirstChild").Will(Return.Value(childChildFrame));
            Stub.On(childFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(childFrame).GetProperty("MeshContainer").Will(Return.Value(meshContainer1));
            Stub.On(childFrame).GetProperty("TransformationMatrix").Will(Return.Value(childMatrix));

            Stub.On(childChildFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(childChildFrame).GetProperty("FrameSibling").Will(Return.Value(childChildSiblingFrame));
            Stub.On(childChildFrame).GetProperty("MeshContainer").Will(Return.Value(null));
            Stub.On(childChildFrame).GetProperty("TransformationMatrix").Will(Return.Value(childChildMatrix));

            Stub.On(childChildSiblingFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(childChildSiblingFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(childChildSiblingFrame).GetProperty("MeshContainer").Will(Return.Value(meshContainer2));
            Stub.On(childChildSiblingFrame).GetProperty("TransformationMatrix").Will(Return.Value(childChildSiblingMatrix));

            Stub.On(meshContainer1).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(meshContainer2).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(meshContainer1).Method("GetMaterials").Will(Return.Value(materials1));
            Stub.On(meshContainer2).Method("GetMaterials").Will(Return.Value(materials2));
            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));

        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        /// <summary>
        /// Test that material textures are loaded as planned
        /// </summary>
        [Test]
        public void ConstructorMaterialTest()
        {
            Stub.On(meshContainer1).GetProperty("SkinInformation").Will(Return.Value(null));
            Stub.On(meshContainer2).GetProperty("SkinInformation").Will(Return.Value(null));

            Material mat = new Material();
            mat.Diffuse = Color.AliceBlue;
            materials1[0].Material3D = mat;
            materials1[0].TextureFilename = "0";
            mat.Diffuse = Color.AntiqueWhite;
            materials2[0].Material3D = mat;
            materials2[0].TextureFilename = "1";
            mat.Diffuse = Color.Aqua;
            materials2[1].Material3D = mat;
            materials2[1].TextureFilename = "2";

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("0");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("1");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("2");

            model = new SkinnedModel(rootFrame, textureFactory);
        }

        /// <summary>
        /// Test that the correct number of bones are used.
        /// </summary>
        [Test]
        public void Constructor10BonesTest()
        {
            IMesh newMesh = mockery.NewMock<IMesh>();
            IGraphicsStream adj = mockery.NewMock<IGraphicsStream>();
            Stub.On(meshContainer1).GetProperty("SkinInformation").Will(Return.Value(null));
            Stub.On(meshContainer2).GetProperty("SkinInformation").Will(Return.Value(skinInformation));
            Stub.On(skinInformation).GetProperty("NumberBones").Will(Return.Value(10));
            Stub.On(meshContainer2).Method("GetAdjacencyStream").Will(Return.Value(adj));

            ResultSynthesizer result = new ResultSynthesizer();
            result.SetResult(typeof(IMesh), newMesh);
            SetNamedParameterAction namedParam1 =
                new SetNamedParameterAction("maxFaceInfluence", 20);
            SetNamedParameterAction namedParam2 =
                new SetNamedParameterAction("boneCombinationTable", null);
            Expect.Once.On(skinInformation).Method("ConvertToIndexedBlendedMesh").
                With(Is.EqualTo(mesh), Is.EqualTo(MeshFlags.OptimizeVertexCache | MeshFlags.Managed),
                Is.EqualTo(adj), Is.EqualTo(10), Is.Out, Is.Out).
                Will(new IAction[] { namedParam1, namedParam2, result });
            Expect.Once.On(meshContainer2).SetProperty("MeshData");

            model = new SkinnedModel(rootFrame, textureFactory);
        }

        /// <summary>
        /// Test that the number of bones used are clamped to 40.
        /// </summary>
        [Test]
        public void Constructor41BonesTest()
        {
            IMesh newMesh = mockery.NewMock<IMesh>();
            IGraphicsStream adj = mockery.NewMock<IGraphicsStream>();
            Stub.On(meshContainer1).GetProperty("SkinInformation").Will(Return.Value(null));
            Stub.On(meshContainer2).GetProperty("SkinInformation").Will(Return.Value(skinInformation));
            Stub.On(skinInformation).GetProperty("NumberBones").Will(Return.Value(41));
            Stub.On(meshContainer2).Method("GetAdjacencyStream").Will(Return.Value(adj));

            ResultSynthesizer result = new ResultSynthesizer();
            result.SetResult(typeof(IMesh), newMesh);
            SetNamedParameterAction namedParam1 =
                new SetNamedParameterAction("maxFaceInfluence", 20);
            SetNamedParameterAction namedParam2 =
                new SetNamedParameterAction("boneCombinationTable", null);
            Expect.Once.On(skinInformation).Method("ConvertToIndexedBlendedMesh").
                With(Is.EqualTo(mesh), Is.EqualTo(MeshFlags.OptimizeVertexCache | MeshFlags.Managed),
                Is.EqualTo(adj), Is.EqualTo(40), Is.Out, Is.Out).
                Will(new IAction[] { namedParam1, namedParam2, result });
            Expect.Once.On(meshContainer2).SetProperty("MeshData");

            model = new SkinnedModel(rootFrame, textureFactory);
        }

        /// <summary>
        /// Test that the correct calls are being made when drawing the model.
        /// </summary>
        [Test]
        public void TestDraw()
        {
            Constructor10BonesTest();
            Matrix[] bonesMatrices = new Matrix[10];
            for (int i = 0; i < 10; i++)
                bonesMatrices[i] = Matrix.Identity;

            using (mockery.Ordered)
            {
                // Mesh 1
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With((childChildSiblingMatrix * (childMatrix * firstMatrix)) * world, view, projection);

                //Subset 1
                Expect.Once.On(effectHandler).Method("SetBones").With(bonesMatrices);
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials2[0]), Is.EqualTo(0));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(0);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");

                // Subset 2
                Expect.Once.On(effectHandler).Method("SetBones").With(bonesMatrices);
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials2[1]), Is.EqualTo(1));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(2));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("BeginPass").With(1);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");

                // Mesh 2
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With((childMatrix * firstMatrix) * world, view, projection);

                //Subset 1
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials1[0]), Is.EqualTo(2));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(0));
                Expect.Once.On(effect).Method("End");

            }

            model.Draw(effectHandler, sceneAmbient, world, view, projection);
        }

        /// <summary>
        /// Test that animation is stepped during the Step method.
        /// </summary>
        [Test]
        public void TestStep1()
        {
            Time.Initialize();
            Time.Step();
            ConstructorMaterialTest();
            Stub.On(rootFrame).
                GetProperty("AnimationController").
                Will(Return.Value(animationController));
            Expect.Once.On(animationController).
                Method("AdvanceTime").
                With((double)Time.DeltaTime);
            model.Step();
        }

        /// <summary>
        /// Test that animation is not stepped if controller is missing.
        /// </summary>
        [Test]
        public void TestStep2()
        {
            Time.Initialize();
            Time.Step();
            ConstructorMaterialTest();
            Stub.On(rootFrame).
                GetProperty("AnimationController").
                Will(Return.Value(null));
            model.Step();
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

    }
}
