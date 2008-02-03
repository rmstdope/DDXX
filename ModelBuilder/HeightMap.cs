using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    public class HeightMap : ModifierBase
    {
        private ITextureGenerator heightMapGenerator;

        public ITextureGenerator HeightMapGenerator
        {
            get { return heightMapGenerator; }
            set { heightMapGenerator = value; }
        }

        public HeightMap()
            : base(1)
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = GetInput(0);
            foreach (Vertex vertex in primitive.Vertices)
            {
                vertex.Position += vertex.Normal *
                    heightMapGenerator.GetPixel(vertex.UV, Vector2.Zero).X;
            }
            ComputeNormals(primitive);
            return primitive;
        }

        //private Vector2 CompensateNonTilableTextures(Vector2 uv)
        //{
        //    float modX = uv.X % 0.5f;
        //    modX = Math.Abs(modX) * 2;
        //    int divX = (int)(uv.X / 0.5f);
        //    if ((divX & 1) == 1)
        //        modX = 1 - modX;

        //    float modY = uv.Y % 0.5f;
        //    modY = Math.Abs(modY) * 2;
        //    int divY = (int)(uv.Y / 0.5f);
        //    if ((divY & 1) == 1)
        //        modY = 1 - modY;

        //    return new Vector2(modX, modY);
        //}
    }
}
