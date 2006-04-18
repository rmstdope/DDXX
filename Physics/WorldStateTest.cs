using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Microsoft.DirectX;

namespace Physics
{
    [TestFixture]
    public class WorldStateTest
    {
        [Test]
        public void PositionTest()
        {
            WorldState state1 = new WorldState();
            Assert.AreEqual(new Vector3(0, 0, 0), state1.GetPosition());
            WorldState state2 = new WorldState(new Vector3(2.0f, 3.0f, 4.0f), new Quaternion(0, 0, 0, 1), new Vector3(1, 1, 1));
            Assert.AreEqual(new Vector3(2, 3, 4), state2.GetPosition());
            state1.SetPosition(new Vector3(2.0f, 3.0f, 4.0f));
            Assert.AreEqual(state1.GetPosition(), state2.GetPosition());

            state1.MoveDelta(new Vector3(1, 1, 1));
            Assert.AreNotEqual(state1.GetPosition(), state2.GetPosition());
            state2.MoveDelta(new Vector3(1, 1, 1));
            Assert.AreEqual(state1.GetPosition(), state2.GetPosition());
        }

        [Test]
        public void ScalingTest()
        {
            WorldState state1 = new WorldState();
            Assert.AreEqual(new Vector3(1, 1, 1), state1.GetScaling());
            WorldState state2 = new WorldState(new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 1), new Vector3(1, 2, 3));
            Assert.AreEqual(new Vector3(1, 2, 3), state2.GetScaling());
            state1.SetScaling(new Vector3(1, 2, 3));
            Assert.AreEqual(state1.GetScaling(), state2.GetScaling());

            state1.Scale(new Vector3(2, 2, 2));
            Assert.AreEqual(new Vector3(2, 4, 6), state1.GetScaling());
            state1.Scale(2.0f);
            Assert.AreEqual(new Vector3(4, 8, 12), state1.GetScaling());
        }

        [Test]
        public void RotationTest()
        {
            const float epsilon = 0.00001f;

            // 0, 0, 0, 1 means no rotation
            WorldState state1 = new WorldState();
            Assert.AreEqual(new Quaternion(0, 0, 0, 1), state1.GetRotation());
            // 1, 0, 0, 0 means rotate 180 deg around y axis (turn)
            WorldState state2 = new WorldState(new Vector3(1, 1, 1), new Quaternion(0, 1, 0, 0), new Vector3(1, 1, 1));
            Assert.AreEqual(new Quaternion(0, 1, 0, 0), state2.GetRotation());
            state1.Turn((float)Math.PI);
            Assert.AreEqual((state1.GetRotation() - state2.GetRotation()).Length(), 0, epsilon);

            state1.SetRotation(state2.GetRotation());
            Assert.AreEqual(state1.GetRotation(), state2.GetRotation());

        }

        public void AssertVectors(Vector3 vec1, Vector3 vec2)
        {
            float epsilon = 0.0001f;
            float len = (vec1 - vec2).Length();
            Assert.AreEqual(0, len, epsilon);
        }

        [Test]
        public void TestWorldMatrix()
        {
            Vector3 vec = new Vector3(1, 2, 3);
            WorldState state1 = new WorldState();
            Assert.AreEqual(Matrix.Identity, state1.GetWorldMatrix());

            state1.SetPosition(new Vector3(100, 200, 300));
            AssertVectors(new Vector3(101, 202, 303), Vector3.TransformCoordinate(vec, state1.GetWorldMatrix()));

            state1.Reset();
            Assert.AreEqual(Matrix.Identity, state1.GetWorldMatrix());

            // 1, 0, 0, 0 means rotate 180 deg around y axis (turn)
            state1.SetRotation(new Quaternion(0, 1, 0, 0));
            AssertVectors(new Vector3(-1, 2, -3), Vector3.TransformCoordinate(vec, state1.GetWorldMatrix()));

            state1.Reset();
            state1.SetScaling(new Vector3(2, 3, 4));
            AssertVectors(new Vector3(2, 6, 12), Vector3.TransformCoordinate(vec, state1.GetWorldMatrix()));

            state1.SetPosition(new Vector3(100, 200, 300));
            state1.SetRotation(new Quaternion(0, 1, 0, 0));
            state1.SetScaling(new Vector3(2, 3, 4));
            AssertVectors(new Vector3(98, 206, 288), Vector3.TransformCoordinate(vec, state1.GetWorldMatrix()));

        }
    }
}
