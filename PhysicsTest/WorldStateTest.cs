using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    [TestFixture]
    public class WorldStateTest
    {
        public void AssertVectors(Vector3 vec1, Vector3 vec2)
        {
            float epsilon = 0.0001f;
            float len = (vec1 - vec2).Length();
            Assert.AreEqual(0, len, epsilon);
        }

        public void AssertMatrices(Matrix m1, Matrix m2)
        {
            float epsilon = 0.0001f;
            Assert.AreEqual(m1.M11, m2.M11, epsilon);
            Assert.AreEqual(m1.M21, m2.M21, epsilon);
            Assert.AreEqual(m1.M31, m2.M31, epsilon);
            Assert.AreEqual(m1.M41, m2.M41, epsilon);
            Assert.AreEqual(m1.M12, m2.M12, epsilon);
            Assert.AreEqual(m1.M22, m2.M22, epsilon);
            Assert.AreEqual(m1.M32, m2.M32, epsilon);
            Assert.AreEqual(m1.M42, m2.M42, epsilon);
            Assert.AreEqual(m1.M13, m2.M13, epsilon);
            Assert.AreEqual(m1.M23, m2.M23, epsilon);
            Assert.AreEqual(m1.M33, m2.M33, epsilon);
            Assert.AreEqual(m1.M43, m2.M43, epsilon);
            Assert.AreEqual(m1.M14, m2.M14, epsilon);
            Assert.AreEqual(m1.M24, m2.M24, epsilon);
            Assert.AreEqual(m1.M34, m2.M34, epsilon);
            Assert.AreEqual(m1.M44, m2.M44, epsilon);
        }

        [Test]
        public void PositionTest()
        {
            WorldState state1 = new WorldState();
            Assert.AreEqual(new Vector3(0, 0, 0), state1.Position);
            WorldState state2 = new WorldState(new Vector3(2.0f, 3.0f, 4.0f), Matrix.Identity, new Vector3(1, 1, 1));
            Assert.AreEqual(new Vector3(2, 3, 4), state2.Position);
            state1.Position = new Vector3(2.0f, 3.0f, 4.0f);
            Assert.AreEqual(state1.Position, state2.Position);

            state1.MoveDelta(new Vector3(1, 1, 1));
            Assert.AreNotEqual(state1.Position, state2.Position);
            state2.MoveDelta(new Vector3(1, 1, 1));
            Assert.AreEqual(state1.Position, state2.Position);
        }

        [Test]
        public void ScalingTest()
        {
            WorldState state1 = new WorldState();
            Assert.AreEqual(new Vector3(1, 1, 1), state1.Scaling);
            WorldState state2 = new WorldState(new Vector3(0, 0, 0), Matrix.Identity, new Vector3(1, 2, 3));
            Assert.AreEqual(new Vector3(1, 2, 3), state2.Scaling);
            state1.Scaling = new Vector3(1, 2, 3);
            Assert.AreEqual(state1.Scaling, state2.Scaling);

            state1.Scale(new Vector3(2, 2, 2));
            Assert.AreEqual(new Vector3(2, 4, 6), state1.Scaling);
            state1.Scale(2.0f);
            Assert.AreEqual(new Vector3(4, 8, 12), state1.Scaling);
        }

        [Test]
        public void RotationOrderTest()
        {
            WorldState state1 = new WorldState();
            Assert.AreEqual(Matrix.CreateFromYawPitchRoll(0, 0, 0), state1.Rotation);
            WorldState state2 = new WorldState(new Vector3(1, 1, 1),
                                               Matrix.CreateFromYawPitchRoll((float)Math.PI, 0, 0), 
                                               new Vector3(1, 1, 1));
            state1.Turn((float)Math.PI);
            AssertMatrices(state1.Rotation, state2.Rotation);

            state2.Rotation = Matrix.CreateFromYawPitchRoll(0, 0, (float)Math.PI);
            state1.Tilt((float)Math.PI);
            AssertMatrices(state1.Rotation, state2.Rotation);

            state2.Rotation = Matrix.Identity;
            state1.Roll((float)Math.PI);
            AssertMatrices(state1.Rotation, state2.Rotation);
        }

        [Test]
        public void DirectionTest()
        {
            WorldState state = new WorldState();
            AssertVectors(state.Forward, new Vector3(0, 0, -1)); // Right-handed system
            AssertVectors(state.Up, new Vector3(0, 1, 0)); // Right-handed system
            AssertVectors(state.Right, new Vector3(1, 0, 0)); // Right-handed system

            state.Turn(-(float)Math.PI / 2);
            AssertVectors(state.Forward, new Vector3(1, 0, 0));
            AssertVectors(state.Up, new Vector3(0, 1, 0));
            AssertVectors(state.Right, new Vector3(0, 0, 1));

            state.Tilt(-(float)Math.PI / 2);
            AssertVectors(state.Forward, new Vector3(0, -1, 0));
            AssertVectors(state.Up, new Vector3(1, 0, 0));
            AssertVectors(state.Right, new Vector3(0, 0, 1));

            state.Roll((float)Math.PI / 2);
            AssertVectors(state.Forward, new Vector3(0, -1, 0));
            AssertVectors(state.Up, new Vector3(0, 0, -1));
            AssertVectors(state.Right, new Vector3(1, 0, 0));
        }

        [Test]
        public void WorldMatrixTest()
        {
            Vector3 vec = new Vector3(1, 2, 3);
            WorldState state1 = new WorldState();
            Assert.AreEqual(Matrix.Identity, state1.GetWorldMatrix());

            state1.Position = new Vector3(100, 200, 300);
            AssertVectors(new Vector3(101, 202, 303), Vector3.Transform(vec, state1.GetWorldMatrix()));

            state1.Reset();
            Assert.AreEqual(Matrix.Identity, state1.GetWorldMatrix());

            // 1, 0, 0, 0 means rotate 180 deg around y axis (turn)
            state1.Rotation = Matrix.CreateRotationY((float)Math.PI);
            AssertVectors(new Vector3(-1, 2, -3), Vector3.Transform(vec, state1.GetWorldMatrix()));

            state1.Reset();
            state1.Scaling = new Vector3(2, 3, 4);
            AssertVectors(new Vector3(2, 6, 12), Vector3.Transform(vec, state1.GetWorldMatrix()));

            state1.Position = new Vector3(100, 200, 300);
            state1.Rotation = Matrix.CreateRotationY((float)Math.PI);
            state1.Scaling = new Vector3(2, 3, 4);
            AssertVectors(new Vector3(98, 206, 288), Vector3.Transform(vec, state1.GetWorldMatrix()));

        }
    }
}
