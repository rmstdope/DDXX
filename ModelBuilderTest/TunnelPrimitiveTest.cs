using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class TunnelPrimitiveTest
    {
        private IPrimitive cylinder;
        private IPrimitive tunnel;

        [Test]
        public void Vertices()
        {
            CreateTunnel(2.0f, 4.0f, 10, 20);
            CreateCylinder(2.0f, 4.0f, 10, 20);
            Assert.AreEqual(cylinder.Vertices.Length, tunnel.Vertices.Length);
            for (int i = 0; i < cylinder.Vertices.Length; i++)
            {
                Assert.AreEqual(cylinder.Vertices[i].Position, tunnel.Vertices[i].Position);
                Assert.AreEqual(true, tunnel.Vertices[i].TextureCoordinatesUsed);
                Assert.AreEqual(cylinder.Vertices[i].UV, tunnel.Vertices[i].UV);
                Assert.AreEqual(-cylinder.Vertices[i].Normal, tunnel.Vertices[i].Normal);
                Assert.AreEqual(-cylinder.Vertices[i].BiNormal, tunnel.Vertices[i].BiNormal);
                Assert.AreEqual(-cylinder.Vertices[i].Tangent, tunnel.Vertices[i].Tangent);
            }
        }

        [Test]
        public void Indices()
        {
            CreateTunnel(2.0f, 4.0f, 10, 20);
            CreateCylinder(2.0f, 4.0f, 10, 20);
            Assert.AreEqual(cylinder.Indices.Length, tunnel.Indices.Length);
            int index = 0;
            for (int i = 0; i < cylinder.Vertices.Length / 3; i++)
            {
                Assert.AreEqual(cylinder.Indices[index], tunnel.Indices[index]);
                Assert.AreEqual(cylinder.Indices[index + 1], tunnel.Indices[index + 2]);
                Assert.AreEqual(cylinder.Indices[index + 2], tunnel.Indices[index + 1]);
                index += 3;
            }
        }

        private void CreateTunnel(float height, float radius, int segments, int heightSegments)
        {
            TunnelPrimitive tunnel = new TunnelPrimitive();
            tunnel.Height = height;
            tunnel.Radius = radius;
            tunnel.Segments = segments;
            tunnel.HeightSegments = heightSegments;
            this.tunnel = tunnel.Generate();
        }

        private void CreateCylinder(float height, float radius, int segments, int heightSegments)
        {
            CylinderPrimitive cylinder = new CylinderPrimitive();
            cylinder.Height = height;
            cylinder.Radius = radius;
            cylinder.Segments = segments;
            cylinder.HeightSegments = heightSegments;
            cylinder.Lid = false;
            this.cylinder = cylinder.Generate();
        }
    }
}
