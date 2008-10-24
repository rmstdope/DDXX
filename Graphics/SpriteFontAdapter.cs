using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.Graphics
{
    public class SpriteFontAdapter : ISpriteFont
    {
        private SpriteFont spriteFont;

        public SpriteFontAdapter(SpriteFont spriteFont)
        {
            this.spriteFont = spriteFont;
        }

        public SpriteFont DxSpriteFont { get { return spriteFont; } }

        #region ISpriteFont Members

        public int LineSpacing
        {
            get { return spriteFont.LineSpacing; }
        }

        public float Spacing
        {
            get { return spriteFont.Spacing; }
            set { spriteFont.Spacing = value; }
        }

        public Vector2 MeasureString(string text)
        {
            return spriteFont.MeasureString(text);
        }

        #endregion
    }
}
