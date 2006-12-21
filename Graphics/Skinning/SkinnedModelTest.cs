using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics.Skinning
{
    [TestFixture]
    public class SkinnedModelTest
    {
        private Mockery mockery;
        private IAnimationRootFrame rootFrame;
        private IFrame frame1;
        private IFrame frame2;
        private IMeshContainer meshContainer;
        private IMeshData meshData;
        private IMesh mesh;
        private ITextureFactory textureFactory;
        private ExtendedMaterial[] materials;

        [SetUp]
        public void SetUp()
        {
            mockery = new Mockery();
            rootFrame = mockery.NewMock<IAnimationRootFrame>();
            frame1 = mockery.NewMock<IFrame>();
            frame2 = mockery.NewMock<IFrame>();
            meshContainer = mockery.NewMock<IMeshContainer>();
            meshData = mockery.NewMock<IMeshData>();
            mesh = mockery.NewMock<IMesh>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            materials = new ExtendedMaterial[2];

            Stub.On(rootFrame).GetProperty("FrameHierarchy").Will(Return.Value(frame1));
            Stub.On(meshContainer).GetProperty("MeshData").Will(Return.Value(meshData));
        }

        [Test]
        public void GetterTest()
        {
            materials[0].TextureFilename = "0";
            materials[1].TextureFilename = "1";

            // Find a real MeshContainer in second level 
            Expect.Once.On(frame1).GetProperty("MeshContainer").Will(Return.Value(null));
            Expect.Once.On(frame1).GetProperty("FrameFirstChild").Will(Return.Value(frame2));
            Stub.On(frame2).GetProperty("MeshContainer").Will(Return.Value(meshContainer));
            Expect.Once.On(meshData).GetProperty("Mesh").Will(Return.Value(mesh));
            Expect.Once.On(meshContainer).Method("GetMaterials").Will(Return.Value(materials));

            Expect.Once.On(textureFactory).Method("CreateFromFile").With("0");
            Expect.Once.On(textureFactory).Method("CreateFromFile").With("1");
            SkinnedModel model = new SkinnedModel(rootFrame, textureFactory);
            Assert.AreSame(mesh, model.Mesh);
        }
    }
}
