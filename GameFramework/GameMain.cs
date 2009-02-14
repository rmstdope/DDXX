using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;

namespace Dope.DDXX.GameFramework
{
    public class GameMain : Game, IGameCallback
    {
        private IGameExecuter gameExecuter;
        private IFsa startFsa;
        private IGraphicsFactory graphicsFactory;

        public GameMain(IGameExecuter gameExecuter, IFsa startFsa)
        {
            this.gameExecuter = gameExecuter;
            this.startFsa = startFsa;
            this.graphicsFactory = new GraphicsFactory(this, Services);
        }

        protected override void Initialize()
        {
            ITextureFactory textureFactory = new TextureFactory(graphicsFactory);
            IInputDriver inputDriver = InputDriver.GetInstance();
            IEffectFactory effectFactory = new EffectFactory(graphicsFactory);
            IPostProcessor postProcessor = new PostProcessor();

            gameExecuter.Initialize(this, startFsa, graphicsFactory, inputDriver, textureFactory, effectFactory, postProcessor);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            Time.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            gameExecuter.Step();
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            gameExecuter.Render();
        }

    }
}
