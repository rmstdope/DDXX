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
        private Quaternion rotation = new Quaternion(0, 0, 0, 1);

        public WorldState()
        {
        }

        public WorldState(Vector3 position, Quaternion rotation, Vector3 scaling)
        {
            this.position = position;
            this.rotation = rotation;
            this.scaling = scaling;
        }

        public Vector3 Forward
        {
            get 
            { 
                Matrix m = Matrix.RotationQuaternion(rotation);
                return new Vector3(m.M31, m.M32, m.M33);
            }
        }

        public Vector3 Up
        {
            get
            {
                Matrix m = Matrix.RotationQuaternion(rotation);
                return new Vector3(m.M21, m.M22, m.M23);
            }
        }

        public Vector3 Right
        {
            get
            {
                Matrix m = Matrix.RotationQuaternion(rotation);
                return new Vector3(m.M11, m.M12, m.M13);
            }
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

        public Quaternion Rotation
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
            Matrix rot = Matrix.RotationQuaternion(Rotation);
            Matrix scale = Matrix.Scaling(Scaling);
            Matrix trans = Matrix.Translation(Position);
            return scale * rot * trans;
        }

        public void Reset()
        {
            position = new Vector3(0, 0, 0);
            scaling = new Vector3(1, 1, 1);
            rotation = new Quaternion(0, 0, 0, 1);
        }

        public void Turn(float angle)
        {
            Quaternion q = Quaternion.RotationAxis(new Vector3(0, 1, 0), angle);
            rotation = Quaternion.Multiply(q, rotation);
        }

        public void Tilt(float angle)
        {
            Quaternion q = Quaternion.RotationAxis(new Vector3(1, 0, 0), angle);
            rotation = Quaternion.Multiply(q, rotation);
        }

        public void Roll(float angle)
        {
            Quaternion q = Quaternion.RotationAxis(new Vector3(0, 0, 1), angle);
            rotation = Quaternion.Multiply(q, rotation);
        }
    }
}
