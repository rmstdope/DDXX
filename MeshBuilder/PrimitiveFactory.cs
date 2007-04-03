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
                body.AddParticle(new PhysicalParticle(cloth.Vertices[i].Position, 1, 1));
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

    }
}
