using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using NMock2;

namespace Dope.DDXX.Skinning
{
    [TestFixture]
    public class SkinnedModelFactoryTest
    {
        SkinnedModelFactory factory;
        Mockery mockery;
        IDevice device;
        IGraphicsFactory graphicsFactory;
        IAnimationRootFrame rootFrame;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            device = mockery.NewMock<IDevice>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            rootFrame = mockery.NewMock<IAnimationRootFrame>();

            factory = new SkinnedModelFactory(device, graphicsFactory, null);
        }

        [Test]
        public void TestCreate()
        {
            Expect.Once.On(graphicsFactory).Method("SkinnedMeshFromFile").
                With(Is.EqualTo(device), Is.EqualTo("Filename"), Is.NotNull).Will(Return.Value(rootFrame));
            factory.FromFile("Filename", ModelFactory.Options.SkinnedModel);
        }
    }
}
