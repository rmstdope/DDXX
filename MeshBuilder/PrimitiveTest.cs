using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Drawing;
using System.Windows.Forms;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class PrimitiveTest : IGraphicsFactory, IMesh, IDevice, IGraphicsStream, IBody, ITexture
    {
        protected IPrimitive primitive;
        private int numFaces;
        private int numVertices;
        private bool vbLocked;
        private bool ibLocked;
        private VertexPosition vertexPosition;
        private List<Vector3> positions;
        private List<Vector3> normals;
        private List<float> textureCoordinates;
        private bool useTextureCoordinates;
        private short[] indices;

        private enum VertexPosition
        {
            POSITION,
            NORMAL,
            TEX_U,
            TEX_V,
        }

        private const float epsilon = 0.000001f;

        [SetUp]
        public virtual void SetUp()
        {
            vbLocked = false;
            ibLocked = false;
            positions = new List<Vector3>();
            normals = new List<Vector3>();
            textureCoordinates = new List<float>();
            vertexPosition = VertexPosition.POSITION;
            useTextureCoordinates = false;
        }

        /// <summary>
        /// Test the CreatePrimitive method on a generic mesh.
        /// </summary>
        [Test]
        public void TestCreateModel()
        {
            Vertex[] vertices;
            short[] indices;
            CreatePrimitive(out vertices, out indices);
            IModel model = primitive.CreateModel(this, this);
            CheckModel(vertices, indices, model);
            Assert.IsNotInstanceOfType(typeof(PhysicalModel), model, "Model shall not be PhysicalModel");
            Assert.IsNotNull(model.Materials[0]);
        }

        /// <summary>
        /// Test the CreatePrimitive method on a generic mesh with texture coordinates.
        /// </summary>
        [Test]
        public void TestCreateModelWithTexCoords()
        {
            useTextureCoordinates = true;
            Vertex[] vertices;
            short[] indices;
            CreatePrimitive(out vertices, out indices);
            IModel model = primitive.CreateModel(this, this);
            CheckModel(vertices, indices, model);
            Assert.IsNotInstanceOfType(typeof(PhysicalModel), model, "Model shall not be PhysicalModel");
        }

        /// <summary>
        /// Test that the created model is a physical model.
        /// </summary>
        [Test]
        public void TestCreatePhysicalModel()
        {
            Vertex[] vertices;
            short[] indices;
            CreatePrimitive(out vertices, out indices);
            primitive.Body = this;
            IModel model = primitive.CreateModel(this, this);
            CheckModel(vertices, indices, model);
            Assert.IsInstanceOfType(typeof(PhysicalModel), model, "Model shall be PhysicalModel");
        }

        /// <summary>
        /// Test that a material is created ok.
        /// </summary>
        [Test]
        public void TestMaterial()
        {
            Material material = new Material();
            material.Ambient = Color.White;
            material.Diffuse = Color.Red;
            ModelMaterial modelMaterial = new ModelMaterial(material);
            Vertex[] vertices;
            short[] indices;
            CreatePrimitive(out vertices, out indices);
            primitive.ModelMaterial = modelMaterial;
            IModel model = primitive.CreateModel(this, this);
            CheckMaterial(model);
            CheckModel(vertices, indices, model);
        }

        /// <summary>
        /// Test that a material is created ok for a physical model.
        /// </summary>
        [Test]
        public void TestMaterialOnBody()
        {
            Material material = new Material();
            material.Ambient = Color.White;
            material.Diffuse = Color.Red;
            ModelMaterial modelMaterial = new ModelMaterial(material);
            Vertex[] vertices;
            short[] indices;
            CreatePrimitive(out vertices, out indices);
            primitive.Body = this;
            primitive.ModelMaterial = modelMaterial;
            IModel model = primitive.CreateModel(this, this);
            CheckMaterial(model);
            CheckModel(vertices, indices, model);
        }

        /// <summary>
        /// Test welding two verices with same position.
        /// Check only positions.
        /// </summary>
        [Test]
        public void TestWeldingSamePosition()
        {
            Vector3[] position = new Vector3[] { new Vector3(), new Vector3(1e-10f, 0, 0) };
            Vector3[] newPositions = new Vector3[] { new Vector3() };
            primitive = CreatePrimitiveFromLists(position, null, null, null);
            primitive.Weld(0.0f);
            CompareVertices(primitive, newPositions, null, null);
        }

        /// <summary>
        /// Test welding of same position with more vertices
        /// </summary>
        [Test]
        public void TestWeldingSamePositionMoreVertices()
        {
            Vector3[] positions = new Vector3[] { new Vector3(0.1f, 0, 0), new Vector3(), new Vector3(0.3f, 0, 0), new Vector3() };
            Vector3[] newPositions = new Vector3[] { new Vector3(0.1f, 0, 0), new Vector3(), new Vector3(0.3f, 0, 0) };
            primitive = CreatePrimitiveFromLists(positions, null, null, null);
            primitive.Weld(0.0f);
            CompareVertices(primitive, newPositions, null, null);
        }

        /// <summary>
        /// Test welding vertices with different normals (should be averaged).
        /// </summary>
        [Test]
        public void TestWeldingDifferentNormals()
        {
            Vector3[] positions = new Vector3[] { new Vector3(), new Vector3() };
            Vector3[] newPositions = new Vector3[] { new Vector3() };
            Vector3[] normals = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0) };
            Vector3[] newNormals = new Vector3[] { new Vector3(1, 1, 0) };
            primitive = CreatePrimitiveFromLists(positions, normals, null, null);
            primitive.Weld(0.0f);
            CompareVertices(primitive, newPositions, newNormals, null);
        }

        /// <summary>
        /// Test welding vertices with different normals (should be averaged).
        /// </summary>
        [Test]
        public void TestWeldingDifferentUV()
        {
            Vector3[] positions = new Vector3[] { new Vector3(), new Vector3() };
            Vector3[] newPositions = new Vector3[] { new Vector3() };
            Vector2[] uv = new Vector2[] { new Vector2(10, 20), new Vector2(30, 20) };
            Vector2[] newUV = new Vector2[] { new Vector2(20, 20) };
            primitive = CreatePrimitiveFromLists(positions, null, uv, null);
            primitive.Weld(0.0f);
            CompareVertices(primitive, newPositions, null, newUV);
        }

        /// <summary>
        /// Test welding two vertices that are close enough but not on the same position.
        /// </summary>
        [Test]
        public void TestWeldingDifferentPositions()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(0.0f, 0, 0), new Vector3(0.5f, 0, 0), 
                new Vector3(0.3f, 0, 0), new Vector3(0.1f, 0, 0) };
            Vector3[] newPositions = new Vector3[] { 
                new Vector3(0.0f, 0, 0), new Vector3(0.5f, 0, 0), 
                new Vector3(0.3f, 0, 0) };
            primitive = CreatePrimitiveFromLists(positions, null, null, null);
            primitive.Weld(0.1f);
            CompareVertices(primitive, newPositions, null, null);
        }

        /// <summary>
        /// Test that indices are changed when welding.
        /// </summary>
        [Test]
        public void TestWeldingIndices1()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 1, 2, 3 };
            short[] newIndices = new short[] { 0, 1, 2 };
            primitive = CreatePrimitiveFromLists(positions, null, null, indices);
            primitive.Weld(0.0f);
            CompareIndices(primitive, newIndices);
        }

        /// <summary>
        /// Test that more indices are changed when welding.
        /// </summary>
        [Test]
        public void TestWeldingIndices2()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 0, 2, 3, 3, 2, 1 };
            short[] newIndices = new short[] { 0, 1, 2, 2, 1, 0 };
            primitive = CreatePrimitiveFromLists(positions, null, null, indices);
            primitive.Weld(0.0f);
            CompareIndices(primitive, newIndices);
        }

        /// <summary>
        /// Test that welding removes triangles with two vertices equals.
        /// </summary>
        [Test]
        public void TestWeldingRemoveTrianglesSameVertices()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 0, 1, 2 };
            short[] newIndices = new short[] {  };
            primitive = CreatePrimitiveFromLists(positions, null, null, indices);
            primitive.Weld(0.0f);
            CompareIndices(primitive, newIndices);
        }

        /// <summary>
        /// Test that welding removes triangles with two vertices equals.
        /// </summary>
        [Test]
        public void TestWeldingRemoveTrianglesSameVertices2()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(), new Vector3() , new Vector3(1, 1, 1), new Vector3(2, 2, 2) };
            short[] indices = new short[] { 0, 0, 0, 0, 1, 2, 1, 2, 3, 2, 2, 2, 3, 3, 3, 1, 1, 1 };
            short[] newIndices = new short[] { 0, 1, 2 };
            primitive = CreatePrimitiveFromLists(positions, null, null, indices);
            primitive.Weld(0.0f);
            CompareIndices(primitive, newIndices);
        }

        /// <summary>
        /// Test that welding remove duplicated triangles.
        /// </summary>
        [Test]
        public void TestWeldingRemoveDuplicateTriangles()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(1,1,1), new Vector3(2,2,2) , new Vector3(3,3,3) };
            short[] indices = new short[] { 0, 1, 2, 0, 1, 2};
            short[] newIndices = new short[] { 0, 1, 2 };
            primitive = CreatePrimitiveFromLists(positions, null, null, indices);
            primitive.Weld(0.0f);
            CompareIndices(primitive, newIndices);
        }

        /// <summary>
        /// Test that welding remove duplicated triangles even if permutated.
        /// We must make sure that culling is honored though!
        /// </summary>
        [Test]
        public void TestWeldingRemoveDuplicateTriangles2()
        {
            Vector3[] positions = new Vector3[] { 
                new Vector3(1,1,1), new Vector3(2,2,2) , new Vector3(3,3,3) };
            short[] indices = new short[] { 0, 1, 2, 0, 1, 2, 1, 2, 0, 2, 0, 1, 2, 1, 0 };
            short[] newIndices = new short[] { 0, 1, 2, 2, 1, 0};
            primitive = CreatePrimitiveFromLists(positions, null, null, indices);
            primitive.Weld(0.0f);
            CompareIndices(primitive, newIndices);
        }

        private IPrimitive CreatePrimitiveFromLists(Vector3[] positions,
            Vector3[] normals, Vector2[] uv, short[] indices)
        {
            Vertex[] vertices = new Vertex[positions.Length];
            for (int i = 0; i < positions.Length; i++)
            {
                vertices[i].Position = positions[i];
                if (normals != null)
                {
                    normals[i].Normalize();
                    vertices[i].Normal = normals[i];
                }
                if (uv != null)
                {
                    vertices[i].U = uv[i].X;
                    vertices[i].V = uv[i].Y;
                }
            }
            if (indices == null)
                indices = new short[] { };
            return new Primitive(vertices, indices);
        }

        private void CompareVertices(IPrimitive primitive, Vector3[] newPositions, Vector3[] newNormals, Vector2[] newUV)
        {
            Assert.AreEqual(newPositions.Length, primitive.Vertices.Length);
            for (int i = 0; i < newPositions.Length; i++)
            {
                Assert.AreEqual(newPositions[i], primitive.Vertices[i].Position);
                if (newNormals != null)
                {
                    newNormals[i].Normalize();
                    Assert.AreEqual(newNormals[i], primitive.Vertices[i].Normal);
                }
                if (newUV != null)
                {
                    Assert.AreEqual(newUV[i].X, primitive.Vertices[i].U);
                    Assert.AreEqual(newUV[i].Y, primitive.Vertices[i].V);
                }
            }
        }

        private void CompareIndices(IPrimitive primitive, short[] newIndices)
        {
            Assert.AreEqual(newIndices.Length, primitive.Indices.Length);
            for (int i = 0; i < newIndices.Length; i++)
                Assert.AreEqual(newIndices[i], primitive.Indices[i]);
        }

        private void CheckMaterial(IModel model)
        {
            Assert.AreEqual(Color.White.ToArgb(), model.Materials[0].Ambient.ToArgb(),
                "Ambient color should be white.");
            Assert.AreEqual(Color.Red.ToArgb(), model.Materials[0].Diffuse.ToArgb(),
                "Diffuse color should be red.");
        }

        private void CheckModel(Vertex[] vertices, short[] indices, IModel model)
        {
            Assert.AreSame(this, model.Mesh, "This should have been returned as Mesh.");
            Assert.IsFalse(vbLocked, "Vertex buffer should not be locked.");
            Assert.IsFalse(ibLocked, "Vertex buffer should not be locked.");
            Assert.AreEqual(numVertices, positions.Count, "Vertices should be " + numVertices);
            Assert.AreEqual(numFaces * 3, this.indices.Length, "Indices should be " + numFaces * 3);
            for (int i = 0; i < numVertices; i++)
            {
                Assert.AreEqual(vertices[i].Position, positions[i]);
                Assert.AreEqual(vertices[i].Normal, normals[i]);
                if (useTextureCoordinates)
                {
                    Assert.AreEqual(vertices[i].U, textureCoordinates[i * 2 + 0]);
                    Assert.AreEqual(vertices[i].V, textureCoordinates[i * 2 + 1]);
                }
            }
            for (int i = 0; i < numFaces * 3; i++)
                Assert.AreEqual(indices[i], this.indices[i]);
        }

        private void CreatePrimitive(out Vertex[] vertices, out short[] indices)
        {
            numFaces = 1;
            numVertices = 2;
            vertices = new Vertex[numVertices];
            indices = new short[numFaces * 3];
            for (int i = 0; i < numVertices; i++)
            {
                vertices[i].Position = new Vector3(i, i + 1, i + 2);
                vertices[i].Normal = new Vector3(i + 2, i + 3, i + 4);
                if (useTextureCoordinates)
                {
                    vertices[i].U = i * 2;
                    vertices[i].V = i * 1;
                }
            }
            for (int i = 0; i < numFaces * 3; i++)
                indices[i] = (short)i;
            primitive = new Primitive(vertices, indices);
        }


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

        public ITexture CreateTexture(IDevice device, Bitmap image, Usage usage, Pool pool)
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
            int index = 0;
            Assert.AreEqual(this.numFaces, numFaces);
            Assert.AreEqual(this.numVertices, numVertices);
            Assert.AreEqual(MeshFlags.Managed, options);
            if (useTextureCoordinates)
                Assert.AreEqual(4, declaration.Length);
            else
                Assert.AreEqual(3, declaration.Length);
            Assert.AreEqual(new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                declaration[index++]);
            Assert.AreEqual(new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                declaration[index++]);
            if (useTextureCoordinates)
            {
                Assert.AreEqual(new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                    declaration[index++]);
            }
            Assert.AreEqual(VertexElement.VertexDeclarationEnd,
                declaration[index++]);
            return this;
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
            get { return 0; }
        }

        public int NumberVertices
        {
            get { return 0; }
        }

        public MeshOptions Options
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IVertexBuffer VertexBuffer
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
            ibLocked = true;
            return this;
        }

        public Array LockIndexBuffer(Type typeIndex, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockVertexBuffer(LockFlags flags)
        {
            vbLocked = true;
            return this;
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
            ibLocked = false;
        }

        public void UnlockVertexBuffer()
        {
            vbLocked = false;
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

        #region IDevice Members

        public int AvailableTextureMemory
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ClipPlanes ClipPlanes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ClipStatus ClipStatus
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public DeviceCreationParameters CreationParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int CurrentTexturePalette
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public ISurface DepthStencilSurface
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Caps DeviceCaps
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public DisplayMode DisplayMode
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IndexBuffer Indices
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public LightsCollection Lights
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Material Material
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public float NPatchMode
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int NumberOfSwapChains
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public PixelShader PixelShader
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public PresentParameters PresentationParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public RasterStatus RasterStatus
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IRenderStateManager RenderState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public SamplerStateManagerCollection SamplerState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Rectangle ScissorRectangle
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool SoftwareVertexProcessing
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public TextureStateManagerCollection TextureState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Transforms Transform
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public VertexDeclaration VertexDeclaration
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        VertexFormats IDevice.VertexFormat
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public VertexShader VertexShader
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Viewport Viewport
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void BeginScene()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void BeginStateBlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CheckCooperativeLevel()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool CheckCooperativeLevel(out int result)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, Color color, float zdepth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, Color color, float zdepth, int stencil, Rectangle[] rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil, Rectangle[] regions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ColorFill(ISurface surface, Rectangle rect, Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ColorFill(ISurface surface, Rectangle rect, int color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface CreateDepthStencilSurface(int width, int height, DepthFormat format, MultiSampleType multiSample, int multiSampleQuality, bool discard)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface CreateOffscreenPlainSurface(int width, int height, Format format, Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface CreateRenderTarget(int width, int height, Format format, MultiSampleType multiSample, int multiSampleQuality, bool lockable)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeletePatch(int handle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawIndexedUserPrimitives(PrimitiveType primitiveType, int minVertexIndex, int numVertexIndices, int primitiveCount, object indexData, bool sixteenBitIndices, object vertexStreamZeroData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, float[] numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, Plane numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, float[] numSegs, RectanglePatchInformation rectPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, Plane numSegs, RectanglePatchInformation rectPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, float[] numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, Plane numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, float[] numSegs, TrianglePatchInformation triPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, Plane numSegs, TrianglePatchInformation triPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawUserPrimitives(PrimitiveType primitiveType, int primitiveCount, object vertexStreamZeroData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EndScene()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public StateBlock EndStateBlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EvictManagedResources()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface GetBackBuffer(int swapChain, int backBuffer, BackBufferType backBufferType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public CubeTexture GetCubeTexture(int stage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetFrontBufferData(int swapChain, ISurface buffer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GammaRamp GetGammaRamp(int swapChain)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PaletteEntry[] GetPaletteEntries(int paletteNumber)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool[] GetPixelShaderBooleanConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] GetPixelShaderInt32Constant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float[] GetPixelShaderSingleConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public RasterStatus GetRasterStatus(int swapChain)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool GetRenderStateBoolean(RenderStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetRenderStateInt32(RenderStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetRenderStateSingle(RenderStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface GetRenderTarget(int renderTargetIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetRenderTargetData(ISurface renderTarget, ISurface destSurface)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool GetSamplerStageStateBoolean(int stage, SamplerStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetSamplerStageStateInt32(int stage, SamplerStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetSamplerStageStateSingle(int stage, SamplerStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IVertexBuffer GetStreamSource(int streamNumber, out int offsetInBytes, out int stride)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetStreamSourceFrequency(int streamNumber)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public SwapChain GetSwapChain(int swapChain)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Texture GetTexture(int stage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool GetTextureStageStateBoolean(int stage, TextureStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetTextureStageStateInt32(int stage, TextureStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetTextureStageStateSingle(int stage, TextureStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix GetTransform(TransformType state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool[] GetVertexShaderBooleanConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] GetVertexShaderInt32Constant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float[] GetVertexShaderSingleConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public VolumeTexture GetVolumeTexture(int stage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MultiplyTransform(TransformType state, Matrix matrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Control overrideWindow)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(IntPtr overrideWindowHandle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Rectangle rectPresent, bool sourceRectangle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Rectangle rectPresent, Control overrideWindow, bool sourceRectangle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Rectangle rectPresent, IntPtr overrideWindowHandle, bool sourceRectangle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Rectangle sourceRectangle, Rectangle destRectangle, Control overrideWindow)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Rectangle sourceRectangle, Rectangle destRectangle, IntPtr overrideWindowHandle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, VertexBuffer destBuffer, VertexDeclaration vertexDeclaration, bool copyData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Reset(params PresentParameters[] presentationParameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetCursor(Cursor cursor, bool addWaterMark)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetCursorPosition(int positionX, int positionY, bool updateImmediate)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetCursorProperties(int hotSpotX, int hotSpotY, ISurface cursorBitmap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetDialogBoxesEnabled(bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetGammaRamp(int swapChain, bool calibrate, GammaRamp ramp)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPaletteEntries(int paletteNumber, PaletteEntry[] entries)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, bool[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, float[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, int[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Matrix constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Matrix[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Vector4 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Vector4[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderState(RenderStates state, bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderState(RenderStates state, float value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderState(RenderStates state, int value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderTarget(int renderTargetIndex, ISurface renderTarget)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSamplerState(int stage, SamplerStageStates state, bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSamplerState(int stage, SamplerStageStates state, float value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSamplerState(int stage, SamplerStageStates state, int value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetStreamSource(int streamNumber, IVertexBuffer streamData, int offsetInBytes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetStreamSource(int streamNumber, IVertexBuffer streamData, int offsetInBytes, int stride)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetStreamSourceFrequency(int streamNumber, int divider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTexture(int stage, BaseTexture texture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTextureStageState(int stage, TextureStageStates state, bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTextureStageState(int stage, TextureStageStates state, float value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTextureStageState(int stage, TextureStageStates state, int value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTransform(TransformType state, Matrix matrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, bool[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, float[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, int[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Matrix constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Matrix[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Vector4 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Vector4[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstantBoolean(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstantInt32(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstantSingle(int startRegister, GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool ShowCursor(bool canShow)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void StretchRectangle(ISurface sourceSurface, Rectangle sourceRectangle, ISurface destSurface, Rectangle destRectangle, TextureFilter filter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void TestCooperativeLevel()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSurface(ISurface sourceSurface, ISurface destinationSurface)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSurface(ISurface sourceSurface, Rectangle sourceRect, ISurface destinationSurface)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSurface(ISurface sourceSurface, ISurface destinationSurface, Point destPoint)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSurface(ISurface sourceSurface, Rectangle sourceRect, ISurface destinationSurface, Point destPoint)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateTexture(BaseTexture sourceTexture, BaseTexture destinationTexture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ValidateDeviceParams ValidateDevice()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IGraphicsStream Members

        public bool CanRead
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool CanSeek
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool CanWrite
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Close()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string Read(bool unicode)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ValueType Read(Type returnType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array Read(Type returnType, params int[] ranks)
        {
            // Used by PhysicalModel constructor
            return new short[0];
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long Seek(long newposition, System.IO.SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetLength(long newLength)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(Array value)
        {
            Assert.IsTrue(ibLocked);
            indices = (short[])value;
        }

        public void Write(string value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(ValueType value)
        {
            Assert.IsTrue(vbLocked);
            switch (vertexPosition)
            {
                case VertexPosition.POSITION:
                    positions.Add((Vector3)value);
                    vertexPosition++;
                    break;
                case VertexPosition.NORMAL:
                    normals.Add((Vector3)value);
                    if (useTextureCoordinates)
                        vertexPosition++;
                    else
                        vertexPosition = VertexPosition.POSITION;
                    break;
                case VertexPosition.TEX_U:
                    textureCoordinates.Add((float)value);
                    vertexPosition++;
                    break;
                case VertexPosition.TEX_V:
                    textureCoordinates.Add((float)value);
                    vertexPosition = VertexPosition.POSITION;
                    break;
                default:
                    throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Write(string value, bool isUnicodeString)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IBody Members

        public void AddConstraint(Dope.DDXX.Physics.IConstraint constraint)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddParticle(IPhysicalParticle particle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector3 Gravity
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public List<IPhysicalParticle> Particles
        {
            get { return new List<IPhysicalParticle>(); }
        }

        public void ApplyForce(Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITexture Members


        public void AddDirtyRectangle()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddDirtyRectangle(Rectangle rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public SurfaceDescription GetLevelDescription(int level)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface GetSurfaceLevel(int level)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(int level, LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(int level, LockFlags flags, out int pitch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(int level, Rectangle rect, LockFlags flags, out int pitch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, int level, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, int level, LockFlags flags, out int pitch, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockRectangle(int level)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IBaseTexture Members


        public int Priority
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public ResourceType Type
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void PreLoad()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int SetPriority(int priorityNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public TextureFilter AutoGenerateFilterType
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int LevelCount
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int LevelOfDetail
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void GenerateMipSubLevels()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int SetLevelOfDetail(int lodNew)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITexture Members

        public void FillTexture(Fill2DTextureCallback callbackFunction)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
