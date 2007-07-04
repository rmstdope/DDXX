using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class SpherePrimitive : IModifier
    {
        protected class DummyPrimitive : IModifier
        {
            private Vertex[] vertices;
            private short[] indices;

            public DummyPrimitive(Vertex[] vertices, short[] indices)
            {
                this.vertices = vertices;
                this.indices = indices;
            }

            public Primitive Generate()
            {
                return new Primitive(vertices, indices);
            }
        }

        private float radius;
        private int rings;

        public float Radius
        {
            get { return radius; }
            set { radius = value; }
        }

        public int Rings
        {
            get { return rings; }
            set { rings = value; }
        }

        public virtual Primitive Generate()
        {
            if ((rings % 4) != 0)
                throw new ArgumentException("Must be multiple of four", "rings");

            List<Vertex> vertexList;
            List<short> indexList;
            CreateSphereLists(radius, rings, out vertexList, out indexList);

            Weld welder = new Weld();
            welder.Distance = 0.0f;
            welder.Input = new DummyPrimitive(vertexList.ToArray(), indexList.ToArray());
            return welder.Generate();
        }

        protected void CreateSphereLists(float radius, int rings, out List<Vertex> vertexList, out List<short> indexList)
        {
            vertexList = new List<Vertex>();
            indexList = new List<short>();

            int phiCount = rings / 2 + 1;
            int chamferRings = rings + 2;
            double phi = 0;
            for (int i = 0; i < phiCount + 1; i++)
            {
                double theta = 0;
                for (int j = 0; j < rings + 4; j++)
                {
                    AddRingVertex(radius, vertexList, phi, theta);
                    // Make sure to add two vertices at theta={PI/2, PI, 3*PI/2}
                    bool shouldAdd = true;
                    for (int k = 1; k < 4; k++)
                        if (j == k - 1 + k * rings / 4)
                            shouldAdd = false;
                    if (shouldAdd)
                        theta += 2 * Math.PI / rings;
                }
                if (i != 0)
                    AddRingIndices(rings + 4, i, indexList);
                // Make sure to add two rings at phi=PI/2
                if (i != phiCount / 2)
                    phi += (Math.PI / (phiCount - 1));
            }

            // Set normals
            for (int i = 0; i < vertexList.Count; i++)
            {
                Vertex vertex = vertexList[i];
                Vector3 normal = vertex.Position;
                normal.Normalize();
                vertex.Normal = normal;
                // http://www.mvps.org/directx/articles/spheremap.htm
                vertex.U = (float)(Math.Asin(normal.X) / Math.PI) + 0.5f;
                vertex.V = (float)(Math.Asin(normal.Y) / Math.PI) + 0.5f;
                vertexList[i] = vertex;
            }
        }

        private Vertex AddRingVertex(float radius, List<Vertex> vertexList, double phi, double theta)
        {
            Vertex v = new Vertex();
            v.Position = GetSpherePos(radius, phi, theta);
            vertexList.Add(v);
            return v;
        }

        private void AddRingIndices(int rings, int ring, List<short> indexList)
        {
            for (int j = 0; j < rings; j++)
            {
                short i1 = GetVertexNumber(rings, ring - 1, j);
                short i2 = GetVertexNumber(rings, ring - 1, j + 1);
                short i3 = GetVertexNumber(rings, ring, j);
                short i4 = GetVertexNumber(rings, ring, j + 1);
                indexList.Add(i1);
                indexList.Add(i2);
                indexList.Add(i4);
                indexList.Add(i1);
                indexList.Add(i4);
                indexList.Add(i3);
            }
        }

        private void AddTopIndices(int rings, List<short> indexList)
        {
            for (int i = 0; i < rings; i++)
            {
                short i1 = 0;
                short i2 = GetVertexNumber(rings, 0, i + 1);
                short i3 = GetVertexNumber(rings, 0, i);
                indexList.Add(i1);
                indexList.Add(i2);
                indexList.Add(i3);
            }
        }

        private void AddBottomIndices(int rings, List<short> indexList)
        {
            int phiCount = (rings - 2) / 2;
            for (int i = 0; i < rings; i++)
            {
                short i1 = GetVertexNumber(rings, phiCount - 1, i);
                short i2 = GetVertexNumber(rings, phiCount - 1, i + 1);
                short i3 = GetVertexNumber(rings, phiCount, 0);
                indexList.Add(i1);
                indexList.Add(i2);
                indexList.Add(i3);
            }
        }

        private short GetVertexNumber(int rings, int ring, int vertex)
        {
            short index = 0;
            // Jump to correct ring
            index += (short)(ring * rings);
            // Get correct vertex (with modulo to support wrapping)
            index += (short)(vertex % rings);

            return index;
        }

        private void AddBottomVertex(float radius, List<Vertex> vertexList)
        {
            AddTopVertices(-radius, vertexList, 0);
        }

        private void AddTopVertices(float radius, List<Vertex> vertexList, int num)
        {
            Vertex v = new Vertex();
            v.Position = new Vector3(0, radius, 0);
            vertexList.Add(v);
        }

        private Vector3 GetSpherePos(float radius, double phi, double theta)
        {
            float y = (float)(radius * Math.Cos(phi));
            float innerRadius = (float)(-radius * Math.Sin(phi));
            return new Vector3((float)(innerRadius * Math.Sin(theta)), y, (float)(-innerRadius * Math.Cos(theta)));
        }

    }
}
