using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

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

        public Vector3 Forward
        {
            get { return new Vector3(rotation.M31, rotation.M32, rotation.M33); }
        }

        public Vector3 Up
        {
            get { return new Vector3(rotation.M21, rotation.M22, rotation.M23); }
        }

        public Vector3 Right
        {
            get { return new Vector3(rotation.M11, rotation.M12, rotation.M13); }
        }

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

        public void MoveRight(float delta)
        {
            MoveDelta(Right * delta);
        }

        public void MoveUp(float delta)
        {
            MoveDelta(Up * delta);
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
            Matrix scale = Matrix.Scaling(scaling);
            Matrix trans = Matrix.Translation(position);
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

        public void Turn(float angle)
        {
            Matrix m = Matrix.RotationY(angle);
            rotation = Matrix.Multiply(m, rotation);
        }

        public void Tilt(float angle)
        {
            Matrix m = Matrix.RotationX(angle);
            rotation = Matrix.Multiply(m, rotation);
        }

        public void Roll(float angle)
        {
            Matrix m = Matrix.RotationZ(angle);
            rotation = Matrix.Multiply(m, rotation);
        }
    }
}
