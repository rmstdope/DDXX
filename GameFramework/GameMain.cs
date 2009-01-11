using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;
using Dope.DDXX.Input;
using Dope.DDXX.Utility;

namespace GameFramework
{
    public class GameMain : Game, IGameCallback
    {
        private IGameExecuter gameExecuter;
        private IGraphicsFactory graphicsFactory;

        public GameMain(IGameExecuter gameExecuter)
        {
            this.gameExecuter = gameExecuter;
            this.graphicsFactory = new GraphicsFactory(this, Services);
        }

        protected override void Initialize()
        {
            ITextureFactory textureFactory = new TextureFactory(graphicsFactory);
            IInputDriver inputDriver = InputDriver.GetInstance();
            IEffectFactory effectFactory = new EffectFactory(graphicsFactory);
            IPostProcessor postProcessor = new PostProcessor();

            gameExecuter.Initialize(this, graphicsFactory, inputDriver, textureFactory, effectFactory, postProcessor);
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
