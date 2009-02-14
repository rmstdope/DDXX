using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using NMock2;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.GameFramework
{
    [TestFixture]
    public class GameExecuterTest
    {
        private GameExecuter executer;
        private Mockery mockery;
        private IGraphicsFactory graphicsFactory;
        private IGraphicsDevice graphicsDevice;
        private IInputDriver inputDriver;
        private IEffectFactory effectFactory;
        private ITextureFactory textureFactory;
        private IPostProcessor postProcessor;
        private IRenderTarget2D renderTarget;
        private IDepthStencilBuffer depthStencilBuffer;
        private PresentationParameters presentationParameters;
        private ISpriteBatch spriteBatch;
        private IGameCallback gameCallback;
        private IFsa baseFsa;
        private ITexture2D texture2D;

        [SetUp]
        public void SetUp()
        {
            executer = new GameExecuter();
            mockery = new Mockery();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            graphicsDevice = mockery.NewMock<IGraphicsDevice>();
            inputDriver = mockery.NewMock<IInputDriver>();
            effectFactory = mockery.NewMock<IEffectFactory>();
            textureFactory = mockery.NewMock<ITextureFactory>();
            postProcessor = mockery.NewMock<IPostProcessor>();
            renderTarget = mockery.NewMock<IRenderTarget2D>();
            depthStencilBuffer = mockery.NewMock<IDepthStencilBuffer>();
            spriteBatch = mockery.NewMock<ISpriteBatch>();
            gameCallback = mockery.NewMock<IGameCallback>();
            baseFsa = mockery.NewMock<IFsa>();
            texture2D = mockery.NewMock<ITexture2D>();
            presentationParameters = new PresentationParameters();
            presentationParameters.AutoDepthStencilFormat = DepthFormat.Depth32;
            Stub.On(graphicsFactory).GetProperty("GraphicsDevice").Will(Return.Value(graphicsDevice));
            Stub.On(graphicsDevice).GetProperty("PresentationParameters").Will(Return.Value(presentationParameters));
            Stub.On(postProcessor).GetProperty("OutputTexture").Will(Return.Value(renderTarget));
            Stub.On(renderTarget).Method("GetTexture").Will(Return.Value(texture2D));
        }

        [TearDown]
        public void TearDown()
        {
            mockery.VerifyAllExpectationsHaveBeenMet();
        }

        [Test]
        public void Initialize()
        {
            Expect.Once.On(textureFactory).Method("CreateFullsizeRenderTarget").With(SurfaceFormat.Color, MultiSampleType.None, 0).Will(Return.Value(renderTarget));
            Expect.Once.On(textureFactory).Method("CreateFullsizeDepthStencil").With(presentationParameters.AutoDepthStencilFormat, MultiSampleType.None).Will(Return.Value(depthStencilBuffer));
            Expect.Once.On(graphicsFactory).Method("CreateSpriteBatch").Will(Return.Value(spriteBatch));
            Expect.Once.On(postProcessor).Method("Initialize").With(graphicsFactory, textureFactory, effectFactory);
            Expect.Once.On(baseFsa).Method("Initialize").With(graphicsFactory);

            executer.Initialize(gameCallback, baseFsa, graphicsFactory, inputDriver, textureFactory, effectFactory, postProcessor);
        }

        [Test]
        public void StepNoBackPressed()
        {
            Initialize();
            Expect.Once.On(inputDriver).Method("Step");
            Expect.Once.On(baseFsa).Method("Step");
            Expect.Once.On(inputDriver).Method("BackPressed").Will(Return.Value(false));
            executer.Step();
        }

        [Test]
        public void StepBackPressed()
        {
            Initialize();
            Expect.Once.On(inputDriver).Method("Step");
            Expect.Once.On(baseFsa).Method("Step");
            Expect.Once.On(inputDriver).Method("BackPressed").Will(Return.Value(true));
            Expect.Once.On(gameCallback).Method("Exit");
            executer.Step();
        }

        [Test]
        public void StepRender()
        {
            Initialize();
            Expect.Once.On(graphicsDevice).Method("SetRenderTarget").With(0, renderTarget);
            Expect.Once.On(graphicsDevice).SetProperty("DepthStencilBuffer").To(depthStencilBuffer);
            Expect.Once.On(graphicsDevice).Method("Clear").With(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            Expect.Once.On(baseFsa).Method("Render");
            Expect.Once.On(graphicsDevice).Method("SetRenderTarget").With(0, null);
            Expect.Once.On(spriteBatch).Method("Begin");
            Expect.Once.On(spriteBatch).Method("Draw");
            Expect.Once.On(spriteBatch).Method("End");

            executer.Render();
        }

    }
}
