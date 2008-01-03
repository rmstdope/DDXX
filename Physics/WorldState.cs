using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.Physics
{
    public class WorldState
    {
        private Vector3 position = new Vector3(0, 0, 0);
        private Vector3 scaling = new Vector3(1, 1, 1);
        private Matrix rotation = Matrix.Identity;

        public WorldState()
        {
            Reset();
        }

        public WorldState(Vector3 position, Matrix rotation, Vector3 scaling)
        {
            this.position = position;
            this.rotation = rotation;
            this.scaling = scaling;
        }

        public Vector3 Forward { get { return rotation.Forward; } }
        public Vector3 Backward { get { return rotation.Backward; } }
        public Vector3 Up { get { return rotation.Up; } }
        public Vector3 Down { get { return rotation.Down; } }
        public Vector3 Right { get { return rotation.Right; } }
        public Vector3 Left { get { return rotation.Left; } }

        public Vector3 Position
        {
            get { return position; }
            set { position = value; }
        }

        public Vector3 Scaling
        {
            get { return scaling; }
            set { scaling = value; }
        }

        public Matrix Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }

        public void MoveDelta(Vector3 delta)
        {
            position += delta;
        }

        public void MoveForward(float delta)
        {
            MoveDelta(Forward * delta);
        }

        public void MoveBackward(float delta)
        {
            MoveForward(-delta);
        }

        public void MoveRight(float delta)
        {
            MoveDelta(Right * delta);
        }

        public void MoveLeft(float delta)
        {
            MoveRight(-delta);
        }

        public void MoveUp(float delta)
        {
            MoveDelta(Up * delta);
        }

        public void MoveDown(float delta)
        {
            MoveUp(-delta);
        }

        public void Scale(Vector3 scale)
        {
            scaling.X *= scale.X;
            scaling.Y *= scale.Y;
            scaling.Z *= scale.Z;
        }

        public void Scale(float scale)
        {
            scaling *= scale;
        }

        public virtual Matrix GetWorldMatrix()
        {
            Matrix scale = Matrix.CreateScale(scaling);
            Matrix trans = Matrix.CreateTranslation(position);
            return scale * rotation * trans;
        }

        public void Reset()
        {
            ResetPosition();
            ResetScaling();
            ResetRotation();
        }

        public void ResetRotation()
        {
            rotation = Matrix.Identity;
        }

        public void ResetScaling()
        {
            scaling = new Vector3(1, 1, 1);
        }

        public void ResetPosition()
        {
            position = new Vector3(0, 0, 0);
        }

        public void Turn(double angle)
        {
            Matrix m = Matrix.CreateRotationY((float)angle);
            rotation = m * rotation;
        }

        public void Tilt(double angle)
        {
            Matrix m = Matrix.CreateRotationX((float)angle);
            rotation = m * rotation;
        }

        public void Roll(double angle)
        {
            Matrix m = Matrix.CreateRotationZ((float)angle);
            rotation = m * rotation;
        }
    }
}
