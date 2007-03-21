using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX.Direct3D;

namespace Dope.DDXX.SceneGraph
{
    public class LightNode : NodeBase
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
        }

        protected override void RenderNode(IScene scene)
        {
        }


        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(WorldState.Position, Light.DiffuseColor, Light.SpecularColor);
        }
    }
}
