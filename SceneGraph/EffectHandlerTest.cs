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
        private INode node;
        private IRenderableScene scene;
        private ICamera camera;

        private EffectHandle defaultTechnique;
        private EffectHandle worldT;
        private EffectHandle worldViewProjectionT;

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projMatrix;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            effect = mockery.NewMock<IEffect>();
            node = mockery.NewMock<INode>();
            scene = mockery.NewMock<IRenderableScene>();
            camera = mockery.NewMock<ICamera>();

            Stub.On(scene).
                GetProperty("ActiveCamera").
                Will(Return.Value(camera));

            defaultTechnique = EffectHandle.FromString("Technique");
            worldT = EffectHandle.FromString("WorldT");
            worldViewProjectionT = EffectHandle.FromString("WorldViewProjectionT");

            worldMatrix = Matrix.Scaling(1, 3, 5);
            viewMatrix = Matrix.RotationYawPitchRoll(1, 2, 3);
            projMatrix = Matrix.PerspectiveLH(1, 1, 1, 10);

            Stub.On(effect).
                Method("FindNextValidTechnique").
                WithAnyArguments().
                Will(Return.Value(defaultTechnique));
            Stub.On(node).
                GetProperty("WorldMatrix").
                Will(Return.Value(worldMatrix));
            Stub.On(camera).
                GetProperty("ViewMatrix").
                Will(Return.Value(viewMatrix));
            Stub.On(camera).
                GetProperty("ProjectionMatrix").
                Will(Return.Value(projMatrix));
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
            EffectHandler effectHandler = new EffectHandler(effect);
        }

        public void SetAllConstantsTest()
        {
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "WorldT").
                Will(Return.Value(worldT));
            Expect.Once.On(effect).Method("GetParameter").
                With(null, "WorldViewProjectionT").
                Will(Return.Value(worldViewProjectionT));
            EffectHandler effectHandler = new EffectHandler(effect);

            Expect.Once.On(effect).
                Method("SetValueTranspose").
                With(worldT, worldMatrix);
            Expect.Once.On(effect).
                Method("SetValueTranspose").
                With(worldViewProjectionT, worldMatrix * viewMatrix * projMatrix);
            effectHandler.SetMeshConstants(scene, node);
        }
    }
}
