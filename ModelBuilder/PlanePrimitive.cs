using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class PlanePrimitive : ModifierBase
    {
        private float width;
        private float height;
        private int widthSegments;
        private int heightSegments;
        private bool textured;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public int WidthSegments
        {
            get { return widthSegments; }
            set { widthSegments = value; }
        }

        public int HeightSegments
        {
            get { return heightSegments; }
            set { heightSegments = value; }
        }

        public bool Textured
        {
            get { return textured; }
            set { textured = value; }
        }

        public PlanePrimitive()
            : base(0)
        {
        }

        public override IPrimitive Generate()
        {
            short v = 0;
            Vertex[] vertices = new Vertex[(widthSegments + 1) * (heightSegments + 1)];
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = new Vertex();
            short[] indices = new short[widthSegments * heightSegments * 6];
            Primitive primitive = new Primitive(vertices, indices);

            BoxPrimitive.BoxAddIndicesForSide(0, 0, indices, widthSegments, heightSegments);
            for (int y = 0; y < heightSegments + 1; y++)
            {
                float yPos = height * (0.5f - y / (float)heightSegments);
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    float xPos = width * (-0.5f + x / (float)widthSegments);
                    vertices[v].Normal = new Vector3(0, 0, 1);
                    vertices[v].Position = new Vector3(xPos, yPos, 0);
                    if (textured)
                    {
                        vertices[v].U = x / (float)widthSegments;
                        vertices[v].V = y / (float)heightSegments;
                    }
                    v++;
                }
            }
            return primitive;
        }

    }
}
