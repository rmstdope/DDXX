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


    }
}
