using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    public class Primitive : IPrimitive
    {
        private Vertex[] vertices;
        private short[] indices;
        private IBody body;
        private ModelMaterial modelMaterial;

        public Primitive(Vertex[] vertices, short[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
        }

        public Vertex[] Vertices
        {
            get
            {
                return vertices;
            }
        }

        public short[] Indices
        {
            get
            {
                return indices;
            }
        }

        public IBody Body
        {
            get
            {
                return body;
            }
            set
            {
                body = value;
            }
        }

        public ModelMaterial ModelMaterial
        {
            get { return modelMaterial; }
            set { modelMaterial = value; }
        }

        public IModel CreateModel(IGraphicsFactory factory, 
            IDevice device)
        {
            IModel model;
            IMesh mesh = CreateMesh(factory, device);
            if (modelMaterial == null)
                modelMaterial = new ModelMaterial(new Material());
            if (body == null)
                model = new Model(mesh, new ModelMaterial[] { modelMaterial });
            else
                model = new PhysicalModel(mesh, body, new ModelMaterial[] { modelMaterial });
            return model;
        }

        private IMesh CreateMesh(IGraphicsFactory factory, IDevice device)
        {
            bool useTexCoords = false;
            foreach (Vertex vertex in vertices)
            {
                if (vertex.TextureCoordinatesUsed)
                    useTexCoords = true;
            }
            VertexElementArray declaration = new VertexElementArray();
            declaration.AddPositions();
            declaration.AddNormals();
            if (useTexCoords)
                declaration.AddTexCoords(0, 2);
            IMesh mesh = factory.CreateMesh(indices.Length / 3, vertices.Length, MeshFlags.Managed,
                declaration.VertexElements, device);
            using (IGraphicsStream stream = mesh.LockVertexBuffer(LockFlags.None))
            {
                for (int i = 0; i < vertices.Length; i++)
                {
                    stream.Write(vertices[i].Position);
                    stream.Write(vertices[i].Normal);
                    if (useTexCoords)
                    {
                        stream.Write(vertices[i].U);
                        stream.Write(vertices[i].V);
                    }
                }
                mesh.UnlockVertexBuffer();
            }
            using (IGraphicsStream stream = mesh.LockIndexBuffer(LockFlags.None))
            {
                stream.Write(indices);
                mesh.UnlockIndexBuffer();
            }
            return mesh;
        }

        public void Weld(float distance)
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
                        RemoveVertex(newVertices, i, j);
                        j--;
                    }
                }
            }
            vertices = newVertices.ToArray();
            RemoveNonTriangles();
            RemoveDuplicateTriangles();
        }

        private void RemoveDuplicateTriangles()
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

        private void RemoveNonTriangles()
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

        private void RemoveVertex(List<Vertex> newVertices, int sourceIndex, int destIndex)
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
            RemoveVertexFromIndices(sourceIndex, destIndex);
        }

        private void RemoveVertexFromIndices(int sourceIndex, int destIndex)
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
