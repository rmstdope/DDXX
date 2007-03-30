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

        public Primitive(Vertex[] vertices, short[] indices)
        {
            this.vertices = vertices;
            this.indices = indices;
        }

        /// <summary>
        /// Create a physical primitive that consists of a cloth.
        /// </summary>
        /// <param name="width">The width of the cloth.</param>
        /// <param name="height">The height of the cloth.</param>
        /// <param name="widthSegments">The number of segments the cloth has in x.</param>
        /// <param name="heightSegments">The number of segments the cloth has in y.</param>
        /// <param name="pinnedParticles">An array containing the indices of all particles that
        /// shall be pinned.</param>
        /// <returns></returns>
        public static Primitive ClothPrimitive(IBody body, float width, float height,
            int widthSegments, int heightSegments, int[] pinnedParticles)
        {
            Primitive cloth = ClothPrimitive(body, width, height, widthSegments, heightSegments);
            foreach (int index in pinnedParticles)
            {
                body.AddConstraint(new PositionConstraint(body.Particles[index],
                    body.Particles[index].Position));
            }
            return cloth;
        }

        /// <summary>
        /// Create a physical primitive that consists of a cloth.
        /// </summary>
        /// <param name="width">The width of the cloth.</param>
        /// <param name="height">The height of the cloth.</param>
        /// <param name="widthSegments">The number of segments the cloth has in x.</param>
        /// <param name="heightSegments">The number of segments the cloth has in y.</param>
        /// <returns></returns>
        public static Primitive ClothPrimitive(IBody body, float width, float height,
            int widthSegments, int heightSegments)
        {
            IPhysicalParticle p1;
            IPhysicalParticle p2;
            IPhysicalParticle p3;
            IPhysicalParticle p4;
            int x;
            int y;

            Primitive cloth = PlanePrimitive(width, height, widthSegments, heightSegments);
            for (int i = 0; i < cloth.vertices.Length; i++)
                body.AddParticle(new PhysicalParticle(cloth.vertices[i].Position, 1, 1));
            for (y = 0; y < heightSegments; y++)
            {
                for (x = 0; x < widthSegments; x++)
                {
                    // p1--p2
                    // | \/     
                    // | /\      
                    // p3  p4
                    p1 = body.Particles[(y + 0) * (widthSegments + 1) + x + 0];
                    p2 = body.Particles[(y + 0) * (widthSegments + 1) + x + 1];
                    p3 = body.Particles[(y + 1) * (widthSegments + 1) + x + 0];
                    p4 = body.Particles[(y + 1) * (widthSegments + 1) + x + 1];
                    AddConstraint(body, p1, p2);
                    AddConstraint(body, p1, p3);
                    //AddConstraint(body, p1, p4);
                    //AddConstraint(body, p1, p4);
                }
                // p1
                // |
                // |
                // p3
                p1 = body.Particles[(y + 0) * (widthSegments + 1) + x + 0];
                p3 = body.Particles[(y + 1) * (widthSegments + 1) + x + 0];
                AddConstraint(body, p1, p3);
            }
            for (x = 0; x < widthSegments; x++)
            {
                // p1--p2
                p1 = body.Particles[(y + 0) * (widthSegments + 1) + x + 0];
                p2 = body.Particles[(y + 0) * (widthSegments + 1) + x + 1];
                AddConstraint(body, p1, p2);
            }
            cloth.body = body;
            return cloth;
        }

        private static void AddConstraint(IBody body, IPhysicalParticle p1, IPhysicalParticle p2)
        {
            body.AddConstraint(new StickConstraint(p1, p2, (p1.Position - p2.Position).Length()));
        }

        /// <summary>
        /// Create a primitive that consists of a plane.
        /// </summary>
        /// <param name="width">The width of the plane.</param>
        /// <param name="height">The height of the plane.</param>
        /// <param name="widthSegments">The number of segments the plane has in x.</param>
        /// <param name="heightSegments">The number of segments the plane has in y.</param>
        /// <returns></returns>
        public static Primitive PlanePrimitive(float width, float height,
            int widthSegments, int heightSegments)
        {
            short v = 0;
            Vertex[] vertices = new Vertex[(widthSegments + 1) * (heightSegments + 1)];
            short[] indices = new short[widthSegments * heightSegments * 6];

            BoxAddIndicesForSide(0, 0, indices, widthSegments, heightSegments);
            for (int y = 0; y < heightSegments + 1; y++)
            {
                float yPos = height  * (0.5f - y / (float)heightSegments);
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    float xPos = width * (-0.5f + x / (float)widthSegments);
                    vertices[v].Normal = new Vector3(0, 0, -1);
                    vertices[v++].Position = new Vector3(xPos, yPos, 0);
                }
            }
            return new Primitive(vertices, indices);
        }

        /// <summary>
        /// Create a primitive that consist of a plane.
        /// </summary>
        /// <param name="length">The length of the plane (z)</param>
        /// <param name="width">The width of the plane (x)</param>
        /// <param name="height">The height of the plane (y)</param>
        /// <param name="lengthSegments">The number of segments the plane has in z.</param>
        /// <param name="widthSegments">The number of segments the plane has in x.</param>
        /// <param name="heightSegments">The number of segments the plane has in y.</param>
        /// <returns></returns>
        public static Primitive BoxPrimitive(float length, float width, float height, 
            int lengthSegments, int widthSegments, int heightSegments)
        {
            short v = 0;
            short i = 0;
            Vertex[] vertices = new Vertex[24];
            short[] indices = new short[36];
            // Front
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, heightSegments);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 0, -1);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            // Back
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, heightSegments);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 0, 1);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            // Top
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, lengthSegments);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, 1, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            // Bottom
            i = BoxAddIndicesForSide(v, i, indices, widthSegments, lengthSegments);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(0, -1, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            // Left
            i = BoxAddIndicesForSide(v, i, indices, lengthSegments, heightSegments);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, length / 2);
            vertices[v].Normal = new Vector3(-1, 0, 0);
            vertices[v++].Position = new Vector3(-width / 2, -height / 2, -length / 2);
            // Right
            i = BoxAddIndicesForSide(v, i, indices, lengthSegments, heightSegments);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, -length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, height / 2, length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, -length / 2);
            vertices[v].Normal = new Vector3(1, 0, 0);
            vertices[v++].Position = new Vector3(width / 2, -height / 2, length / 2);
            return new Primitive(vertices, indices);
        }

        private static short BoxAddIndicesForSide(short v, short i, 
            short[] indices, int widthSegments, int heightSegments)
        {
            for (int y = 0; y < heightSegments; y++)
            {
                for (int x = 0; x < widthSegments; x++)
                {
                    int vertex = v + x + y * (widthSegments + 1);
                    indices[i++] = (short)vertex;
                    indices[i++] = (short)(vertex + 1);
                    indices[i++] = (short)(vertex + (widthSegments + 1));
                    indices[i++] = (short)(vertex + 1);
                    indices[i++] = (short)(vertex + 1 + +(widthSegments + 1));
                    indices[i++] = (short)(vertex + +(widthSegments + 1));
                }
            }
            return i;
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

        public IModel CreateModel(IGraphicsFactory factory, IDevice device)
        {
            IModel model;
            IMesh mesh = CreateMesh(factory, device);
            if (body == null)
                model = new Model(mesh);
            else
                model = new PhysicalModel(mesh, body);
            return model;
        }

        private IMesh CreateMesh(IGraphicsFactory factory, IDevice device)
        {
            VertexElementArray declaration = new VertexElementArray();
            declaration.AddPositions();
            declaration.AddNormals();
            IMesh mesh = factory.CreateMesh(indices.Length / 3, vertices.Length, MeshFlags.Managed,
                declaration.VertexElements, device);
            IGraphicsStream stream = mesh.LockVertexBuffer(LockFlags.None);
            for (int i = 0; i < vertices.Length; i++)
            {
                stream.Write(vertices[i].Position);
                stream.Write(vertices[i].Normal);
            }
            mesh.UnlockVertexBuffer();
            stream = mesh.LockIndexBuffer(LockFlags.None);
            stream.Write(indices);
            mesh.UnlockIndexBuffer();
            return mesh;
        }


    }
}
