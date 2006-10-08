using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseControl
    {
        protected RectangleF rectangle;
        private BaseControl parent;
        public List<BaseControl> Children = new List<BaseControl>();
        public BaseControl(RectangleF rectangle, BaseControl parent)
        {
            this.rectangle = rectangle;
            this.parent = parent;
            if (parent != null)
                parent.Children.Add(this);
        }

        internal void DrawControl(ISprite sprite, ILine line, IFont font, ITexture whiteTexture)
        {
            Draw(sprite, line, font, whiteTexture);
            foreach (BaseControl child in Children)
                child.DrawControl(sprite, line, font, whiteTexture);
        }

        internal abstract void Draw(ISprite sprite, ILine line, IFont font, ITexture whiteTexture);

        protected float GetParentHeight()
        {
            if (parent == null)
                return D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferHeight;
            else
                return parent.GetHeight();
        }

        protected float GetParentWidth()
        {
            if (parent == null)
                return D3DDriver.GetInstance().GetDevice().PresentationParameters.BackBufferWidth;
            else
                return parent.GetWidth();
        }

        protected float GetParentX1()
        {
            if (parent == null)
                return 0;
            else
                return parent.GetX1();
        }

        protected float GetParentY1()
        {
            if (parent == null)
                return 0;
            else
                return parent.GetY1();
        }

        protected float GetHeight()
        {
            return GetParentHeight() * rectangle.Height;
        }

        protected float GetWidth()
        {
            return GetParentWidth() * rectangle.Width;
        }

        protected float GetX1()
        {
            return GetParentX1() + GetParentWidth() * rectangle.X;
        }

        protected float GetY1()
        {
            return GetParentY1() + GetParentHeight() * rectangle.Y;
        }

        protected float GetX2()
        {
            return GetParentX1() + GetParentWidth() * (rectangle.X + rectangle.Width);
        }

        protected float GetY2()
        {
            return GetParentY1() + GetParentHeight() * (rectangle.Y + rectangle.Height);
        }
    }

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
        internal override void Draw(ISprite sprite, ILine line, IFont font, ITexture whiteTexture)
        {
            sprite.Begin(SpriteFlags.AlphaBlend);
            sprite.Draw2D(whiteTexture, Rectangle.Empty, new SizeF(GetWidth(), GetHeight()), 
                          new PointF(GetX1(), GetY1()), Color.FromArgb((int)(255 * Alpha), Color));
            sprite.End();

            line.Begin();
            line.Draw(new Vector2[] { new Vector2(GetX1(), GetY1()), 
                                      new Vector2(GetX2(), GetY1()), 
                                      new Vector2(GetX2(), GetY2()), 
                                      new Vector2(GetX1(), GetY2()), 
                                      new Vector2(GetX1(), GetY1()) },
                                      Color.White);
            line.End();
        }

    }

    public class TextControl : BaseControl
    {
        public string Text;
        public DrawTextFormat Format;
        public float Alpha;
        public Color Color;
        public TextControl(string text, RectangleF rectangle, DrawTextFormat format, float alpha, Color color, BaseControl parent)
            : base(rectangle, parent)
        {
            Text = text;
            Format = format;
            Alpha = alpha;
            Color = color;
        }
        internal override void Draw(ISprite sprite, ILine line, IFont font, ITexture whiteTexture)
        {
            Rectangle rect = new Rectangle((int)(GetX1()), (int)(GetY1()), (int)(GetWidth()), (int)(GetHeight()));
            font.DrawText(null, Text, rect, Format, Color.FromArgb((int)(255 * Alpha), Color));
        }
    }

    public class UserInterface
    {
        private ILine line;
        private ISprite sprite;
        private ITexture whiteTexture;
        private IFont font;

        internal UserInterface()
        {
        }

        internal void Initialize()
        {
            IDevice device = D3DDriver.GetInstance().GetDevice();

            line = D3DDriver.Factory.CreateLine(device);
            line.Width = 1.0f;
            line.Antialias = true;
            
            sprite = D3DDriver.Factory.CreateSprite(device);

            font = D3DDriver.Factory.CreateFont(device, 14, 0, FontWeight.Normal, 1, false, CharacterSet.Default, Precision.Default, FontQuality.Default, PitchAndFamily.DefaultPitch | PitchAndFamily.FamilyDoNotCare, "Courier");
            
            whiteTexture = D3DDriver.Factory.CreateTexture(device, 1, 1, 1, Usage.RenderTarget, Format.X8R8G8B8, Pool.Default);
            device.ColorFill(whiteTexture.GetSurfaceLevel(0), new Rectangle(0, 0, 1, 1), Color.White);
        }

        internal void DrawControl(BaseControl control)
        {
            control.DrawControl(sprite, line, font, whiteTexture);
        }

    }
}
