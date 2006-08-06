using System;
using System.Collections.Generic;
using System.Text;
using Direct3D;

namespace SceneGraph
{
    public class MeshNode : NodeBase
    {
        private IMesh mesh;

        public MeshNode(string name, IMesh mesh) 
            : base(name)
        {
            this.mesh = mesh;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode()
        {
        }
    }
}
