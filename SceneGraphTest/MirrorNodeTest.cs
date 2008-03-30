using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Animation;

namespace Dope.DDXX.SceneGraph
{
    [TestFixture]
    public class MirrorNodeTest : IModel, IScene, INode, IModelMesh, IGraphicsDevice, IRenderState, IModelMeshPart, IMaterialHandler, IRenderableCamera, IAnimationController
    {
        private string originalName;
        private CullMode newCulling;
        private Color ambientColor;
        private Color oldAmbientColor;
        private Color diffuseColor;
        private Color oldDiffuseColor;
        private Color specularColor;
        private Color oldSpecularColor;

        [Test]
        public void StepNode()
        {
            //Setup
            MirrorNode node = new MirrorNode(this);
            // Exercise SUT
            node.Step();
        }

        [Test]
        public void Getters()
        {
            //Setup
            MirrorNode node = new MirrorNode(this);
            // Exercise SUT
            node.Brightness = 2.5f;
            // Verify
            Assert.AreEqual(2.5f, node.Brightness);
        }

        [Test]
        public void NodeName()
        {
            //Setup
            originalName = "NodeName";
            // Exercise SUT
            MirrorNode node = new MirrorNode(this);
            // Verify
            Assert.AreEqual("NodeName_Mirror", node.Name);
        }

        [Test]
        public void ReflectY()
        {
            //Setup
            DummyNode dummy = new DummyNode("");
            MirrorNode node = new MirrorNode(dummy);
            // Verify
            Assert.AreEqual(new Vector3(1, -1, 1), node.WorldState.Scaling);
            Assert.AreEqual(Matrix.Identity, dummy.WorldMatrix);
        }

