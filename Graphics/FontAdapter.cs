using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using System.Drawing;

namespace Dope.DDXX.Graphics
{
    public class FontAdapter : IFont
    {
        private Microsoft.DirectX.Direct3D.Font font;

        public FontAdapter(Microsoft.DirectX.Direct3D.Font font)
        {
            this.font = font;
        }

        #region IFont Members

        public FontDescription Description
        {
            get { return font.Description; }
        }

        public int DrawText(ISprite sprite, string text, Point pos, Color color)
        {
            return font.DrawText(((SpriteAdapter)sprite).DXSprite, text, pos, color);
        }

        public int DrawText(ISprite sprite, string text, Point pos, int color)
        {
            return font.DrawText(((SpriteAdapter)sprite).DXSprite, text, pos, color);
        }

        public int DrawText(ISprite sprite, string text, int x, int y, Color color)
        {
            if (sprite == null)
                return font.DrawText(null, text, x, y, color);
            else
                return font.DrawText(((SpriteAdapter)sprite).DXSprite, text, x, y, color);
        }

        public int DrawText(ISprite sprite, string text, int x, int y, int color)
        {
            if (sprite == null)
                return font.DrawText(null, text, x, y, color);
            else
                return font.DrawText(((SpriteAdapter)sprite).DXSprite, text, x, y, color);
        }

        public int DrawText(ISprite sprite, string text, Rectangle rect, DrawTextFormat format, Color color)
        {
            if (sprite == null)
                return font.DrawText(null, text, rect, format, color);
            else
                return font.DrawText(((SpriteAdapter)sprite).DXSprite, text, rect, format, color);
        }

        public int DrawText(ISprite sprite, string text, Rectangle rect, DrawTextFormat format, int color)
        {
            return font.DrawText(((SpriteAdapter)sprite).DXSprite, text, rect, format, color);
        }

        public Texture GetGlyphData(int glyph)
        {
            return font.GetGlyphData(glyph);
        }

        public Texture GetGlyphData(int glyph, out Rectangle blackBox, out Point cellInc)
        {
            return font.GetGlyphData(glyph, out blackBox, out cellInc);
        }

        public Rectangle MeasureString(ISprite sprite, string text, DrawTextFormat format, Color color)
        {
            return font.MeasureString(((SpriteAdapter)sprite).DXSprite, text, format, color);
        }

        public Rectangle MeasureString(ISprite sprite, string text, DrawTextFormat format, int color)
        {
            return font.MeasureString(((SpriteAdapter)sprite).DXSprite, text, format, color);
        }

        public void PreloadCharacters(int first, int last)
        {
            font.PreloadCharacters(first, last);
        }

        public void PreloadGlyphs(int first, int last)
        {
            font.PreloadGlyphs(first, last);
        }

        public void PreloadText(string text)
        {
            font.PreloadText(text);
        }

        #endregion
    }
}
