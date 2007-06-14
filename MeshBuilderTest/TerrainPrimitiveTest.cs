using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Dope.DDXX.TextureBuilder;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class TerrainPrimitiveTest : IGenerator
    {
        private Vertex[] terrainVertices;
        private Vertex[] planeVertices;
        private short[] terrainIndices;
        private short[] planeIndices;
        private IBody terrainBody;
        private IBody planeBody;
        private Vector4 pixelValue;

        [Test]
        public void TestTerrainHeightOne()
        {
            pixelValue = new Vector4(1, 1, 1, 1);
            CreateTerrain(this, 1.0f, 2.0f, 2.0f, 2, 2, false);
            CreatePlane(2.0f, 2.0f, 2, 2, false);
            Assert.AreEqual(terrainIndices.Length, planeIndices.Length);
            Assert.AreEqual(terrainVertices.Length, planeVertices.Length);
            for (int i = 0; i < terrainIndices.Length; i++)
                Assert.AreEqual(terrainIndices[i], planeIndices[i]);
            for (int i = 0; i < terrainVertices.Length; i++)
            {
                Assert.AreEqual(terrainVertices[i].Position.X, planeVertices[i].Position.X);
                Assert.AreEqual(terrainVertices[i].Position.Y, 1.0f);
                Assert.AreEqual(terrainVertices[i].Position.Z, planeVertices[i].Position.Y);
            }
        }

        [Test]
        public void TestTerrainHeightX()
        {
            pixelValue = new Vector4(0, 1, 1, 1);
            CreateTerrain(this, 1.0f, 4.0f, 4.0f, 4, 4, true);
            CreatePlane(4.0f, 4.0f, 4, 4, true);
            Assert.AreEqual(terrainIndices.Length, planeIndices.Length);
            Assert.AreEqual(terrainVertices.Length, planeVertices.Length);
            for (int i = 0; i < terrainIndices.Length; i++)
                Assert.AreEqual(terrainIndices[i], planeIndices[i]);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Assert.AreEqual(terrainVertices[y * 5 + x].Position.X, planeVertices[y * 5 + x].Position.X);
                    Assert.AreEqual(terrainVertices[y * 5 + x].Position.Y, x * (1.0f / 4.0f));
                    Assert.AreEqual(terrainVertices[y * 5 + x].Position.Z, planeVertices[y * 5 + x].Position.Y);
                }
            }
        }

        [Test]
        public void TestTerrainHeightY()
        {
            pixelValue = new Vector4(1, 0, 1, 1);
            CreateTerrain(this, 2.0f, 4.0f, 4.0f, 4, 4, true);
            CreatePlane(4.0f, 4.0f, 4, 4, true);
            Assert.AreEqual(terrainIndices.Length, planeIndices.Length);
            Assert.AreEqual(terrainVertices.Length, planeVertices.Length);
            for (int i = 0; i < terrainIndices.Length; i++)
                Assert.AreEqual(terrainIndices[i], planeIndices[i]);
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    Assert.AreEqual(terrainVertices[y * 5 + x].Position.X, planeVertices[y * 5 + x].Position.X);
                    Assert.AreEqual(terrainVertices[y * 5 + x].Position.Y, 2.0f * y * (1.0f / 4.0f));
                    Assert.AreEqual(terrainVertices[y * 5 + x].Position.Z, planeVertices[y * 5 + x].Position.Y);
                }
            }
        }

        private void CreateTerrain(IGenerator generator, float heightScale, float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            TerrainPrimitive terrain = new TerrainPrimitive();
            terrain.HeightMapGenerator = generator;
            terrain.Height = height;
            terrain.Width = width;
            terrain.WidthSegments = widthSegments;
            terrain.HeightSegments = heightSegments;
            terrain.HeightScale = heightScale;
            terrain.Textured = textured;
            terrain.Generate(out terrainVertices, out terrainIndices, out terrainBody);
        }

        private void CreatePlane(float width, float height, int widthSegments, int heightSegments, bool textured)
        {
            PlanePrimitive plane = new PlanePrimitive();
            plane.Height = height;
            plane.Width = width;
            plane.WidthSegments = widthSegments;
            plane.HeightSegments = heightSegments;
            plane.Textured = textured;
            plane.Generate(out planeVertices, out planeIndices, out planeBody);
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

        public void ConnectToInput(int inputPin, IGenerator outputGenerator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
