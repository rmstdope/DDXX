using System;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public class MirrorNode : NodeBase
    {
        private INode targetNode;
        private float brightness;

        public float Brightness
        {
            get { return brightness; }
            set { brightness = value; }
        }

        public MirrorNode(INode targetNode)
            : base(targetNode.Name + "_Mirror")
        {
            WorldState.Scaling = new Vector3(1, -1, 1);
            this.targetNode = targetNode;
            brightness = 1;
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
            ModulateMaterials(this, brightness);
        }

        protected override void AfterRenderingChildren()
        {
            ModulateMaterials(this, 1 / brightness);
            SwitchCulling(this);
            RemoveChild(targetNode);
        }

        void SwitchCulling(INode node)
        {
            if (node is ModelNode)
            {
                ModelNode modelNode = node as ModelNode;
                switch (modelNode.CullMode)
                {
                    case CullMode.CullClockwiseFace:
                        modelNode.CullMode = CullMode.CullCounterClockwiseFace;
                        break;
                    case CullMode.CullCounterClockwiseFace:
                        modelNode.CullMode = CullMode.CullClockwiseFace;
                        break;
                }
                //modelNode.Model.UseStencil = !modelNode.Model.UseStencil;
            }
            foreach (INode child in node.Children)
                SwitchCulling(child);
        }

        void ModulateMaterials(INode node, float value)
        {
            if (node is ModelNode)
            {
                ModelNode modelNode = node as ModelNode;
                foreach (IModelMesh mesh in modelNode.Model.Meshes)
                {
                    foreach (IModelMeshPart part in mesh.MeshParts)
                    {
                        part.MaterialHandler.AmbientColor = new Color(part.MaterialHandler.AmbientColor.ToVector3() * value);
                        part.MaterialHandler.DiffuseColor = new Color(part.MaterialHandler.DiffuseColor.ToVector3() * value);
                        part.MaterialHandler.SpecularColor = new Color(part.MaterialHandler.SpecularColor.ToVector3() * value);
                    }
                }
            }
            foreach (INode child in node.Children)
                ModulateMaterials(child, value);
        }
    }
}
