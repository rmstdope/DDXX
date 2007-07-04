using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class Weld : IModifier
    {
        private IModifier input;
        private float distance;

        public IModifier Input
        {
            get { return input; }
            set { input = value; }
        }

        public float Distance
        {
            get { return distance; }
            set { distance = value; }
        }


        public Weld()
        {
        }

        public Primitive Generate()
        {
            Primitive primitive = input.Generate();
            DoWeld(ref primitive.Vertices, ref primitive.Indices);
            return primitive;
        }

        private void DoWeld(ref Vertex[] vertices, ref short[] indices)
        {
            const float epsilon = 1e-9f;
            List<Vertex> newVertices = new List<Vertex>(vertices);
            for (int i = 0; i < newVertices.Count; i++)
            {
                for (int j = i + 1; j < newVertices.Count; j++)
                {
                    if ((newVertices[i].Position - newVertices[j].Position).Length() <=
                        distance + epsilon)
                    {
                        RemoveVertex(newVertices, ref indices, i, j);
                        j--;
                    }
                }
            }
            vertices = newVertices.ToArray();
            RemoveNonTriangles(ref indices);
            RemoveDuplicateTriangles(ref indices);
        }

        private void RemoveDuplicateTriangles(ref short[] indices)
        {
            List<short> newIndices = new List<short>(indices);
            for (int i = 0; i < newIndices.Count; i += 3)
            {
                short i1 = newIndices[i + 0];
                short i2 = newIndices[i + 1];
                short i3 = newIndices[i + 2];
                for (int j = i + 3; j < newIndices.Count; j += 3)
                {
                    short j1 = newIndices[j + 0];
                    short j2 = newIndices[j + 1];
                    short j3 = newIndices[j + 2];
                    // Remove only if triangles have the same culling!
                    if (i1 == j1 && i2 == j2 && i3 == j3 ||
                        i1 == j2 && i2 == j3 && i3 == j1 ||
                        i1 == j3 && i2 == j1 && i3 == j2)
                    {
                        newIndices.RemoveRange(j, 3);
                        j -= 3;
                    }
                }
            }
            indices = newIndices.ToArray();
        }

        private void RemoveNonTriangles(ref short[] indices)
        {
            List<short> newIndices = new List<short>(indices);
            for (int i = 0; i < newIndices.Count; i += 3)
            {
                short i1 = newIndices[i + 0];
                short i2 = newIndices[i + 1];
                short i3 = newIndices[i + 2];
                if (i1 == i2 || i1 == i3 || i2 == i3)
                {
                    newIndices.RemoveRange(i, 3);
                    i -= 3;
                }
            }
            indices = newIndices.ToArray();
        }

        private void RemoveVertex(List<Vertex> newVertices, ref short[] indices, int sourceIndex, int destIndex)
        {
            Vertex v1 = newVertices[sourceIndex];
            Vertex v2 = newVertices[destIndex];
            Vector3 normal = v1.Normal + v2.Normal;
            normal.Normalize();
            v1.Normal = normal;
            v1.U = (v1.U + v2.U) / 2;
            v1.V = (v1.V + v2.V) / 2;
            newVertices.RemoveAt(destIndex);
            newVertices[sourceIndex] = v1;
            RemoveVertexFromIndices(ref indices, sourceIndex, destIndex);
        }

        private void RemoveVertexFromIndices(ref short[] indices, int sourceIndex, int destIndex)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                if (indices[i] == destIndex)
                    indices[i] = (short)sourceIndex;
                if (indices[i] > destIndex)
                    indices[i]--;
            }
        }
    }
}
