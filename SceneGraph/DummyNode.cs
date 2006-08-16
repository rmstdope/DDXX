using System;
using System.Collections.Generic;
using System.Text;

namespace Dope.DDXX.SceneGraph
{
    public class DummyNode : NodeBase
    {
        public DummyNode(string name)
            : base(name)
        {
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(CameraNode camera)
        {
        }
    }
}
