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
        private ExtendedMaterial[] materials = new ExtendedMaterial[2];

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
            materials[0] = new ExtendedMaterial();
            Material m = new Material();
            m.AmbientColor = new ColorValue(1, 1, 1);
            materials[0].Material3D = m;
            materials[1] = new ExtendedMaterial();

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
            private ExtendedMaterial material;

            public MaterialMatcher(ExtendedMaterial material)
            {
                this.material = material;
            }

            public override bool Matches(object o)
            {
                if (!(o is ExtendedMaterial)) return false;
                ExtendedMaterial m = (ExtendedMaterial)o;

                if (m.Material3D == material.Material3D ||
                    m.TextureFilename == material.TextureFilename)
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
            //using (mockery.Ordered)
            {
                //Subset 1
                Expect.Once.On(effectHandler).Method("SetMeshConstants").With(scene, node);
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
