using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.DemoFramework
{
    public class LineControl : BaseControl
    {
        public Color Color;
        private bool vertical;

        public LineControl(Vector4 rectangle, byte alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            Color = new Color(color.R, color.G, color.B, alpha);
            if (rectangle.Z == 0)
                vertical = true;
            else if (rectangle.W == 0)
                vertical = false;
            else
                throw new DDXXException("Lines must be either vertical or horizontal.");
        }

        public override void Draw(ISpriteBatch spriteBatch, ISpriteFont spriteFont, ITexture2D whiteTexture)
        {
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            int screenWidth = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = spriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int x1 = (int)(screenWidth * GetX1());
            int y1 = (int)(screenHeight * GetY1());
            int width = (int)(screenWidth * GetWidth());
            int height = (int)(screenHeight * GetHeight());
            if (vertical)
                DrawVerticalLine(spriteBatch, whiteTexture, x1, y1, height, Color);
            else
                DrawHorizontalLine(spriteBatch, whiteTexture, x1, y1, width, Color);
            spriteBatch.End();

            //line.Begin();
            //line.Draw(new Vector2[] { new Vector2(GetX1() + 1, GetY1() + 1), 
            //                          new Vector2(GetX2() + 1, GetY2() + 1) },
            //                          Color.FromArgb((int)(255 * Alpha), Color.Black));
            //line.Draw(new Vector2[] { new Vector2(GetX1(), GetY1()), 
            //                          new Vector2(GetX2(), GetY2()) },
            //                          Color.FromArgb((int)(255 * Alpha), Color));
            //line.End();
        }
    }
}
