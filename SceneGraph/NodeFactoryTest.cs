using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics.Skinning;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class NodeFactoryTest : IFrame, IMesh, ITextureFactory, IEffect, IAnimationRootFrame, IMeshContainer
    {
        private NodeFactory nodeFactory;
        private string name;
        private ExtendedMaterial[] materials = new ExtendedMaterial[1];
        private Matrix transformationMatrix;

        [SetUp]
        public void SetUp()
        {
            transformationMatrix = Matrix.Identity;
            nodeFactory = new NodeFactory(this);
        }

        [Test]
        public void TestModelNode()
        {
            name = "A name";
            ModelNode node = nodeFactory.CreateModelNode(this, this, "prefix");
            Assert.AreEqual(0, node.Children.Count, "Node should have no children.");
            Assert.AreEqual("A name", node.Name, "The name of the node should be 'A name'");
            Assert.IsNotNull(node.Model, "Model shall not be null.");
            Assert.AreEqual(node.Model.Mesh, this);
            Assert.AreEqual(node.Model.Materials.Length, materials.Length);
            Assert.IsFalse(node.Model.IsSkinned());
        }

        [Test]
        public void TestSkinnedModelNode()
        {
            name = "A name";
            ModelNode node = nodeFactory.CreateSkinnedModelNode(this, this, this, "prefix");
            Assert.AreEqual(0, node.Children.Count, "Node should have no children.");
            Assert.AreEqual("A name", node.Name, "The name of the node should be 'A name'");
            Assert.IsNotNull(node.Model, "Model shall not be null.");
            Assert.AreEqual(node.Model.Mesh, this);
            Assert.AreEqual(node.Model.Materials.Length, materials.Length);
            Assert.IsTrue(node.Model.IsSkinned());
        }

        [Test]
        public void TestCameraNode()
        {
            name = "Another name";
            CameraNode node = nodeFactory.CreateCameraNode(this);
            Assert.AreEqual(0, node.Children.Count, "Node should have no children.");
            Assert.AreEqual("Another name", node.Name, "The name of the node should be 'A name'");
        }

        [Test]
        public void TestDummyNode()
        {
            name = "A third name";
            DummyNode node = nodeFactory.CreateDummyNode(this);
            Assert.AreEqual(0, node.Children.Count, "Node should have no children.");
            Assert.AreEqual("A third name", node.Name, "The name of the node should be 'A name'");
        }

        #region IFrame Members

        public IFrame FrameFirstChild
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IFrame FrameSibling
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IMeshContainer MeshContainer
        {
            get
            {
                return this;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public Matrix TransformationMatrix
        {
            get
            {
                return transformationMatrix;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public IFrame Find(IFrame rootFrame, string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix CombinedTransformationMatrix
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

        public IMesh Mesh
        {
            get { return this; }
        }

        public ISkinInformation SkinInformation
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ExtendedMaterial[] ExtendedMaterials
        {
            get { return materials; }
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

        #region ITextureFactory Members

        public ITexture CreateFromFile(string file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateFullsizeRenderTarget(Format format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateFullsizeRenderTarget()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEffect Members

        public int Description_Parameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public EffectDescription Description
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public EffectPool Pool
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public EffectStateManager StateManager
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

        public EffectHandle Technique
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

        public void ApplyParameterBlock(EffectHandle parameterBlock)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int Begin(FX flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void BeginParameterBlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void BeginPass(int passNumber)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Effect Clone(Device dev)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CommitChanges()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeleteParameterBlock(EffectHandle parameterBlock)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string Disassemble(bool enableColorCode)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void End()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle EndParameterBlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EndPass()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle FindNextValidTechnique(EffectHandle technique)
        {
            return EffectHandle.FromString("prefixName");
        }

        public EffectHandle GetAnnotation(EffectHandle technique, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetAnnotation(EffectHandle technique, string name)
        {
            return null;
        }

        public EffectHandle GetFunction(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetFunction(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public FunctionDescription GetFunctionDescription(EffectHandle shader)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetParameter(EffectHandle constant, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetParameter(EffectHandle constant, string name)
        {
            return EffectHandle.FromString("Handle");
        }

        public EffectHandle GetParameterBySemantic(EffectHandle constant, string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ParameterDescription GetParameterDescription(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetParameterDescription_Elements(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetParameterElement(EffectHandle constant, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetPass(EffectHandle technique, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetPass(EffectHandle technique, string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PassDescription GetPassDescription(EffectHandle pass)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetTechnique(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetTechnique(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetTechniqueName(EffectHandle technique)
        {
            return "prefixName";
        }

        public GraphicsStream GetValue(EffectHandle parameter, int numberBytes)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool GetValueBoolean(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool[] GetValueBooleanArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ColorValue GetValueColor(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ColorValue[] GetValueColorArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetValueFloat(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float[] GetValueFloatArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetValueInteger(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int[] GetValueIntegerArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix GetValueMatrix(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix[] GetValueMatrixArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix GetValueMatrixTranspose(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix[] GetValueMatrixTransposeArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public PixelShader GetValuePixelShader(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public string GetValueString(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture GetValueTexture(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector4 GetValueVector(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector4[] GetValueVectorArray(EffectHandle parameter, int count)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public VertexShader GetValueVertexShader(EffectHandle parameter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsParameterUsed(EffectHandle parameter, EffectHandle technique)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsTechniqueValid(EffectHandle technique)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool IsTechniqueValid(EffectHandle technique, out int returnValue)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnLostDevice()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void OnResetDevice()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetArrayRange(EffectHandle parameter, int start, int end)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRawValue(EffectHandle parameter, GraphicsStream data, int byteOffset)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, IBaseTexture texture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, bool b)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, bool[] b)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, ColorValue color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, ColorValue[] color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, float f)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, float[] f)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, GraphicsStream data)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, int n)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, int[] n)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, Matrix matrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, Matrix[] matrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, string str)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, Vector4 vector)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(EffectHandle parameter, Vector4[] vector)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValueTranspose(EffectHandle parameter, Matrix matrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValueTranspose(EffectHandle parameter, Matrix[] matrix)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ValidateTechnique(EffectHandle technique)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IAnimationRootFrame Members

        public IAnimationController AnimationController
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IFrame FrameHierarchy
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IMeshContainer Members

        public MeshDataAdapter MeshData
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

        public IMeshContainer NextContainer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        ISkinInformation IMeshContainer.SkinInformation
        {
            get
            {
                return null;
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public int[] GetAdjacency()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream GetAdjacencyStream()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectInstance[] GetEffectInstances()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ExtendedMaterial[] GetMaterials()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetAdjacency(IGraphicsStream adj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetAdjacency(int[] adj)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetEffectInstances(EffectInstance[] effects)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetMaterials(ExtendedMaterial[] mtrl)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix[] RestMatrices
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

        public BoneCombination[] Bones
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

        public IFrame[] Frames
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

        #endregion

        #region ITextureFactory Members


        public ICubeTexture CreateCubeFromFile(string file)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
