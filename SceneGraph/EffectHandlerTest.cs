using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class EffectHandlerTest
    {
        private Mockery mockery;
        private IEffect effect;
        private IRenderableMesh mesh;
        private IRenderableScene scene;
        private IRenderableCamera camera;
        private ModelMaterial modelMaterial;
        private Material material;
        private ITexture texture;

        private EffectHandle defaultTechnique;
        private EffectHandle worldT;
        private EffectHandle worldViewProjectionT;
        private EffectHandle ambientColor;
        private EffectHandle baseTexture;

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projMatrix;
        private ColorValue sceneAmbient;
        private ColorValue materialAmbient;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            effect = mockery.NewMock<IEffect>();
            mesh = mockery.NewMock<IRenderableMesh>();
            scene = mockery.NewMock<IRenderableScene>();
            camera = mockery.NewMock<IRenderableCamera>();
            texture = mockery.NewMock<ITexture>();

            Stub.On(scene).
                GetProperty("ActiveCamera").
                Will(Return.Value(camera));

            defaultTechnique = EffectHandle.FromString("Technique");
            worldT = EffectHandle.FromString("WorldT");
            worldViewProjectionT = EffectHandle.FromString("WorldViewProjectionT");
            ambientColor = EffectHandle.FromString("AmbientColor");
            baseTexture = EffectHandle.FromString("BaseTexture");

            worldMatrix = Matrix.Scaling(1, 3, 5);
            viewMatrix = Matrix.RotationYawPitchRoll(1, 2, 3);
            projMatrix = Matrix.PerspectiveLH(1, 1, 1, 10);
            sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f, 0.4f);
            materialAmbient = new ColorValue(0.8f, 0.7f, 0.6f, 0.5f);
            material = new Material();
            material.AmbientColor = materialAmbient;
            modelMaterial = new ModelMaterial(material, texture);

            Stub.On(effect).
                Method("FindNextValidTechnique").
                WithAnyArguments().
                Will(Return.Value(defaultTechnique));
            Stub.On(mesh).
                GetProperty("WorldMatrix").
                Will(Return.Value(worldMatrix));
            Stub.On(camera).
                GetProperty("ViewMatrix").
                Will(Return.Value(viewMatrix));
            Stub.On(camera).
                GetProperty("ProjectionMatrix").
                Will(Return.Value(projMatrix));
            Stub.On(scene).
                GetProperty("AmbientColor").
                Will(Return.Value(sceneAmbient));
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void ConstructorTest()
        {
            Stub.On(effect).Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(worldT));
            Stub.On(effect).Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(worldViewProjectionT));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AmbientColor").
                Will(Return.Value(null));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "BaseTexture").
                Will(Return.Value(null));
            EffectHandler effectHandler = new EffectHandler(effect);
            Assert.AreSame(effect, effectHandler.Effect);
            Assert.AreSame(defaultTechnique, effectHandler.Technique);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestFail1()
        {
            Stub.On(effect).Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(worldT));
            Stub.On(effect).Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(null));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AmbientColor").
                Will(Return.Value(null));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "BaseTexture").
                Will(Return.Value(null));
            EffectHandler effectHandler = new EffectHandler(effect);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestFail2()
        {
            Stub.On(effect).Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(null));
            Stub.On(effect).Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(worldViewProjectionT));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AmbientColor").
                Will(Return.Value(null));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "BaseTexture").
                Will(Return.Value(null));
            EffectHandler effectHandler = new EffectHandler(effect);
        }

        [Test]
        public void SetMeshConstantsTest()
        {
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(worldT));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(worldViewProjectionT));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AmbientColor").
                Will(Return.Value(null));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "BaseTexture").
                Will(Return.Value(null));
            EffectHandler effectHandler = new EffectHandler(effect);

            Expect.Once.On(effect).
                Method("SetValueTranspose").
                With(worldT, worldMatrix);
            Expect.Once.On(effect).
                Method("SetValueTranspose").
                With(worldViewProjectionT, worldMatrix * viewMatrix * projMatrix);
            effectHandler.SetMeshConstants(scene, mesh);
        }

        [Test]
        public void SetMaterialConstantsTest()
        {
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(worldT));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(worldViewProjectionT));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AmbientColor").
                Will(Return.Value(ambientColor));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "BaseTexture").
                Will(Return.Value(baseTexture));
            EffectHandler effectHandler = new EffectHandler(effect);

            Expect.Once.On(effect).
                Method("SetValue").
                With(ambientColor, ColorOperator.Modulate(sceneAmbient, materialAmbient));
            Expect.Once.On(effect).
                Method("SetValue").
                With(baseTexture, texture);
            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }
    }
}
