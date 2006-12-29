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
        private IModel model;

        private EffectHandle validTechnique;
        private EffectHandle invalidTechnique;

        private EffectHandle worldT;
        private EffectHandle worldViewProjectionT;
        private EffectHandle projectionT;
        private EffectHandle worldViewT;
        private EffectHandle ambientColor;
        private EffectHandle baseTexture;
        private EffectHandle normalTextureHandle;
        private EffectHandle materialDiffuseColor;
        private EffectHandle materialSpecularColor;
        private EffectHandle annotation;

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
            ambientColor = EffectHandle.FromString("AmbientColor");
            baseTexture = EffectHandle.FromString("BaseTexture");
            normalTextureHandle = EffectHandle.FromString("NormalTexture");
            materialDiffuseColor = EffectHandle.FromString("MaterialDiffuseColor");
            materialSpecularColor = EffectHandle.FromString("MaterialSpecularColor");
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
            modelMaterial = new ModelMaterial(material, texture, normalTexture);

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
        /// No skinning annotation
        /// </summary>
        [Test]
        public void ConstructorTestOK()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();

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
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();

            EffectHandler effectHandler = new EffectHandler(effect, "", null);
            Assert.AreSame(effect, effectHandler.Effect);
            Assert.AreSame(validTechnique, effectHandler.Techniques[0]);
        }

        /// <summary>
        /// Model is skinned and only valid technique is not
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void ConstructorTestSkinFail1()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);

            Expect.Once.On(effect).Method("FindNextValidTechnique").With(validTechnique).Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueString").With(annotation).Will(Return.Value("fAlSE"));
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
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);

            Expect.Once.On(effect).Method("FindNextValidTechnique").With(validTechnique).Will(Return.Value(null));
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueString").With(annotation).Will(Return.Value("tRue"));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(false));

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// Model and valid technique are both skinned
        /// </summary>
        [Test]
        public void ConstructorTestSkinOK1()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);

            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueString").With(annotation).Will(Return.Value("tRue"));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(true));

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// Model and valid technique are not skinned
        /// </summary>
        [Test]
        public void ConstructorTestSkinOK2()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);

            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
            Expect.Once.On(effect).Method("GetValueString").With(annotation).Will(Return.Value("fAlse"));
            Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(false));

            EffectHandler effectHandler = new EffectHandler(effect, "", model);
        }

        /// <summary>
        /// Model and valid technique are both skinned (three materials)
        /// </summary>
        [Test]
        public void ConstructorTestSkinOK3()
        {
            ExpectNumberOfMaterials(3);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);

            for (int i = 0; i < 3; i++)
            {
                ExpectFindTechniques(1);
                Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(annotation));
                Expect.Once.On(effect).Method("GetValueString").With(annotation).Will(Return.Value("tRue"));
                Expect.Once.On(model).Method("IsSkinned").Will(Return.Value(true));
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
            ExpectNumberOfMaterials(1);
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
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(5);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "Valid", model);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest1()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest2()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(null, worldViewProjectionT, projectionT, worldViewT);
            ExpectMeshParametersSet(null, worldViewProjectionT, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest3()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, null, projectionT, worldViewT);
            ExpectMeshParametersSet(worldT, null, projectionT, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest4()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, null, worldViewT);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, null, worldViewT);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMeshConstantsTest5()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectMeshParameters(worldT, worldViewProjectionT, projectionT, null);
            ExpectMeshParametersSet(worldT, worldViewProjectionT, projectionT, null);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetNodeConstants(scene, mesh);
        }

        [Test]
        public void SetMaterialConstantsTest1()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(scene, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest2()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(null, baseTexture, normalTextureHandle, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(false, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(scene, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest3()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, null, normalTextureHandle, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), null, normalTexture, true, materialDiffuse, true, materialSpecular);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(scene, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest4()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, null, materialDiffuseColor, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, null, true, materialDiffuse, true, materialSpecular);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(scene, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest5()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, null, materialSpecularColor);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, false, materialDiffuse, true, materialSpecular);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(scene, modelMaterial, 0);
        }

        [Test]
        public void SetMaterialConstantsTest6()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(ambientColor, baseTexture, normalTextureHandle, materialDiffuseColor, null);
            ExpectMaterialParametersSet(true, ColorOperator.Modulate(sceneAmbient, materialAmbient), texture, normalTexture, true, materialDiffuse, false, materialSpecular);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);

            effectHandler.SetMaterialConstants(scene, modelMaterial, 0);
        }

        /// <summary>
        /// Another material number
        /// </summary>
        [Test]
        public void SetMaterialConstantsTest7()
        {
            ExpectNumberOfMaterials(1);
            ExpectFindTechniques(1);
            ExpectSetTechnique();
            ExpectMeshParameters(null, null, null, null);
            ExpectMaterialParameters(null, null, null, null, null);
            ExpectNoSkinningAnnotation();
            EffectHandler effectHandler = new EffectHandler(effect, "", model);
            effectHandler.Techniques = new EffectHandle[] { null, validTechnique };

            effectHandler.SetMaterialConstants(scene, modelMaterial, 1);
        }

        private void ExpectSetTechnique()
        {
            Expect.Once.On(effect).
                SetProperty("Technique").
                To(validTechnique);
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

        private void ExpectNoSkinningAnnotation()
        {
            Expect.Once.On(effect).Method("GetAnnotation").With(validTechnique, "Skinning").Will(Return.Value(null));
        }

        private void ExpectNumberOfMaterials(int num)
        {
            Expect.Once.On(model).
                GetProperty("Materials").
                Will(Return.Value(new ModelMaterial[num]));
        }

    }
}
