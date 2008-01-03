using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.ModelBuilder
{
    public class TubePrimitive : ModifierBase
    {
        private float innerRadius;
        private float outerRadius;
        private float height;
        private int segments;
        private int heightSegments;

        public float InnerRadius
        {
            get { return innerRadius; }
            set { innerRadius = value; }
        }

        public float OuterRadius
        {
            get { return outerRadius; }
            set { outerRadius = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Segments
        {
            get { return segments; }
            set { segments = value; }
        }

        public int HeightSegments
        {
            get { return heightSegments; }
            set { heightSegments = value; }
        }

        public TubePrimitive()
            : base(0)
        {
        }

        public override IPrimitive Generate()
        {
            CheckArguments();

            Vertex[] vertices = new Vertex[GetNumVertices()];
            short[] indices = new short[GetNumIndices()];

            CopyOuterCylinder(vertices, indices);
            CopyInnerCylinder(vertices, indices);
            FillIndexGaps(indices);

            Primitive primitive = new Primitive(vertices, indices);
            return primitive;
        }

        private void FillIndexGaps(short[] indices)
        {
            int startIndex = 6 * segments * heightSegments * 2;
            int verticesBetweenCylinders = GetNumVertices() / 2;
            int verticesBetweenLids = (segments + 1) * heightSegments;
            for (int i = 0; i < segments; i++)
            {
                indices[startIndex++] = (short)(i + verticesBetweenCylinders);
                indices[startIndex++] = (short)(i);
                indices[startIndex++] = (short)(i + 1);
                indices[startIndex++] = (short)(i + verticesBetweenCylinders);
                indices[startIndex++] = (short)(i + 1);
                indices[startIndex++] = (short)(i + verticesBetweenCylinders + 1);
            }

            for (int i = 0; i < segments; i++)
            {
                indices[startIndex++] = (short)(i + verticesBetweenCylinders + verticesBetweenLids);
                indices[startIndex++] = (short)(i + 1 + verticesBetweenLids);
                indices[startIndex++] = (short)(i + verticesBetweenLids);
                indices[startIndex++] = (short)(i + verticesBetweenCylinders + verticesBetweenLids);
                indices[startIndex++] = (short)(i + verticesBetweenCylinders + 1 + verticesBetweenLids);
                indices[startIndex++] = (short)(i + 1 + verticesBetweenLids);
            }
        }

        private void CopyInnerCylinder(Vertex[] vertices, short[] indices)
        {
            IPrimitive innerPrimitive = GenerateCylinder(innerRadius);
            Array.Copy(innerPrimitive.Vertices, 1, vertices, vertices.Length / 2, innerPrimitive.Vertices.Length - 2);
            Array.Copy(innerPrimitive.Indices, segments * 3, indices, 6 * segments * heightSegments, 6 * segments * heightSegments);
            int startIndex = 6 * segments * heightSegments;
            for (int i = 0; i < 6 * segments * heightSegments / 3; i++)
            {
                short i1 = (short)(indices[startIndex + 0] + GetNumVertices() / 2 - 1);
                short i2 = (short)(indices[startIndex + 1] + GetNumVertices() / 2 - 1);
                short i3 = (short)(indices[startIndex + 2] + GetNumVertices() / 2 - 1);
                indices[startIndex++] = i1;
                indices[startIndex++] = i3;
                indices[startIndex++] = i2;
            }
        }

        private void CopyOuterCylinder(Vertex[] vertices, short[] indices)
        {
            IPrimitive outerPrimitive = GenerateCylinder(outerRadius);
            Array.Copy(outerPrimitive.Vertices, 1, vertices, 0, outerPrimitive.Vertices.Length - 2);
            Array.Copy(outerPrimitive.Indices, segments * 3, indices, 0, 6 * segments * heightSegments);
            for (int i = 0; i < 6 * segments * heightSegments; i++)
            {
                indices[i]--;
            }
        }

        private int GetNumIndices()
        {
            return ((6 * segments) + (6 * heightSegments * segments)) * 2;
        }

        private int GetNumVertices()
        {
            return (segments + 1) * (heightSegments + 1) * 2;
        }

        private void CheckArguments()
        {
            if (outerRadius < innerRadius)
                throw new ArgumentException("OuterRadius may not be smaller or equal to InnerRadius.");
            if (segments < 3)
                throw new ArgumentOutOfRangeException("segments", "Should be larger than or equal to three.");
            if (heightSegments < 1)
                throw new ArgumentOutOfRangeException("heightSegments", "Should be larger than or equal to one.");
        }

        private IPrimitive GenerateCylinder(float radius)
        {
            CylinderPrimitive cylinder = new CylinderPrimitive();
            cylinder.Height = height;
            cylinder.HeightSegments = heightSegments;
            cylinder.Radius = radius;
            cylinder.Segments = segments;
            return cylinder.Generate();
        }
    }
}
