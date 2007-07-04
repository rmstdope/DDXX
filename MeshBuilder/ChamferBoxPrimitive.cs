using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class ChamferBoxPrimitive : SpherePrimitive
    {
        private float width;
        private float height;
        private float length;
        private float fillet;
        private int filletSegments;

        public float Width
        {
            get { return width; }
            set { width = value; }
        }

        public float Height
        {
            get { return height; }
            set { height = value; }
        }

        public float Length
        {
            get { return length; }
            set { length = value; }
        }

        public float Fillet
        {
            get { return fillet; }
            set { fillet = value; }
        }

        public int FilletSegments
        {
            get { return filletSegments; }
            set { filletSegments = value; }
        }

        public ChamferBoxPrimitive()
        {
            width = 1;
            height = 1;
            length = 1;
            fillet = 0.1f;
            filletSegments = 4;
        }

        public override Primitive Generate()
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
                    add.Y = (height / 2 - fillet);
                else
                    add.Y = -(height / 2 - fillet);
                int quarter = ((i / verticesPerQuarter) % 4);
                if (quarter == 0)
                    add += new Vector3(-(width / 2 - fillet), 0, length / 2 - fillet);
                if (quarter == 1)
                    add += new Vector3(-(width / 2 - fillet), 0, -(length / 2 - fillet));
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
                normal.Normalize();
                // http://www.mvps.org/directx/articles/spheremap.htm
                vertex.U = (float)(Math.Asin(normal.X) / Math.PI) + 0.5f;
                vertex.V = (float)(Math.Asin(normal.Y) / Math.PI) + 0.5f;
                vertexList[i] = vertex;
            }

            // Add a top
            indexList.Add(0);
            indexList.Add((short)(filletSegments * 2));
            indexList.Add((short)filletSegments);
            indexList.Add(0);
            indexList.Add((short)(filletSegments * 3));
            indexList.Add((short)(filletSegments * 2));
            // Add bottom
            indexList.Add((short)(vertexList.Count - 1 - 0));
            indexList.Add((short)(vertexList.Count - 1 - filletSegments * 2));
            indexList.Add((short)(vertexList.Count - 1 - filletSegments));
            indexList.Add((short)(vertexList.Count - 1 - 0));
            indexList.Add((short)(vertexList.Count - 1 - filletSegments * 3));
            indexList.Add((short)(vertexList.Count - 1 - filletSegments * 2));

            //// FIXME: This if for test!!!!
            //// Recalc normals
            //for (int i = 0; i < vertexList.Count; i++)
            //{
            //    Vertex vertex = vertexList[i];
            //    Vector3 normal = vertex.Position;
            //    normal.Normalize();
            //    vertex.Normal = normal;
            //    vertexList[i] = vertex;
            //}

            Weld welder = new Weld();
            welder.Distance = 0.0f;
            welder.Input = new DummyPrimitive(vertexList.ToArray(), indexList.ToArray());
            return welder.Generate();
        }
    }
}
