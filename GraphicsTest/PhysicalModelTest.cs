using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;
using Microsoft.DirectX;
using System.IO;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class PhysicalModelTest : IMesh, IBody, IGraphicsStream
    {
        private List<IPhysicalParticle> particles;
        private bool bodyStep;
        private bool vbLocked;
        private bool ibLocked;
        private List<Vector3> positions;
        private List<Vector3> normals;
        private List<float> textureCoordinates;
        private bool useTextureCoordinates;
        private VertexPosition vertexPosition;
        private int numVerticesAdd;
        private short[] indices;

        private enum VertexPosition
        {
            POSITION,
            NORMAL,
            TEX_U,
            TEX_V,
        }

        [SetUp]
        public void SetUp()
        {
            bodyStep = false;
            particles = new List<IPhysicalParticle>();
            positions = new List<Vector3>();
            normals = new List<Vector3>();
            textureCoordinates = new List<float>();
            useTextureCoordinates = false;
            vbLocked = false;
            vertexPosition = VertexPosition.POSITION;
            numVerticesAdd = 0;
            indices = new short[0];
        }

        [TearDown]
        public void TearDown()
        {
            Assert.IsFalse(vbLocked, "Vertex buffer should not be locked.");
            Assert.IsFalse(ibLocked, "Index buffer should not be locked.");
        }

        /// <summary>
        /// Test creation of a model which does not have the same number of vertices 
        /// and particles in the IBody.
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestInvalidConstructor()
        {
            numVerticesAdd = 1;
            for (int i = 0; i < 3; i++)
                particles.Add(new PhysicalParticle(1, 1));
            PhysicalModel model = new PhysicalModel(this, this);
        }

        /// <summary>
        /// Test creation of a model which does have the same number of vertices 
        /// and particles in the IBody.
        /// </summary>
        [Test]
        public void TestConstructor()
        {
            for (int i = 0; i < 4; i++)
                particles.Add(new PhysicalParticle(1, 1));
            PhysicalModel model = new PhysicalModel(this, this);
            Assert.AreEqual(this, model.Body, "This pointer should be Body.");
        }

        /// <summary>
        /// Test creation of a model which does have the same number of vertices 
        /// and particles in the IBody.
        /// </summary>
        [Test]
        public void TestConstructorWithMaterials()
        {
            for (int i = 0; i < 4; i++)
                particles.Add(new PhysicalParticle(1, 1));
            PhysicalModel model = new PhysicalModel(this, this, new ModelMaterial[] { null });
            Assert.AreEqual(this, model.Body, "This pointer should be Body.");
            Assert.AreEqual(null, model.Materials[0], "Material should be null");
        }

        /// <summary>
        /// Test that calling Step() updates the models vertices.
        /// </summary>
        [Test]
        public void TestStep()
        {
            Vector3[] myNormals = new Vector3[] 
            {
                new Vector3(1, 0, 1),
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 0)
            };
            for (int i = 0; i < myNormals.Length; i++)
                myNormals[i].Normalize();
            // One triangle facing 0, 0, -1 and one facing 0, 0, 1
            particles.Add(new PhysicalParticle(new Vector3(0, 10, 0), 1, 1));
            particles.Add(new PhysicalParticle(new Vector3(0, 0, 0), 1, 1));
            particles.Add(new PhysicalParticle(new Vector3(-10, 0, 0), 1, 1));
            particles.Add(new PhysicalParticle(new Vector3(0, 0, -10), 1, 1));
            indices = new short[] { 0, 2, 1, 0, 1, 3 };
            PhysicalModel model = new PhysicalModel(this, this);
            model.Step();
            Assert.IsTrue(bodyStep, "Body.Step() should have been called.");
            Assert.AreEqual(particles.Count, positions.Count,
                "Number of written positions should equal particles.Count.");
            for (int i = 0; i < particles.Count; i++)
            {
                Assert.AreEqual(particles[i].Position, positions[i]);
                Assert.AreEqual(myNormals[i], normals[i]);
            }
        }

        /// <summary>
        /// Test that calling Step() updates the models vertices even with tex coords.
        /// </summary>
        [Test]
        public void TestStepWithTexture()
        {
            useTextureCoordinates = true;
            for (int i = 0; i < 4; i++)
                particles.Add(new PhysicalParticle(new Vector3(i, i, i), 1, 1));
            PhysicalModel model = new PhysicalModel(this, this);
            model.Step();
            Assert.IsTrue(bodyStep, "Body.Step() should have been called.");
            Assert.AreEqual(particles.Count, positions.Count,
                "Number of written positions should equal particles.Count.");
            for (int i = 0; i < particles.Count; i++)
            {
                Assert.AreEqual(particles[i].Position, positions[i]);
                // Normals should be 0, 0, 0 since we have no indices
                Assert.AreEqual(new Vector3(0, 0, 0), normals[i]);
            }
        }

        #region IMesh Members

        public Microsoft.DirectX.Direct3D.VertexElement[] Declaration
        {
            get 
            {
                if (useTextureCoordinates)
                    return new VertexElement[]
                        {
                            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                            new VertexElement(0, 24, DeclarationType.Float2, DeclarationMethod.Default, DeclarationUsage.TextureCoordinate, 0),
                            VertexElement.VertexDeclarationEnd
                        };
                else
                    return new VertexElement[]
                        {
                            new VertexElement(0, 0, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Position, 0),
                            new VertexElement(0, 12, DeclarationType.Float3, DeclarationMethod.Default, DeclarationUsage.Normal, 0),
                            VertexElement.VertexDeclarationEnd
                        };
            }
        }

        public IDevice Device
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

        public IIndexBuffer IndexBuffer
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
            get { return indices.Length / 3; }
        }

        public int NumberVertices
        {
            get { return particles.Count + numVerticesAdd; }
        }

        public Microsoft.DirectX.Direct3D.MeshOptions Options
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IVertexBuffer VertexBuffer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.VertexFormats VertexFormat
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IMesh Clean(Microsoft.DirectX.Direct3D.CleanType cleanType, IGraphicsStream adjacency, IGraphicsStream adjacencyOut)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clean(Microsoft.DirectX.Direct3D.CleanType cleanType, int[] adjacency, out int[] adjacencyOut)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clean(Microsoft.DirectX.Direct3D.CleanType cleanType, IGraphicsStream adjacency, IGraphicsStream adjacencyOut, out string errorsAndWarnings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clean(Microsoft.DirectX.Direct3D.CleanType cleanType, int[] adjacency, out int[] adjacencyOut, out string errorsAndWarnings)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clone(Microsoft.DirectX.Direct3D.MeshFlags options, IGraphicsStream declaration, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clone(Microsoft.DirectX.Direct3D.MeshFlags options, Microsoft.DirectX.Direct3D.VertexElement[] declaration, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Clone(Microsoft.DirectX.Direct3D.MeshFlags options, Microsoft.DirectX.Direct3D.VertexFormats vertexFormat, IDevice device)
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

        public Microsoft.DirectX.Direct3D.AttributeRange[] GetAttributeTable()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockIndexBuffer(Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            ibLocked = true;
            return this;
        }

        public Array LockIndexBuffer(Type typeIndex, Microsoft.DirectX.Direct3D.LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockVertexBuffer(Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            vbLocked = true;
            return this;
        }

        public Array LockVertexBuffer(Type typeVertex, Microsoft.DirectX.Direct3D.LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetIndexBufferData(object data, Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexBufferData(object data, Microsoft.DirectX.Direct3D.LockFlags flags)
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

        public void UpdateSemantics(Microsoft.DirectX.Direct3D.VertexElement[] declaration)
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

        public void ComputeTangentFrame(Microsoft.DirectX.Direct3D.TangentOptions options)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir, out Microsoft.DirectX.Direct3D.IntersectInformation closestHit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir, out Microsoft.DirectX.Direct3D.IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool Intersect(Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir, out Microsoft.DirectX.Direct3D.IntersectInformation closestHit, out Microsoft.DirectX.Direct3D.IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir, out Microsoft.DirectX.Direct3D.IntersectInformation closestHit)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir, out Microsoft.DirectX.Direct3D.IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IntersectSubset(int attributeId, Microsoft.DirectX.Vector3 rayPos, Microsoft.DirectX.Vector3 rayDir, out Microsoft.DirectX.Direct3D.IntersectInformation closestHit, out Microsoft.DirectX.Direct3D.IntersectInformation[] allHits)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockAttributeBuffer(Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] LockAttributeBufferArray(Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(Microsoft.DirectX.Direct3D.MeshFlags flags, IGraphicsStream adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(Microsoft.DirectX.Direct3D.MeshFlags flags, int[] adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(Microsoft.DirectX.Direct3D.MeshFlags flags, IGraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh Optimize(Microsoft.DirectX.Direct3D.MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(Microsoft.DirectX.Direct3D.MeshFlags flags, IGraphicsStream adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(Microsoft.DirectX.Direct3D.MeshFlags flags, int[] adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(Microsoft.DirectX.Direct3D.MeshFlags flags, IGraphicsStream adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OptimizeInPlace(Microsoft.DirectX.Direct3D.MeshFlags flags, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, IGraphicsStream adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, int[] adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, IGraphicsStream adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, int[] adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, IGraphicsStream adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.EffectInstance[] effects, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(System.IO.Stream stream, int[] adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.EffectInstance[] effects, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, IGraphicsStream adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.EffectInstance[] effects, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Save(string filename, int[] adjacency, Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials, Microsoft.DirectX.Direct3D.EffectInstance[] effects, Microsoft.DirectX.Direct3D.XFileFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetAttributeTable(Microsoft.DirectX.Direct3D.AttributeRange[] table)
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

        public void WeldVertices(Microsoft.DirectX.Direct3D.WeldEpsilonsFlags flags, Microsoft.DirectX.Direct3D.WeldEpsilons epsilons, int[] adjacencyIn)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(Microsoft.DirectX.Direct3D.WeldEpsilonsFlags flags, Microsoft.DirectX.Direct3D.WeldEpsilons epsilons, IGraphicsStream adjacencyIn, IGraphicsStream adjacencyOut)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(Microsoft.DirectX.Direct3D.WeldEpsilonsFlags flags, Microsoft.DirectX.Direct3D.WeldEpsilons epsilons, int[] adjacencyIn, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(Microsoft.DirectX.Direct3D.WeldEpsilonsFlags flags, Microsoft.DirectX.Direct3D.WeldEpsilons epsilons, IGraphicsStream adjacencyIn, IGraphicsStream adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void WeldVertices(Microsoft.DirectX.Direct3D.WeldEpsilonsFlags flags, Microsoft.DirectX.Direct3D.WeldEpsilons epsilons, int[] adjacencyIn, out int[] adjacencyOut, out int[] faceRemap, out IGraphicsStream vertexRemap)
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

        public void Step()
        {
            bodyStep = true;
        }

        public List<IPhysicalParticle> Particles
        {
            get { return particles; }
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

        public void ApplyForce(Vector3 vector3)
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
            Assert.IsTrue(ibLocked);
            Assert.AreEqual(typeof(short), returnType);
            return indices;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public long Seek(long newposition, SeekOrigin origin)
        {
            Assert.AreEqual(VertexPosition.TEX_U, vertexPosition);
            Assert.AreEqual(4 * 2, newposition);
            Assert.AreEqual(SeekOrigin.Current, origin);
            vertexPosition = VertexPosition.POSITION;
            return 0;
        }

        public void SetLength(long newLength)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Write(Array value)
        {
            throw new Exception("The method or operation is not implemented.");
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


        public List<Dope.DDXX.Physics.IConstraint> Constraints
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
