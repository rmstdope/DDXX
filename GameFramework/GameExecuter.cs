using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.ModelBuilder;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.GameFramework
{
    public class GameExecuter : IGameExecuter
    {
        private IGameCallback game;
        private IFsa fsa;
        private IGraphicsFactory graphicsFactory;
        private IInputDriver inputDriver;
        private TextureFactory textureFactory;
        private EffectFactory effectFactory;
        private RenderTarget2D renderTarget;
        private PostProcessor postProcessor;
        private SpriteBatch spriteBatch;

        public GameExecuter()
        {
        }

        public void Initialize(IGameCallback game, IFsa startFsa, IGraphicsFactory graphicsFactory, IInputDriver inputDriver, TextureFactory textureFactory, EffectFactory effectFactory, PostProcessor postProcessor)
        {
            this.game = game;
            this.fsa = startFsa;
            this.graphicsFactory = graphicsFactory;
            this.textureFactory = textureFactory;
            this.inputDriver = inputDriver;
            this.effectFactory = effectFactory;
            this.postProcessor = postProcessor;
            
            renderTarget = textureFactory.CreateFullsizeRenderTarget(SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0);
            spriteBatch = new SpriteBatch(graphicsFactory.GraphicsDevice);
            postProcessor.Initialize(graphicsFactory);

            fsa.Initialize(graphicsFactory);
        }

        public void Step()
        {
            inputDriver.Step();
            fsa.Step();
            if (inputDriver.BackPressed())
                game.Exit();
        }

        public void Render()
        {
            // Render to render target
            graphicsFactory.GraphicsDevice.SetRenderTarget(renderTarget);
            graphicsFactory.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
            graphicsFactory.GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1.0f, 0);
            //NodeFactory.Instance.GetScene().Render();
            fsa.Render();

            // Post processing
            //postProcessor.StartFrame(renderTarget);

            //RenderTarget2D startTexture = postProcessor.OutputTexture;
            //List<RenderTarget2D> textures = postProcessor.GetTemporaryTextures(2, true);
            //postProcessor.SetValue("Luminance", 0.3f);
            //postProcessor.SetValue("Exposure", 0.12f);
            //postProcessor.SetValue("WhiteCutoff", 0.12f);
            //postProcessor.SetValue("BloomScale", 1.5f);
            //postProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.Zero, Color.Black);
            //postProcessor.Process("DownSample4x", startTexture, textures[1]);
            ////postProcessor.Process("DownSample4x", textures[0], textures[1]);
            //postProcessor.Process("AdvancedBrighten", textures[1], textures[0]);
            //postProcessor.Process("HorizontalBloom", textures[0], textures[1]);
            //postProcessor.Process("VerticalBloom", textures[1], textures[0]);
            //postProcessor.Process("HorizontalBloom", textures[0], textures[1]);
            //postProcessor.Process("VerticalBloom", textures[1], textures[0]);
            ////postProcessor.Process("UpSample4x", textures[1], textures[0]);
            //postProcessor.SetBlendParameters(BlendFunction.Add, Blend.One, Blend.One, Color.Black);
            //postProcessor.Process("UpSample4x", textures[0], startTexture);

            // User Interface
            //graphicsFactory.GraphicsDevice.SetRenderTarget(0, postProcessor.OutputTexture);
            //userInterface.DrawControl(rootControl);

            // Render to backbuffer
            graphicsFactory.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            spriteBatch.Draw(postProcessor.OutputTexture, new Rectangle(0, 0, graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferWidth,
                graphicsFactory.GraphicsDevice.PresentationParameters.BackBufferHeight), Color.White);
            spriteBatch.End();
        }
    }
}
