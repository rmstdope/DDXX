using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class PrimitiveTest
    {
        [Test]
        public void CalculateOneTriangleSamePosition()
        {
            // Setup
            Vertex[] vertices = new Vertex[] { 
                new Vertex(new Vector3(), new Vector3(0, 1, 0), new Vector2(0, 0)),
                new Vertex(new Vector3(), new Vector3(0, 1, 0), new Vector2(0, 1)),
                new Vertex(new Vector3(), new Vector3(0, 1, 0), new Vector2(1, 0))
            };
            short[] indices = new short[] { 0, 1, 2 };
            Primitive primitive = new Primitive(vertices, indices);
            // Exercise SUT
            primitive.Calculate();
            // Verify
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(new Vector3(1, 0, 0), vertices[i].Tangent);
                Assert.AreEqual(new Vector3(0, 0, 1), vertices[i].BiNormal);
            }
        }

        [Test]
        public void CalculateOneTriangleSameUV()
        {
            // Setup
            Vertex[] vertices = new Vertex[] { 
                new Vertex(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector2(0, 0)),
                new Vertex(new Vector3(0, 1, 0), new Vector3(0, 1, 0), new Vector2(0, 0)),
                new Vertex(new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector2(0, 0))
            };
            short[] indices = new short[] { 0, 1, 2 };
            Primitive primitive = new Primitive(vertices, indices);
            // Exercise SUT
            primitive.Calculate();
            // Verify
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(new Vector3(1, 0, 0), vertices[i].Tangent);
                Assert.AreEqual(new Vector3(0, 0, 1), vertices[i].BiNormal);
            }
        }

        [Test]
        public void CalculateOneTriangle()
        {
            // Setup
            Vertex[] vertices = new Vertex[] { 
                new Vertex(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector2(0, 0)),
                new Vertex(new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector2(0, 1)),
                new Vertex(new Vector3(0, 0, 1), new Vector3(0, 1, 0), new Vector2(1, 0))
            };
            short[] indices = new short[] { 0, 1, 2 };
            Primitive primitive = new Primitive(vertices, indices);
            // Exercise SUT
            primitive.Calculate();
            // Verify
            for (int i = 0; i < 3; i++)
            {
                Assert.AreEqual(new Vector3(0, 0, 1), vertices[i].Tangent);
                Assert.AreEqual(new Vector3(-1, 0, 0), vertices[i].BiNormal);
            }
        }
    }
}
