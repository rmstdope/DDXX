using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.Utility
{
    public struct RectangleF
    {
        public float Height;
        public float Width;
        public float X;
        public float Y;

        public RectangleF(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public float Bottom
        { 
            get 
            { 
                return Y + Height; 
            } 
        }

        public static RectangleF Empty
        {
            get
            {
                return new RectangleF(0, 0, 0, 0);
            }
        }

        public float Left
        {
            get
            {
                return X;
            }
        }

        public float Right
        {
            get
            {
                return X + Width;
            }
        }

        public float Top
        {
            get
            {
                return Y;
            }
        }

    }
}
