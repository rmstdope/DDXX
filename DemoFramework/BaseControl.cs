using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.DemoFramework
{
    public abstract class BaseControl
    {
        protected Vector4 rectangle;
        private BaseControl parent;
        public List<BaseControl> Children = new List<BaseControl>();

        public BaseControl(Vector4 rectangle, BaseControl parent)
        {
            this.rectangle = rectangle;
            this.parent = parent;
            if (parent != null)
                parent.Children.Add(this);
        }

        public void DrawControl(ISpriteBatch spriteBatch, ISpriteFont spriteFont, ITexture2D whiteTexture)
        {
            Draw(spriteBatch, spriteFont, whiteTexture);
            foreach (BaseControl child in Children)
                child.DrawControl(spriteBatch, spriteFont, whiteTexture);
        }

        public abstract void Draw(ISpriteBatch spriteBatch, ISpriteFont spriteFont, ITexture2D whiteTexture);

        protected float GetParentHeight()
        {
            if (parent == null)
                return 1;
            else
                return parent.GetHeight();
        }

        protected float GetParentWidth()
        {
            if (parent == null)
                return 1;
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
            return GetParentHeight() * rectangle.W;
        }

        protected float GetWidth()
        {
            return GetParentWidth() * rectangle.Z;
        }

        protected float GetX1()
        {
            return GetParentX1() + GetParentWidth() * rectangle.X;
        }

        protected float GetY1()
        {
            return GetParentY1() + GetParentHeight() * rectangle.Y;
        }

        protected void DrawVerticalLine(ISpriteBatch spriteBatch, ITexture2D whiteTexture, int x, int y, int height, Color color)
        {
            spriteBatch.Draw(whiteTexture, new Rectangle(x, y, 1, height), color);
        }

        protected void DrawHorizontalLine(ISpriteBatch spriteBatch, ITexture2D whiteTexture, int x, int y, int width, Color color)
        {
            spriteBatch.Draw(whiteTexture, new Rectangle(x, y, width, 1), color);
        }

        public void RemoveChildren()
        {
            Children = new List<BaseControl>();
        }
    }
}
