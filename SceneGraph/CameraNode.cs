using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.SceneGraph
{
    public class CameraNode : NodeBase, IRenderableCamera
    {
        private float fov = (float)Math.PI / 4;
        private float nearZ = 0.01f;
        private float farZ = 10000.0f;
        private float aspectRatio = 16.0f / 9.0f;//4.0f / 3.0f;

        public CameraNode(String name, float aspect)
            : base(name)
        {
            AspectRatio = aspect;
        }

        public object GetFOV()
        {
            return fov;
        }

        public void SetFOV(float f)
        {
            fov = f;
        }

        public void SetClippingPlanes(float near, float far)
        {
            nearZ = near;
            farZ = far;
        }

        public float AspectRatio
        {
            get { return aspectRatio; }
            set { aspectRatio = value; }
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
        }

        public Matrix ProjectionMatrix
        {
            get { return Matrix.CreatePerspectiveFieldOfView(fov, aspectRatio, nearZ, farZ); }
        }

        public Matrix ViewMatrix
        {
            get 
            {
                return Matrix.Invert(WorldMatrix);
            }
        }

        public void LookAt(Vector3 target, Vector3 up)
        {
            Matrix m = Matrix.CreateLookAt(WorldState.Position, target, up);
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            WorldState.Rotation = Matrix.Invert(m);
        }

    }
}
