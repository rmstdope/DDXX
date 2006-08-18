using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    class LightNode : NodeBase
    {
        private Light light;

        public Light Light
        {
            get { return light; }
            set { light = value; }
        }

        public LightNode(string name, Light light)
            : base(name)
        {
            this.light = light;
        }

        protected override void StepNode()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        protected override void RenderNode(CameraNode camera)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
