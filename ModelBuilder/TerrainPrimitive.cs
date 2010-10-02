using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.ModelBuilder
{
    public class TerrainPrimitive : PlanePrimitive
    {
        private ITextureGenerator heightMapGenerator;
        private float heightScale;

        public ITextureGenerator HeightMapGenerator
        {
            get { return heightMapGenerator; }
            set { heightMapGenerator = value; }
        }

        public float HeightScale
        {
            get { return heightScale; }
            set { heightScale = value; }
        }

        public TerrainPrimitive()
        {
        }

        public override IPrimitive Generate()
        {
            IPrimitive primitive = base.Generate();
            Vector4[,] grid = heightMapGenerator.GenerateTexture(WidthSegments + 1, HeightSegments + 1);
            for (int y = 0; y < HeightSegments + 1; y++)
            {
                for (int x = 0; x < WidthSegments + 1; x++)
                {
                    Vector4 heightValue = grid[x, y];
                    Vertex vertex = primitive.Vertices[y * (WidthSegments + 1) + x];
                    Vector3 position = new Vector3(vertex.Position.X, heightValue.X * heightScale, -vertex.Position.Y);
                    vertex.Position = position;
                    vertex.Normal = new Vector3(0, 1, 0);
                    primitive.Vertices[y * (WidthSegments + 1) + x] = vertex;
                }
            }
            return primitive;
        }
    }
}
