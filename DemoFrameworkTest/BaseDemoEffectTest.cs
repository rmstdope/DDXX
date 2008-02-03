using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using NMock2;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.DemoFramework
{
    [TestFixture]
    public class BaseDemoEffectTest : BaseDemoEffect, IGraphicsFactory, IEffectFactory, ITextureFactory,
        IDemoMixer, IEffect, IPostProcessor
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
            Initialize(this, this, this, this, this);
            Assert.IsTrue(initializeCalled);
            Assert.AreSame(this, GraphicsFactory);
            Assert.AreSame(this, EffectFactory);
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
        public void CameraAndSceneCreation()
        {
            // Setup fixture
            IScene scene;
            CameraNode camera;
            Initialize(this, this, this, this, this);
            //Exercise SUT
            CreateStandardSceneAndCamera(out scene, out camera, 10);
            // Verify
            Assert.AreEqual(new Vector3(0, 0, 10), camera.Position);
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

        #endregion

        #region IPostProcessor Members

        public void Initialize(IGraphicsDevice device, IGraphicsFactory graphicsFactory, ITextureFactory textureFactory, IEffectFactory effectFactory) {
            throw new Exception("The method or operation is not implemented.");
        }

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

        #region IGraphicsFactory Members

        public IDeviceManager GraphicsDeviceManager
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IRenderTarget2D CreateRenderTarget2D(int width, int height, int numLevels, Microsoft.Xna.Framework.Graphics.SurfaceFormat format, Microsoft.Xna.Framework.Graphics.MultiSampleType multiSampleType, int multiSampleQuality)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture2D CreateTexture2D(int width, int height, int numLevels, TextureUsage usage, Microsoft.Xna.Framework.Graphics.SurfaceFormat format)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture2D Texture2DFromFile(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITextureCube TextureCubeFromFile(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IVertexBuffer CreateVertexBuffer(Type typeVertexType, int numVerts, BufferUsage usage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISpriteBatch CreateSpriteBatch()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEffect EffectFromFile(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ISpriteFont SpriteFontFromFile(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IEffectFactory Members

        public IEffect CreateFromFile(string file)
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

        public Microsoft.Xna.Framework.Graphics.Color ClearColor
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

        public string Creator
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public IEffectTechnique CurrentTechnique
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

        public Microsoft.Xna.Framework.Graphics.EffectPool EffectPool
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ICollectionAdapter<IEffectFunction> Functions
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public new IGraphicsDevice GraphicsDevice
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ICollectionAdapter<IEffectParameter> Parameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public ICollectionAdapter<IEffectTechnique> Techniques
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void Begin()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void Begin(Microsoft.Xna.Framework.Graphics.SaveStateMode saveStateMode)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IEffect Clone(IGraphicsDevice device)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void CommitChanges()
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

        #endregion

        #region ITextureFactory Members

        ITexture2D ITextureFactory.CreateFromName(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITextureCube CreateCubeFromFile(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IRenderTarget2D CreateFullsizeRenderTarget(SurfaceFormat format, MultiSampleType multiSampleType, int multiSampleQuality)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IRenderTarget2D CreateFullsizeRenderTarget()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public ITexture2D CreateFromFunction(int width, int height, int numLevels, TextureUsage usage, SurfaceFormat format, Fill2DTextureCallback callbackFunction)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void RegisterTexture(string name, ITexture2D texture)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IGraphicsFactory Members


        public IModel ModelFromFile(string name)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IGraphicsFactory Members


        public IIndexBuffer CreateIndexBuffer(Type indexType, int elementCount, BufferUsage usage)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public IVertexDeclaration CreateVertexDeclaration(VertexElement[] vertexElement)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IGraphicsFactory Members


        public IDepthStencilBuffer CreateDepthStencilBuffer(int width, int height, DepthFormat format, MultiSampleType multiSampleType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITextureFactory Members


        public IDepthStencilBuffer CreateFullsizeDepthStencil(DepthFormat format, MultiSampleType multiSampleType)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region ITextureFactory Members


        public ITexture2D WhiteTexture
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region ITextureFactory Members


        public List<Texture2DParameters> Texture2DParameters
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion

        #region ITextureFactory Members


        public ITexture2D CreateFromGenerator(string name, int width, int height, int numMipLevels, TextureUsage usage, SurfaceFormat format, ITextureGenerator generator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
