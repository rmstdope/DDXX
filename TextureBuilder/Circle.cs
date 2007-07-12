using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.TextureBuilder
{
    public class Circle : Generator
    {
        private float innerRadius;
        private float outerRadius;

        public float InnerRadius
        {
            get { return innerRadius; }
            set 
            {
                if (value > outerRadius)
                    outerRadius = value;
                innerRadius = value; 
            }
        }

        public float OuterRadius
        {
            get { return outerRadius; }
            set
            {
                if (value < innerRadius) 
                    innerRadius = value;
                outerRadius = value; 
            }
        }

        public Circle()
            : base(0)
        {
            innerRadius = 0.5f;
            outerRadius = 0.5f;
        }

        public override Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            Vector2 recenteredCoordinate = textureCoordinate - new Vector2(0.5f, 0.5f);
            if (recenteredCoordinate.Length() <= innerRadius)
                return new Vector4(1, 1, 1, 1);
            if (recenteredCoordinate.Length() <= outerRadius)
            {
                float v = 1 - (recenteredCoordinate.Length() - innerRadius) / (outerRadius - innerRadius);
                return new Vector4(v, v, v, v);
            }
            return new Vector4();
        }
    }
}
