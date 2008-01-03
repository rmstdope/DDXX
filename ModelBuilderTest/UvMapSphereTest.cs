using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Dope.DDXX.Physics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.ModelBuilder
{
    [TestFixture]
    public class UvMapSphereTest : IModifier, IBody
    {
        private Primitive primitive;
        private UvMapSphere map;
        private IPrimitive outputPrimitive;
        private const float epsilon = 0.001f;

        [SetUp]
        public void SetUp()
        {
            map = new UvMapSphere();
            map.ConnectToInput(0, this);
        }

        [Test]
        public void TestEmptyPrimitive()
        {
            primitive = new Primitive(new Vertex[] {}, null);
            outputPrimitive = map.Generate();
            Assert.AreEqual(0, outputPrimitive.Vertices.Length);
        }

        [Test]
        public void TestBodyAndIndices()
        {
            primitive = new Primitive(new Vertex[] { }, null);
            primitive.Body = this;
            outputPrimitive = map.Generate();
            Assert.AreSame(this, outputPrimitive.Body);
            Assert.AreSame(primitive.Indices, outputPrimitive.Indices);
        }

        [Test]
        public void TestMapOrio()
        {
            primitive = new Primitive(new Vertex[] { new Vertex() }, null);
            outputPrimitive = map.Generate();
            Assert.AreEqual(1, outputPrimitive.Vertices.Length);
            Assert.AreEqual(0 + 0.5f, outputPrimitive.Vertices[0].U);
            Assert.AreEqual(0 + 0.5f, outputPrimitive.Vertices[0].V);
        }

        [Test]
        public void TestMapXisOne()
        {
            primitive = new Primitive(new Vertex[] { new Vertex() }, null);
            primitive.Vertices[0].Position = new Vector3(1.0f, 0.0f, 0.0f);
            outputPrimitive = map.Generate();
            Assert.AreEqual(1, outputPrimitive.Vertices.Length);
            Assert.AreEqual(0.5f + 0.5f, outputPrimitive.Vertices[0].U, epsilon);
            Assert.AreEqual(0.0f + 0.5f, outputPrimitive.Vertices[0].V, epsilon);
        }

        [Test]
        public void TestMapXisTwo()
        {
            primitive = new Primitive(new Vertex[] { new Vertex() }, null);
            primitive.Vertices[0].Position = new Vector3(2.0f, 0.0f, 0.0f);
            outputPrimitive = map.Generate();
            Assert.AreEqual(1, outputPrimitive.Vertices.Length);
            Assert.AreEqual(0.5f + 0.5f, outputPrimitive.Vertices[0].U, epsilon);
            Assert.AreEqual(0.0f + 0.5f, outputPrimitive.Vertices[0].V, epsilon);
        }

        [Test]
        public void TestMapYisOne()
        {
            primitive = new Primitive(new Vertex[] { new Vertex() }, null);
            primitive.Vertices[0].Position = new Vector3(0.0f, 1.0f, 0.0f);
            outputPrimitive = map.Generate();
            Assert.AreEqual(1, outputPrimitive.Vertices.Length);
            Assert.AreEqual(0.0f + 0.5f, outputPrimitive.Vertices[0].U, epsilon);
            Assert.AreEqual(0.5f + 0.5f, outputPrimitive.Vertices[0].V, epsilon);
        }

        public IPrimitive Generate()
        {
            return primitive;
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

        public Vector3 Gravity
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

        public void ApplyForce(Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IBody Members


        public void Step(float time)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

        #region IPrimitive Members

        public void ConnectToInput(int inputPin, IModifier outputGenerator)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
