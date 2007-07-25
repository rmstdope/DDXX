using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX;
using Dope.DDXX.Graphics;

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

        protected override void StepNode(IRenderableCamera camera)
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
                switch (modelNode.Model.CullMode)
                {
                    case Cull.Clockwise:
                        modelNode.Model.CullMode = Cull.CounterClockwise;
                        break;
                    case Cull.CounterClockwise:
                        modelNode.Model.CullMode = Cull.Clockwise;
                        break;
                }
                modelNode.Model.UseStencil = !modelNode.Model.UseStencil;
            }
            foreach (INode child in node.Children)
                SwitchCulling(child);
        }

        void ModulateMaterials(INode node, float value)
        {
            if (node is ModelNode)
            {
                ModelNode modelNode = node as ModelNode;
                foreach (ModelMaterial material in modelNode.Model.Materials)
                {
                    material.SpecularColor = new ColorValue(
                        material.SpecularColor.Red * value, material.SpecularColor.Green * value,
                        material.SpecularColor.Blue * value, material.SpecularColor.Alpha);
                    material.DiffuseColor = new ColorValue(
                        material.DiffuseColor.Red * value, material.DiffuseColor.Green * value,
                        material.DiffuseColor.Blue * value, material.DiffuseColor.Alpha);
                    material.AmbientColor = new ColorValue(
                        material.AmbientColor.Red * value, material.AmbientColor.Green * value,
                        material.AmbientColor.Blue * value, material.AmbientColor.Alpha);
                }
            }
            foreach (INode child in node.Children)
                ModulateMaterials(child, value);
        }
    }
}
