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
        private ITexture normalTexture;

        private EffectHandle defaultTechnique;
        private EffectHandle worldT;
        private EffectHandle worldViewProjectionT;
        private EffectHandle projectionT;
        private EffectHandle worldViewT;
        private EffectHandle ambientColor;
        private EffectHandle baseTexture;
        private EffectHandle normalTextureHandle;
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projMatrix;
        private ColorValue sceneAmbient;
        private ColorValue materialAmbient;
        private ColorValue materialDiffuse;
        private ColorValue materialSpecular;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            effect = mockery.NewMock<IEffect>();
            mesh = mockery.NewMock<IRenderableMesh>();
            scene = mockery.NewMock<IRenderableScene>();
            camera = mockery.NewMock<IRenderableCamera>();
            texture = mockery.NewMock<ITexture>();
            normalTexture = mockery.NewMock<ITexture>();

            Stub.On(scene).
                GetProperty("ActiveCamera").
                Will(Return.Value(camera));

            defaultTechnique = EffectHandle.FromString("Technique");
            worldT = EffectHandle.FromString("WorldT");
            worldViewProjectionT = EffectHandle.FromString("WorldViewProjectionT");
            projectionT = EffectHandle.FromString("ProjectionT");
            worldViewT = EffectHandle.FromString("WorldViewT");
            ambientColor = EffectHandle.FromString("AmbientColor");
            baseTexture = EffectHandle.FromString("BaseTexture");
            normalTextureHandle = EffectHandle.FromString("NormalTexture");
            materialDiffuseColor = EffectHandle.FromString("MaterialDiffuseColor");
            materialSpecularColor = EffectHandle.FromString("MaterialSpecularColor");

            worldMatrix = Matrix.Scaling(1, 3, 5);
            viewMatrix = Matrix.RotationYawPitchRoll(1, 2, 3);
            projMatrix = Matrix.PerspectiveLH(1, 1, 1, 10);
            sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f, 0.4f);
            materialAmbient = new ColorValue(0.8f, 0.7f, 0.6f, 0.5f);
            materialDiffuse = new ColorValue(0.81f, 0.71f, 0.61f, 0.51f);
            materialSpecular = new ColorValue(0.82f, 0.72f, 0.62f, 0.52f);
            material = new Material();
            material.AmbientColor = materialAmbient;
            material.DiffuseColor = materialDiffuse;
            material.SpecularColor = materialSpecular;
            modelMaterial = new ModelMaterial(material, texture, normalTexture);

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
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);

            EffectHandler effectHandler = new EffectHandler(effect);
            Assert.AreSame(effect, effectHandler.Effect);
            Assert.AreSame(defaultTechnique, effectHandler.Technique);
        }

        [Test]
        public void SetMeshConstantsTest1()
        {
            ExpectTechnique();
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest2()
        {
            ExpectTechnique();
            ExpectMeshParameters(null, worldViewProjectionT, projectionT, worldViewT);
            ExpectMeshParametersSet(null, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest3()
        {
            ExpectTechnique();
            ExpectMeshParameters(worldT, null, projectionT, worldViewT);
            ExpectMeshParametersSet(worldT, null, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest4()
        {
            ExpectTechnique();
            ExpectMeshParameters(worldT, worldViewProjectionT, null, worldViewT);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, null, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest5()
        {
            ExpectTechnique();
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, null);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, null);
            ExpectMaterialParameters(null, null, null, null, null);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMaterialConstantsTest1()
        {
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, true, materialDiffuse, true, materialSpecular);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }

        [Test]
        public void SetMaterialConstantsTest2()
        {
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(null, baseTexture, normalTextureHandle, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(false, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, true, materialDiffuse, true, materialSpecular);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }

        [Test]
        public void SetMaterialConstantsTest3()
        {
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, null, normalTextureHandle, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), null, normalTexture, true, materialDiffuse, true, materialSpecular);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }

        [Test]
        public void SetMaterialConstantsTest4()
        {
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, null, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, null, true, materialDiffuse, true, materialSpecular);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }

        [Test]
        public void SetMaterialConstantsTest5()
        {
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, null, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, false, materialDiffuse, true, materialSpecular);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }

        [Test]
        public void SetMaterialConstantsTest6()
        {
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, materialDiffuseColor, null);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, true, materialDiffuse, false, materialSpecular);
            EffectHandler effectHandler = new EffectHandler(effect);

            effectHandler.SetMaterialConstants(scene, modelMaterial);
        }

        private void ExpectTechnique()
        {
            Expect.Once.On(effect).
                SetProperty("Technique").
                To(defaultTechnique);
        }

        private void ExpectMeshParameters(Object world, Object worldViewProj, Object proj, Object worldView)
        {
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(world));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(worldViewProj));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "ProjectionT").
                Will(Return.Value(proj));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "WorldViewT").
                Will(Return.Value(worldView));
        }

        private void ExpectMeshParametersSet(Object world, Object worldViewProj, Object proj, Object worldView)
        {
            if (world != null)
                Expect.Once.On(effect).
                    Method("SetValueTranspose").
                    With(worldT, worldMatrix);
            if (worldViewProj != null)
                Expect.Once.On(effect).
                    Method("SetValueTranspose").
                    With(worldViewProjectionT, worldMatrix * viewMatrix * projMatrix);
            if (proj != null)
                Expect.Once.On(effect).
                    Method("SetValueTranspose").
                    With(projectionT, projMatrix);
            if (worldView != null)
                Expect.Once.On(effect).
                    Method("SetValueTranspose").
                    With(worldViewT, worldMatrix * viewMatrix);
        }

        private void ExpectMaterialParameters(Object ambient, Object texture, Object normalTexture, 
            Object diffuse, Object specular)
        {
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AmbientColor").
                Will(Return.Value(ambient));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "BaseTexture").
                Will(Return.Value(texture));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "NormalTexture").
                Will(Return.Value(normalTexture));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "MaterialDiffuseColor").
                Will(Return.Value(diffuse));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "MaterialSpecularColor").
                Will(Return.Value(specular));
        }

        private void ExpectMaterialParametersSet(bool useAmbient, ColorValue ambientParam, ITexture diffuseTexParam, ITexture normalTexParam, bool useDiffuse, ColorValue diffuseParam, bool useSpecular, ColorValue specularParam)
        {
            if (useAmbient)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(ambientColor, ambientParam);
            if (diffuseTexParam != null)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(baseTexture, diffuseTexParam);
            if (normalTexParam != null)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(normalTextureHandle, normalTexParam);
            if (useDiffuse)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(materialDiffuseColor, diffuseParam);
            if (useSpecular)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(materialSpecularColor, specularParam);
        }

    }
}
