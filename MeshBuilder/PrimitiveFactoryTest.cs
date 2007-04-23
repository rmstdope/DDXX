using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    [TestFixture]
    public class PrimitiveFactoryTest : IBody
    {
        private PrimitiveFactory factory;
        private IPrimitive primitive;
        private List<IPhysicalParticle> particles;
        private List<Dope.DDXX.Physics.IConstraint> constraints;
        private const float epsilon = 0.000001f;
        protected enum Side
        {
            FRONT = 0,
            BACK,
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }

        [SetUp]
        public void SetUp()
        {
            factory = new PrimitiveFactory();
            particles = new List<IPhysicalParticle>();
            constraints = new List<Dope.DDXX.Physics.IConstraint>();
        }

        /// <summary>
        /// Check number of vertices for a plane.
        /// </summary>
        [Test]
        public void TestNumVerticesSingleSegment()
        {
            IPrimitive box = factory.CreateBox(10, 20, 30, 1, 1, 1);
            Assert.AreEqual(24, box.Vertices.Length, "The plane should have 24 vertices.");
            Assert.AreEqual(36, box.Indices.Length, "The plane should have 36 indices.");
        }

        /// <summary>
        /// Test the front side of the plane when it only has one segment.
        /// </summary>
        [Test]
        public void TestFrontSingleSegment()
        {
            TestBoxSingleSegment(Side.FRONT, new Vector3(0, 0, -1));
        }

        /// <summary>
        /// Test the back side of the plane when it only has one segment.
        /// </summary>
        [Test]
        public void TestBackSingleSegment()
        {
            TestBoxSingleSegment(Side.BACK, new Vector3(0, 0, 1));
        }

        /// <summary>
        /// Test the top side of the plane when it only has one segment.
        /// </summary>
        [Test]
        public void TestTopSingleSegment()
        {
            TestBoxSingleSegment(Side.TOP, new Vector3(0, 1, 0));
        }

        /// <summary>
        /// Test the bottom side of the plane when it only has one segment.
        /// </summary>
        [Test]
        public void TestBottomSingleSegment()
        {
            TestBoxSingleSegment(Side.BOTTOM, new Vector3(0, -1, 0));
        }

        /// <summary>
        /// Test the left side of the plane when it only has one segment.
        /// </summary>
        [Test]
        public void TestLeftSingleSegment()
        {
            TestBoxSingleSegment(Side.LEFT, new Vector3(-1, 0, 0));
        }

        /// <summary>
        /// Test the right side of the plane when it only has one segment.
        /// </summary>
        [Test]
        public void TestRightSingleSegment()
        {
            TestBoxSingleSegment(Side.RIGHT, new Vector3(1, 0, 0));
        }

        /// <summary>
        /// Check number of vertices for a plane with one segment.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneSingleSegment()
        {
            IPrimitive plane = factory.CreatePlane(10, 30, 1, 1, false);
            Assert.AreEqual(4, plane.Vertices.Length, "The plane should have 4 vertices.");
            Assert.AreEqual(6, plane.Indices.Length, "The plane should have 6 indices.");
        }

        /// <summary>
        /// Check number of vertices for a plane with multiple segments.
        /// </summary>
        [Test]
        public void TestNumVerticesPlaneMultipleSegments()
        {
            IPrimitive plane = factory.CreatePlane(20, 40, 1, 4, false);
            Assert.AreEqual(10, plane.Vertices.Length, "The plane should have 10 vertices.");
            Assert.AreEqual(24, plane.Indices.Length, "The plane should have 24 indices.");
        }

        /// <summary>
        /// Test a plane with only one segment.
        /// </summary>
        [Test]
        public void TestPlaneSingleSegment()
        {
            primitive = factory.CreatePlane(10, 20, 1, 1, false);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 4;
            int numTriangles = 2;
            // Check vertices against plane (0, 0, -1, 0)
            CheckRectangleInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckPlaneVertexLimits(10.0f, 20.0f, 1, 1);
            // Check no texture coordinates
            CheckPlaneTexCoords(false, 1, 1);
        }

        /// <summary>
        /// Test a plane with more segments.
        /// </summary>
        [Test]
        public void TestPlaneMultipleSegment()
        {
            primitive = factory.CreatePlane(20, 40, 2, 4, false);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 15;
            int numTriangles = 16;
            // Check vertices against plane (0, 0, -1, 0)
            CheckRectangleInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckPlaneVertexLimits(20.0f, 40.0f, 2, 4);
            // Check no texture coordinates
            CheckPlaneTexCoords(false, 2, 4);
        }

        /// <summary>
        /// Test a plane with more segments and texture coordinates.
        /// </summary>
        [Test]
        public void TestPlaneMultipleSegmentTextured()
        {
            primitive = factory.CreatePlane(20, 40, 2, 4, true);
            int startVertex = 0;
            int startIndex = 0;
            int numVertices = 15;
            int numTriangles = 16;
            // Check vertices against plane (0, 0, -1, 0)
            CheckRectangleInPlane(startVertex, numVertices, new Plane(0, 0, -1, 0));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(startVertex, numVertices, startIndex, numTriangles * 3);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(startIndex, numTriangles, new Vector3(0, 0, -1));
            // Check normals
            CheckRectangleNormals(startVertex, numVertices, new Vector3(0, 0, -1));
            // Check limits
            CheckPlaneVertexLimits(20.0f, 40.0f, 2, 4);
            // Check no texture coordinates
            CheckPlaneTexCoords(true, 2, 4);
        }

        /// <summary>
        /// Test that the body is set in the Primitive.
        /// </summary>
        [Test]
        public void TestClothBody()
        {
            IPrimitive cloth = factory.CreateCloth(this, 10, 30, 1, 1, false);
            Assert.AreSame(this, cloth.Body, "Body should have been set to this.");
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestClothNumParticlesInBody1()
        {
            IPrimitive cloth = factory.CreateCloth(this, 10, 30, 1, 1, false);
            Assert.AreEqual(4, particles.Count, "We should have four particles.");
        }

        /// <summary>
        /// Test that as many particles are added to the body as there is vertices.
        /// </summary>
        [Test]
        public void TestClothNumParticlesInBody2()
        {
            IPrimitive cloth = factory.CreateCloth(this, 20, 40, 4, 2, false);
            Assert.AreEqual(15, particles.Count, "We should have 15 particles.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInBody1()
        {
            IPrimitive cloth = factory.CreateCloth(this, 10, 30, 1, 1, false);
            Assert.AreEqual(6, constraints.Count, "We should have six constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInBody2()
        {
            IPrimitive cloth = factory.CreateCloth(this, 20, 40, 4, 2, false);
            Assert.AreEqual(38, constraints.Count, "We should have 38 constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints in a pinned cloth.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInPinnedCloth1()
        {
            IPrimitive cloth = factory.CreateCloth(this, 10, 30, 1, 1, new int[] { }, false);
            Assert.AreEqual(6, constraints.Count, "We should have six constraints.");
        }

        /// <summary>
        /// Test that we have the correct number of constraints in a pinned cloth.
        /// </summary>
        [Test]
        public void TestClothNumConstraintsInPinnedCloth2()
        {
            IPrimitive cloth = factory.CreateCloth(this, 10, 30, 1, 1, new int[] { 0, 1 }, false);
            Assert.AreEqual(8, constraints.Count, "We should have eight constraints.");
            // Now if we move particles 0 and 1 and then satisfy contraints 6 and 7, the two particles
            // shall be moved back.
            Vector3 original0 = particles[0].Position;
            Vector3 original1 = particles[1].Position;
            particles[0].Position = new Vector3(-100, -100, -100);
            particles[1].Position = new Vector3(-100, -100, -100);
            constraints[constraints.Count - 2].Satisfy();
            constraints[constraints.Count - 1].Satisfy();
            Assert.AreEqual(original0, particles[0].Position,
                "Particle[0] should have been moved back.");
            Assert.AreEqual(original1, particles[1].Position,
                "Particle[1] should have been moved back.");
        }

        /// <summary>
        /// Test that all particles have the same positions are a plane of the same size.
        /// </summary>
        [Test]
        public void TestClothParticlePosition()
        {
            IPrimitive cloth = factory.CreateCloth(this, 56, 87, 7, 9, false);
            IPrimitive plane = factory.CreatePlane(56, 87, 7, 9, false);
            for (int i = 0; i < plane.Vertices.Length; i++)
            {
                Assert.IsInstanceOfType(typeof(PhysicalParticle), particles[i],
                    "All particles should be PhysicalParticles");
                Assert.AreEqual(plane.Vertices[i].Position, particles[i].Position,
                    "Position of particle " + i + " should be the same as the plane vertex.");
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSphereNuOutOfRange()
        {
            factory.CreateSphere(1.0f, 7, 4);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestSphereNvOutOfRange()
        {
            factory.CreateSphere(1.0f, 8, 2);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSphereNuNotMultOf4()
        {
            factory.CreateSphere(1.0f, 9, 4);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSphereNvNotEven()
        {
            factory.CreateSphere(1.0f, 8, 5);
        }

        [Test]
        public void TestSphereVertexCount()
        {
            IPrimitive sphere = factory.CreateSphere(1.0f, 4, 2);
            Assert.AreEqual(8+2, sphere.Vertices.Length);
            sphere = factory.CreateSphere(1.0f, 8, 8);
            Assert.AreEqual(64+2, sphere.Vertices.Length);
        }

        [Test]
        public void TestSphereVertexPositions()
        {
            float radius = 10.0f;
            IPrimitive sphere = factory.CreateSphere(radius, 8, 8);
            foreach (Vertex v in sphere.Vertices) {
                Assert.AreEqual(radius, v.Position.Length());
            }
        }

        [Test]
        public void TestSphereIndices()
        {
            short Nu = 4;
            short Nv = 2;
            IPrimitive sphere = factory.CreateSphere(1.0f, Nu, Nv);
            // Nv+2 because top and bottom special to generate square cover
            Assert.AreEqual(3*2*Nu*(Nv-1) + 2*3*(Nu/4+1)*4 + 2*3*2, sphere.Indices.Length);
            short[] indices = new short[] {
                0,1,5,
                1,5,4,
                1,2,6,
                2,6,5,
                2,3,7,
                3,7,6,
                3,0,4,
                0,4,7,
                8,1,0,  // extra row to bottom square
                8,9,1,
                9,2,1,
                9,10,2,
                10,3,2,
                10,11,3,
                11,0,3,
                11,8,0,
                11,10,9, // bottom
                10,9,8,
                12,4,5, // extra row to top square
                13,12,5,
                13,5,6,
                14,13,6,
                14,6,7,
                15,14,7,
                15,7,4,
                12,15,4,
                12,13,14, // top
                13,14,15,
            };
            Assert.AreEqual(84, indices.Length);
            for (int i = 0; i < indices.Length; i+=3)
            {
                CheckTriangle(i, indices[i], indices[i + 1], indices[i + 2], sphere.Indices); 
            }
        }

        private void CheckTriangle(int t, int i, int j, int k, short[] indices)
        {
            Assert.AreEqual(i, indices[t], "t=" + t + ", i=" + i + ", j=" + j + ", k=" + k);
            Assert.AreEqual(j, indices[t + 1], "t=" + t + ", i=" + i + ", j=" + j + ", k=" + k);
            Assert.AreEqual(k, indices[t + 2], "t=" + t + ", i=" + i + ", j=" + j + ", k=" + k);
        }

        private void CheckPlaneVertexLimits(float width, float height, int widthSegments, int heightSegments)
        {
            int v = 0;
            for (int y = 0; y < heightSegments + 1; y++)
            {
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    if (y == 0)
                        Assert.AreEqual(height / 2, primitive.Vertices[v].Position.Y);
                    else
                        Assert.IsTrue(primitive.Vertices[v].Position.Y <
                            primitive.Vertices[v - widthSegments - 1].Position.Y);
                    if (y == heightSegments)
                        Assert.AreEqual(-height / 2, primitive.Vertices[v].Position.Y);
                    if (x == 0)
                        Assert.AreEqual(-width / 2, primitive.Vertices[v].Position.X);
                    else
                        Assert.IsTrue(primitive.Vertices[v].Position.X >
                            primitive.Vertices[v - 1].Position.X);
                    if (x == widthSegments)
                        Assert.AreEqual(width / 2, primitive.Vertices[v].Position.X);
                    v++;
                }
            }
        }

        private void CheckPlaneTexCoords(bool texCoords, int widthSegments, int heightSegments)
        {
            int v = 0;
            for (int y = 0; y < heightSegments + 1; y++)
            {
                for (int x = 0; x < widthSegments + 1; x++)
                {
                    if (!texCoords)
                        Assert.IsFalse(primitive.Vertices[v].TextureCoordinatesUsed,
                            "Texture coordinates should not be used.");
                    else
                    {
                        Assert.IsTrue(primitive.Vertices[v].TextureCoordinatesUsed,
                            "Texture coordinates should not be used.");
                        if (y == 0)
                            Assert.AreEqual(0, primitive.Vertices[v].V);
                        else
                            Assert.IsTrue(primitive.Vertices[v].V >
                                primitive.Vertices[v - widthSegments - 1].V);
                        if (y == heightSegments)
                            Assert.AreEqual(1.0f, primitive.Vertices[v].V);
                        if (x == 0)
                            Assert.AreEqual(0, primitive.Vertices[v].U);
                        else
                            Assert.IsTrue(primitive.Vertices[v].U >
                                primitive.Vertices[v - 1].U);
                        if (x == widthSegments)
                            Assert.AreEqual(1.0f, primitive.Vertices[v].U);
                        v++;
                    }
                }
            }
        }

        private void TestBoxSingleSegment(Side side, Vector3 normal)
        {
            float sideLength;
            if (side == Side.BACK || side == Side.FRONT)
                sideLength = 5;
            else if (side == Side.TOP || side == Side.BOTTOM)
                sideLength = 15;
            else
                sideLength = 10;
            primitive = factory.CreateBox(10, 20, 30, 1, 1, 1);
            int v = GetBoxStartVertex(side, 1, 1, 1);
            int i = GetBoxStartIndex(side, 1, 1, 1);
            // Check vertices against plane (0, 0, -1, -sideLength)
            CheckRectangleInPlane(v, v + 4, new Plane(normal.X, normal.Y, normal.Z, -sideLength));
            // Check that indices points to the correct vertices
            CheckRectangleIndices(v, v + 4, i, i + 6);
            // Check that the indices create clockwise triangles
            CheckRectangleClockwise(i, 2, normal);
            // Check normals
            CheckRectangleNormals(v, v + 4, normal);
        }

        private int GetBoxStartVertex(Side side, int lengthSegments, int widthSegments, int heightSegments)
        {
            return 4 * (int)side;
        }

        private int GetBoxStartIndex(Side side, int lengthSegments, int widthSegments, int heightSegments)
        {
            return 6 * (int)side;
        }

        private void CheckRectangleInPlane(int startIndex, int endIndex, Plane plane)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                Assert.AreEqual(0.0f, plane.Dot(primitive.Vertices[i].Position), epsilon,
                    "All points should be in plane (" + startIndex + ", " + endIndex + ")");
            }
        }

        private void CheckRectangleNormals(int startIndex, int endIndex, Vector3 normal)
        {
            for (int i = startIndex; i < endIndex; i++)
                Assert.AreEqual(normal, primitive.Vertices[i].Normal);
        }

        private void CheckRectangleClockwise(int startI, int numTriangles, Vector3 normal)
        {
            for (int i = 0; i < numTriangles; i++)
            {
                Vector3 v1 = primitive.Vertices[primitive.Indices[startI + i * 3 + 0]].Position;
                Vector3 v2 = primitive.Vertices[primitive.Indices[startI + i * 3 + 1]].Position;
                Vector3 v3 = primitive.Vertices[primitive.Indices[startI + i * 3 + 2]].Position;
                Vector3 testNormal = Vector3.Cross(v2 - v1, v3 - v1);
                testNormal.Normalize();
                Assert.AreEqual(normal.X, testNormal.X, epsilon, "Normals should be equal.");
                Assert.AreEqual(normal.Y, testNormal.Y, epsilon, "Normals should be equal.");
                Assert.AreEqual(normal.Z, testNormal.Z, epsilon, "Normals should be equal.");
            }
        }

        private void CheckRectangleIndices(int startV, int endV, int startI, int endI)
        {
            for (int i = startI; i < endI; i++)
            {
                Assert.IsTrue(primitive.Indices[i] >= startV, "Indices must not point to vertex smaller than " + startV);
                Assert.IsTrue(primitive.Indices[i] < endV, "Indices must not point to vertex larger or equal to " + endV);
            }
        }


        #region IBody Members

        public void AddConstraint(Dope.DDXX.Physics.IConstraint constraint)
        {
            constraints.Add(constraint);
        }

        public void AddParticle(IPhysicalParticle particle)
        {
            particles.Add(particle);
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
            get { return particles; }
        }

        public void ApplyForce(Vector3 vector3)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
