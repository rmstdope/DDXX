using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using NUnit.Framework;
using NMock2;

namespace Dope.DDXX.Graphics
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
        public VertexElement[] Declaration { get { return null; } }
        public Device Device { get { return null; } } 
        public bool Disposed { get { return true; } }
        public IndexBuffer IndexBuffer { get { return null; } } 
        public int NumberAttributes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int NumberBytesPerVertex
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int NumberFaces
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int NumberVertices
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public MeshOptions Options
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public VertexBuffer VertexBuffer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public VertexFormats VertexFormat
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Mesh Clone(MeshFlags options, GraphicsStream declaration, Device device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Mesh Clone(MeshFlags options, VertexElement[] declaration, Device device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Mesh Clone(MeshFlags options, VertexFormats vertexFormat, Device device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeNormals()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeNormals(GraphicsStream adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeNormals(int[] adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertAdjacencyToPointReps(GraphicsStream adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertAdjacencyToPointReps(int[] adjaceny)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertPointRepsToAdjacency(GraphicsStream pointReps)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertPointRepsToAdjacency(int[] pointReps)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawSubset(int attributeID)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GenerateAdjacency(float epsilon, int[] adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public AttributeRange[] GetAttributeTable()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockIndexBuffer(LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockIndexBuffer(Type typeIndex, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockVertexBuffer(LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockVertexBuffer(Type typeVertex, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetIndexBufferData(object data, LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexBufferData(object data, LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockIndexBuffer()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockVertexBuffer()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSemantics(GraphicsStream declaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSemantics(VertexElement[] declaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
    class TestFactory : IGraphicsFactory
    {
        public IDevice CreateDevice(int adapter, DeviceType deviceType, System.Windows.Forms.Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters) { return null; }
        public IManager CreateManager() { return null; }
        public ITexture CreateTexture(IDevice device, System.Drawing.Bitmap image, Usage usage, Pool pool) { return null; }
        public ITexture CreateTexture(IDevice device, System.IO.Stream data, Usage usage, Pool pool) { return null; }
        public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool) { return null; }
        public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth) { return new TestMesh(); }
        public IEffect CreateEffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool) { return null; }
        public IMesh MeshFromFile(IDevice device, string fileName, out EffectInstance[] effectInstance) { effectInstance = new EffectInstance[2]; return new TestMesh(); }
    }

    [TestFixture]
    public class MeshFactoryTest
    {
        IGraphicsFactory factory = new TestFactory();
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
            MeshContainer mesh1 = meshFactory.CreateBox(10.0f, 20.0f, 30.0f);

            MeshContainer mesh2 = meshFactory.CreateBox(10.0f, 20.0f, 30.0f);

            MeshContainer mesh3 = meshFactory.CreateBox(10.0f, 22.0f, 30.0f);

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh3);
            Assert.AreSame(mesh1, mesh2);
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

            mesh2.Mesh.Validate((int[])null);
            mesh2 = null;
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountBoxes);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(1, meshFactory.Count);
            Assert.AreEqual(1, meshFactory.CountBoxes);

            mesh3.Mesh.Validate((int[])null);
            mesh3 = null;
            Assert.AreEqual(1, meshFactory.Count);
            Assert.AreEqual(1, meshFactory.CountBoxes);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(0, meshFactory.Count);
            Assert.AreEqual(0, meshFactory.CountBoxes);
        }

        [Test]
        public void CreateFileTest()
        {
            MeshContainer mesh1 = meshFactory.FromFile("file1");

            MeshContainer mesh2 = meshFactory.FromFile("file1");

            MeshContainer mesh3 = meshFactory.FromFile("file2");

            Assert.IsNotNull(mesh1);
            Assert.IsNotNull(mesh3);
            Assert.AreSame(mesh1, mesh2);
            Assert.AreNotSame(mesh1, mesh3);
            Assert.AreNotSame(mesh2, mesh3);

            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountFiles);

            mesh1 = null;
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountFiles);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountFiles);

            mesh2.Mesh.Validate((int[])null);
            mesh2 = null;
            Assert.AreEqual(2, meshFactory.Count);
            Assert.AreEqual(2, meshFactory.CountFiles);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(1, meshFactory.Count);
            Assert.AreEqual(1, meshFactory.CountFiles);

            mesh3.Mesh.Validate((int[])null);
            mesh3 = null;
            Assert.AreEqual(1, meshFactory.Count);
            Assert.AreEqual(1, meshFactory.CountFiles);

            GC.Collect();
            meshFactory.AutoExpire();
            Assert.AreEqual(0, meshFactory.Count);
            Assert.AreEqual(0, meshFactory.CountFiles);
        }
    }
}
