using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class ModelFactoryTest : D3DMockTest
    {
        IModelFactory modelFactory;
        IModel model;
        IModelMesh modelMesh1;
        IModelMesh modelMesh2;
        IModelMeshPart modelMeshPart1;
        IModelMeshPart modelMeshPart2;
        IModelMeshPart modelMeshPart3;
        IEffect effect2;
        IEffect effect3;
        IEffect oldEffect;
        IEffect oldEffect2;
        IEffect oldEffect3;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            model = mockery.NewMock<IModel>();
            modelMesh1 = mockery.NewMock<IModelMesh>();
            modelMesh2 = mockery.NewMock<IModelMesh>();
            modelMeshPart1 = mockery.NewMock<IModelMeshPart>();
            modelMeshPart2 = mockery.NewMock<IModelMeshPart>();
            modelMeshPart3 = mockery.NewMock<IModelMeshPart>();
            oldEffect = mockery.NewMock<IEffect>();
            oldEffect2 = mockery.NewMock<IEffect>();
            oldEffect3 = mockery.NewMock<IEffect>();
            effect2 = mockery.NewMock<IEffect>();
            effect3 = mockery.NewMock<IEffect>();

            modelFactory = new ModelFactory(device, graphicsFactory, textureFactory);
        }

        [TearDown]
        public override void TearDown()
        {
            base.TearDown();
        }

        [Test]
        public void SingleEffectOnePart()
        {
            // Setup
            StubModelMesh(model, new IModelMesh[] { modelMesh1 });
            StubModelMeshPart(modelMesh1, new IModelMeshPart[] { modelMeshPart1 });
            Expect.Once.On(graphicsFactory).Method("ModelFromFile").
                With("File1").Will(Return.Value(model));
            Expect.Once.On(graphicsFactory).Method("EffectFromFile").
                With("Effect").Will(Return.Value(effect));
            Expect.Once.On(modelMeshPart1).SetProperty("Effect").To(effect);

            // Exercise SUT
            IModel model1 = modelFactory.FromFile("File1", "Effect");

            // Verify
            Assert.AreSame(model, model1);
        }

        [Test]
        public void SingleEffectMultipleParts()
        {
            // Setup
            StubModelMesh(model, new IModelMesh[] { modelMesh1, modelMesh2 });
            StubModelMeshPart(modelMesh1, new IModelMeshPart[] { modelMeshPart1, modelMeshPart2 });
            StubModelMeshPart(modelMesh2, new IModelMeshPart[] { modelMeshPart3 });
            Expect.Once.On(graphicsFactory).Method("ModelFromFile").
                With("File1").Will(Return.Value(model));
            Expect.Once.On(graphicsFactory).Method("EffectFromFile").
                With("Effect").Will(Return.Value(effect));
            Expect.Once.On(graphicsFactory).Method("EffectFromFile").
                With("Effect").Will(Return.Value(effect2));
            Expect.Once.On(graphicsFactory).Method("EffectFromFile").
                With("Effect").Will(Return.Value(effect3));

            Expect.Once.On(modelMeshPart1).SetProperty("Effect").To(effect);
            Expect.Once.On(modelMeshPart2).SetProperty("Effect").To(effect2);
            Expect.Once.On(modelMeshPart3).SetProperty("Effect").To(effect3);

            // Exercise SUT
            IModel model1 = modelFactory.FromFile("File1", "Effect");

            // Verify
            Assert.AreSame(model, model1);
        }

    }
}
