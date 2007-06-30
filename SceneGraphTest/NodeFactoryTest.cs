using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class NodeFactoryTest : IFrame, IMesh, ITextureFactory, IEffect, IAnimationRootFrame, IMeshContainer, IDevice
    {
        private NodeFactory nodeFactory;
        private string name;
        private ExtendedMaterial[] materials = new ExtendedMaterial[1];
        private Matrix transformationMatrix;

        [SetUp]
        public void SetUp()
        {
            transformationMatrix = Matrix.Identity;
            nodeFactory = new NodeFactory(this, this);
        }

        [Test]
        public void TestModelNode()
        {
            name = "A name";
            ModelNode node = nodeFactory.CreateModelNode(this, this,
                TechniqueChooser.MeshPrefix("prefix"));
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
            ModelNode node = nodeFactory.CreateSkinnedModelNode(this, this, this,
                TechniqueChooser.MeshPrefix("prefix"));
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
                MeshDataAdapter data = new MeshDataAdapter();
                data.Mesh = this;
                return data;
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

        #region ITextureFactory Members


        public ITexture CreateFromFunction(int width, int height, int numLevels, Usage usage, Format format, Pool pool, Fill2DTextureCallback callbackFunction)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITextureFactory Members


        public void RegisterTexture(string name, ITexture texture)
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

        public System.Drawing.Rectangle ScissorRectangle
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

        public void Clear(ClearFlags flags, System.Drawing.Color color, float zdepth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, System.Drawing.Color color, float zdepth, int stencil, System.Drawing.Rectangle[] rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearFlags flags, int color, float zdepth, int stencil, System.Drawing.Rectangle[] regions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ColorFill(ISurface surface, System.Drawing.Rectangle rect, System.Drawing.Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ColorFill(ISurface surface, System.Drawing.Rectangle rect, int color)
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

        public void Present(System.Windows.Forms.Control overrideWindow)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(IntPtr overrideWindowHandle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(System.Drawing.Rectangle rectPresent, bool sourceRectangle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(System.Drawing.Rectangle rectPresent, System.Windows.Forms.Control overrideWindow, bool sourceRectangle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(System.Drawing.Rectangle rectPresent, IntPtr overrideWindowHandle, bool sourceRectangle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(System.Drawing.Rectangle sourceRectangle, System.Drawing.Rectangle destRectangle, System.Windows.Forms.Control overrideWindow)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(System.Drawing.Rectangle sourceRectangle, System.Drawing.Rectangle destRectangle, IntPtr overrideWindowHandle)
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

        public void SetCursor(System.Windows.Forms.Cursor cursor, bool addWaterMark)
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

        public void StretchRectangle(ISurface sourceSurface, System.Drawing.Rectangle sourceRectangle, ISurface destSurface, System.Drawing.Rectangle destRectangle, TextureFilter filter)
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

        public void UpdateSurface(ISurface sourceSurface, System.Drawing.Rectangle sourceRect, ISurface destinationSurface)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSurface(ISurface sourceSurface, ISurface destinationSurface, System.Drawing.Point destPoint)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UpdateSurface(ISurface sourceSurface, System.Drawing.Rectangle sourceRect, ISurface destinationSurface, System.Drawing.Point destPoint)
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
    }
}
