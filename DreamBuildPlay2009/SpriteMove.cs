using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dope.DDXX.GameFramework;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DreamBuildPlay2009
{
    public class SpriteMove : IFsa
    {
        private int num;
        private List<ITexture2D> textures;
        private ISpriteBatch spriteBatch;

        public SpriteMove()
        {
            textures = new List<ITexture2D>();
        }

        public void Initialize(IGraphicsFactory graphicsFactory)
        {
            spriteBatch = graphicsFactory.CreateSpriteBatch();
            for (int i = 0; i < 47; i++)
                textures.Add(graphicsFactory.Texture2DFromFile("Content\\textures\\Rob_run_00" + (i + 1).ToString("00")));
        }

        public void Step()
        {
            if (++num >= textures.Count)
                num = 0;
        }

        public void Render()
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textures[num], Vector2.Zero, Color.White);
            spriteBatch.End();
        }

    }
}
