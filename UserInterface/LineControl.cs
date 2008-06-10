using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace Dope.DDXX.UserInterface
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

        public override void Draw(IDrawResources resources)
        {
            resources.SpriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            int screenWidth = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferWidth;
            int screenHeight = resources.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferHeight;
            int x1 = (int)(screenWidth * GetX1(resources));
            int y1 = (int)(screenHeight * GetY1(resources));
            int width = (int)(screenWidth * GetWidth(resources));
            int height = (int)(screenHeight * GetHeight(resources));
            if (vertical)
                DrawVerticalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1, height, Color);
            else
                DrawHorizontalLine(resources.SpriteBatch, resources.WhiteTexture, x1, y1, width, Color);
            resources.SpriteBatch.End();

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
