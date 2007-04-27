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

        public IPrimitive CreateChamferBox(float length, float width, float height, float fillet, 
            int lengthSegments, int widthSegments, int heightSegments, int filletSegments)
        {
            if (fillet > length / 2 || fillet > width / 2 || fillet > height / 2)
                throw new ArgumentException("Must not be larger than max(length, width, height)", 
                    "fillet");
            if (filletSegments < 2)
                throw new ArgumentException("Must be at least 2", "filletSegments");

            int rings = (filletSegments - 1) * 4;
            List<Vertex> vertexList;
            List<short> indexList;
            CreateSphereLists(fillet, rings, out vertexList, out indexList);
            int verticesPerQuarter = 1 + rings / 4;
            for (int i = 0; i < vertexList.Count; i++)
            {
                Vector3 add = new Vector3();
                if (i < vertexList.Count / 2)
                    add.Y = height / 2 - fillet;
                else
                    add.Y = -(height / 2 - fillet);
                int quarter = ((i / verticesPerQuarter) % 4);
                if (quarter == 0)
                    add += new Vector3(-(width / 2 - fillet), 0, length / 2 - fillet);
                if (quarter == 1)
                    add += new Vector3(-(width/ 2 - fillet), 0, -(length / 2 - fillet));
                if (quarter == 2)
                    add += new Vector3(width / 2 - fillet, 0, -(length / 2 - fillet));
                if (quarter == 3)
                    add += new Vector3(width / 2 - fillet, 0, length / 2 - fillet);
                Vertex vertex = vertexList[i];
                Vector3 position = vertex.Position;
                position += add;
                vertex.Position = position;
                vertexList[i] = vertex;
            }

            // Remap uv
            for (int i = 0; i < vertexList.Count; i++)
            {
                Vertex vertex = vertexList[i];
                Vector3 normal = vertex.Position;
                //normal.Normalize();
                // http://www.mvps.org/directx/articles/spheremap.htm
                vertex.U = (float)(Math.Asin(normal.X) / Math.PI) + 0.5f;
                vertex.V = (float)(Math.Asin(normal.Y) / Math.PI) + 0.5f;
                //vertexList[i] = vertex;
            }

            // create and return primitive
            Vertex[] vertices = vertexList.ToArray();
            short[] indices = indexList.ToArray();

            IPrimitive primitive = new Primitive(vertices, indices);
            primitive.Weld(0.0f);
            return primitive;
        }

        //        |z
        //        |th   _
        //        |eta_/
        //        | _/r 
        //        |/_______ y
        //       / \_   
        //      /    \_ 
        //    x/  phi  \
        // theta in 0,2pi    phi in 0,pi
        public IPrimitive CreateSphere2(float radius, int rings)
        {
            if ((rings  % 4) != 0)
                throw new ArgumentException("Must be multiple of four", "rings");

            List<Vertex> vertexList;
            List<short> indexList;
            CreateSphereLists(radius, rings, out vertexList, out indexList);

            // create and return primitive
            Vertex[] vertices = vertexList.ToArray();
            short[] indices = indexList.ToArray();

            IPrimitive primitive = new Primitive(vertices, indices);
            primitive.Weld(0.0f);
            return primitive;
        }

        private void CreateSphereLists(float radius, int rings, out List<Vertex> vertexList, out List<short> indexList)
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
