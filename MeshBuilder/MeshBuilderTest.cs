using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using System.Windows.Forms;
using Dope.DDXX.Utility;
using System.Drawing;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class MeshBuilderTest : IGraphicsFactory, IModel, IPrimitive, IDevice, ITextureFactory, IPrimitiveFactory, IBody, ITexture
    {
        private MeshBuilder builder;
        private float width;
        private float height;
        private float length;
        private int widthSegments;
        private int heightSegments;
        private int lengthSegments;
        private bool textured;
        private int[] pinnedParticles;
        private bool createPlaneCalled;
        private bool createBoxCalled;
        private bool createClothCalled;
        private bool setMaterialCalled;
        private string fileName;
        
        [SetUp]
        public void SetUp()
        {
            builder = new MeshBuilder(this, this, this, this);
            width = -1.0f;
            height = -1.0f;
            length = -1.0f;
            widthSegments = -1;
            heightSegments = -1;
            lengthSegments = -1;
            textured = false;
            pinnedParticles = new int[0];
            createPlaneCalled = false;
            createBoxCalled = false;
            createClothCalled = false;
            setMaterialCalled = false;
            fileName = null;
        }

        /// <summary>
        /// Test creating a Model from a primitive
        /// </summary>
        [Test]
        public void TestCreateMesh()
        {
            builder.AddPrimitive(this, "Name1");
            IModel model = builder.CreateModel("Name1");
            Assert.AreSame(this, model, "This instance should be returned as IModel.");
        }

        /// <summary>
        /// Test calling CreateMesh without having added the primitive first.
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestCreateWithoutAdd()
        {
            IModel model = builder.CreateModel("Name1");
        }

        /// <summary>
        /// Test box creation using the factory
        /// </summary>
        [Test]
        public void TestBoxCreation()
        {
            width = 1;
            height = 2;
            length = 3;
            widthSegments = 4;
            heightSegments = 5;
            lengthSegments = 6;
            builder.CreateBox("Box", length, width, height, 
                lengthSegments, widthSegments, heightSegments);
            Assert.AreSame(this, builder.GetPrimitive("Box"));
            Assert.IsTrue(createBoxCalled, "CreateBox should have been called.");
        }

        /// <summary>
        /// Test box creation of duplicate name
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestBoxCreationFail()
        {
            TestBoxCreation();
            builder.CreateBox("Box", length, width, height,
                lengthSegments, widthSegments, heightSegments);
        }

        /// <summary>
        /// Test plane creation using the factory
        /// </summary>
        [Test]
        public void TestPlaneCreation()
        {
            width = 1;
            height = 2;
            widthSegments = 4;
            heightSegments = 5;
            textured = true;
            builder.CreatePlane("Plane", width, height, widthSegments, heightSegments, textured);
            Assert.AreSame(this, builder.GetPrimitive("Plane"));
            Assert.IsTrue(createPlaneCalled, "CreatePlane should have been called.");
        }

        /// <summary>
        /// Test plane creation of duplicate name
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestPlaneCreationFail()
        {
            TestPlaneCreation();
            builder.CreatePlane("Plane", width, height, widthSegments, heightSegments, textured);
        }

        /// <summary>
        /// Test plane and box creation of duplicate name
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestDuplicates()
        {
            TestBoxCreation();
            builder.CreatePlane("Box", width, height, widthSegments, heightSegments, textured);
        }

        /// <summary>
        /// Test cloth creation using the factory
        /// </summary>
        [Test]
        public void TestClothCreationNoPinned()
        {
            width = 1;
            height = 2;
            widthSegments = 4;
            heightSegments = 5;
            textured = true;
            builder.CreateCloth("Cloth", this, width, height, widthSegments, heightSegments, textured);
            Assert.AreSame(this, builder.GetPrimitive("Cloth"));
            Assert.IsTrue(createClothCalled, "CreateCloth should have been called.");
        }

        /// <summary>
        /// Test cloth creation using the factory
        /// </summary>
        [Test]
        public void TestClothCreationPinned()
        {
            width = 1;
            height = 2;
            widthSegments = 4;
            heightSegments = 5;
            textured = true;
            pinnedParticles = new int[] { 3, 7, 6, 4, 8, 9, 65 };
            builder.CreateCloth("Cloth", this, width, height, widthSegments, heightSegments, pinnedParticles, textured);
            Assert.AreSame(this, builder.GetPrimitive("Cloth"));
            Assert.IsTrue(createClothCalled, "CreateCloth should have been called.");
        }

        /// <summary>
        /// Test material assignment with invalid material.
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestMaterialAssignmentFail1()
        {
            TestBoxCreation();
            builder.AssignMaterial("Box", "InvalidMaterial");
        }

        /// <summary>
        /// Test material assignment.
        /// </summary>
        [Test]
        public void TestMaterialAssignmentOK()
        {
            TestBoxCreation();
            for (int i = 0; i < 6; i++)
            {
                setMaterialCalled = false;
                builder.AssignMaterial("Box", "Default" + (i + 1));
                Assert.IsTrue(setMaterialCalled);
            }
        }

        /// <summary>
        /// Test material assignment with invalid primitive.
        /// </summary>
        [Test]
        [ExpectedException(typeof(DDXXException))]
        public void TestMaterialAssignmentFail2()
        {
            builder.AssignMaterial("InvalidPrimitive", "Default1");
        }

        /// <summary>
        /// Test setting of the diffuse texture of a material.
        /// </summary>
        [Test]
        public void TestDiffuseTexture()
        {
            Assert.AreSame(null, builder.GetMaterial("Default1").DiffuseTexture);
            Assert.AreSame(null, builder.GetMaterial("Default1").NormalTexture);
            fileName = "DiffuseTexture";
            builder.SetDiffuseTexture("Default1", fileName);
            Assert.AreSame(this, builder.GetMaterial("Default1").DiffuseTexture);
        }

        /// <summary>
        /// Test removing the diffuse texture of a material.
        /// </summary>
        [Test]
        public void TestDiffuseTextureRemove()
        {
            TestDiffuseTexture();
            builder.SetDiffuseTexture("Default1", null);
            Assert.AreSame(null, builder.GetMaterial("Default1").DiffuseTexture);
            TestDiffuseTexture();
            builder.SetDiffuseTexture("Default1", "");
            Assert.AreSame(null, builder.GetMaterial("Default1").DiffuseTexture);
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

        public short[] Indices
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IBody Body
        {
            get { throw new Exception("The method or operation is not implemented."); }
            set { throw new Exception("The method or operation is not implemented."); }
        }

        public IModel CreateModel(IGraphicsFactory factory, 
            IDevice device)
        {
            Assert.AreSame(this, factory, "Factory should be same as this.");
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

        IndexBuffer IDevice.Indices
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

        public bool Disposed
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Dispose()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IModel Members

        public ModelMaterial[] Materials
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
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public bool IsSkinned()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Draw(IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion


        #region ITextureFactory Members

        public ITexture CreateFromFile(string file)
        {
            Assert.AreEqual(fileName, file);
            return this;
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


        #region IPrimitiveFactory Members

        public IPrimitive CreateCloth(IBody body, float width, float height, 
            int widthSegments, int heightSegments, int[] pinnedParticles, bool textured)
        {
            createClothCalled = true;
            Assert.AreSame(this, body);
            Assert.AreEqual(this.width, width);
            Assert.AreEqual(this.height, height);
            Assert.AreEqual(this.widthSegments, widthSegments);
            Assert.AreEqual(this.heightSegments, heightSegments);
            Assert.AreEqual(this.textured, textured);
            Assert.AreSame(this.pinnedParticles, pinnedParticles);
            return this;
        }

        public IPrimitive CreateCloth(IBody body, float width, float height, 
            int widthSegments, int heightSegments, bool textured)
        {
            createClothCalled = true;
            Assert.AreSame(this, body);
            Assert.AreEqual(this.width, width);
            Assert.AreEqual(this.height, height);
            Assert.AreEqual(this.widthSegments, widthSegments);
            Assert.AreEqual(this.heightSegments, heightSegments);
            Assert.AreEqual(this.textured, textured);
            return this;
        }

        public IPrimitive CreatePlane(float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            createPlaneCalled = true;
            Assert.AreEqual(this.width, width);
            Assert.AreEqual(this.height, height);
            Assert.AreEqual(this.widthSegments, widthSegments);
            Assert.AreEqual(this.heightSegments, heightSegments);
            Assert.AreEqual(this.textured, textured);
            return this;
        }

        public IPrimitive CreateBox(float length, float width, float height, int lengthSegments, int widthSegments, int heightSegments)
        {
            createBoxCalled = true;
            Assert.AreEqual(this.length, length);
            Assert.AreEqual(this.width, width);
            Assert.AreEqual(this.height, height);
            Assert.AreEqual(this.lengthSegments, lengthSegments);
            Assert.AreEqual(this.widthSegments, widthSegments);
            Assert.AreEqual(this.heightSegments, heightSegments);
            return this;
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

        public List<IPhysicalParticle> Particles
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void ApplyForce(Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IPrimitive Members


        public ModelMaterial ModelMaterial
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                setMaterialCalled = true;
            }
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

        public Device Device
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

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
    }
}
