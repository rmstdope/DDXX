using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    [TestFixture]
    public class SkinnedModelFactoryTest
    {
        private SkinnedModelFactory factory;
        private Mockery mockery;
        private IDevice device;
        private IGraphicsFactory graphicsFactory;
        private IAnimationRootFrame rootFrame;
        private IFrame frame;
        private IMeshContainer meshContainer;
        private MeshDataAdapter meshData;
        private IMesh mesh;
        private ExtendedMaterial[] materials;
        private ITextureFactory textureFactory;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            device = mockery.NewMock<IDevice>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            rootFrame = mockery.NewMock<IAnimationRootFrame>();
            frame = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            mesh = mockery.NewMock<IMesh>();
            meshData = new MeshDataAdapter();
            meshData.Mesh = mesh;
            materials = new ExtendedMaterial[2];
            textureFactory = mockery.NewMock<ITextureFactory>();

            factory = new SkinnedModelFactory(device, graphicsFactory, textureFactory);
            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(frame));
            Stub.On(frame).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Stub.On(frame).GetProperty("FrameSibling").Will(Return.Value(null));
            Stub.On(frame).GetProperty("FrameFirstChild").Will(Return.Value(null));
            Stub.On(meshContainer).Method("GetMaterials").Will(Return.Value(materials));
            Stub.On(meshContainer).GetProperty("MeshData").Will(Return.Value(meshData));
            Stub.On(meshContainer).GetProperty("SkinInformation").Will(Return.Value(null));
        }

        [Test]
        public void TestCreate()
        {
            materials[0].TextureFilename = "0";
            materials[1].TextureFilename = "1";

            Expect.Once.On(graphicsFactory).Method("SkinnedMeshFromFile").
                With(Is.EqualTo(device), Is.EqualTo("Filename"), Is.NotNull).Will(Return.Value(rootFrame));
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("0");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("1");
            factory.FromFile("Filename", ModelFactory.Options.SkinnedModel);
        }
    }
}
