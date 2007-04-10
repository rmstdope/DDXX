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
        private IScene scene;
        private IRenderableCamera camera;
        private ModelMaterial modelMaterial;
        private Material material;
        private ITexture texture;
        private ITexture normalTexture;
        private ICubeTexture reflectiveTexture;
        private float reflectiveFactor;
        private IModel model;

        private EffectHandle validTechnique;
        private EffectHandle invalidTechnique;

        private EffectHandle worldT;
        private EffectHandle worldViewProjectionT;
        private EffectHandle projectionT;
        private EffectHandle worldViewT;
        private EffectHandle invMatrixT;
        private EffectHandle ambientColor;
        private EffectHandle baseTexture;
        private EffectHandle normalTextureHandle;
        private EffectHandle reflectiveTextureHandle;
        private EffectHandle reflectiveFactorHandle;
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;
        private EffectHandle materialShininessHandle;
        private EffectHandle animationMatrices;
        private EffectHandle annotation;

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projMatrix;
        private ColorValue sceneAmbient;
        private ColorValue materialAmbient;
        private ColorValue materialDiffuse;
        private ColorValue materialSpecular;
        private float materialShininess;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            effect = mockery.NewMock<IEffect>();
            mesh = mockery.NewMock<IRenderableMesh>();
            scene = mockery.NewMock<IScene>();
            camera = mockery.NewMock<IRenderableCamera>();
            texture = mockery.NewMock<ITexture>();
            normalTexture = mockery.NewMock<ITexture>();
            reflectiveTexture = mockery.NewMock<ICubeTexture>();
            model = mockery.NewMock<IModel>();

            Stub.On(scene).
                GetProperty("ActiveCamera").
                Will(Return.Value(camera));

            validTechnique = EffectHandle.FromString("ValidTechnique");
            invalidTechnique = EffectHandle.FromString("InvalidTechnique");

            worldT = EffectHandle.FromString("WorldT");
            worldViewProjectionT = EffectHandle.FromString("WorldViewProjectionT");
            projectionT = EffectHandle.FromString("ProjectionT");
            worldViewT = EffectHandle.FromString("WorldViewT");
            invMatrixT = EffectHandle.FromString("InvWorldViewProjectionT");
            ambientColor = EffectHandle.FromString("AmbientColor");
            baseTexture = EffectHandle.FromString("BaseTexture");
            normalTextureHandle = EffectHandle.FromString("NormalTexture");
            reflectiveTextureHandle = EffectHandle.FromString("ReflectiveTexture");
            reflectiveFactorHandle = EffectHandle.FromString("ReflectiveFactor");
            materialDiffuseColor = EffectHandle.FromString("MaterialDiffuseColor");
            materialSpecularColor = EffectHandle.FromString("MaterialSpecularColor");
            materialShininessHandle = EffectHandle.FromString("MaterialShininess");
            animationMatrices = EffectHandle.FromString("AnimationMatrices");
            annotation = EffectHandle.FromString("Annotation");

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
            material.SpecularSharpness = 4;
            modelMaterial = new ModelMaterial(material, texture, normalTexture, reflectiveTexture);

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

        /// <summary>
        /// No annotations
        /// </summary>
        [Test]
        public void ConstructorTestOK()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
            Assert.AreSame(effect, effectHandler.Effect);
            Assert.AreSame(validTechnique, effectHandler.Techniques[0]);
        }

        /// <summary>
        /// No model given
        /// </summary>
        [Test]
        public void ConstructorTestOK2()
        {
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();

            EffectHandler effectHandler = new EffectHandler(effect, "", null);
            Assert.AreSame(effect, effectHandler.Effect);
            Assert.AreSame(validTechnique, effectHandler.Techniques[0]);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorMissingReflectiveFactor()
        {
            ExpectFindTechniques(1);
            ExpectMeshParameters(null, null, null, null, null, null, null);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();

            EffectHandler effectHandler = new EffectHandler(effect, "", null);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorMissingShininess()
        {
            ExpectFindTechniques(1);
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, null);
            ExpectNoAnnotations();

            EffectHandler effectHandler = new EffectHandler(effect, "", null);
        }

        /// <summary>
        /// Model is skinned and only valid technique is not
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestSkinFail1()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);

            Expect.Once.On(effect).Method("FindNextValidTechnique").With(validTechnique).Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(false));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(true));

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// Model is not skinned and only valid technique is 
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestSkinFail2()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);

            Expect.Once.On(effect).Method("FindNextValidTechnique").With(validTechnique).Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(true));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(false));

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// Model is normal mapped and only valid technique is not
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestSkinFail3()
        {
            ExpectNumberOfMaterials(1, modelMaterial);
            ExpectFindTechniques(1);

            Expect.Once.On(effect).Method("FindNextValidTechnique").With(validTechnique).Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "NormalMapping").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(false));
            modelMaterial.NormalTexture = normalTexture;

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// Model is not normal mapped and only valid technique is 
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestSkinFail4()
        {
            ExpectNumberOfMaterials(1, modelMaterial);
            ExpectFindTechniques(1);

            Expect.Once.On(effect).Method("FindNextValidTechnique").With(validTechnique).Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "NormalMapping").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(true));
            modelMaterial.NormalTexture = null;

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// All annotations are correct (false)
        /// </summary>
        [Test]
        public void ConstructorTestAnnotationOK1()
        {
            ExpectNumberOfMaterials(1, modelMaterial);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);

            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(true));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(true));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "NormalMapping").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(true));
            modelMaterial.NormalTexture = normalTexture;

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// All annotations are correct (true)
        /// </summary>
        [Test]
        public void ConstructorTestAnnotationOK2()
        {
            ExpectNumberOfMaterials(1, modelMaterial);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);

            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(false));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(false));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "NormalMapping").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(false));
            modelMaterial.NormalTexture = null;

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// All annotations are correct (three materials)
        /// </summary>
        [Test]
        public void ConstructorTestSkinOK3()
        {
            ExpectNumberOfMaterials(3, modelMaterial);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            modelMaterial.NormalTexture = normalTexture;

            for (int i = 0; i < 3; i++)
            {
                ExpectFindTechniques(1);
                Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
                Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(true));
                Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(true));
                Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "NormalMapping").Will(Return.Value(annotation));
                Expect.Once.On(effect).Method("GetValueBoolean").With(annotation).Will(Return.Value(true));
            }

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
            Assert.AreEqual(effectHandler.Techniques.Length, 3);
            Assert.AreEqual(effectHandler.Techniques[0], validTechnique);
            Assert.AreEqual(effectHandler.Techniques[1], validTechnique);
            Assert.AreEqual(effectHandler.Techniques[2], validTechnique);
        }

        /// <summary>
        /// No technique with given name found
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TechniqueConstructorTestFail()
        {
            ExpectNumberOfMaterials(1, null);
            Expect.Once.On(effect).Method("FindNextValidTechnique").
                With(Is.Null).Will(Return.Value(null));
            EffectHandler effectHandler = new EffectHandler(effect, "Valid", model);
        }

        /// <summary>
        /// Find correct technique after fifth try
        /// </summary>
        [Test]
        public void TechniqueConstructorTestOK()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(5);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "Valid", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMeshConstantsTest1()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, worldViewT, invMatrixT);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMeshConstantsTest2()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(null, worldViewProjectionT, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(null, worldViewProjectionT, projectionT, worldViewT, invMatrixT);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMeshConstantsTest3()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, null, projectionT, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(worldT, null, projectionT, worldViewT, invMatrixT);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMeshConstantsTest4()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, null, worldViewT, invMatrixT, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, null, worldViewT, invMatrixT);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMeshConstantsTest5()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, null, invMatrixT, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, null, invMatrixT);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMeshConstantsTest6()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT, null, null, reflectiveFactorHandle);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, worldViewT, null);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(worldMatrix, viewMatrix, projMatrix);
        }

        [Test]
        public void SetMaterialConstantsTest1()
        {
            reflectiveFactor = 0.1f;
            materialShininess = 2;
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, reflectiveTextureHandle, materialDiffuseColor, materialSpecularColor, materialShininessHandle);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, reflectiveTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest2()
        {
            reflectiveFactor = 0.5f;
            materialShininess = 4;
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, baseTexture, normalTextureHandle, reflectiveTextureHandle, materialDiffuseColor, materialSpecularColor, materialShininessHandle);
            ExpectMaterialParametersSet(false, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, reflectiveTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest3()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(ambientColor, null, normalTextureHandle, reflectiveTextureHandle, materialDiffuseColor, materialSpecularColor, materialShininessHandle);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), null, normalTexture, reflectiveTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest4()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(ambientColor, baseTexture, null, reflectiveTextureHandle, materialDiffuseColor, materialSpecularColor, materialShininessHandle);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, null, reflectiveTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest5()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, null, materialDiffuseColor, materialSpecularColor, materialShininessHandle);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, null, true, materialDiffuse, true, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest6()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, reflectiveTextureHandle, null, materialSpecularColor, materialShininessHandle);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, reflectiveTexture, false, materialDiffuse, true, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest7()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, reflectiveTextureHandle, materialDiffuseColor, null, materialShininessHandle);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, reflectiveTexture, true, materialDiffuse, false, materialSpecular);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 0);
        }

        /// <summary>
        /// Another material number
        /// </summary>
        [Test]
        public void SetMaterialConstantsTest8()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectMaterialParametersSet(false, new ColorValue(), null, null, null, false, new ColorValue(), false, new ColorValue());
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);
            effectHandler.Techniques = new EffectHandle[] { null, validTechnique };

            effectHandler.SetMaterialConstants(sceneAmbient, modelMaterial, 1);
        }

        /// <summary>
        /// Test setting of bones with effect that has none
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetBonesTestFail()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(null, null, null, null, null, null, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            Matrix[] matrices = new Matrix[2];
            matrices[0] = Matrix.RotationX(1);
            matrices[1] = Matrix.RotationZ(3);

            effectHandler.SetBones(matrices);
        }

        /// <summary>
        /// Test setting of bones
        /// </summary>
        [Test]
        public void SetBonesTest()
        {
            ExpectNumberOfMaterials(1, null);
            ExpectFindTechniques(1);
            ExpectMeshParameters(null, null, null, null, null, animationMatrices, reflectiveFactorHandle);
            ExpectMaterialParameters(null, null, null, null, null, null, materialShininessHandle);
            ExpectNoAnnotations();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            Matrix[] matrices = new Matrix[2];
            matrices[0] = Matrix.RotationX(1);
            matrices[1] = Matrix.RotationZ(3);

            Expect.Once.On(effect).Method("SetValue").With(animationMatrices, matrices);
            effectHandler.SetBones(matrices);
        }

        private void ExpectSetTechnique()
        {
            Expect.Once.On(effect).
                SetProperty("Technique").
                To(validTechnique);
        }

        private void ExpectMeshParameters(Object world, Object worldViewProj, Object proj, Object worldView, Object inv, Object animation, Object reflectiveFactor)
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
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "InvWorldViewProjectionT").
                Will(Return.Value(inv));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "AnimationMatrices").
                Will(Return.Value(animation));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "ReflectiveFactor").
                Will(Return.Value(reflectiveFactor));
        }

        private void ExpectMeshParametersSet(Object world, Object worldViewProj, Object proj, Object worldView, Object invMatrix)
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
            if (invMatrix != null)
            {
                Matrix matrix = worldMatrix * viewMatrix * projMatrix;
                matrix.Invert();
                Expect.Once.On(effect).
                    Method("SetValueTranspose").
                    With(invMatrixT, matrix);
            }
        }

        private void ExpectMaterialParameters(Object ambient, Object texture, Object normalTexture, 
            Object reflectiveTexture, Object diffuse, Object specular, Object shininess)
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
                With(null, "ReflectiveTexture").
                Will(Return.Value(reflectiveTexture));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "MaterialDiffuseColor").
                Will(Return.Value(diffuse));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "MaterialSpecularColor").
                Will(Return.Value(specular));
            Expect.Once.On(effect).
                Method("GetParameter").
                With(null, "MaterialShininess").
                Will(Return.Value(shininess));
        }

        private void ExpectMaterialParametersSet(bool useAmbient, ColorValue ambientParam, ITexture diffuseTexParam, ITexture normalTexParam, ICubeTexture reflectiveTexParam, bool useDiffuse, ColorValue diffuseParam, bool useSpecular, ColorValue specularParam)
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
            if (reflectiveTexParam != null)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(reflectiveTextureHandle, reflectiveTexParam);
            if (useDiffuse)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(materialDiffuseColor, diffuseParam);
            if (useSpecular)
                Expect.Once.On(effect).
                    Method("SetValue").
                    With(materialSpecularColor, specularParam);
            modelMaterial.Shininess = materialShininess;
            Expect.Once.On(effect).
                Method("SetValue").
                With(materialShininessHandle, materialShininess);
            modelMaterial.ReflectiveFactor = reflectiveFactor;
            Expect.Once.On(effect).
                Method("SetValue").
                With(reflectiveFactorHandle, reflectiveFactor);
        }

        private void ExpectFindTechniques(int num)
        {
            // First argument is null
            if (num == 1)
            {
                Expect.Once.On(effect).Method("FindNextValidTechnique").
                    With(Is.Null).Will(Return.Value(validTechnique));
                Expect.Once.On(effect).Method("GetTechniqueName").
                    With(validTechnique).Will(Return.Value("ValidTechnique"));
                return;
            }
            else
            {
                Expect.Once.On(effect).Method("FindNextValidTechnique").
                    With(Is.Null).Will(Return.Value(invalidTechnique));
                Expect.Once.On(effect).Method("GetTechniqueName").
                    With(invalidTechnique).Will(Return.Value("InvalidTechnique"));
            }

            // Rest of the arguments are last effect
            if (num > 2)
            {
                Expect.Exactly(num - 2).On(effect).Method("FindNextValidTechnique").
                    With(invalidTechnique).Will(Return.Value(invalidTechnique));
                Expect.Exactly(num - 2).On(effect).Method("GetTechniqueName").
                    With(invalidTechnique).Will(Return.Value("InvalidTechnique"));
            }
            Expect.Once.On(effect).Method("FindNextValidTechnique").
                With(invalidTechnique).Will(Return.Value(validTechnique));
            Expect.Once.On(effect).Method("GetTechniqueName").
                With(validTechnique).Will(Return.Value("ValidTechnique"));
        }

        private void ExpectNoAnnotations()
        {
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "NormalMapping").Will(Return.Value(null));
        }

        private void ExpectNumberOfMaterials(int num, ModelMaterial material)
        {
            ModelMaterial[] materials = new ModelMaterial[num];
            for (int i = 0; i < materials.Length; i++)
                materials[i] = material;
            Stub.On(model).
                GetProperty("Materials").
                Will(Return.Value(materials));
        }

    }
}
