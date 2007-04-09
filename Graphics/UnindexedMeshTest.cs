using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Utility;

namespace Dope.DDXX.Graphics
{
    [TestFixture]
    public class UnindexedMeshTest : IGraphicsFactory, IVertexBuffer, IDevice
    {
        private UnindexedMesh mesh;
        private Type type;
        private int numVerts;
        private IDevice device;
        private Usage usage;
        private VertexFormats vertexFormats;
        private Pool pool;
        private int streamNumber;
        private IVertexBuffer streamData;
        private int offsetInBytes;
        private PrimitiveType primitiveType;
        private int startVertex;
        private int primitiveCount;
        private object data;
        private int lockAtOffset;
        private LockFlags flags;

        [SetUp]
        public void SetUp()
        {
            type = null;
            numVerts = -1;
            device = null;
            usage = Usage.None;
            vertexFormats = VertexFormats.None;
            pool = Pool.Scratch;
            streamNumber = -1;
            streamData = null;
            offsetInBytes = -1;
            primitiveType = PrimitiveType.TriangleFan;
            startVertex = -1;
            primitiveCount = -1;
            data = null;
            lockAtOffset = -1;
            flags = LockFlags.None;
        }

        [Test]
        public void TestConstructor10Vertices()
        {
            mesh = new UnindexedMesh(this, typeof(int), 10, this, Usage.WriteOnly, 
                VertexFormats.PointSize, Pool.Default);
            Assert.AreEqual(typeof(int), type);
            Assert.AreEqual(10, numVerts);
            Assert.AreSame(this, device);
            Assert.AreEqual(Usage.WriteOnly, usage);
            Assert.AreEqual(VertexFormats.PointSize, vertexFormats);
            Assert.AreEqual(Pool.Default, pool);
            Assert.AreSame(this, mesh.VertexBuffer);
        }

        [Test]
        public void TestConstructor1Vertex()
        {
            mesh = new UnindexedMesh(this, typeof(float), 1, this, Usage.SoftwareProcessing,
                VertexFormats.Normal, Pool.Managed);
            Assert.AreEqual(typeof(float), type);
            Assert.AreEqual(1, numVerts);
            Assert.AreSame(this, device);
            Assert.AreEqual(Usage.SoftwareProcessing, usage);
            Assert.AreEqual(VertexFormats.Normal, vertexFormats);
            Assert.AreEqual(Pool.Managed, pool);
            Assert.AreSame(this, mesh.VertexBuffer);
        }

        [Test]
        public void TestDrawSubsetOK()
        {
            TestConstructor10Vertices();
            mesh.DrawSubset(0);
            Assert.AreEqual(0, streamNumber);
            Assert.AreEqual(this, streamData);
            Assert.AreEqual(0, offsetInBytes);
            Assert.AreEqual(PrimitiveType.LineList, primitiveType);
            Assert.AreEqual(0, startVertex);
            Assert.AreEqual(5, primitiveCount);
        }

        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestDrawSubsetFail()
        {
            TestConstructor10Vertices();
            mesh.DrawSubset(1);
            Assert.AreEqual(this, data);
            Assert.AreEqual(0, lockAtOffset);
            Assert.AreEqual(LockFlags.Discard, flags);
        }

