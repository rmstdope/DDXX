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
    public class MeshNodeTest
    {
        private Mockery mockery;
        private IMesh mesh;
        private IEffect effect;
        private IEffectHandler effectHandler;
        private IRenderableScene scene;
        private CameraNode camera;
        private ModelMaterial[] materials;

        private MeshNode node;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            effect = mockery.NewMock<IEffect>();
            effectHandler = mockery.NewMock<IEffectHandler>();
            scene = mockery.NewMock<IRenderableScene>();
            camera = new CameraNode("Camera");
            Material m = new Material();
            m.AmbientColor = new ColorValue(1, 1, 1);
            materials = new ModelMaterial[2];
            materials[0] = new ModelMaterial(m, null);
            materials[1] = new ModelMaterial(new Material(), null);

            Stub.On(effectHandler).
                GetProperty("Effect").
                Will(Return.Value(effect));

            node = new MeshNode("Name", new Dope.DDXX.Graphics.Model(mesh, materials), effectHandler);
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

                if (m.Material == material.Material &&
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
            using (mockery.Ordered)
            {
                //Subset 1
                Expect.Once.On(effectHandler).Method("SetNodeConstants").With(scene, node);
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.Same(scene), new MaterialMatcher(materials[0]));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(0);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
                // Subset 2
                Expect.Once.On(effectHandler).Method("SetMaterialConstants").With(Is.Same(scene), new MaterialMatcher(materials[1]));
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(2));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("BeginPass").With(1);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
            }

            node.Render(scene);
        }
    }
}
