using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Physics
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

        public Vector3 GetPosition()
        {
            return position;
        }

        public void SetPosition(Vector3 position)
        {
            this.position = position;
        }

        public void MoveDelta(Vector3 delta)
        {
            position += delta;
        }

        public Vector3 GetScaling()
        {
            return scaling;
        }

        public void SetScaling(Vector3 scaling)
        {
            this.scaling = scaling;
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

        public Quaternion GetRotation()
        {
            return rotation;
        }

        public void Turn(float angle)
        {
            Quaternion q = Quaternion.RotationAxis(new Vector3(0, 1, 0), angle);
            rotation = q * rotation;
        }

        public void SetRotation(Quaternion rot)
        {
            rotation = rot;
        }

        public virtual Matrix GetWorldMatrix()
        {
            Matrix rot = Matrix.RotationQuaternion(GetRotation());
            Matrix scale = Matrix.Scaling(GetScaling());
            Matrix trans = Matrix.Translation(GetPosition());
            return scale * rot * trans;
        }

        public void Reset()
        {
            position = new Vector3(0, 0, 0);
            scaling = new Vector3(1, 1, 1);
            rotation = new Quaternion(0, 0, 0, 1);
        }
    }
}
