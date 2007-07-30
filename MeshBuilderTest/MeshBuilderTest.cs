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
    public class MeshBuilderTest : IGraphicsFactory, IModel, IDevice, ITextureFactory, IBody, ICubeTexture, IMesh, ITexture, IModifier, IGraphicsStream
    {
        private MeshBuilder builder;
        private string fileName;
        private Viewport viewport;

        private Vertex[] vertices;
        private short[] indices;
        private int numFaces;
        private int numVertices;
        private bool vbLocked;
        private bool ibLocked;
        private List<Vector3> positions;
        private List<Vector3> normals;
        private List<float> textureCoordinates;
        private bool useTextureCoordinates;
        private VertexPosition vertexPosition;
        private bool body;

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
            vbLocked = false;
            ibLocked = false;
            positions = new List<Vector3>();
            normals = new List<Vector3>();
            textureCoordinates = new List<float>();
            vertexPosition = VertexPosition.POSITION;
            useTextureCoordinates = false;
            body = false;
            builder = new MeshBuilder(this, this, this);
            fileName = null;
            viewport = new Viewport();
        }

        [Test]
        public void TestDefaultColors()
        {
            Assert.AreEqual(new ColorValue(0.6f, 0.6f, 0.6f, 0.6f), builder.GetMaterial("Default1").DiffuseColor);
            Assert.AreEqual(new ColorValue(0.3f, 0.3f, 0.3f, 0.3f), builder.GetMaterial("Default1").AmbientColor);
        }

        /// <summary>
        /// Test setting of the diffuse texture of a material.
        /// </summary>
        [Test]
        public void TestDiffuseTexture()
        {
            Assert.AreSame(null, builder.GetMaterial("Default1").DiffuseTexture);
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

        /// <summary>
        /// Test setting of the normal texture of a material.
        /// </summary>
        [Test]
        public void TestNormalTexture()
        {
            Assert.AreSame(null, builder.GetMaterial("Default1").NormalTexture);
            fileName = "NormalTexture";
            builder.SetNormalTexture("Default1", fileName);
            Assert.AreSame(this, builder.GetMaterial("Default1").NormalTexture);
        }

        /// <summary>
        /// Test removing the normal texture of a material.
        /// </summary>
        [Test]
        public void TestNormalTextureRemove()
        {
            TestNormalTexture();
            builder.SetNormalTexture("Default1", null);
            Assert.AreSame(null, builder.GetMaterial("Default1").NormalTexture);
            TestNormalTexture();
            builder.SetNormalTexture("Default1", "");
            Assert.AreSame(null, builder.GetMaterial("Default1").NormalTexture);
        }

        /// <summary>
        /// Test setting of the reflective texture of a material.
        /// </summary>
        [Test]
        public void TestReflectiveTexture()
        {
            Assert.AreSame(null, builder.GetMaterial("Default1").ReflectiveTexture);
            fileName = "ReflectiveTexture";
            builder.SetReflectiveTexture("Default1", fileName);
            Assert.AreSame(this, builder.GetMaterial("Default1").ReflectiveTexture);
        }

        /// <summary>
        /// Test removing the reflective texture of a material.
        /// </summary>
        [Test]
        public void TestReflectiveTextureRemove()
        {
            TestReflectiveTexture();
            builder.SetReflectiveTexture("Default1", null);
            Assert.AreSame(null, builder.GetMaterial("Default1").ReflectiveTexture);
            TestReflectiveTexture();
            builder.SetReflectiveTexture("Default1", "");
            Assert.AreSame(null, builder.GetMaterial("Default1").ReflectiveTexture);
        }

        /// <summary>
        /// Test creating a SkyBox Model
        /// </summary>
        [Test]
        public void TestCreateSkyBox()
        {
            numFaces = 2;
            numVertices = 4;
            viewport.Width = 100;
            viewport.Height = 200;
            fileName = "SkyBoxTexture";
            IModel model = builder.CreateSkyBoxModel("SkyBoxName", fileName);
            Assert.AreSame(this, model.Mesh, "This instance should be returned as Mesh.");
            Assert.AreEqual(1, model.Materials.Length, "We should have one material."); 
            Assert.AreSame(this, model.Materials[0].ReflectiveTexture, 
                "This instance should be returned as reflective texture.");
            Assert.IsNull(model.Materials[0].DiffuseTexture, "Diffuse texture should be null.");
            Assert.IsNull(model.Materials[0].NormalTexture, "Normal texture should be null.");
        }

        /// <summary>
        /// Test the CreatePrimitive method on a generic mesh with texture coordinates.
        /// </summary>
        [Test]
        public void TestCreateModelWithTexCoords()
        {
            useTextureCoordinates = true;
            CreatePrimitive();
            IModel model = builder.CreateModel(this, "");
            CheckModel(model);
            Assert.IsNotInstanceOfType(typeof(PhysicalModel), model, "Model shall not be PhysicalModel");
        }

        /// <summary>
        /// Test the CreatePrimitive method on a generic mesh.
        /// </summary>
        [Test]
        public void TestCreateModel()
        {
            CreatePrimitive();
            IModel model = builder.CreateModel(this, "");
            CheckModel(model);
            Assert.IsNotInstanceOfType(typeof(PhysicalModel), model, "Model shall not be PhysicalModel");
            Assert.IsNotNull(model.Materials[0]);
        }

        /// <summary>
        /// Test that the created model is a physical model.
        /// </summary>
        [Test]
        public void TestCreatePhysicalModel()
        {
            CreatePrimitive();
            body = true;
            IModel model = builder.CreateModel(this, "");
            CheckModel(model);
            Assert.IsInstanceOfType(typeof(PhysicalModel), model, "Model shall be PhysicalModel");
        }

        [Test]
        public void TestCreateMaterial()
        {
            CreatePrimitive();
            builder.SetAmbientColor("Default1", ColorValue.FromColor(Color.AliceBlue));
            builder.SetDiffuseColor("Default1", ColorValue.FromColor(Color.AntiqueWhite));
            fileName = "DiffuseTexture";
            builder.SetDiffuseTexture("Default1", fileName);
            fileName = "NormalTexture";
            builder.SetNormalTexture("Default1", fileName);
            fileName = "ReflectiveTexture";
            builder.SetReflectiveTexture("Default1", fileName);
            builder.SetReflectiveFactor("Default1", 0.3f);
            builder.SetSpecularColor("Default1", ColorValue.FromColor(Color.Aqua));
            IModel model = builder.CreateModel(this, "Default1");
            Assert.AreEqual(model.Materials[0].AmbientColor, ColorValue.FromColor(Color.AliceBlue));
            Assert.AreEqual(model.Materials[0].DiffuseColor, ColorValue.FromColor(Color.AntiqueWhite));
            Assert.AreEqual(model.Materials[0].DiffuseTexture, this);
            Assert.AreEqual(model.Materials[0].NormalTexture, this);
            Assert.AreEqual(model.Materials[0].ReflectiveFactor, 0.3f);
            Assert.AreEqual(model.Materials[0].ReflectiveTexture, this);
            Assert.AreEqual(model.Materials[0].SpecularColor, ColorValue.FromColor(Color.Aqua));
            Assert.AreNotSame(model.Materials[0], builder.GetMaterial("Default1"));
        }

        [Test]
        public void TestDiffuseColor()
        {
            builder.SetDiffuseColor("Default1", new ColorValue(1, 2, 3, 4));
            Assert.AreEqual(new ColorValue(1, 2, 3, 4), builder.GetMaterial("Default1").DiffuseColor);
        }

        [Test]
        public void TestAmbientColor()
        {
            builder.SetAmbientColor("Default1", new ColorValue(5, 6, 7, 8));
            Assert.AreEqual(new ColorValue(5, 6, 7, 8), builder.GetMaterial("Default1").AmbientColor);
        }

        [Test]
        public void TestSpecularColor()
        {
            builder.SetSpecularColor("Default1", new ColorValue(3, 5, 7, 9));
            Assert.AreEqual(new ColorValue(3, 5, 7, 9), builder.GetMaterial("Default1").SpecularColor);
        }

        private void CheckModel(IModel model)
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

        private void CreatePrimitive()
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
            Assert.AreEqual(numFaces, this.numFaces);
            Assert.AreEqual(this.numVertices, numVertices);
            Assert.AreEqual(MeshFlags.Managed, options);
            //Assert.AreEqual(declaration.Length, 2);
            Assert.AreEqual(DeclarationUsage.Position, declaration[0].DeclarationUsage);
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

        public Dope.DDXX.Graphics.IAnimationRootFrame SkinnedMeshFromFile(IDevice device, string fileName, AllocateHierarchy allocHierarchy)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Dope.DDXX.Graphics.IAnimationRootFrame LoadHierarchy(string fileName, IDevice device, AllocateHierarchy allocHierarchy, LoadUserData loadUserData)
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
                return viewport;
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

        public void Render(IDevice device, IEffectHandler effectHandler, ColorValue ambient, Matrix world, Matrix view, Matrix projection)
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

        public ITexture CreateRenderTarget(int width, int height) {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture CreateRenderTarget(int width, int height, Format format) {
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

        public List<IPhysicalParticle> Particles
        {
            get { return new List<IPhysicalParticle>(); }
        }

        public void ApplyForce(Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITextureFactory Members


        public ICubeTexture CreateCubeFromFile(string file)
        {
            return this;
        }

        #endregion

        #region IMesh Members

        public VertexElement[] Declaration
        {
            get { throw new Exception("The method or operation is not implemented."); }
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
            Assert.AreEqual(new short[] { 0, 1, 2, 3, 2, 1 }, (short[])data);
        }

        public void SetVertexBufferData(object data, LockFlags flags)
        {
            Vector3[] vertices = (Vector3[])data;
            Assert.AreEqual(4, vertices.Length);
            Assert.AreEqual(vertices[0].X, 1.01f);
            Assert.AreEqual(vertices[0].Y, 1.005f);
            Assert.AreEqual(vertices[1].X, 1.01f);
            Assert.AreEqual(vertices[1].Y, -1.005f);
            Assert.AreEqual(vertices[2].X, -1.01f);
            Assert.AreEqual(vertices[2].Y, 1.005f);
            Assert.AreEqual(vertices[3].X, -1.01f);
            Assert.AreEqual(vertices[3].Y, -1.005f);
            for (int i = 0; i < vertices.Length; i++)
                Assert.AreEqual(1, vertices[i].Z);
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

        #region ICubeTexture Members

        public void AddDirtyRectangle(CubeMapFace faceType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddDirtyRectangle(CubeMapFace faceType, Rectangle rect)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Surface GetCubeMapSurface(CubeMapFace faceType, int level)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, LockFlags flags, out int pitch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public GraphicsStream LockRectangle(CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, LockFlags flags, out int pitch, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Array LockRectangle(Type typeLock, CubeMapFace faceType, int level, Rectangle rect, LockFlags flags, out int pitch, params int[] ranks)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void UnlockRectangle(CubeMapFace faceType, int level)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IBaseTexture Members

        public IDevice Device
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

        #region IModel Members


        public IModel Clone()
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

        ITexture ITextureFactory.CreateFromFile(string file)
        {
            Assert.AreEqual(fileName, file);
            return this;
        }

        ICubeTexture ITextureFactory.CreateCubeFromFile(string file)
        {
            Assert.AreEqual(fileName, file);
            return this;
        }

        ITexture ITextureFactory.CreateFullsizeRenderTarget(Format format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        ITexture ITextureFactory.CreateFullsizeRenderTarget()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        ITexture ITextureFactory.CreateFromFunction(int width, int height, int numLevels, Usage usage, Format format, Pool pool, Fill2DTextureCallback callbackFunction)
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

        #region IBaseTexture Members


        public void Save(string destFile, ImageFileFormat destFormat)
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

        #region IBody Members


        public List<Dope.DDXX.Physics.IConstraint> Constraints
        {
            get { throw new Exception("The method or operation is not implemented."); }
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

        #region IPrimitive Members

        public Primitive Generate()
        {
            Primitive primitive = new Primitive(this.vertices, this.indices);
            if (this.body)
                primitive.Body = this;
            return primitive;
        }

        #endregion


        #region IModel Members


        public Cull CullMode
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

        #region IModel Members


        public bool UseStencil
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

        public int StencilReference
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

        #region IBody Members


        public void Step(float time)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
