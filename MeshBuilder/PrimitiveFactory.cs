using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    internal class PrimitiveFactory : IPrimitiveFactory
    {
        internal const float HORIZONTAL_STIFFNESS = 0.8f;
        internal const float VERTICAL_STIFFNESS = 0.8f;
        internal const float DIAGONAL_STIFFNESS = 0.1f;

        public IPrimitive CreateCloth(IBody body, float width, float height,
            int widthSegments, int heightSegments, int[] pinnedParticles, bool textured)
        {
            IPrimitive cloth = CreateCloth(body, width, height, widthSegments, heightSegments, textured);
            HandlePinnedParticles(body, pinnedParticles);
            return cloth;
        }

        public IPrimitive CreateCloth(IBody body, float width, float height,
            int widthSegments, int heightSegments, bool textured)
        {
            IPhysicalParticle p1;
            IPhysicalParticle p2;
            IPhysicalParticle p3;
            IPhysicalParticle p4;
            int x;
            int y;

            IPrimitive cloth = CreatePlane(width, height, widthSegments, heightSegments, textured);
            for (int i = 0; i < cloth.Vertices.Length; i++)
                body.AddParticle(new PhysicalParticle(cloth.Vertices[i].Position, 1, 0.01f));
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
                    AddStickConstraint(body, p1, p2, HORIZONTAL_STIFFNESS);
                    AddStickConstraint(body, p1, p3, VERTICAL_STIFFNESS);
                    AddStickConstraint(body, p1, p4, DIAGONAL_STIFFNESS);
                    AddStickConstraint(body, p1, p4, DIAGONAL_STIFFNESS);
                }
                // p1
                // |
                // |
                // p3
                p1 = body.Particles[(y + 0) * (widthSegments + 1) + x + 0];
                p3 = body.Particles[(y + 1) * (widthSegments + 1) + x + 0];
                AddStickConstraint(body, p1, p3, VERTICAL_STIFFNESS);
            }
            for (x = 0; x < widthSegments; x++)
            {
                // p1--p2
                p1 = body.Particles[(y + 0) * (widthSegments + 1) + x + 0];
                p2 = body.Particles[(y + 0) * (widthSegments + 1) + x + 1];
                AddStickConstraint(body, p1, p2, HORIZONTAL_STIFFNESS);
            }
            cloth.Body = body;
            return cloth;
        }

        private static void HandlePinnedParticles(IBody body, int[] pinnedParticles)
        {
            foreach (int index in pinnedParticles)
            {
                body.AddConstraint(new PositionConstraint(body.Particles[index],
                    body.Particles[index].Position));
            }
        }

        private void AddStickConstraint(IBody body, IPhysicalParticle p1, IPhysicalParticle p2, float stiffness)
        {
            body.AddConstraint(new StickConstraint(p1, p2, (p1.Position - p2.Position).Length(), stiffness));
        }

        public IPrimitive CreatePlane(float width, float height,
            int widthSegments, int heightSegments, bool textured)
        {
            short v = 0;
            Vertex[] vertices = new Vertex[(widthSegments + 1) * (heightSegments + 1)];
            short[] indices = new short[widthSegments * heightSegments * 6];

            BoxAddIndicesForSide(0, 0, indices, widthSegments, heightSegments);
            for (int y = 0; y < heightSegments + 1; y++)
            {
                float yPos = height * (0.5f - y / (float)heightSegments);
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    float xPos = width * (-0.5f + x / (float)widthSegments);
                    vertices[v].Normal = new Vector3(0, 0, -1);
                    vertices[v].Position = new Vector3(xPos, yPos, 0);
                    if (textured)
                    {
                        vertices[v].U = x / (float)widthSegments;
                        vertices[v].V = y / (float)heightSegments;
                    }
                    v++;
                }
            }
            return new Primitive(vertices, indices);
        }

        public IPrimitive CreateBox(float length, float width, float height, int lengthSegments, int widthSegments, int heightSegments)
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

        private short BoxAddIndicesForSide(short v, short i,
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

        //        |z
        //        |     _
        //        |phi_/
        //        | _/r 
        //        |/_______ y
        //       / \_   
        //      / th \_ 
        //    x/  eta  \
        // theta in 0,2pi    phi in 0,pi
        public IPrimitive CreateSphere(float radius, short Nu, short Nv)
        {
            if (Nu < 4)
                throw new ArgumentOutOfRangeException("Nu", "Must be at least 4");
            if (Nv < 2)
                throw new ArgumentOutOfRangeException("Nv", "Must be at least 2");
            if ((Nu % 4) != 0)
                throw new ArgumentException("Must be multiple of four", "Nu");
            List<Vertex> vertexList = new List<Vertex>();
            double du = 2 * Math.PI / Nu;
            double dv = Math.PI / Nv;
            double phi;
            for (double invphi = dv / 2.0f; invphi < Math.PI; invphi += dv)
            {
                phi = Math.PI - invphi; // want to go from down upwards
                for (double theta = du / 2.0f; theta < 2 * Math.PI; theta += du)
                {
                    Vector3 p = new Vector3();
                    p.X = (float)(radius * Math.Cos(theta) * Math.Sin(phi));
                    p.Y = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
                    p.Z = (float)(radius * Math.Cos(phi));
                    Vertex v = new Vertex();
                    v.Position = p;
                    vertexList.Add(v);
                }
            }
            // Bottom
            // a square => 4 step circle, halfway to bottom center
            // from the bottommost circle with Nu vertices,
            // where each 1/4th of the vertices will be connected to 
            // one corner of the bottom square
            short firstBottomIndex = (short)vertexList.Count;
            du = 2 * Math.PI / 4;
            phi = Math.PI - (dv / 2);
            for (double theta = 0; theta < 2 * Math.PI; theta += du)
            {
                Vector3 p = new Vector3();
                p.X = (float)(radius * Math.Cos(theta) * Math.Sin(phi));
                p.Y = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
                p.Z = (float)(radius * Math.Cos(phi));
                Vertex v = new Vertex();
                v.Position = p;
                vertexList.Add(v);
            }
            // Top
            short firstTopIndex = (short)vertexList.Count;
            du = 2 * Math.PI / 4;
            phi = (dv / 2);
            for (double theta = 0; theta < 2 * Math.PI; theta += du)
            {
                Vector3 p = new Vector3();
                p.X = (float)(radius * Math.Cos(theta) * Math.Sin(phi));
                p.Y = (float)(radius * Math.Sin(theta) * Math.Sin(phi));
                p.Z = (float)(radius * Math.Cos(phi));
                Vertex v = new Vertex();
                v.Position = p;
                vertexList.Add(v);
            }

            // indices
            List<short> indexList = new List<short>();
            for (short i = 0; i < Nv-1; i++)
            {
                for (short j = 0; j < Nu; j++)
                {
                    SphereAddQuadIndices(j, i, Nu, Nv, indexList);
                }
            }
            // bottom
            for (short i = 0; i < 4; i++)
            {
                short c = (short)(Nu / 4);
                short inext = (short)((i + 1) % 4);
                short k = (short)(c * i);
                short jnext = 1;
                for (short j = 0; j < c; j++)
                {
                    k = (short)(c * i);
                    jnext = (short)(j + 1);
                    indexList.Add((short)(firstBottomIndex + i));
                    indexList.Add((short)((k + jnext) % Nu));
                    indexList.Add((short)(k + j));
                }
                indexList.Add((short)(firstBottomIndex + i));
                indexList.Add((short)(firstBottomIndex + inext));
                indexList.Add((short)((k + jnext) % Nu));
            }
            // bottom cover
            indexList.Add((short)(firstBottomIndex + 3));
            indexList.Add((short)(firstBottomIndex + 2));
            indexList.Add((short)(firstBottomIndex + 1));
            indexList.Add((short)(firstBottomIndex + 2));
            indexList.Add((short)(firstBottomIndex + 1));
            indexList.Add((short)(firstBottomIndex + 0));
            // top
            for (short i = 0; i < 4; i++)
            {
                short c = (short)(Nu / 4);
                short inext = (short)((i + 1) % 4);
                short k = (short)(c * i);
                short jnext = 1;
                for (short j = 0; j < c; j++)
                {
                    k = (short)(c * i + (Nv - 1) * Nu);
                    jnext = (short)(j + 1);
                    indexList.Add((short)(firstTopIndex + i));
                    indexList.Add((short)(k + j));
                    indexList.Add((short)(((k + jnext) % Nu) + (Nv - 1) * Nu));
                }
                indexList.Add((short)(firstTopIndex + inext));
                indexList.Add((short)(firstTopIndex + i));
                indexList.Add((short)(((k + jnext) % Nu) + (Nv - 1) * Nu));
            }
            // top cover
            indexList.Add((short)(firstTopIndex + 0));
            indexList.Add((short)(firstTopIndex + 1));
            indexList.Add((short)(firstTopIndex + 2));
            indexList.Add((short)(firstTopIndex + 1));
            indexList.Add((short)(firstTopIndex + 2));
            indexList.Add((short)(firstTopIndex + 3));
            
            // create and return primitive
            Vertex[] vertices = vertexList.ToArray();
            short[] indices = indexList.ToArray();
            return new Primitive(vertices, indices);
        }

        private static void SphereAddQuadIndices(short theta, short phi,
            short Nu, short Nv, List<short> indexList)
        {
            short lastOnRow = (short)((Nu - 1) + phi * Nu);
            short firstOnRow = (short)(phi * Nu);

            short vertex = (short)(theta + phi * Nu);
            short nextVertex = (short)(vertex + 1);
            if (vertex == lastOnRow)
                nextVertex = firstOnRow;
            indexList.Add((short)(vertex));
            indexList.Add((short)(nextVertex));
            indexList.Add((short)(nextVertex + Nu));
            indexList.Add((short)(nextVertex));
            indexList.Add((short)(nextVertex + Nu));
            indexList.Add((short)(vertex + Nu));
        }
    }
}
