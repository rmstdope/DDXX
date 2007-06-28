using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class UvRemap : IPrimitive
    {
        private IPrimitive input;
        private float translateU;
        private float scaleU;
        private float translateV;
        private float scaleV;

        public float TranslateU
        {
            get { return translateU; }
            set { translateU = value; }
        }

        public float ScaleU
        {
            get { return scaleU; }
            set { scaleU = value; }
        }

        public float TranslateV
        {
            get { return translateV; }
            set { translateV = value; }
        }

        public float ScaleV
        {
            get { return scaleV; }
            set { scaleV = value; }
        }

        public IPrimitive Input
        {
            get { return input; }
            set { input = value; }
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            input.Generate(out vertices, out indices, out body);
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].U = vertices[i].U * scaleU + translateU;
                vertices[i].V = vertices[i].V * scaleV + translateV;
            }
        }
    }
}
