using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.DirectX;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class UvMapSphereTest : IPrimitive, IBody
    {
        private Vertex[] vertices;
        private short[] indices = new short[] { };
        private IBody body;
        private UvMapSphere map;
        private Vertex[] outputVertices;
        private short[] outputIndices;
        private IBody outputBody;
        private const float epsilon = 0.001f;

        [SetUp]
        public void SetUp()
        {
            body = this;
            map = new UvMapSphere();
            map.Input = this;
        }

        [Test]
        public void TestEmptyPrimitive()
        {
            this.vertices = new Vertex[] {};
            map.Generate(out outputVertices, out outputIndices, out outputBody);
            Assert.AreEqual(0, outputVertices.Length);
        }

        [Test]
        public void TestBodyAndIndices()
        {
            this.vertices = new Vertex[] { };
            map.Generate(out outputVertices, out outputIndices, out outputBody);
            Assert.AreSame(this, body);
            Assert.AreSame(indices, outputIndices);
        }

        [Test]
        public void TestMapOrio()
        {
            this.vertices = new Vertex[] { new Vertex() };
            map.Generate(out outputVertices, out outputIndices, out outputBody);
            Assert.AreEqual(1, outputVertices.Length);
            Assert.AreEqual(0 + 0.5f, outputVertices[0].U);
            Assert.AreEqual(0 + 0.5f, outputVertices[0].V);
        }

        [Test]
        public void TestMapXisOne()
        {
            this.vertices = new Vertex[] { new Vertex() };
            this.vertices[0].Position = new Vector3(1.0f, 0.0f, 0.0f);
            map.Generate(out outputVertices, out outputIndices, out outputBody);
            Assert.AreEqual(1, outputVertices.Length);
            Assert.AreEqual(0.5f + 0.5f, outputVertices[0].U, epsilon);
            Assert.AreEqual(0.0f + 0.5f, outputVertices[0].V, epsilon);
        }

        [Test]
        public void TestMapXisTwo()
        {
            this.vertices = new Vertex[] { new Vertex() };
            this.vertices[0].Position = new Vector3(2.0f, 0.0f, 0.0f);
            map.Generate(out outputVertices, out outputIndices, out outputBody);
            Assert.AreEqual(1, outputVertices.Length);
            Assert.AreEqual(0.5f + 0.5f, outputVertices[0].U, epsilon);
            Assert.AreEqual(0.0f + 0.5f, outputVertices[0].V, epsilon);
        }

        [Test]
        public void TestMapYisOne()
        {
            this.vertices = new Vertex[] { new Vertex() };
            this.vertices[0].Position = new Vector3(0.0f, 1.0f, 0.0f);
            map.Generate(out outputVertices, out outputIndices, out outputBody);
            Assert.AreEqual(1, outputVertices.Length);
            Assert.AreEqual(0.0f + 0.5f, outputVertices[0].U, epsilon);
            Assert.AreEqual(0.5f + 0.5f, outputVertices[0].V, epsilon);
        }

        public void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            vertices = this.vertices;
            indices = this.indices;
            body = this.body;
        }

        #region IBody Members

        public void AddConstraint(Dope.DDXX.Physics.IConstraint constraint)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public void AddParticle(IPhysicalParticle particle)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public Microsoft.DirectX.Vector3 Gravity
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public void Step()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public List<IPhysicalParticle> Particles
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public List<Dope.DDXX.Physics.IConstraint> Constraints
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public void ApplyForce(Microsoft.DirectX.Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
