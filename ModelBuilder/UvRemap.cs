using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.ModelBuilder
{
    public class UvRemap : ModifierBase
    {
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

        public UvRemap()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            for (int i = 0; i < primitive.Vertices.Length; i++)
            {
                primitive.Vertices[i].U = primitive.Vertices[i].U * scaleU + translateU;
                primitive.Vertices[i].V = primitive.Vertices[i].V * scaleV + translateV;
            }
            return primitive;
        }
    }
}
