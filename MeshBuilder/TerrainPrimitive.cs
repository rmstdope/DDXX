using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;
using Microsoft.DirectX;
using Dope.DDXX.TextureBuilder;

namespace Dope.DDXX.MeshBuilder
{
    public class TerrainPrimitive : PlanePrimitive
    {
        private IGenerator heightMapGenerator;
        private float heightScale;

        public IGenerator HeightMapGenerator
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

        public override void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            base.Generate(out vertices, out indices, out body);
            for (int y = 0; y < HeightSegments + 1; y++)
            {
                for (int x = 0; x < WidthSegments + 1; x++)
                {
                    Vector4 heightValue = heightMapGenerator.GetPixel(
                        new Vector2(x / (float)WidthSegments, y / (float)HeightSegments), new Vector2());
                    Vertex vertex = vertices[y * (WidthSegments + 1) + x];
                    Vector3 position = new Vector3(vertex.Position.X, heightValue.X * heightScale, vertex.Position.Y);
                    vertex.Position = position;
                    vertex.Normal = new Vector3(0, 1, 0);
                    vertices[y * (WidthSegments + 1) + x] = vertex;
                }
            }
        }
    }
}