        [Test]
        public void TestSetData()
        {
            TestConstructor10Vertices();
            mesh.SetVertexBufferData(this, LockFlags.NoSystemLock);
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

        public IDevice CreateDevice(int adapter, Microsoft.DirectX.Direct3D.DeviceType deviceType, System.Windows.Forms.Control renderWindow, Microsoft.DirectX.Direct3D.CreateFlags behaviorFlags, Microsoft.DirectX.Direct3D.PresentParameters presentationParameters)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateTexture(IDevice device, System.Drawing.Bitmap image, Microsoft.DirectX.Direct3D.Usage usage, Microsoft.DirectX.Direct3D.Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateTexture(IDevice device, System.IO.Stream data, Microsoft.DirectX.Direct3D.Usage usage, Microsoft.DirectX.Direct3D.Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateTexture(IDevice device, int width, int height, int numLevels, Microsoft.DirectX.Direct3D.Usage usage, Microsoft.DirectX.Direct3D.Format format, Microsoft.DirectX.Direct3D.Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICubeTexture CreateCubeTexture(IDevice device, int edgeLength, int levels, Microsoft.DirectX.Direct3D.Usage usage, Microsoft.DirectX.Direct3D.Format format, Microsoft.DirectX.Direct3D.Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh CreateMesh(int numFaces, int numVertices, Microsoft.DirectX.Direct3D.MeshFlags options, Microsoft.DirectX.Direct3D.VertexElement[] declaration, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh CreateMesh(int numFaces, int numVertices, Microsoft.DirectX.Direct3D.MeshFlags options, Microsoft.DirectX.Direct3D.VertexFormats vertexFormat, IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out Microsoft.DirectX.Direct3D.EffectInstance[] effectInstance)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh MeshFromFile(IDevice device, string fileName, out Microsoft.DirectX.Direct3D.ExtendedMaterial[] materials)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dope.DDXX.Graphics.Skinning.IAnimationRootFrame SkinnedMeshFromFile(IDevice device, string fileName, Microsoft.DirectX.Direct3D.AllocateHierarchy allocHierarchy)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dope.DDXX.Graphics.Skinning.IAnimationRootFrame LoadHierarchy(string fileName, IDevice device, Microsoft.DirectX.Direct3D.AllocateHierarchy allocHierarchy, Microsoft.DirectX.Direct3D.LoadUserData loadUserData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IMesh CreateBoxMesh(IDevice device, float width, float height, float depth)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEffect EffectFromFile(IDevice device, string sourceDataFile, Microsoft.DirectX.Direct3D.Include includeFile, string skipConstants, Microsoft.DirectX.Direct3D.ShaderFlags flags, Microsoft.DirectX.Direct3D.EffectPool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture TextureFromFile(IDevice device, string srcFile, int width, int height, int mipLevels, Microsoft.DirectX.Direct3D.Usage usage, Microsoft.DirectX.Direct3D.Format format, Microsoft.DirectX.Direct3D.Pool pool, Microsoft.DirectX.Direct3D.Filter filter, Microsoft.DirectX.Direct3D.Filter mipFilter, int colorKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICubeTexture CubeTextureFromFile(IDevice device, string fileName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ICubeTexture CubeTextureFromFile(IDevice device, string fileName, int size, int mipLevels, Microsoft.DirectX.Direct3D.Usage usage, Microsoft.DirectX.Direct3D.Format format, Microsoft.DirectX.Direct3D.Pool pool, Microsoft.DirectX.Direct3D.Filter filter, Microsoft.DirectX.Direct3D.Filter mipFilter, int colorKey)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISprite CreateSprite(IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, 
            IDevice device, Usage usage, VertexFormats vertexFormat, Pool pool)
        {
            this.type = typeVertexType;
            this.numVerts = numVerts;
            this.device = device;
            this.usage = usage;
            this.vertexFormats = vertexFormat;
            this.pool = pool;
            return this;
        }

        public ILine CreateLine(IDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.VertexDeclaration CreateVertexDeclaration(IDevice device, Microsoft.DirectX.Direct3D.VertexElement[] elements)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFont CreateFont(IDevice device, Microsoft.DirectX.Direct3D.FontDescription description)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFont CreateFont(IDevice device, System.Drawing.Font font)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IFont CreateFont(IDevice device, int height, int width, Microsoft.DirectX.Direct3D.FontWeight weight, int miplevels, bool italic, Microsoft.DirectX.Direct3D.CharacterSet charset, Microsoft.DirectX.Direct3D.Precision outputPrecision, Microsoft.DirectX.Direct3D.FontQuality quality, Microsoft.DirectX.Direct3D.PitchAndFamily pitchFamily, string faceName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IVertexBuffer Members

        public Microsoft.DirectX.Direct3D.VertexBufferDescription Description
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public bool Disposed
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public int SizeInBytes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Array Lock(int offsetToLock, Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IGraphicsStream Lock(int offsetToLock, int sizeToLock, Microsoft.DirectX.Direct3D.LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array Lock(int offsetToLock, Type typeVertex, Microsoft.DirectX.Direct3D.LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetData(object data, int lockAtOffset, LockFlags flags)
        {
            this.data = data;
            this.lockAtOffset = lockAtOffset;
            this.flags = flags;
        }

        public void Unlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IDevice Members

        public int AvailableTextureMemory
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.ClipPlanes ClipPlanes
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.ClipStatus ClipStatus
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

        public Microsoft.DirectX.Direct3D.DeviceCreationParameters CreationParameters
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

        public Microsoft.DirectX.Direct3D.Caps DeviceCaps
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.DisplayMode DisplayMode
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.IndexBuffer Indices
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

        public Microsoft.DirectX.Direct3D.LightsCollection Lights
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.Material Material
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

        public Microsoft.DirectX.Direct3D.PixelShader PixelShader
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

        public Microsoft.DirectX.Direct3D.PresentParameters PresentationParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.RasterStatus RasterStatus
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IRenderStateManager RenderState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.SamplerStateManagerCollection SamplerState
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

        public Microsoft.DirectX.Direct3D.TextureStateManagerCollection TextureState
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.Transforms Transform
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public Microsoft.DirectX.Direct3D.VertexDeclaration VertexDeclaration
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

        public Microsoft.DirectX.Direct3D.VertexFormats VertexFormat
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

        public Microsoft.DirectX.Direct3D.VertexShader VertexShader
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

        public Microsoft.DirectX.Direct3D.Viewport Viewport
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

        public void Clear(Microsoft.DirectX.Direct3D.ClearFlags flags, System.Drawing.Color color, float zdepth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(Microsoft.DirectX.Direct3D.ClearFlags flags, int color, float zdepth, int stencil)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(Microsoft.DirectX.Direct3D.ClearFlags flags, System.Drawing.Color color, float zdepth, int stencil, System.Drawing.Rectangle[] rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Clear(Microsoft.DirectX.Direct3D.ClearFlags flags, int color, float zdepth, int stencil, System.Drawing.Rectangle[] regions)
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

        public ISurface CreateDepthStencilSurface(int width, int height, Microsoft.DirectX.Direct3D.DepthFormat format, Microsoft.DirectX.Direct3D.MultiSampleType multiSample, int multiSampleQuality, bool discard)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface CreateOffscreenPlainSurface(int width, int height, Microsoft.DirectX.Direct3D.Format format, Microsoft.DirectX.Direct3D.Pool pool)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface CreateRenderTarget(int width, int height, Microsoft.DirectX.Direct3D.Format format, Microsoft.DirectX.Direct3D.MultiSampleType multiSample, int multiSampleQuality, bool lockable)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DeletePatch(int handle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawIndexedPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType primitiveType, int baseVertex, int minVertexIndex, int numVertices, int startIndex, int primCount)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawIndexedUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType primitiveType, int minVertexIndex, int numVertexIndices, int primitiveCount, object indexData, bool sixteenBitIndices, object vertexStreamZeroData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawPrimitives(PrimitiveType primitiveType, int startVertex, int primitiveCount)
        {
            this.primitiveType = primitiveType;
            this.startVertex = startVertex;
            this.primitiveCount = primitiveCount;
        }

        public void DrawRectanglePatch(int handle, float[] numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, Microsoft.DirectX.Plane numSegs)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, float[] numSegs, Microsoft.DirectX.Direct3D.RectanglePatchInformation rectPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawRectanglePatch(int handle, Microsoft.DirectX.Plane numSegs, Microsoft.DirectX.Direct3D.RectanglePatchInformation rectPatchInformation)
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

        public void DrawTrianglePatch(int handle, float[] numSegs, Microsoft.DirectX.Direct3D.TrianglePatchInformation triPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawTrianglePatch(int handle, Microsoft.DirectX.Plane numSegs, Microsoft.DirectX.Direct3D.TrianglePatchInformation triPatchInformation)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void DrawUserPrimitives(Microsoft.DirectX.Direct3D.PrimitiveType primitiveType, int primitiveCount, object vertexStreamZeroData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EndScene()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.StateBlock EndStateBlock()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void EvictManagedResources()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISurface GetBackBuffer(int swapChain, int backBuffer, Microsoft.DirectX.Direct3D.BackBufferType backBufferType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.CubeTexture GetCubeTexture(int stage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void GetFrontBufferData(int swapChain, ISurface buffer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.GammaRamp GetGammaRamp(int swapChain)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.PaletteEntry[] GetPaletteEntries(int paletteNumber)
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

        public Microsoft.DirectX.Direct3D.RasterStatus GetRasterStatus(int swapChain)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool GetRenderStateBoolean(Microsoft.DirectX.Direct3D.RenderStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetRenderStateInt32(Microsoft.DirectX.Direct3D.RenderStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetRenderStateSingle(Microsoft.DirectX.Direct3D.RenderStates state)
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

        public bool GetSamplerStageStateBoolean(int stage, Microsoft.DirectX.Direct3D.SamplerStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetSamplerStageStateInt32(int stage, Microsoft.DirectX.Direct3D.SamplerStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetSamplerStageStateSingle(int stage, Microsoft.DirectX.Direct3D.SamplerStageStates state)
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

        public Microsoft.DirectX.Direct3D.SwapChain GetSwapChain(int swapChain)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.Texture GetTexture(int stage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public bool GetTextureStageStateBoolean(int stage, Microsoft.DirectX.Direct3D.TextureStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int GetTextureStageStateInt32(int stage, Microsoft.DirectX.Direct3D.TextureStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public float GetTextureStageStateSingle(int stage, Microsoft.DirectX.Direct3D.TextureStageStates state)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Matrix GetTransform(Microsoft.DirectX.Direct3D.TransformType state)
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

        public Microsoft.DirectX.Direct3D.VolumeTexture GetVolumeTexture(int stage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void MultiplyTransform(Microsoft.DirectX.Direct3D.TransformType state, Microsoft.DirectX.Matrix matrix)
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

        public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, Microsoft.DirectX.Direct3D.VertexBuffer destBuffer, Microsoft.DirectX.Direct3D.VertexDeclaration vertexDeclaration)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void ProcessVertices(int srcStartIndex, int destIndex, int vertexCount, Microsoft.DirectX.Direct3D.VertexBuffer destBuffer, Microsoft.DirectX.Direct3D.VertexDeclaration vertexDeclaration, bool copyData)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Reset(params Microsoft.DirectX.Direct3D.PresentParameters[] presentationParameters)
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

        public void SetGammaRamp(int swapChain, bool calibrate, Microsoft.DirectX.Direct3D.GammaRamp ramp)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetPaletteEntries(int paletteNumber, Microsoft.DirectX.Direct3D.PaletteEntry[] entries)
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

        public void SetRenderState(Microsoft.DirectX.Direct3D.RenderStates state, bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderState(Microsoft.DirectX.Direct3D.RenderStates state, float value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderState(Microsoft.DirectX.Direct3D.RenderStates state, int value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetRenderTarget(int renderTargetIndex, ISurface renderTarget)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSamplerState(int stage, Microsoft.DirectX.Direct3D.SamplerStageStates state, bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSamplerState(int stage, Microsoft.DirectX.Direct3D.SamplerStageStates state, float value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetSamplerState(int stage, Microsoft.DirectX.Direct3D.SamplerStageStates state, int value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetStreamSource(int streamNumber, IVertexBuffer streamData, int offsetInBytes)
        {
            this.streamNumber = streamNumber;
            this.streamData = streamData;
            this.offsetInBytes = offsetInBytes;
        }

        public void SetStreamSource(int streamNumber, IVertexBuffer streamData, int offsetInBytes, int stride)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetStreamSourceFrequency(int streamNumber, int divider)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTexture(int stage, Microsoft.DirectX.Direct3D.BaseTexture texture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTextureStageState(int stage, Microsoft.DirectX.Direct3D.TextureStageStates state, bool value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTextureStageState(int stage, Microsoft.DirectX.Direct3D.TextureStageStates state, float value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTextureStageState(int stage, Microsoft.DirectX.Direct3D.TextureStageStates state, int value)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void SetTransform(Microsoft.DirectX.Direct3D.TransformType state, Microsoft.DirectX.Matrix matrix)
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

        public void StretchRectangle(ISurface sourceSurface, System.Drawing.Rectangle sourceRectangle, ISurface destSurface, System.Drawing.Rectangle destRectangle, Microsoft.DirectX.Direct3D.TextureFilter filter)
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

        public void UpdateTexture(Microsoft.DirectX.Direct3D.BaseTexture sourceTexture, Microsoft.DirectX.Direct3D.BaseTexture destinationTexture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Direct3D.ValidateDeviceParams ValidateDevice()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
