using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.Utility;

namespace Dope.DDXX.MeshBuilder
{
    public class UvMapPlane : IPrimitive
    {
        // 0=x, 1=y, 2=z
        private int alignToAxis;
        private IPrimitive input;
        private float tileU;
        private float tileV;

        // Temporary variables
        private float maxX;
        private float minX;
        private float maxY;
        private float minY;
        private float maxZ;
        private float minZ;

        public int AlignToAxis
        {
            get { return alignToAxis; }
            set 
            {
                if (value > 2 || value < 0)
                    throw new DDXXException("AlignToAxis must be withing range [0..2].");
                alignToAxis = value; 
            }
        }

        public IPrimitive Input
        {
            get { return input; }
            set { input = value; }
        }

        public float TileU
        {
            get { return tileU; }
            set { tileU = value; }
        }

        public float TileV
        {
            get { return tileV; }
            set { tileV = value; }
        }

        public UvMapPlane()
        {
            alignToAxis = 1;
            tileU = 1;
            tileV = 1;
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            if (input == null)
                throw new DDXXException("Input not set for UvMapPlane.");

            input.Generate(out vertices, out indices, out body);

            CalculateMinMax(vertices);
            for (int i = 0; i < vertices.Length; i++)
            {
                Vertex vertex;
                switch (alignToAxis)
                {
                    case 0:
                        vertex = vertices[i];
                        vertex.U = tileU * CalculateCoordinate(minY, maxY, vertices[i].Position.Y);
                        vertex.V = tileV * CalculateCoordinate(minZ, maxZ, vertices[i].Position.Z);
                        vertices[i] = vertex;
                        break;
                    case 1:
                        vertex = vertices[i];
                        vertex.U = tileU * CalculateCoordinate(minX, maxX, vertices[i].Position.X);
                        vertex.V = tileV * CalculateCoordinate(minZ, maxZ, vertices[i].Position.Z);
                        vertices[i] = vertex;
                        break;
                    case 2:
                        vertex = vertices[i];
                        vertex.U = tileU * CalculateCoordinate(minX, maxX, vertices[i].Position.X);
                        vertex.V = tileV * CalculateCoordinate(minY, maxY, vertices[i].Position.Y);
                        vertices[i] = vertex;
                        break;
                }
            }
        }

        private float CalculateCoordinate(float min, float max, float value)
        {
            if (max == min)
                return 0;
            else
                return (value - min) / (max - min);
        }

        private void CalculateMinMax(Vertex[] vertices)
        {
            maxX = vertices[0].Position.X;
            minX = vertices[0].Position.X;
            maxY = vertices[0].Position.Y;
            minY = vertices[0].Position.Y;
            maxZ = vertices[0].Position.Z;
            minZ = vertices[0].Position.Z;
            foreach (Vertex vertex in vertices)
            {
                minX = (float)Math.Min(vertex.Position.X, minX);
                maxX = (float)Math.Max(vertex.Position.X, maxX);
                minY = (float)Math.Min(vertex.Position.Y, minY);
                maxY = (float)Math.Max(vertex.Position.Y, maxY);
                minZ = (float)Math.Min(vertex.Position.Z, minZ);
                maxZ = (float)Math.Max(vertex.Position.Z, maxZ);
            }
        }
    }
}
