using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Dope.DDXX.Graphics;

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
                return D3DDriver.GetInstance().Device.PresentationParameters.BackBufferHeight;
            else
                return parent.GetHeight();
        }

        protected float GetParentWidth()
        {
            if (parent == null)
                return D3DDriver.GetInstance().Device.PresentationParameters.BackBufferWidth;
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
}
