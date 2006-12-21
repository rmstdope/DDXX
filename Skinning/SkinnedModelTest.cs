using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class SkinnedModelTest
    {
        private Mockery mockery;
        private IAnimationRootFrame rootFrame;
        private IFrame frame;
        private IMeshContainer meshContainer;
        private IMeshData meshData;
        private IMesh mesh;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            rootFrame = mockery.NewMock<IAnimationRootFrame>();
            frame = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            meshData = mockery.NewMock<IMeshData>();
            mesh = mockery.NewMock<IMesh>();

            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(frame));
            Stub.On(frame).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Stub.On(meshContainer).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(meshData).GetProperty("Mesh").Will(Return.Value(mesh));
        }

        [Test]
        public void GetterTest()
        {
            SkinnedModel model = new SkinnedModel(rootFrame);

            Assert.AreSame(mesh, model.Mesh);
        }
    }
}
