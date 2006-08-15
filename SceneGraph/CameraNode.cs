using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace SceneGraph
{
    public class CameraNode : NodeBase
    {
        private float fov = (float)Math.PI / 4;

        private float nearZ = 1.0f;

        private float farZ = 1000.0f;

        private float aspectRatio = 4.0f / 3.0f;

        public CameraNode(String name)
            : base(name)
        {
        }

        internal object GetFOV()
        {
            return fov;
        }

        internal void SetFOV(float f)
        {
            fov = f;
        }

        internal Matrix GetProjectionMatrix()
        {
            return Matrix.PerspectiveFovLH(fov, aspectRatio, nearZ, farZ);
        }

        internal void SetClippingPlanes(float near, float far)
        {
            nearZ = near;
            farZ = far;
        }

        internal void SetAspect(float aspect)
        {
            aspectRatio = aspect;
        }

        internal Matrix GetViewMatrix()
        {
            Matrix res = WorldState.GetWorldMatrix();
            res.Invert();
            return res;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode()
        {
        }
    }
}
