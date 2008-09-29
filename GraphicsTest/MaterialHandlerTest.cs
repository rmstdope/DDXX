using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class MaterialHandlerTest : D3DMockTest
    {
        private IMaterialHandler materialHandler;
        private IModel model;
        private ICollectionAdapter<IEffectParameter> parameterCollection;
        private IBasicEffect basicEffect;
        private IBasicDirectionalLight directionalLight0;
        private IBasicDirectionalLight directionalLight1;
        private IBasicDirectionalLight directionalLight2;
        private IEffectConverter effectConverter;

        private LightState lightState;
        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projMatrix;
        private Color sceneAmbient;

        private IEffectParameter worldParameter;
        private IEffectParameter viewParameter;
        private IEffectParameter projParameter;
        private IEffectParameter lightPosParameter;
        private IEffectParameter lightDirParameter;
        private IEffectParameter diffuseColorParameter;
        private IEffectParameter specularColorParameter;
        private IEffectParameter ambientParameter;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            model = mockery.NewMock<IModel>();
            parameterCollection = mockery.NewMock<ICollectionAdapter<IEffectParameter>>();
            basicEffect = mockery.NewMock<IBasicEffect>();
            directionalLight0 = mockery.NewMock<IBasicDirectionalLight>();
            directionalLight1 = mockery.NewMock<IBasicDirectionalLight>();
            directionalLight2 = mockery.NewMock<IBasicDirectionalLight>();
            effectConverter = mockery.NewMock<IEffectConverter>();

            lightState = new LightState();
            worldMatrix = Matrix.CreateScale(1, 3, 5);
            viewMatrix = Matrix.CreateRotationX(1);
            projMatrix = Matrix.CreatePerspective(1, 1, 1, 10);
            sceneAmbient = new Color(1, 2, 3, 4);

            Stub.On(effect).GetProperty("Parameters").Will(Return.Value(parameterCollection));
            worldParameter = mockery.NewMock<IEffectParameter>();
            viewParameter = mockery.NewMock<IEffectParameter>();
            projParameter = mockery.NewMock<IEffectParameter>();
            lightPosParameter = mockery.NewMock<IEffectParameter>();
            lightDirParameter = mockery.NewMock<IEffectParameter>();
            diffuseColorParameter = mockery.NewMock<IEffectParameter>();
            specularColorParameter = mockery.NewMock<IEffectParameter>();
            ambientParameter = mockery.NewMock<IEffectParameter>();

            StubParameter("World", worldParameter);
            StubParameter("View", viewParameter);
            StubParameter("Projection", projParameter);
            StubParameter("LightPositions", lightPosParameter);
            StubParameter("LightDirections", lightDirParameter);
            StubParameter("LightDiffuseColors", diffuseColorParameter);
            StubParameter("LightSpecularColors", specularColorParameter);
            StubParameter("AmbientLightColor", ambientParameter);
            Stub.On(basicEffect).GetProperty("DirectionalLight0").Will(Return.Value(directionalLight0));
            Stub.On(basicEffect).GetProperty("DirectionalLight1").Will(Return.Value(directionalLight1));
            Stub.On(basicEffect).GetProperty("DirectionalLight2").Will(Return.Value(directionalLight2));
            Stub.On(basicEffect).GetProperty("GraphicsDevice").Will(Return.Value(device));
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        private void StubParameter(string name, IEffectParameter parameter)
        {
            Stub.On(parameterCollection).Method("get_Item").With(name).
                Will(Return.Value(parameter));
        }

        private void UseEffect()
        {
            materialHandler = new MaterialHandler(effect, effectConverter);
        }

        private void UseBasicEffect()
        {
            materialHandler = new MaterialHandler(basicEffect, effectConverter);
        }

        [Test]
        public void SetupRenderingEffectNoLights()
        {
            //Setup
            UseEffect();
            Expect.Once.On(worldParameter).Method("SetValue").With(new Matrix[] { worldMatrix });
            Expect.Once.On(viewParameter).Method("SetValue").With(viewMatrix);
            Expect.Once.On(projParameter).Method("SetValue").With(projMatrix);
            Expect.Once.On(lightPosParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(lightDirParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(diffuseColorParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(specularColorParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(ambientParameter).Method("SetValue").With(sceneAmbient.ToVector3());
            Expect.Once.On(renderState).SetProperty("AlphaBlendEnable").To(false);

            // Exercise SUT
            materialHandler.SetupRendering(new Matrix[] { worldMatrix }, viewMatrix, projMatrix, sceneAmbient, lightState);
        }

        [Test]
        public void SetupRenderingTransparency()
        {
            //Setup
            UseEffect();
            materialHandler.BlendFunction = BlendFunction.Max;
            materialHandler.SourceBlend = Blend.SourceAlpha;
            materialHandler.DestinationBlend = Blend.SourceAlphaSaturation;
            Expect.Once.On(worldParameter).Method("SetValue").With(new Matrix[] { worldMatrix });
            Expect.Once.On(viewParameter).Method("SetValue").With(viewMatrix);
            Expect.Once.On(projParameter).Method("SetValue").With(projMatrix);
            Expect.Once.On(lightPosParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(lightDirParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(diffuseColorParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(specularColorParameter).Method("SetValue").With(new Vector3[] { });
            Expect.Once.On(ambientParameter).Method("SetValue").With(sceneAmbient.ToVector3());

            Expect.Once.On(renderState).SetProperty("AlphaBlendEnable").To(true);
            Expect.Once.On(renderState).SetProperty("BlendFunction").To(BlendFunction.Max);
            Expect.Once.On(renderState).SetProperty("SourceBlend").To(Blend.SourceAlpha);
            Expect.Once.On(renderState).SetProperty("DestinationBlend").To(Blend.SourceAlphaSaturation);

            // Exercise SUT
            materialHandler.SetupRendering(new Matrix[] { worldMatrix }, viewMatrix, projMatrix, sceneAmbient, lightState);
        }

        [Test]
        public void SetupRenderingEffectTwoLights()
        {
            //Setup
            UseEffect();
            lightState.NewState(new Vector3(1, 2, 3), new Vector3(4, 5, 6), new Color(7, 8, 9), new Color(10, 11, 12));
            lightState.NewState(new Vector3(11, 12, 13), new Vector3(14, 15, 16), new Color(17, 18, 19), new Color(20, 21, 22));
            Expect.Once.On(worldParameter).Method("SetValue").With(new Matrix[] { worldMatrix });
            Expect.Once.On(viewParameter).Method("SetValue").With(viewMatrix);
            Expect.Once.On(projParameter).Method("SetValue").With(projMatrix);
            Expect.Once.On(lightPosParameter).Method("SetValue").With(new Vector3[] { new Vector3(1, 2, 3), new Vector3(11, 12, 13) });
            Expect.Once.On(lightDirParameter).Method("SetValue").With(new Vector3[] { new Vector3(4, 5, 6), new Vector3(14, 15, 16) });
            Expect.Once.On(diffuseColorParameter).Method("SetValue").With(new Vector3[] { new Color(7, 8, 9).ToVector3(), new Color(17, 18, 19).ToVector3() });
            Expect.Once.On(specularColorParameter).Method("SetValue").With(new Vector3[] { new Color(10, 11, 12).ToVector3(), new Color(20, 21, 22).ToVector3() });
            Expect.Once.On(ambientParameter).Method("SetValue").With(sceneAmbient.ToVector3());
            Expect.Once.On(renderState).SetProperty("AlphaBlendEnable").To(false);

            // Exercise SUT
            materialHandler.SetupRendering(new Matrix[] { worldMatrix }, viewMatrix, projMatrix, sceneAmbient, lightState);
        }

        [Test]
        public void SetupRenderingBasicEffectNoLights()
        {
            //Setup
            UseBasicEffect();
            Expect.Once.On(basicEffect).SetProperty("LightingEnabled").To(true);
            Expect.Once.On(basicEffect).SetProperty("AmbientLightColor").To(sceneAmbient.ToVector3());
            Expect.Once.On(directionalLight0).SetProperty("Enabled").To(false);
            Expect.Once.On(directionalLight1).SetProperty("Enabled").To(false);
            Expect.Once.On(directionalLight2).SetProperty("Enabled").To(false);
            Expect.Once.On(basicEffect).SetProperty("World").To(worldMatrix);
            Expect.Once.On(basicEffect).SetProperty("View").To(viewMatrix);
            Expect.Once.On(basicEffect).SetProperty("Projection").To(projMatrix);
            Expect.Once.On(renderState).SetProperty("AlphaBlendEnable").To(false);

            // Exercise SUT
            materialHandler.SetupRendering(new Matrix[] { worldMatrix }, viewMatrix, projMatrix, sceneAmbient, lightState);
        }

        [Test]
        public void SetupRenderingBasicEffectTwoLights()
        {
            //Setup
            UseBasicEffect();
            lightState.NewState(new Vector3(1, 2, 3), new Vector3(4, 5, 6), new Color(7, 8, 9), new Color(10, 11, 12));
            lightState.NewState(new Vector3(11, 12, 13), new Vector3(14, 15, 16), new Color(17, 18, 19), new Color(20, 21, 22));
            Expect.Once.On(basicEffect).SetProperty("LightingEnabled").To(true);
            Expect.Once.On(basicEffect).SetProperty("AmbientLightColor").To(sceneAmbient.ToVector3());
            Expect.Once.On(directionalLight0).SetProperty("Enabled").To(true);
            Expect.Once.On(directionalLight1).SetProperty("Enabled").To(true);
            Expect.Once.On(directionalLight2).SetProperty("Enabled").To(false);
            Expect.Once.On(basicEffect).SetProperty("World").To(worldMatrix);
            Expect.Once.On(basicEffect).SetProperty("View").To(viewMatrix);
            Expect.Once.On(basicEffect).SetProperty("Projection").To(projMatrix);
            Expect.Once.On(renderState).SetProperty("AlphaBlendEnable").To(false);

            Expect.Once.On(directionalLight0).SetProperty("DiffuseColor").To(new Color(7, 8, 9).ToVector3());
            Expect.Once.On(directionalLight0).SetProperty("SpecularColor").To(new Color(7, 8, 9).ToVector3());
            Expect.Once.On(directionalLight0).SetProperty("Direction").To(new Vector3(4, 5, 6));
            Expect.Once.On(directionalLight1).SetProperty("DiffuseColor").To(new Color(17, 18, 19).ToVector3());
            Expect.Once.On(directionalLight1).SetProperty("SpecularColor").To(new Color(17, 18, 19).ToVector3());
            Expect.Once.On(directionalLight1).SetProperty("Direction").To(new Vector3(14, 15, 16));

            // Exercise SUT
            materialHandler.SetupRendering(new Matrix[] { worldMatrix }, viewMatrix, projMatrix, sceneAmbient, lightState);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetAmbientColorFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            materialHandler.AmbientColor = new Color(1, 2, 3);
        }

        [Test]
        public void SetAmbientColor()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("AmbientColor", ambientParameter);
            Expect.Once.On(ambientParameter).Method("SetValue").With(new Color(1, 2, 3).ToVector3());
            // Exercise SUT
            materialHandler.AmbientColor = new Color(1, 2, 3);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void GetAmbientColorFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            Color c = materialHandler.AmbientColor;
        }

        [Test]
        public void GetAmbientColor()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("AmbientColor", ambientParameter);
            Expect.Once.On(ambientParameter).Method("GetValueVector3").Will(Return.Value(new Vector3(1, 2, 3)));
            // Exercise SUT
            Color color = materialHandler.AmbientColor;
            // Verify
            Assert.AreEqual(new Color(new Vector3(1, 2, 3)), color);
        }

        [Test]
        public void SetDiffuseColor()
        {
            //Setup 
            IEffectParameter diffuseParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("DiffuseColor", diffuseParameter);
            Expect.Once.On(diffuseParameter).Method("SetValue").With(new Color(1, 2, 3).ToVector3());
            // Exercise SUT
            materialHandler.DiffuseColor = new Color(1, 2, 3);
        }

        [Test]
        public void SetDiffuseColorBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).SetProperty("DiffuseColor").To(new Color(1, 2, 3).ToVector3());
            // Exercise SUT
            materialHandler.DiffuseColor = new Color(1, 2, 3);
        }

        [Test]
        public void GetDiffuseColor()
        {
            //Setup 
            IEffectParameter diffuseParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("DiffuseColor", diffuseParameter);
            Expect.Once.On(diffuseParameter).Method("GetValueVector3").Will(Return.Value(new Vector3(1, 2, 3)));
            // Exercise SUT
            Color color = materialHandler.DiffuseColor;
            // Verify
            Assert.AreEqual(new Color(new Vector3(1, 2, 3)), color);
        }

        [Test]
        public void GetDiffuseColorBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).GetProperty("DiffuseColor").Will(Return.Value(new Vector3(1, 2, 3)));
            // Exercise SUT
            Color color = materialHandler.DiffuseColor;
            // Verify
            Assert.AreEqual(new Color(new Vector3(1, 2, 3)), color);
        }

        [Test]
        public void SetSpecularColor()
        {
            //Setup 
            IEffectParameter specularParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("SpecularColor", specularParameter);
            Expect.Once.On(specularParameter).Method("SetValue").With(new Color(1, 2, 3).ToVector3());
            // Exercise SUT
            materialHandler.SpecularColor = new Color(1, 2, 3);
        }

        [Test]
        public void SetSpecularColorBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).SetProperty("SpecularColor").To(new Color(1, 2, 3).ToVector3());
            // Exercise SUT
            materialHandler.SpecularColor = new Color(1, 2, 3);
        }

        [Test]
        public void GetSpecularColor()
        {
            //Setup 
            IEffectParameter specularParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("SpecularColor", specularParameter);
            Expect.Once.On(specularParameter).Method("GetValueVector3").Will(Return.Value(new Vector3(1, 2, 3)));
            // Exercise SUT
            Color color = materialHandler.SpecularColor;
            // Verify
            Assert.AreEqual(new Color(new Vector3(1, 2, 3)), color);
        }

        [Test]
        public void GetSpecularColorBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).GetProperty("SpecularColor").Will(Return.Value(new Vector3(1, 2, 3)));
            // Exercise SUT
            Color color = materialHandler.SpecularColor;
            // Verify
            Assert.AreEqual(new Color(new Vector3(1, 2, 3)), color);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetShininessFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            materialHandler.Shininess = 1;
        }

        [Test]
        public void SetShininess()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("Shininess", ambientParameter);
            Expect.Once.On(ambientParameter).Method("SetValue").With(10.0f);
            // Exercise SUT
            materialHandler.Shininess = 10;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void GetShininessFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            float c = materialHandler.Shininess;
        }

        [Test]
        public void GetShininess()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("Shininess", ambientParameter);
            Expect.Once.On(ambientParameter).Method("GetValueSingle").Will(Return.Value(2.0f));
            // Exercise SUT
            float shininess = materialHandler.Shininess;
            // Verify
            Assert.AreEqual(2.0f, shininess);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetTransparencyFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            materialHandler.Transparency = 1;
        }

        [Test]
        public void SetTransparency()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("Transparency", ambientParameter);
            Expect.Once.On(ambientParameter).Method("SetValue").With(10.0f);
            // Exercise SUT
            materialHandler.Transparency = 10;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void GetTransparencyFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            float c = materialHandler.Transparency;
        }

        [Test]
        public void GetTransparency()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("Transparency", ambientParameter);
            Expect.Once.On(ambientParameter).Method("GetValueSingle").Will(Return.Value(2.0f));
            // Exercise SUT
            float transparency = materialHandler.Transparency;
            // Verify
            Assert.AreEqual(2.0f, transparency);
        }

        [Test]
        public void SetSpecularPower()
        {
            //Setup 
            IEffectParameter specularParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("SpecularPower", specularParameter);
            Expect.Once.On(specularParameter).Method("SetValue").With(1.2f);
            // Exercise SUT
            materialHandler.SpecularPower = 1.2f;
        }

        [Test]
        public void SetSpecularPowerBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).SetProperty("SpecularPower").To(2.3f);
            // Exercise SUT
            materialHandler.SpecularPower = 2.3f;
        }

        [Test]
        public void GetSpecularPower()
        {
            //Setup 
            IEffectParameter specularParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("SpecularPower", specularParameter);
            Expect.Once.On(specularParameter).Method("GetValueSingle").Will(Return.Value(3.4f));
            // Exercise SUT
            float power = materialHandler.SpecularPower;
            // Verify
            Assert.AreEqual(3.4f, power);
        }

        [Test]
        public void GetSpecularPowerBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).GetProperty("SpecularPower").Will(Return.Value(4.5f));
            // Exercise SUT
            float power = materialHandler.SpecularPower;
            // Verify
            Assert.AreEqual(4.5f, power);
        }

        [Test]
        public void SetDiffuseTexture()
        {
            //Setup 
            IEffectParameter textureParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("Texture", textureParameter);
            Expect.Once.On(textureParameter).Method("SetValue").With(texture2D);
            // Exercise SUT
            materialHandler.DiffuseTexture = texture2D;
        }

        [Test]
        public void SetSDiffuseTextureBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).SetProperty("Texture").To(texture2D);
            Expect.Once.On(basicEffect).SetProperty("TextureEnabled").To(true);
            // Exercise SUT
            materialHandler.DiffuseTexture = texture2D;
        }

        [Test]
        public void SetSDiffuseTextureBasicEffectNull()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).SetProperty("Texture").To(Is.Null);
            Expect.Once.On(basicEffect).SetProperty("TextureEnabled").To(false);
            // Exercise SUT
            materialHandler.DiffuseTexture = null;
        }

        [Test]
        public void GetDiffuseTexture()
        {
            //Setup 
            IEffectParameter textureParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("Texture", textureParameter);
            Expect.Once.On(textureParameter).Method("GetValueTexture2D").Will(Return.Value(texture2D));
            // Exercise SUT
            ITexture2D texture = materialHandler.DiffuseTexture;
            // Verify
            Assert.AreEqual(texture2D, texture);
        }

        [Test]
        public void GetDiffuseTextureBasicEffect()
        {
            //Setup 
            UseBasicEffect();
            Expect.Once.On(basicEffect).GetProperty("Texture").Will(Return.Value(texture2D));
            // Exercise SUT
            ITexture2D texture = materialHandler.DiffuseTexture;
            // Verify
            Assert.AreEqual(texture2D, texture);
        }

        [Test]
        public void SetNormalTexture()
        {
            //Setup 
            IEffectParameter textureParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("NormalMap", textureParameter);
            Expect.Once.On(textureParameter).Method("SetValue").With(texture2D);
            // Exercise SUT
            materialHandler.NormalTexture = texture2D;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetNormalTextureFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            materialHandler.NormalTexture = null;
        }

        [Test]
        public void GetNormalTexture()
        {
            //Setup 
            IEffectParameter textureParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("NormalMap", textureParameter);
            Expect.Once.On(textureParameter).Method("GetValueTexture2D").Will(Return.Value(texture2D));
            // Exercise SUT
            ITexture2D texture = materialHandler.NormalTexture;
            // Verify
            Assert.AreEqual(texture2D, texture);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void GetNormalTextureFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            ITexture2D texture = materialHandler.NormalTexture;
        }

        [Test]
        public void SetReflectiveTexture()
        {
            //Setup 
            IEffectParameter textureParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("ReflectiveMap", textureParameter);
            Expect.Once.On(textureParameter).Method("SetValue").With(textureCube);
            // Exercise SUT
            materialHandler.ReflectiveTexture = textureCube;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetReflectiveTextureFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            materialHandler.ReflectiveTexture = null;
        }

        [Test]
        public void GetReflectiveTexture()
        {
            //Setup 
            IEffectParameter textureParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("ReflectiveMap", textureParameter);
            Expect.Once.On(textureParameter).Method("GetValueTextureCube").Will(Return.Value(textureCube));
            // Exercise SUT
            ITextureCube texture = materialHandler.ReflectiveTexture;
            // Verify
            Assert.AreEqual(textureCube, texture);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void GetReflectiveTextureFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            ITextureCube texture = materialHandler.ReflectiveTexture;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void SetReflectiveFactorFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            materialHandler.ReflectiveFactor = 1;
        }

        [Test]
        public void SetReflectiveFactor()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("ReflectiveFactor", ambientParameter);
            Expect.Once.On(ambientParameter).Method("SetValue").With(10.0f);
            // Exercise SUT
            materialHandler.ReflectiveFactor = 10;
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void GetReflectiveFactorFail()
        {
            //Setup 
            UseBasicEffect();
            // Exercise SUT
            float c = materialHandler.ReflectiveFactor;
        }

        [Test]
        public void GetReflectiveFactor()
        {
            //Setup 
            IEffectParameter ambientParameter = mockery.NewMock<IEffectParameter>();
            UseEffect();
            StubParameter("ReflectiveFactor", ambientParameter);
            Expect.Once.On(ambientParameter).Method("GetValueSingle").Will(Return.Value(2.0f));
            // Exercise SUT
            float reflectiveFactor = materialHandler.ReflectiveFactor;
            // Verify
            Assert.AreEqual(2.0f, reflectiveFactor);
        }

        [Test]
        public void ChangeEffect()
        {
            // Setup
            IEffect newEffect = mockery.NewMock<IEffect>();
            UseEffect();
            Expect.Once.On(effectConverter).Method("Convert").With(effect, newEffect);
            // Exercise SUT
            materialHandler.Effect = newEffect;
            // Verify
            Assert.AreSame(newEffect, materialHandler.Effect);
        }

    }
}
