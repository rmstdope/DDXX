using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using Dope.DDXX.Utility;

namespace MeshBuilder
{
    [TestFixture]
    public class MeshBuilderTest : IGraphicsFactory, IMesh, IPrimitive
    {
        private MeshBuilder builder;

        [SetUp]
        public void SetUp()
        {
            builder = new MeshBuilder(this);
        }

        /// <summary>
        /// Test creating a mesh from a primitive
        /// </summary>
        [Test]
        public void TestCreateMesh()
        {
            builder.AddPrimitive(this, "Name1");
            IMesh mesh = builder.CreateMesh("Name1");
            Assert.AreSame(this, mesh, "This instance should be returned as IMesh.");
        }

        /// <summary>
        /// Test to add two primitives with the same name.
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestMultipleAdd()
        {
            builder.AddPrimitive(this, "Name1");
            builder.AddPrimitive(this, "Name1");
        }

        /// <summary>
        /// Test calling CreateMesh without having added the primitive first.
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestCreateWithoutAdd()
        {
            IMesh mesh = builder.CreateMesh("Name1");
        }


        #region IMesh Members

        public VertexElement[] Declaration
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Device Device
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Disposed
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IndexBuffer IndexBuffer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

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

        public IMesh Clean(CleanType cleanType, IGraphicsStream adjacency, IGraphicsStream adjacencyOut)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clean(CleanType cleanType, int[] adjacency, out int[] adjacencyOut)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clean(CleanType cleanType, IGraphicsStream adjacency, IGraphicsStream adjacencyOut, out string errorsAndWarnings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clean(CleanType cleanType, int[] adjacency, out int[] adjacencyOut, out string errorsAndWarnings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clone(MeshFlags options, IGraphicsStream declaration, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clone(MeshFlags options, VertexElement[] declaration, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clone(MeshFlags options, VertexFormats vertexFormat, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeNormals()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeNormals(IGraphicsStream adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeNormals(int[] adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertAdjacencyToPointReps(IGraphicsStream adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertAdjacencyToPointReps(int[] adjaceny)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] ConvertPointRepsToAdjacency(IGraphicsStream pointReps)
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

        public IGraphicsStream LockIndexBuffer(LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockIndexBuffer(Type typeIndex, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockVertexBuffer(LockFlags flags)
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

        public void UpdateSemantics(IGraphicsStream declaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSemantics(VertexElement[] declaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, IGraphicsStream adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeTangent(int texStage, int tangentIndex, int binormIndex, int wrap, int[] adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ComputeTangentFrame(TangentOptions options)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Vector3 rayPos, Vector3 rayDir, out IntersectInformation closestHit, out IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockAttributeBuffer(LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] LockAttributeBufferArray(LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(MeshFlags flags, IGraphicsStream adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(MeshFlags flags, int[] adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(MeshFlags flags, IGraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(MeshFlags flags, IGraphicsStream adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(MeshFlags flags, IGraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, IGraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, IGraphicsStream adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, IGraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, IGraphicsStream adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, int[] adjacency, ExtendedMaterial[] materials, EffectInstance[] effects, XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetAttributeTable(AttributeRange[] table)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockAttributeBuffer()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockAttributeBuffer(int[] dataAttribute)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate(IGraphicsStream adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate(int[] adjacency)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate(IGraphicsStream adjacency, out string errorsAndWarnings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate(int[] adjacency, out string errorsAndWarnings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, IGraphicsStream adjacencyIn, IGraphicsStream adjacencyOut)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, IGraphicsStream adjacencyIn, IGraphicsStream adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(WeldEpsilonsFlags flags, WeldEpsilons epsilons, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IGraphicsFactory Members

        public IManager Manager
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ISphericalHarmonics SphericalHarmonics
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IDevice CreateDevice(int adapter, DeviceType deviceType, Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateTexture(IDevice device, System.Drawing.Bitmap image, Usage usage, Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateTexture(IDevice device, System.IO.Stream data, Usage usage, Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Usage usage, Format format, Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICubeTexture CreateCubeTexture(IDevice device, int edgeLength, int levels, Usage usage, Format format, Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexElement[] declaration, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh CreateMesh(int numFaces, int numVertices, MeshFlags options, VertexFormats vertexFormat, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out EffectInstance[] effectInstance)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out ExtendedMaterial[] materials)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dope.DDXX.Graphics.Skinning.IAnimationRootFrame SkinnedMeshFromFile(IDevice device, string fileName, AllocateHierarchy allocHierarchy)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dope.DDXX.Graphics.Skinning.IAnimationRootFrame LoadHierarchy(string fileName, IDevice device, AllocateHierarchy allocHierarchy, LoadUserData loadUserData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEffect EffectFromFile(IDevice device, string sourceDataFile, Include includeFile, string skipConstants, ShaderFlags flags, EffectPool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture TextureFromFile(IDevice device, string srcFile, int width, int height, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICubeTexture CubeTextureFromFile(IDevice device, string fileName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICubeTexture CubeTextureFromFile(IDevice device, string fileName, int size, int mipLevels, Usage usage, Format format, Pool pool, Filter filter, Filter mipFilter, int colorKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISprite CreateSprite(IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, IDevice device, Usage usage, VertexFormats vertexFormat, Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ILine CreateLine(IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public VertexDeclaration CreateVertexDeclaration(IDevice device, VertexElement[] elements)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFont CreateFont(IDevice device, FontDescription description)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFont CreateFont(IDevice device, System.Drawing.Font font)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFont CreateFont(IDevice device, int height, int width, FontWeight weight, int miplevels, bool italic, CharacterSet charset, Precision outputPrecision, FontQuality quality, PitchAndFamily pitchFamily, string faceName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IPrimitive Members

        public Vertex[] Vertices
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int[] Indices
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IMesh CreateMesh(IGraphicsFactory factory)
        {
            Assert.AreSame(this, factory, "Factory should be same as this.");
            return this;
        }

        #endregion
    }
}
