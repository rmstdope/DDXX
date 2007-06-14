using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.DemoFramework
{
    public class BoxControl : BaseControl
    {
        public float Alpha;
        public Color Color;

        public BoxControl(RectangleF rectangle, float alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            Alpha = alpha;
            Color = color;
        }
        
        public override void Draw(ISprite sprite, ILine line, IFont font, ITexture whiteTexture)
        {
            if (Alpha == 0.0f)
                return;

            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(whiteTexture, Rectangle.Empty, new SizeF(GetWidth(), GetHeight()),
                          new PointF(GetX1(), GetY1()), Color.FromArgb((int)(255 * Alpha), Color));
            sprite.End();

            line.Begin();
            line.Draw(new Vector2[] { new Vector2(GetX1() + 1, GetY1() + 1), 
                                      new Vector2(GetX2() + 1, GetY1() + 1), 
                                      new Vector2(GetX2() + 1, GetY2() + 1), 
                                      new Vector2(GetX1() + 1, GetY2() + 1), 
                                      new Vector2(GetX1() + 1, GetY1() + 1) },
                                      Color.FromArgb((int)(255 * Alpha), Color.Black));
            line.Draw(new Vector2[] { new Vector2(GetX1(), GetY1()), 
                                      new Vector2(GetX2(), GetY1()), 
                                      new Vector2(GetX2(), GetY2()), 
                                      new Vector2(GetX1(), GetY2()), 
                                      new Vector2(GetX1(), GetY1()) },
                                      Color.FromArgb((int)(255 * Alpha), Color.White));
            line.End();
        }

    }
}
