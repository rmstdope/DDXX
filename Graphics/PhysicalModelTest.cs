using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class PhysicalModelTest : IMesh, IBody
    {
        private bool bodyStep;

        [SetUp]
        public void SetUp()
        {
            bodyStep = false;
        }

        [Test]
        public void TestStep()
        {
            PhysicalModel model = new PhysicalModel(this, this);
            model.Step();
            Assert.IsTrue(bodyStep, "Body.Step() should have been called.");
        }

        #region IMesh Members

        public Microsoft.DirectX.Direct3D.VertexElement[] Declaration
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.Device Device
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

        public Microsoft.DirectX.Direct3D.IndexBuffer IndexBuffer
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

        public Microsoft.DirectX.Direct3D.MeshOptions Options
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.VertexBuffer VertexBuffer
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
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockIndexBuffer(Type typeIndex, Microsoft.DirectX.Direct3D.LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream LockVertexBuffer(Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
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

        public void SetGravity(Microsoft.DirectX.Vector3 gravity)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Step()
        {
            bodyStep = true;
        }

        public List<IPhysicalParticle> Particles
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }
}
