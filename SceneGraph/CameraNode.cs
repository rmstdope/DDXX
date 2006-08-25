using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class CameraNode : NodeBase, ICamera
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

        internal void SetClippingPlanes(float near, float far)
        {
            nearZ = near;
            farZ = far;
        }

        internal void SetAspect(float aspect)
        {
            aspectRatio = aspect;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IRenderableScene scene)
        {
        }

        #region ICamera Members

        public Matrix ProjectionMatrix
        {
            get { return Matrix.PerspectiveFovLH(fov, aspectRatio, nearZ, farZ); }
        }

        public Matrix ViewMatrix
        {
            get 
            { 
                Matrix res = WorldState.GetWorldMatrix();
                res.Invert();
                return res;
            }
        }

        #endregion
    }
}
