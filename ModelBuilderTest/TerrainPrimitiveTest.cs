using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class TerrainPrimitiveTest : ITextureGenerator
    {
        private TerrainPrimitive terrain;
        private IPrimitive terrainPrimitive;
        private IPrimitive planePrimitive;
        private Vector4 pixelValue;

        [Test]
        public void Getters()
        {
            CreateTerrain(this, 1.0f, 2.0f, 3.0f, 2, 3, false);
            Assert.AreEqual(this, terrain.HeightMapGenerator);
            Assert.AreEqual(1.0f, terrain.HeightScale);
            Assert.AreEqual(2.0f, terrain.Width);
            Assert.AreEqual(3.0f, terrain.Height);
            Assert.AreEqual(2, terrain.WidthSegments);
            Assert.AreEqual(3, terrain.HeightSegments);
            Assert.AreEqual(false, terrain.Textured);
        }

        [Test]
        public void TestTerrainHeightOne()
        {
            pixelValue = new Vector4(1, 1, 1, 1);
            CreateTerrain(this, 1.0f, 2.0f, 2.0f, 2, 2, false);
            CreatePlane(2.0f, 2.0f, 2, 2, false);
            Assert.AreEqual(terrainPrimitive.Indices.Length, planePrimitive.Indices.Length);
            Assert.AreEqual(terrainPrimitive.Vertices.Length, planePrimitive.Vertices.Length);
            for (int i = 0; i < terrainPrimitive.Indices.Length; i++)
                Assert.AreEqual(terrainPrimitive.Indices[i], planePrimitive.Indices[i]);
            for (int i = 0; i < terrainPrimitive.Vertices.Length; i++)
            {
                Assert.AreEqual(terrainPrimitive.Vertices[i].Position.X, planePrimitive.Vertices[i].Position.X);
                Assert.AreEqual(terrainPrimitive.Vertices[i].Position.Y, 1.0f);
                Assert.AreEqual(terrainPrimitive.Vertices[i].Position.Z, -planePrimitive.Vertices[i].Position.Y);
            }
        }

        [Test]
        public void TestTerrainHeightX()
        {
            pixelValue = new Vector4(0, 1, 1, 1);
            CreateTerrain(this, 1.0f, 4.0f, 4.0f, 4, 4, true);
            CreatePlane(4.0f, 4.0f, 4, 4, true);
            Assert.AreEqual(terrainPrimitive.Indices.Length, planePrimitive.Indices.Length);
            Assert.AreEqual(terrainPrimitive.Vertices.Length, planePrimitive.Vertices.Length);
            for (int i = 0; i < terrainPrimitive.Indices.Length; i++)
                Assert.AreEqual(terrainPrimitive.Indices[i], planePrimitive.Indices[i]);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Assert.AreEqual(terrainPrimitive.Vertices[y * 5 + x].Position.X, planePrimitive.Vertices[y * 5 + x].Position.X);
                    Assert.AreEqual(terrainPrimitive.Vertices[y * 5 + x].Position.Y, x * (1.0f / 4.0f));
                    Assert.AreEqual(terrainPrimitive.Vertices[y * 5 + x].Position.Z, -planePrimitive.Vertices[y * 5 + x].Position.Y);
                }
            }
        }

        [Test]
        public void TestTerrainHeightY()
        {
            pixelValue = new Vector4(1, 0, 1, 1);
            CreateTerrain(this, 2.0f, 4.0f, 4.0f, 4, 4, true);
            CreatePlane(4.0f, 4.0f, 4, 4, true);
            Assert.AreEqual(terrainPrimitive.Indices.Length, planePrimitive.Indices.Length);
            Assert.AreEqual(terrainPrimitive.Vertices.Length, planePrimitive.Vertices.Length);
            for (int i = 0; i < terrainPrimitive.Indices.Length; i++)
                Assert.AreEqual(terrainPrimitive.Indices[i], planePrimitive.Indices[i]);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Assert.AreEqual(terrainPrimitive.Vertices[y * 5 + x].Position.X, planePrimitive.Vertices[y * 5 + x].Position.X);
                    Assert.AreEqual(terrainPrimitive.Vertices[y * 5 + x].Position.Y, 2.0f * y * (1.0f / 4.0f));
                    Assert.AreEqual(terrainPrimitive.Vertices[y * 5 + x].Position.Z, -planePrimitive.Vertices[y * 5 + x].Position.Y);
                }
            }
        }

        private void CreateTerrain(ITextureGenerator generator, float heightScale, float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            terrain = new TerrainPrimitive();
            terrain.HeightMapGenerator = generator;
            terrain.Height = height;
            terrain.Width = width;
            terrain.WidthSegments = widthSegments;
            terrain.HeightSegments = heightSegments;
            terrain.HeightScale = heightScale;
            terrain.Textured = textured;
            terrainPrimitive = terrain.Generate();
        }

        private void CreatePlane(float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            PlanePrimitive plane = new PlanePrimitive();
            plane.Height = height;
            plane.Width = width;
            plane.WidthSegments = widthSegments;
            plane.HeightSegments = heightSegments;
            plane.Textured = textured;
            planePrimitive = plane.Generate();
        }

        #region IGenerator Members

        public Vector4 GetPixel(Vector2 textureCoordinate, Vector2 texelSize)
        {
            if (pixelValue.X == 0)
                return new Vector4(textureCoordinate.X, 0, 0, 0);
            if (pixelValue.Y == 0)
                return new Vector4(textureCoordinate.Y, 0, 0, 0);
            return pixelValue;
        }

        public void ConnectToInput(int inputPin, ITextureGenerator outputGenerator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public int NumInputPins
        {
            get { return 0; }
        }

        public ITextureGenerator GetInput(int inputPin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
