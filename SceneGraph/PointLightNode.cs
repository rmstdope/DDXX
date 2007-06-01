using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class PointLightNode : LightNode
    {
        public PointLightNode(string name)
            : base(name)
        {
        }

        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(Position, new Vector3(), DiffuseColor, SpecularColor);
        }
    }
}
