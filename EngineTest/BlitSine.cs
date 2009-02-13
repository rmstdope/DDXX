using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;

namespace EngineTest
{
    public class BlitSine
    {
        private Vector2 offset;
        private Vector2 period;
        private float size;
        private Vector2 scale;

        public BlitSine()
        {
            offset = new Vector2(Rand.Float(0, 2 * Math.PI), Rand.Float(0, 2 * Math.PI));
            period = new Vector2(Rand.Float(0.5f, 3.5f), Rand.Float(0.5f, 3.5f));
            size = Rand.Int(100, 180);
            scale = new Vector2(Rand.Float(30, 90), Rand.Float(30, 90));
        }

        public void Draw(ISpriteBatch spriteBatch, ITexture2D texture)
        {
            float x = offset.X + Time.CurrentTime / period.X;
            float y = offset.Y + Time.CurrentTime / period.Y;
            Vector2 center = new Vector2(128, 128);
            Vector2 distortion =
                new Vector2(scale.X * (float)Math.Sin(x),
                            scale.Y * (float)Math.Cos(y));
            Vector2 s = new Vector2(size, size);
            Vector2 point = center - s * 0.5f + distortion;
            spriteBatch.Draw(texture, new Rectangle((int)point.X, (int)point.Y, (int)s.X, (int)s.Y), Color.White);
        }
    }
}
