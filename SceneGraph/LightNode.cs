using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public abstract class LightNode : NodeBase
    {
        private ColorValue diffuseColor;
        private ColorValue specularColor;

        public LightNode(string name)
            : base(name)
        {
            diffuseColor = new ColorValue(1, 1, 1, 1);
            specularColor = new ColorValue(1, 1, 1, 1);
        }

        public ColorValue DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        public ColorValue SpecularColor
        {
            get { return specularColor; }
            set { specularColor = value; }
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
        }

        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(Position, DiffuseColor, SpecularColor);
        }
    }
}
