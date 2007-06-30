using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class ModelNodeTest
    {
        private Mockery mockery;
        private IMesh mesh;
        private IEffect effect;
        private IEffectHandler effectHandler;
        private IScene scene;
        private IRenderableCamera camera;
        private IModel model;
        private IDevice device;
        private ModelMaterial[] materials;
        private Matrix worldMatrix = Matrix.Identity;
        private Matrix viewMatrix = Matrix.RotationX(4.0f);
        private Matrix projectionMatrix = Matrix.RotationY(1.3f);
        private ColorValue sceneAmbient = new ColorValue(0.1f, 0.2f, 0.3f);

        private ModelNode node;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();
            scene = mockery.NewMock<IScene>();
            camera = mockery.NewMock<IRenderableCamera>();
            model = mockery.NewMock<IModel>();
            device = mockery.NewMock<IDevice>();
            Material m = new Material();
            m.AmbientColor = new ColorValue(1, 1, 1);
            materials = new ModelMaterial[2];
            materials[0] = new ModelMaterial(m, null);
            materials[1] = new ModelMaterial(new Material(), null);

            Stub.On(effectHandler).GetProperty("Effect").Will(Return.Value(effect));
            Stub.On(scene).GetProperty("ActiveCamera").Will(Return.Value(camera));
            Stub.On(camera).GetProperty("ViewMatrix").Will(Return.Value(viewMatrix));
            Stub.On(camera).GetProperty("ProjectionMatrix").Will(Return.Value(projectionMatrix));
            Stub.On(scene).GetProperty("AmbientColor").Will(Return.Value(sceneAmbient));
            Stub.On(model).GetProperty("Mesh").Will(Return.Value(mesh));
            Stub.On(model).GetProperty("Materials").Will(Return.Value(materials));

            node = new ModelNode("Name", model, effectHandler, device);
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        class MaterialMatcher : Matcher
        {
            private ModelMaterial material;

            public MaterialMatcher(ModelMaterial material)
            {
                this.material = material;
            }

            public override bool Matches(object o)
            {
                if (!(o is ModelMaterial)) return false;
                ModelMaterial m = (ModelMaterial)o;

                if (m.Ambient == material.Ambient &&
                    m.Diffuse == material.Diffuse &&
                    m.NormalTexture == material.NormalTexture &&
                    m.DiffuseTexture == m.DiffuseTexture)
                    return true;

                return false;
            }

            public override void DescribeTo(TextWriter writer)
            {
                writer.Write(material.ToString());
            }
        }

        [Test]
        public void RenderTestOK()
        {
            Expect.Once.On(model).Method("Render").With(device, effectHandler, sceneAmbient, worldMatrix, viewMatrix, projectionMatrix);
            node.Render(scene);
        }

        [Test]
        public void TestStep()
        {
            Expect.Once.On(model).Method("Step");
            node.Step();
        }
    }
}