        [Test]
        public void CullingClockwise()
        {
            // Setup
            ModelNode modelNode = new ModelNode("", this, this);
            modelNode.CullMode = CullMode.CullCounterClockwiseFace;
            MirrorNode node = new MirrorNode(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(CullMode.CullClockwiseFace, newCulling);
        }

        [Test]
        public void CullingCouterClockwise()
        {
            // Setup
            ModelNode modelNode = new ModelNode("", this, this);
            modelNode.CullMode = CullMode.CullClockwiseFace;
            MirrorNode node = new MirrorNode(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(CullMode.CullCounterClockwiseFace, newCulling);
        }

        [Test]
        public void CullingNone()
        {
            // Setup
            ModelNode modelNode = new ModelNode("", this, this);
            modelNode.CullMode = CullMode.None;
            MirrorNode node = new MirrorNode(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(CullMode.None, newCulling);
        }

        [Test]
        public void CullingWithChildren()
        {
            // Setup
            DummyNode baseNode = new DummyNode(""); ;
            ModelNode modelNode = new ModelNode("", this, this);
            modelNode.CullMode = CullMode.CullCounterClockwiseFace;
            MirrorNode node = new MirrorNode(baseNode);
            baseNode.AddChild(modelNode);
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(CullMode.CullClockwiseFace, newCulling);
        }

        [Test]
        public void ColorModulationBrightness05()
        {
            // Setup
            ModelNode modelNode = new ModelNode("", this, this);
            MirrorNode node = new MirrorNode(modelNode);
            ambientColor = new Color(2, 2, 2);
            diffuseColor = new Color(4, 4, 4);
            specularColor = new Color(6, 6, 6);
            node.Brightness = 0.5f;
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(new Color(2, 2, 2), ambientColor);
            Assert.AreEqual(new Color(4, 4, 4), diffuseColor);
            Assert.AreEqual(new Color(6, 6, 6), specularColor);
            Assert.AreEqual(new Color(1, 1, 1), oldAmbientColor);
            Assert.AreEqual(new Color(2, 2, 2), oldDiffuseColor);
            Assert.AreEqual(new Color(3, 3, 3), oldSpecularColor);
        }

        [Test]
        public void ColorModulationBrightness2()
        {
            // Setup
            ModelNode modelNode = new ModelNode("", this, this);
            MirrorNode node = new MirrorNode(modelNode);
            ambientColor = new Color(20, 20, 20);
            diffuseColor = new Color(40, 40, 40);
            specularColor = new Color(60, 60, 60);
            node.Brightness = 2;
            // Exercise SUT
            node.Render(this);
            // Verify
            Assert.AreEqual(new Color(20, 20, 20), ambientColor);
            Assert.AreEqual(new Color(40, 40, 40), diffuseColor);
            Assert.AreEqual(new Color(60, 60, 60), specularColor);
            Assert.AreEqual(new Color(40, 40, 40), oldAmbientColor);
            Assert.AreEqual(new Color(80, 80, 80), oldDiffuseColor);
            Assert.AreEqual(new Color(120, 120, 120), oldSpecularColor);
        }

        #region IModel Members

        public ModelBoneCollection Bones
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ReadOnlyCollection<IModelMesh> Meshes
        {
            get 
            {
                List<IModelMesh> list = new List<IModelMesh>();
                list.Add(this);
                return new ReadOnlyCollection<IModelMesh>(list); 
            }
        }

        public ModelBone Root
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public object Tag
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

        public void CopyAbsoluteBoneTransformsTo(Microsoft.Xna.Framework.Matrix[] destinationBoneTransforms)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyBoneTransformsFrom(Microsoft.Xna.Framework.Matrix[] sourceBoneTransforms)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CopyBoneTransformsTo(Microsoft.Xna.Framework.Matrix[] destinationBoneTransforms)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IScene Members

        public IRenderableCamera ActiveCamera
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

        public Color AmbientColor
        {
            get
            {
                return ambientColor;
            }
            set
            {
                oldAmbientColor = ambientColor;
                ambientColor = value;
            }
        }

        public int NumNodes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void AddNode(INode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Render()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public INode GetNodeByName(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Validate()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DebugPrintGraph()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region INode Members

        public string Name
        {
            get { return originalName; }
        }

        public INode Parent
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public List<INode> Children
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Dope.DDXX.Physics.WorldState WorldState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.Xna.Framework.Matrix WorldMatrix
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void AddChild(INode child)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool HasChild(INode node)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetLightState(LightState state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Render(IScene scene)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int CountNodes()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.Xna.Framework.Vector3 Position
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region IModelMesh Members

        public BoundingSphere BoundingSphere
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ModelEffectCollectionAdapter Effects
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IIndexBuffer IndexBuffer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ReadOnlyCollection<IModelMeshPart> MeshParts
        {
            get 
            {
                List<IModelMeshPart> list = new List<IModelMeshPart>();
                list.Add(this);
                return new ReadOnlyCollection<IModelMeshPart>(list); 
            }
        }

        public ModelBone ParentBone
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IVertexBuffer VertexBuffer
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Draw()
        {
        }

        public void Draw(SaveStateMode saveStateMode)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IGraphicsDevice Members

        public ClipPlaneCollection ClipPlanes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public GraphicsDeviceCreationParameters CreationParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IDepthStencilBuffer DepthStencilBuffer
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

        public DisplayMode DisplayMode
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int DriverLevel
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public GraphicsDeviceCapabilities GraphicsDeviceCapabilities
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public GraphicsDeviceStatus GraphicsDeviceStatus
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IIndexBuffer Indices
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

        public bool IsDisposed
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

        public PresentationParameters PresentationParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public RasterStatus RasterStatus
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IRenderState RenderState
        {
            get { return this; }
        }

        public SamplerStateCollection SamplerStates
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

        public TextureCollection Textures
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IVertexDeclaration VertexDeclaration
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

        public SamplerStateCollection VertexSamplerStates
        {
            get { throw new Exception("The method or operation is not implemented."); }
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

        public TextureCollection VertexTextures
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IVertexStreamCollection Vertices
        {
            get { throw new Exception("The method or operation is not implemented."); }
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

        public event EventHandler DeviceLost;

        public event EventHandler DeviceReset;

        public event EventHandler DeviceResetting;

        public event EventHandler Disposing;

        public event EventHandler<ResourceCreatedEventArgs> ResourceCreated;

        public event EventHandler<ResourceDestroyedEventArgs> ResourceDestroyed;

        public void Clear(Color color)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearOptions options, Color color, float depth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearOptions options, Vector4 color, float depth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearOptions options, Color color, float depth, int stencil, Rectangle[] regions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(ClearOptions options, Vector4 color, float depth, int stencil, Rectangle[] regions)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawIndexedPrimitives(PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primitiveCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, int[] indexData, int indexOffset, int primitiveCount) where T : struct
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawUserIndexedPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int numVertices, short[] indexData, int indexOffset, int primitiveCount) where T : struct
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawUserPrimitives<T>(PrimitiveType primitiveType, T[] vertexData, int vertexOffset, int primitiveCount) where T : struct
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EvictManagedResources()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GammaRamp GetGammaRamp()
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

        public Matrix[] GetPixelShaderMatrixArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix GetPixelShaderMatrixConstant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Quaternion[] GetPixelShaderQuaternionArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Quaternion GetPixelShaderQuaternionConstant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float[] GetPixelShaderSingleConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector2[] GetPixelShaderVector2ArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector2 GetPixelShaderVector2Constant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector3[] GetPixelShaderVector3ArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector3 GetPixelShaderVector3Constant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector4[] GetPixelShaderVector4ArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector4 GetPixelShaderVector4Constant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IRenderTarget GetRenderTarget(int renderTargetIndex)
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

        public Matrix[] GetVertexShaderMatrixArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Matrix GetVertexShaderMatrixConstant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Quaternion[] GetVertexShaderQuaternionArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Quaternion GetVertexShaderQuaternionConstant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float[] GetVertexShaderSingleConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector2[] GetVertexShaderVector2ArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector2 GetVertexShaderVector2Constant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector3[] GetVertexShaderVector3ArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector3 GetVertexShaderVector3Constant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector4[] GetVertexShaderVector4ArrayConstant(int startRegister, int constantCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Vector4 GetVertexShaderVector4Constant(int startRegister)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(IntPtr overrideWindowHandle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Present(Rectangle? sourceRectangle, Rectangle? destinationRectangle, IntPtr overrideWindowHandle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Reset()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Reset(PresentationParameters presentationParameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ResolveBackBuffer(ITexture2D resolveTarget)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ResolveBackBuffer(ITexture2D resolveTarget, int backBufferIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ResolveRenderTarget(int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetGammaRamp(bool calibrate, GammaRamp ramp)
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

        public void SetPixelShaderConstant(int startRegister, Quaternion constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Quaternion[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Vector2 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Vector2[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Vector3 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Vector3[] constantData)
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

        public void SetRenderTarget(int renderTargetIndex, IRenderTarget2D renderTarget)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderTarget(int renderTargetIndex, IRenderTargetCube renderTarget, CubeMapFace faceType)
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

        public void SetVertexShaderConstant(int startRegister, Quaternion constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Quaternion[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Vector2 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Vector2[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Vector3 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Vector3[] constantData)
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

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IRenderState Members

        public bool AlphaBlendEnable
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

        public BlendFunction AlphaBlendOperation
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

        public Blend AlphaDestinationBlend
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

        public CompareFunction AlphaFunction
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

        public Blend AlphaSourceBlend
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

        public bool AlphaTestEnable
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

        public Color BlendFactor
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

        public BlendFunction BlendFunction
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

        public ColorWriteChannels ColorWriteChannels
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

        public ColorWriteChannels ColorWriteChannels1
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

        public ColorWriteChannels ColorWriteChannels2
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

        public ColorWriteChannels ColorWriteChannels3
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

        public StencilOperation CounterClockwiseStencilDepthBufferFail
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

        public StencilOperation CounterClockwiseStencilFail
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

        public CompareFunction CounterClockwiseStencilFunction
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

        public StencilOperation CounterClockwiseStencilPass
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

        public CullMode CullMode
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                newCulling = value;
            }
        }

        public float DepthBias
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

        public bool DepthBufferEnable
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

        public CompareFunction DepthBufferFunction
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

        public bool DepthBufferWriteEnable
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

        public Blend DestinationBlend
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

        public FillMode FillMode
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

        public Color FogColor
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

        public float FogDensity
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

        public bool FogEnable
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

        public float FogEnd
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

        public float FogStart
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

        public FogMode FogTableMode
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

        public FogMode FogVertexMode
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

        public bool MultiSampleAntiAlias
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

        public int MultiSampleMask
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

        public float PointSize
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

        public float PointSizeMax
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

        public float PointSizeMin
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

        public bool PointSpriteEnable
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

        public bool RangeFogEnable
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

        public int ReferenceAlpha
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

        public int ReferenceStencil
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

        public bool ScissorTestEnable
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

        public bool SeparateAlphaBlendEnabled
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

        public float SlopeScaleDepthBias
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

        public Blend SourceBlend
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

        public StencilOperation StencilDepthBufferFail
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

        public bool StencilEnable
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

        public StencilOperation StencilFail
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

        public CompareFunction StencilFunction
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

        public int StencilMask
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

        public StencilOperation StencilPass
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

        public int StencilWriteMask
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

        public bool TwoSidedStencilMode
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

        public TextureWrapCoordinates Wrap0
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

        public TextureWrapCoordinates Wrap1
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

        public TextureWrapCoordinates Wrap10
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

        public TextureWrapCoordinates Wrap11
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

        public TextureWrapCoordinates Wrap12
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

        public TextureWrapCoordinates Wrap13
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

        public TextureWrapCoordinates Wrap14
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

        public TextureWrapCoordinates Wrap15
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

        public TextureWrapCoordinates Wrap2
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

        public TextureWrapCoordinates Wrap3
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

        public TextureWrapCoordinates Wrap4
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

        public TextureWrapCoordinates Wrap5
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

        public TextureWrapCoordinates Wrap6
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

        public TextureWrapCoordinates Wrap7
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

        public TextureWrapCoordinates Wrap8
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

        public TextureWrapCoordinates Wrap9
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                DeviceLost(null, null);
                DeviceReset(null, null);
                DeviceResetting(null, null);
                Disposing(null, null);
                ResourceCreated(null, null);
                ResourceDestroyed(null, null);
            }
        }

        #endregion

        #region IModelMeshPart Members

        public int BaseVertex
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IEffect Effect
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

        public int NumVertices
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int PrimitiveCount
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int StartIndex
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int StreamOffset
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int VertexStride
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IMaterialHandler MaterialHandler
        {
            get { return this; }
        }

        #endregion

        #region IMaterialHandler Members

        public void SetupRendering(Matrix[] worldMatrix, Matrix viewMatrix, Matrix projectionMatrx, Color ambientLight, LightState lightState)
        {
        }

        public Color DiffuseColor
        {
            get
            {
                return diffuseColor;
            }
            set
            {
                oldDiffuseColor = diffuseColor;
                diffuseColor = value;
            }
        }

        public Color SpecularColor
        {
            get
            {
                return specularColor;
            }
            set
            {
                oldSpecularColor = specularColor;
                specularColor = value;
            }
        }

        public float SpecularPower
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

        public float Shininess
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

        #region IRenderableCamera Members

        public Matrix ProjectionMatrix
        {
            get { return new Matrix(); }
        }

        public Matrix ViewMatrix
        {
            get { return new Matrix(); }
        }

        #endregion

        #region IMaterialHandler Members


        public ITexture2D DiffuseTexture
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

        public ITexture2D NormalTexture
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

        public ITextureCube ReflectiveTexture
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

        public float ReflectiveFactor
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

        #region IRenderableCamera Members


        public float AspectRatio
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

        #region IGraphicsDevice Members


        public void Reset(GraphicsAdapter graphicsAdapter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Reset(PresentationParameters presentationParameters, GraphicsAdapter graphicsAdapter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ResolveBackBuffer(IResolveTexture2D resolveTarget)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ResolveBackBuffer(IResolveTexture2D resolveTarget, int backBufferIndex)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IModel Members


        public IAnimationController AnimationController
        {
            get { return this; }
        }

        #endregion

        #region IAnimationController Members

        public IDictionary<string, IAnimationClip> AnimationClips
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ISkinInformation SkinInformation
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Matrix[] WorldMatrices
        {
            get { return new Matrix[] { Matrix.Identity }; }
        }

        public void Step(Matrix rootWorldMatrix)
        {
        }

        #endregion

        #region IAnimationController Members


        public float Speed
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

        public PlayMode PlayMode
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

        #region IMaterialHandler Members


        public void SetupRendering(Matrix[] worldMatrices, Matrix viewMatrix, Matrix projectionMatrx, Color ambientLight)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
