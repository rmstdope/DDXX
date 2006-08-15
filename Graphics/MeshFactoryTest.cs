using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;

namespace Graphics
{
    class TestMesh : IMesh
    {
        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap) { }
        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, GraphicsStream adjacency) { }
        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, int[] adjacency) { }
        public void ComputeTangentFrame(TangentOptions options) { }
        public bool Intersect(Vector3 rayPos, Vector3 rayDir) { return true; }
        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit) { closestHit = new IntersectInformation(); return true; }
        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits) { allHits = null;  return true; }
        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits) { closestHit = new IntersectInformation(); allHits = null; return true; }
        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir) { return true; }
        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit) { closestHit = new IntersectInformation(); return true; }
        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits) { allHits = new IntersectInformation[1]; return true; }
        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits) { allHits = new IntersectInformation[1]; closestHit = new IntersectInformation(); return true; }
        public GraphicsStream LockAttributeBuffer(LockFlags flags) { return null; }
        public int[] LockAttributeBufferArray(LockFlags flags) { return null; }
        public Mesh Optimize(MeshFlags flags, GraphicsStream adjacencyIn) { return null; }
        public Mesh Optimize(MeshFlags flags, int[] adjacencyIn) { return null; }
        public Mesh Optimize(MeshFlags flags, GraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap) { adjacencyIn = null; adjacencyOut = null; faceRemap = null; vertexRemap = null; return null; }
        public Mesh Optimize(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap) { adjacencyIn = null; adjacencyOut = null; faceRemap = null; vertexRemap = null; return null; }
        public void OptimizeInPlace(MeshFlags flags, GraphicsStream adjacencyIn) { }
        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn) { }
        public void OptimizeInPlace(MeshFlags flags, GraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap) { adjacencyIn = null; adjacencyOut = null; faceRemap = null; vertexRemap = null; }
        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap) { adjacencyIn = null; adjacencyOut = null; faceRemap = null; vertexRemap = null; }
        public void Save(System.IO.Stream stream, GraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format) { }
        public void Save(System.IO.Stream stream, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format) { }
        public void Save(string filename, GraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format) { }
        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format) { }
        public void Save(System.IO.Stream stream, GraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format) { }
        public void Save(System.IO.Stream stream, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format) { }
        public void Save(string filename, GraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format) { }
        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format) { }
        public void SetAttributeTable(AttributeRange[] table) { }
        public void UnlockAttributeBuffer() { }
        public void UnlockAttributeBuffer(int[] dataAttribute) { }
        public void Validate(GraphicsStream adjacency) { }
        public void Validate(int[] adjacency) { }
        public void Validate(GraphicsStream adjacency, out string errorsAndWarnings) { errorsAndWarnings = null; }
        public void Validate(int[] adjacency, out string errorsAndWarnings) { errorsAndWarnings = null; }
        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn) { adjacencyIn = null; }
        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, GraphicsStream adjacencyIn, GraphicsStream adjacencyOut) { adjacencyIn = null; adjacencyOut = null; }
        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out GraphicsStream vertexRemap) { adjacencyIn = null; vertexRemap = null; }
        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, GraphicsStream adjacencyIn, GraphicsStream adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap) { adjacencyIn = null; adjacencyOut = null; vertexRemap = null; faceRemap = null; }
        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out GraphicsStream vertexRemap) { adjacencyIn = null; adjacencyOut = null; faceRemap = null; vertexRemap = null; }
    }
    class TestFactory : IFactory
    {
        public IDevice CreateDevice(int adapter, DeviceType deviceType, System.Windows.Forms.Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters) { return null; }
        public IManager CreateManager() { return null; }
        public ITexture CreateTexture(IDevice device, System.Drawing.Bitmap image, Usage usage, Pool pool) { return null; }
        public ITexture CreateTexture(IDevice device, System.IO.Stream data, Usage usage, Pool pool) { return null; }
        public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool) { return null; }
        public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth) { return new TestMesh(); }
        public IEffect CreateEffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool) { return null; }
    }

    [TestFixture]
    public class MeshFactoryTest
    {
        IFactory factory = new TestFactory();
        MeshFactory meshFactory;

        [SetUp]
        public void SetUp()
        {
            meshFactory = new MeshFactory(null, factory);
        }

        [TearDown]
        public void TearDown()
        {
        }

        [Test]
        public void CreateBoxTest()
        {
            IMesh mesh1 = meshFactory.CreateBox(10.0f, 20.0f, 30.0f, MeshFactory.Usage.Static);
            Assert.IsNotNull(mesh1);

            IMesh mesh2 = meshFactory.CreateBox(10.0f, 20.0f, 30.0f, MeshFactory.Usage.Static);
            Assert.AreSame(mesh1, mesh2);

            IMesh mesh3 = meshFactory.CreateBox(10.0f, 22.0f, 30.0f, MeshFactory.Usage.Static);

            Assert.AreNotSame(mesh1, mesh3);
            Assert.AreNotSame(mesh2, mesh3);

            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountBoxes);

            mesh1 = null;
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountBoxes);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountBoxes);

            mesh2.Validate((int[])null);
            mesh2 = null;
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountBoxes);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(1, meshFactory.Count);
            Assert.AreEqual(1, meshFactory.CountBoxes);

            mesh3.Validate((int[])null);
            mesh3 = null;
            Assert.AreEqual(1, meshFactory.Count);
            Assert.AreEqual(1, meshFactory.CountBoxes);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(0, meshFactory.Count);
            Assert.AreEqual(0, meshFactory.CountBoxes);
        }
    }
}
