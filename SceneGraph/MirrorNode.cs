using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class MirrorNode : NodeBase
    {
        private INode targetNode;

        public MirrorNode(INode targetNode)
            : base(targetNode.Name + "_Mirror")
        {
            WorldState.Scaling = new Vector3(1, -1, 1);
            this.targetNode = targetNode;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
        }

        protected override void BeforeRenderingChildren()
        {
            AddChild(targetNode);
            SwitchCulling(this);
        }

        protected override void AfterRenderingChildren()
        {
            SwitchCulling(this);
            RemoveChild(targetNode);
        }

        void SwitchCulling(INode node)
        {
            if (node is ModelNode)
            {
                ModelNode modelNode = node as ModelNode;
                switch (modelNode.Model.CullMode)
                {
                    case Cull.Clockwise:
                        modelNode.Model.CullMode = Cull.CounterClockwise;
                        break;
                    case Cull.CounterClockwise:
                        modelNode.Model.CullMode = Cull.Clockwise;
                        break;
                }
            }
            foreach (INode child in node.Children)
                SwitchCulling(child);
        }
    }
}
