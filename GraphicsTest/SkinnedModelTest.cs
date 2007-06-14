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

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class SkinnedModelTest
    {
        private Mockery mockery;
        private IAnimationRootFrame rootFrame;
        private IFrame frame;
        private Matrix transformationMatrix = Matrix.RotationX(1);

        private IFrame firstFrame;
        private Matrix firstMatrix = Matrix.RotationX(1);
        private IFrame childFrame;
        private Matrix childMatrix = Matrix.RotationX(2);
        private IFrame childChildFrame;
        private Matrix childChildMatrix = Matrix.RotationX(3);
        private IFrame childChildSiblingFrame;
        private Matrix childChildSiblingMatrix = Matrix.RotationX(4);
        private IMeshContainer meshContainer;
        private MeshDataAdapter meshData;
        private IMesh mesh;
        private IMesh newMesh;
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
            frame = mockery.NewMock<IFrame>();

            firstFrame = mockery.NewMock<IFrame>();
            childFrame = mockery.NewMock<IFrame>();
            childChildFrame = mockery.NewMock<IFrame>();
            childChildSiblingFrame = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            mesh = mockery.NewMock<IMesh>();
            newMesh = mockery.NewMock<IMesh>();
            meshData = new MeshDataAdapter();
            meshData.Mesh = mesh;
            textureFactory = mockery.NewMock<ITextureFactory>();
            materials1 = new ExtendedMaterial[1];
            materials2 = new ExtendedMaterial[2];
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();
            animationController = mockery.NewMock<IAnimationController>();
            skinInformation = mockery.NewMock<ISkinInformation>();

            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(firstFrame));
            Stub.On(frame).Method("Find").With(firstFrame, "NameOfBone").Will(Return.Value(firstFrame));
            Stub.On(frame).GetProperty("TransformationMatrix").Will(Return.Value(transformationMatrix));

            Stub.On(meshContainer).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(meshContainer).Method("GetMaterials").Will(Return.Value(materials1));
            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));

            Stub.On(skinInformation).Method("GetBoneOffsetMatrix").Will(Return.Value(Matrix.Identity));
            Stub.On(skinInformation).Method("GetBoneName").Will(Return.Value("NameOfBone"));

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
            Stub.On(meshContainer).GetProperty("SkinInformation").Will(Return.Value(null));
            Stub.On(frame).GetProperty("ExtendedMaterials").Will(Return.Value(materials2));
            Stub.On(frame).GetProperty("MeshContainer").Will(Return.Value(meshContainer));

            Material mat = new Material();
            mat.Diffuse = Color.AntiqueWhite;
            materials2[0].Material3D = mat;
            materials2[0].TextureFilename = "1";
            mat.Diffuse = Color.Aqua;
            materials2[1].Material3D = mat;
            materials2[1].TextureFilename = "2";

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("1");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("2");

            model = new SkinnedModel(rootFrame, frame, textureFactory);
        }

        /// <summary>
        /// Test that the correct number of bones are used.
        /// </summary>
        [Test]
        public void Constructor10BonesTest()
        {
            IGraphicsStream adj = mockery.NewMock<IGraphicsStream>();
            Stub.On(frame).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Stub.On(meshContainer).GetProperty("SkinInformation").Will(Return.Value(skinInformation));
            Stub.On(skinInformation).GetProperty("NumberBones").Will(Return.Value(10));
            Stub.On(meshContainer).Method("GetAdjacencyStream").Will(Return.Value(adj));
            Stub.On(frame).GetProperty("ExtendedMaterials").Will(Return.Value(materials2));

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
            Expect.Once.On(meshContainer).SetProperty("Bones").To(Is.Null);
            Expect.Once.On(meshContainer).SetProperty("RestMatrices");
            Expect.Once.On(meshContainer).SetProperty("Frames");

            model = new SkinnedModel(rootFrame, frame, textureFactory);
            Assert.AreSame(newMesh, model.Mesh);
        }

        /// <summary>
        /// Test that the number of bones used are clamped to 60.
        /// </summary>
        [Test]
        public void Constructor61BonesTest()
        {
            IGraphicsStream adj = mockery.NewMock<IGraphicsStream>();
            Stub.On(frame).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Stub.On(meshContainer).GetProperty("SkinInformation").Will(Return.Value(skinInformation));
            Stub.On(skinInformation).GetProperty("NumberBones").Will(Return.Value(61));
            Stub.On(meshContainer).Method("GetAdjacencyStream").Will(Return.Value(adj));
            Stub.On(frame).GetProperty("ExtendedMaterials").Will(Return.Value(materials2));

            ResultSynthesizer result = new ResultSynthesizer();
            result.SetResult(typeof(IMesh), newMesh);
            SetNamedParameterAction namedParam1 =
                new SetNamedParameterAction("maxFaceInfluence", 20);
            SetNamedParameterAction namedParam2 =
                new SetNamedParameterAction("boneCombinationTable", null);
            Expect.Once.On(skinInformation).Method("ConvertToIndexedBlendedMesh").
                With(Is.EqualTo(mesh), Is.EqualTo(MeshFlags.OptimizeVertexCache | MeshFlags.Managed),
                Is.EqualTo(adj), Is.EqualTo(60), Is.Out, Is.Out).
                Will(new IAction[] { namedParam1, namedParam2, result });
            Expect.Once.On(meshContainer).SetProperty("Bones").To(Is.Null);
            Expect.Once.On(meshContainer).SetProperty("RestMatrices");
            Expect.Once.On(meshContainer).SetProperty("Frames");

            model = new SkinnedModel(rootFrame, frame, textureFactory);
            Assert.AreSame(newMesh, model.Mesh);
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
            BoneCombination[] boneCombo = new BoneCombination[2];
            boneCombo[0] = new BoneCombination();
            boneCombo[1] = new BoneCombination();
            boneCombo[0].BoneId = new int[10];
            boneCombo[1].BoneId = boneCombo[0].BoneId;
            for (int i = 0; i < 10; i++)
            {
                boneCombo[0].BoneId[i] = 0;
            }
            IFrame[] frames = new IFrame[1];
            frames[0] = mockery.NewMock<IFrame>();
            Stub.On(meshContainer).GetProperty("RestMatrices").Will(Return.Value(bonesMatrices));
            Stub.On(frames[0]).GetProperty("CombinedTransformationMatrix").Will(Return.Value(Matrix.Identity));
            Stub.On(meshContainer).GetProperty("Frames").Will(Return.Value(frames));

            using (mockery.Ordered)
            {
                // Mesh 1
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With(Matrix.Identity, view, projection);
                Expect.Once.On(meshContainer).GetProperty("Bones").Will(Return.Value(boneCombo));

                //Subset 1
                Expect.Once.On(effectHandler).Method("SetBones").With(bonesMatrices);
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.EqualTo(sceneAmbient), new MaterialMatcher(materials2[0]), Is.EqualTo(0));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(newMesh).Method("DrawSubset").With(0);
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
                Expect.Once.On(newMesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");

            }

            model.Draw(effectHandler, sceneAmbient, world, view, projection);
        }

        /// <summary>
        /// Test that animation is stepped during the Step method.
        /// </summary>
        [Test]
        public void TestStep()
        {
            Time.Initialize();
            Time.Step();
            ConstructorMaterialTest();
            ExpectFrameUpdate();

            model.Step();
        }

        private void ExpectFrameUpdate()
        {
            // firstFrame
            //  |-childFrame
            //     |-childChildFrame
            //     |-childChildSiblingFrame

            Stub.On(firstFrame).GetProperty("FrameFirstChild").Will(Return.Value(childFrame));
            Stub.On(firstFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(firstFrame).GetProperty("TransformationMatrix").Will(Return.Value(firstMatrix));

            Stub.On(childFrame).GetProperty("FrameFirstChild").Will(Return.Value(childChildFrame));
            Stub.On(childFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(childFrame).GetProperty("TransformationMatrix").Will(Return.Value(childMatrix));

            Stub.On(childChildFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(childChildFrame).GetProperty("FrameSibling").Will(Return.Value(childChildSiblingFrame));
            Stub.On(childChildFrame).GetProperty("TransformationMatrix").Will(Return.Value(childChildMatrix));

            Stub.On(childChildSiblingFrame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(childChildSiblingFrame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(childChildSiblingFrame).GetProperty("TransformationMatrix").Will(Return.Value(childChildSiblingMatrix));
            Stub.On(childChildSiblingFrame).Method("Find").With(firstFrame, "NameOfBone").Will(Return.Value(firstFrame));

            Expect.Once.On(firstFrame).SetProperty("CombinedTransformationMatrix").
                To(firstMatrix);
            Expect.Once.On(firstFrame).GetProperty("CombinedTransformationMatrix").
                Will(Return.Value(firstMatrix));
            Expect.Once.On(childFrame).SetProperty("CombinedTransformationMatrix").
                To(childMatrix * firstMatrix);
            Expect.Once.On(childFrame).GetProperty("CombinedTransformationMatrix").
                Will(Return.Value(childMatrix * firstMatrix));
            Expect.Once.On(childChildFrame).SetProperty("CombinedTransformationMatrix").
                To(childChildMatrix * (childMatrix * firstMatrix));
            Expect.Once.On(childChildSiblingFrame).SetProperty("CombinedTransformationMatrix").
                To(childChildSiblingMatrix * (childMatrix * firstMatrix));
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
