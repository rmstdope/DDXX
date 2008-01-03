using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest
{
    public class Simple2DEffect : BaseDemoEffect
    {
        private ISpriteBatch spriteBatch;
        private ITexture2D texture;

        public Simple2DEffect(string name, float startTime, float endTime)
            : base(name, startTime, endTime)
        {
        }

        protected override void Initialize()
        {
            texture = GraphicsFactory.Texture2DFromFile("Content\\textures\\Noise2");
            spriteBatch = GraphicsFactory.CreateSpriteBatch();
        }

        public override void Step()
        {
        }

        public override void Render()
        {
            spriteBatch.Begin(SpriteBlendMode.None, SpriteSortMode.Immediate, SaveStateMode.None);
            spriteBatch.Draw(texture, new Vector2(), Color.White);
            spriteBatch.End();
        }
    }
}
