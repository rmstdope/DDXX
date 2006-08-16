using System;
using System.Collections.Generic;
using System.Text;
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
        private CameraNode camera;
        private EffectInstance[] instances = new EffectInstance[2];

        private MeshNode node;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            mesh = mockery.NewMock<IMesh>();
            effect = mockery.NewMock<IEffect>();
            camera = new CameraNode("Camera");
            instances[0] = new EffectInstance();
            instances[1] = new EffectInstance();

            node = new MeshNode("Name", new Dope.DDXX.Graphics.MeshContainer(mesh, instances));
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void RenderTestFail()
        {
            // No effect set
            node.Render(camera);
        }

        [Test]
        public void RenderTestOK()
        {
            node.EffectTechnique = new EffectContainer(effect, null);

            using (mockery.Ordered)
            {
                //Subset 1
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(1));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(0);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
                // Subset 2
                Expect.Once.On(effect).Method("Begin").With(FX.None).Will(Return.Value(2));
                Expect.Once.On(effect).Method("BeginPass").With(0);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("BeginPass").With(1);
                Expect.Once.On(mesh).Method("DrawSubset").With(1);
                Expect.Once.On(effect).Method("EndPass");
                Expect.Once.On(effect).Method("End");
            }


            node.Render(camera);
        }
    }
}
