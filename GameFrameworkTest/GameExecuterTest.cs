using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using NMock2;
using Microsoft.Xna.Framework.Graphics;

namespace GameFramework
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

        [SetUp]
        public void SetUp()
        {
            executer = new GameExecuter();
            mockery = new Mockery();
            textureFactory = mockery.NewMock<ITextureFactory>();
            depthStencilBuffer = mockery.NewMock<IDepthStencilBuffer>();
            graphicsFactory = mockery.NewMock<IGraphicsFactory>();
            graphicsDevice = mockery.NewMock<IGraphicsDevice>();
            effectFactory = mockery.NewMock<IEffectFactory>();
            postProcessor = mockery.NewMock<IPostProcessor>();
            inputDriver = mockery.NewMock<IInputDriver>();
            gameCallback = mockery.NewMock<IGameCallback>();
            presentationParameters = new PresentationParameters();
            presentationParameters.AutoDepthStencilFormat = DepthFormat.Depth32;
            Stub.On(graphicsFactory).GetProperty("GraphicsDevice").Will(Return.Value(graphicsDevice));
            Stub.On(graphicsDevice).GetProperty("PresentationParameters").Will(Return.Value(presentationParameters));
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

            executer.Initialize(gameCallback, graphicsFactory, inputDriver, textureFactory, effectFactory, postProcessor);
        }

        [Test]
        public void StepNoBackPressed()
        {
            Initialize();
            Expect.Once.On(inputDriver).Method("Step");
            Expect.Once.On(inputDriver).Method("BackPressed").Will(Return.Value(false));
            executer.Step();
        }

        [Test]
        public void StepBackPressed()
        {
            Initialize();
            Expect.Once.On(inputDriver).Method("Step");
            Expect.Once.On(inputDriver).Method("BackPressed").Will(Return.Value(true));
            Expect.Once.On(gameCallback).Method("Exit");
            executer.Step();
        }

    }
}
