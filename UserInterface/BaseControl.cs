using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.UserInterface
{
    public abstract class BaseControl : IControl
    {
        protected Vector4 rectangle;
        private BaseControl parent;
        public List<BaseControl> Children = new List<BaseControl>();

        public Vector4 Rectangle
        {
            get
            {
                return rectangle;
            }
        }

        public BaseControl(Vector4 rectangle, BaseControl parent)
        {
            this.rectangle = rectangle;
            this.parent = parent;
            if (parent != null)
                parent.Children.Add(this);
        }

        public void DrawControl(IDrawResources resources)
        {
            Draw(resources);
            foreach (BaseControl child in Children)
                child.DrawControl(resources);
        }

        public abstract void Draw(IDrawResources resources);

        protected float GetParentHeight(IDrawResources resources)
        {
            if (parent == null)
                return 1;
            else
                return parent.GetHeight(resources);
        }

        protected float GetParentWidth(IDrawResources resources)
        {
            if (parent == null)
                return 1;
            else
                return parent.GetWidth(resources);
        }

        protected float GetParentX1(IDrawResources resources)
        {
            if (parent == null)
                return 0;
            else
                return parent.GetX1(resources);
        }

        protected float GetParentY1(IDrawResources resources)
        {
            if (parent == null)
                return 0;
            else
                return parent.GetY1(resources);
        }

        protected float GetHeight(IDrawResources resources)
        {
            if (rectangle.W == -1)
                return GetParentWidth(resources) * rectangle.Z * resources.AspectRatio;
            else
                return GetParentHeight(resources) * rectangle.W;
        }

        protected float GetWidth(IDrawResources resources)
        {
            if (rectangle.Z == -1)
                return GetParentHeight(resources) * rectangle.W / resources.AspectRatio;
            else
                return GetParentWidth(resources) * rectangle.Z;
        }

        protected float GetX1(IDrawResources resources)
        {
            return GetParentX1(resources) + GetParentWidth(resources) * rectangle.X;
        }

        protected float GetY1(IDrawResources resources)
        {
            return GetParentY1(resources) + GetParentHeight(resources) * rectangle.Y;
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

        public void RemoveFromParent()
        {
            parent.RemoveChild(this);
        }

        private void RemoveChild(BaseControl control)
        {
            Children.Remove(control);
        }


        public void AddChild(BaseControl control)
        {
            Children.Add(control);
        }
    }
}
