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
            List<Vertex> newVertices = new List<Vertex>(vertices);
            for (int i = 0; i < newVertices.Count; i++)
            {
                for (int j = i + 1; j < newVertices.Count; j++)
                {
                    if ((newVertices[i].Position - newVertices[j].Position).Length() <= distance)
                    {
                        Vertex v1 = newVertices[i];
                        Vertex v2 = newVertices[j];
                        Vector3 normal = v1.Normal + v2.Normal;
                        normal.Normalize();
                        v1.Normal = normal;
                        v1.U = (v1.U + v2.U) / 2;
                        v1.V = (v1.V + v2.V) / 2;
                        newVertices.RemoveAt(j);
                        newVertices[i] = v1;
                        for (int k = 0; k < indices.Length; k++)
                        {
                            if (indices[k] == j)
                                indices[k] = (short)i;
                            if (indices[k] > j)
                                indices[k]--;
                        }
                        j--;
                    }
                }
            }
            vertices = newVertices.ToArray();
        }
    }
}
