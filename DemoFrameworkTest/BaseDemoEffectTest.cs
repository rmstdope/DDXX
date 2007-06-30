using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class BaseDemoEffectTest : BaseDemoEffect, IGraphicsFactory, IEffectFactory, IDevice, IDemoMixer, IEffect
    {
        public bool startCalled;
        public bool endCalled;
        public bool initializeCalled;

        public BaseDemoEffectTest()
            : base(0, 0)
        {
        }

        [SetUp]
        public void SetUp()
        {
            startCalled = false;
            endCalled = false;
            initializeCalled = false;
        }
        
        [Test]
        public void Initialization()
        {
            Initialize(this, this, this, this);
            Assert.IsTrue(initializeCalled);
            Assert.AreSame(this, GraphicsFactory);
            Assert.AreSame(this, Device);
            Assert.AreSame(this, EffectFactory);
            Assert.AreSame(this, Mixer);
            Assert.IsNotNull(XLoader);
        }

        [Test]
        public void StartingTime()
        {
            StartTime = 1.0f;
            Assert.AreEqual(1.0f, StartTime);
            Assert.IsTrue(startCalled);
        }

        [Test]
        public void EndingTime()
        {
            EndTime = 11.0f;
            Assert.AreEqual(11.0f, EndTime);
            Assert.IsTrue(endCalled);
        }

        [Test]
        public void CameraAndSceneCreation()
        {
            // Setup fixture
            IScene scene;
            CameraNode camera;
            Initialize(this, this, this, this);
            //Exercise SUT
            CreateStandardSceneAndCamera(out scene, out camera, 10);
            // Verify
            Assert.AreEqual(new Vector3(0, 0, -10), camera.Position);
            Assert.AreEqual(camera, scene.ActiveCamera, "ActiveCamera set");
            Assert.AreEqual(2, scene.NumNodes, "Only CameraNode and root node");
            Assert.AreEqual(camera, scene.GetNodeByName("Standard Camera"), "Only CameraNode added");
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

        public override void StartTimeUpdated()
        {
            startCalled = true;
        }

        public override void EndTimeUpdated()
        {
            endCalled = true;
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

        public IDevice CreateDevice(int adapter, DeviceType deviceType, System.Windows.Forms.Control renderWindow, CreateFlags behaviorFlags, PresentParameters presentationParameters)
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

        public IAnimationRootFrame SkinnedMeshFromFile(IDevice device, string fileName, AllocateHierarchy allocHierarchy)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IAnimationRootFrame LoadHierarchy(string fileName, IDevice device, AllocateHierarchy allocHierarchy, LoadUserData loadUserData)
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

        #region IEffectFactory Members

        public IEffect CreateFromFile(string file)
        {
            return this;
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

        public bool Disposed
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

        public VertexFormats VertexFormat
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

        public void DrawRectanglePatch(int handle, Microsoft.DirectX.Plane numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, float[] numSegs, RectanglePatchInformation rectPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, Microsoft.DirectX.Plane numSegs, RectanglePatchInformation rectPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, float[] numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, Microsoft.DirectX.Plane numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, float[] numSegs, TrianglePatchInformation triPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, Microsoft.DirectX.Plane numSegs, TrianglePatchInformation triPatchInformation)
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

        public Microsoft.DirectX.Matrix GetTransform(TransformType state)
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

        public void MultiplyTransform(TransformType state, Microsoft.DirectX.Matrix matrix)
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

        public void SetPixelShaderConstant(int startRegister, Microsoft.DirectX.Matrix constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.DirectX.Matrix[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.DirectX.Vector4 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstant(int startRegister, Microsoft.DirectX.Vector4[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstantBoolean(int startRegister, Microsoft.DirectX.GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstantInt32(int startRegister, Microsoft.DirectX.GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPixelShaderConstantSingle(int startRegister, Microsoft.DirectX.GraphicsStream constantData, int numberRegisters)
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

        public void SetTransform(TransformType state, Microsoft.DirectX.Matrix matrix)
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

        public void SetVertexShaderConstant(int startRegister, Microsoft.DirectX.Matrix constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.DirectX.Matrix[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.DirectX.Vector4 constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstant(int startRegister, Microsoft.DirectX.Vector4[] constantData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstantBoolean(int startRegister, Microsoft.DirectX.GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstantInt32(int startRegister, Microsoft.DirectX.GraphicsStream constantData, int numberRegisters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetVertexShaderConstantSingle(int startRegister, Microsoft.DirectX.GraphicsStream constantData, int numberRegisters)
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

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        #region IDemoMixer Members

        public System.Drawing.Color ClearColor
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

        #region IEffect Members

        public int Description_Parameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public EffectDescription Description
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public new IDevice Device
        {
            get { return base.Device; }
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
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetAnnotation(EffectHandle technique, int index)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public EffectHandle GetAnnotation(EffectHandle technique, string name)
        {
            throw new Exception("The method or operation is not implemented.");
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
            return EffectHandle.FromString("");
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
            throw new Exception("The method or operation is not implemented.");
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
    }
}
