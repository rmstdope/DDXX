using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.DemoFramework
{
    public class LineControl : BaseControl
    {
        public float Alpha;
        public Color Color;

        public LineControl(RectangleF rectangle, float alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            Alpha = alpha;
            Color = color;
        }
        
        public override void Draw(ISprite sprite, ILine line, IFont font, ITexture whiteTexture)
        {
            line.Begin();
            line.Draw(new Vector2[] { new Vector2(GetX1() + 1, GetY1() + 1), 
                                      new Vector2(GetX2() + 1, GetY2() + 1) },
                                      Color.FromArgb((int)(255 * Alpha), Color.Black));
            line.Draw(new Vector2[] { new Vector2(GetX1(), GetY1()), 
                                      new Vector2(GetX2(), GetY2()) },
                                      Color.FromArgb((int)(255 * Alpha), Color));
            line.End();
        }
    }
}
