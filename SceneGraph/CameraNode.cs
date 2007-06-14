using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class CameraNode : NodeBase, IRenderableCamera
    {
        private float fov = (float)Math.PI / 4;
        private float nearZ = 0.01f;
        private float farZ = 1000.0f;
        private float aspectRatio = 4.0f / 3.0f;

        public CameraNode(String name)
            : base(name)
        {
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

        public void SetAspect(float aspect)
        {
            aspectRatio = aspect;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
        }

        public Matrix ProjectionMatrix
        {
            get { return Matrix.PerspectiveFovLH(fov, aspectRatio, nearZ, farZ); }
        }

        public Matrix ViewMatrix
        {
            get 
            {
                Matrix res = WorldMatrix;// WorldState.GetWorldMatrix();
                res.Invert();
                return res;
            }
        }

        public void LookAt(Vector3 target, Vector3 up)
        {
            Matrix m = Matrix.LookAtLH(WorldState.Position, target, up);
            m.M41 = 0;
            m.M42 = 0;
            m.M43 = 0;
            m.Invert();
            WorldState.Rotation = m;
        }

    }
}
