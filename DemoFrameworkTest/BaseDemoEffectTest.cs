using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.MidiProcessorLib;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class BaseDemoEffectTest : BaseDemoEffect, IDemoMixer, 
        IPostProcessor, IGraphicsFactory, IGraphicsDevice
    {
        public bool initializeCalled;

        public BaseDemoEffectTest()
            : base("", 0, 0)
        {
        }

        [SetUp]
        public void SetUp()
        {
            initializeCalled = false;
        }
        
        [Test]
        public void Initialization()
        {
            Initialize(this, this, this);
            Assert.IsTrue(initializeCalled);
            Assert.AreSame(this, GraphicsFactory);
            Assert.AreSame(this, Mixer);
        }

        [Test]
        public void StartingTime()
        {
            StartTime = 1.0f;
            Assert.AreEqual(1.0f, StartTime);
        }

        [Test]
        public void EndingTime()
        {
            EndTime = 11.0f;
            Assert.AreEqual(11.0f, EndTime);
        }

        [Test]
        public void CameraCreation()
        {
            // Setup fixture
            CameraNode camera;
            Initialize(this, this, this);
            //Exercise SUT
            CreateStandardCamera(out camera, 10);
            // Verify
            Assert.AreEqual(new Vector3(0, 0, 10), camera.Position);
            Assert.AreEqual(camera, Scene.ActiveCamera, "ActiveCamera set");
            Assert.AreEqual(2, Scene.NumNodes, "Only CameraNode and root node");
            Assert.AreEqual(camera, Scene.GetNodeByName("Standard Camera"), "Only CameraNode added");
        }

        #region BaseDemoEffect Members

        protected override void Initialize()
        {
            initializeCalled = true;
        }

        public override void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void Render()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IPostProcessor Members

        public IRenderTarget2D OutputTexture
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void StartFrame(IRenderTarget2D startTexture) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Process(string technique, IRenderTarget2D sourceTexture, IRenderTarget2D destinationTexture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Process(string technique, ITexture2D sourceTexture, IRenderTarget2D destinationTexture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetBlendParameters(BlendFunction blendOperation, Blend sourceBlend, Blend destinatonBlend, Color blendFactor) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(string parameter, float value) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(string parameter, float[] value) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(string parameter, Vector2 value) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetValue(string parameter, Vector4 value) {
            throw new Exception("The method or operation is not implemented.");
        }

        public List<IRenderTarget2D> GetTemporaryTextures(int num, bool skipOutput) {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AllocateTexture(IRenderTarget2D texture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void FreeTexture(IRenderTarget2D texture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
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
            get { throw new Exception("The method or operation is not implemented."); }
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

        public void Reset(GraphicsAdapter graphicsAdapter)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Reset(PresentationParameters presentationParameters)
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

        public float AspectRatio
        {
            get { return 1; }
        }

        #endregion

        #region IPostProcessor Members

        public void Initialize(IGraphicsFactory graphicsFactory)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGraphicsFactory Members

        public IDeviceManager GraphicsDeviceManager
        {
            get { throw new NotImplementedException(); }
        }

        public new ITextureFactory TextureFactory
        {
            get { throw new NotImplementedException(); }
        }

        public new IModelFactory ModelFactory
        {
            get { throw new NotImplementedException(); }
        }

        public new IEffectFactory EffectFactory
        {
            get { throw new NotImplementedException(); }
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, int numLevels, SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
        {
            throw new NotImplementedException();
        }

        public ITexture2D CreateTexture2D(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format)
        {
            throw new NotImplementedException();
        }

        public IDepthStencilBuffer CreateDepthStencilBuffer(int width, int height, DepthFormat format, MultiSampleType multiSampleType)
        {
            throw new NotImplementedException();
        }

        public ITexture2D Texture2DFromFile(string name)
        {
            throw new NotImplementedException();
        }

        public ITextureCube TextureCubeFromFile(string name)
        {
            throw new NotImplementedException();
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, BufferUsage usage)
        {
            throw new NotImplementedException();
        }

        public ISpriteBatch CreateSpriteBatch()
        {
            throw new NotImplementedException();
        }

        public IEffect EffectFromFile(string name)
        {
            throw new NotImplementedException();
        }

        public ISpriteFont SpriteFontFromFile(string name)
        {
            throw new NotImplementedException();
        }

        public IModel ModelFromFile(string name)
        {
            throw new NotImplementedException();
        }

        public IIndexBuffer CreateIndexBuffer(Type indexType, int elementCount, BufferUsage usage)
        {
            throw new NotImplementedException();
        }

        public IVertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElement)
        {
            throw new NotImplementedException();
        }

        public void SetScreen(int width, int height, bool fullscreen)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGraphicsFactory Members


        public new IGraphicsDevice GraphicsDevice
        {
            get { return this; }
        }

        public IBasicEffect CreateBasicEffect()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IGraphicsFactory Members


        public IContentManager ContentManager
        {
            get { throw new NotImplementedException(); }
        }

        #endregion


        #region IDemoMixer Members

        public void SetClearColor(int track, Color color)
        {
            throw new NotImplementedException();
        }

        public Color GetClearColor(int track)
        {
            throw new NotImplementedException();
        }

        public CompiledMidi CompiledMidi
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region IPostProcessor Members


        public void SetValue(string parameter, ITexture2D value)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
