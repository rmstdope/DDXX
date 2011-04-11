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
        private IControl parent;
        public List<IControl> Children = new List<IControl>();

        public Vector4 Rectangle
        {
            get
            {
                return rectangle;
            }
        }

        public BaseControl(Vector4 rectangle, IControl parent)
        {
            this.rectangle = rectangle;
            this.parent = parent;
            if (parent != null)
                (parent as BaseControl).Children.Add(this);
        }

        public int DrawControl(IDrawResources resources)
        {
            int num = 1;
            Draw(resources);
            foreach (IControl child in Children)
                num += child.DrawControl(resources);
            return num;
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

        public float GetHeight(IDrawResources resources)
        {
            if (rectangle.W == -1)
                return GetParentWidth(resources) * rectangle.Z * resources.AspectRatio;
            else
                return GetParentHeight(resources) * rectangle.W;
        }

        public float GetWidth(IDrawResources resources)
        {
            if (rectangle.Z == -1)
                return GetParentHeight(resources) * rectangle.W / resources.AspectRatio;
            else
                return GetParentWidth(resources) * rectangle.Z;
        }

        public float GetX1(IDrawResources resources)
        {
            if (rectangle.X == -1)
                return GetParentX1(resources) + GetParentWidth(resources) / 2 - GetWidth(resources) / 2;
            return GetParentX1(resources) + GetParentWidth(resources) * rectangle.X;
        }

        public float GetY1(IDrawResources resources)
        {
            if (rectangle.Y == -1)
                return GetParentY1(resources) + GetParentHeight(resources) / 2 - GetHeight(resources) / 2;
            return GetParentY1(resources) + GetParentHeight(resources) * rectangle.Y;
        }

        protected void DrawVerticalLine(SpriteBatch spriteBatch, Texture2D whiteTexture, int x, int y, int height, Color color)
        {
            spriteBatch.Draw(whiteTexture, new Rectangle(x, y, 1, height), color);
        }

        protected void DrawHorizontalLine(SpriteBatch spriteBatch, Texture2D whiteTexture, int x, int y, int width, Color color)
        {
            spriteBatch.Draw(whiteTexture, new Rectangle(x, y, width, 1), color);
        }

        public void RemoveChildren()
        {
            Children = new List<IControl>();
        }

        public void RemoveFromParent()
        {
            if (parent != null)
                parent.RemoveChild(this);
        }

        public void RemoveChild(IControl control)
        {
            Children.Remove(control);
        }


        public void AddChild(IControl control)
        {
            Children.Add(control);
            (control as BaseControl).parent = this;
        }
    }
}
