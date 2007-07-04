using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    public class UvRemap : IModifier
    {
        private IModifier input;
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

        public IModifier Input
        {
            get { return input; }
            set { input = value; }
        }

        public Primitive Generate()
        {
            Primitive primitive = input.Generate();
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                primitive.Vertices[i].U = primitive.Vertices[i].U * scaleU + translateU;
                primitive.Vertices[i].V = primitive.Vertices[i].V * scaleV + translateV;
            }
            return primitive;
        }
    }
}
