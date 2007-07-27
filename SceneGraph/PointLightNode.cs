using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class PointLightNode : LightNode
    {
        private float range;

        public PointLightNode(string name)
            : base(name)
        {
        }

        public float Range
        {
            get { return range; }
            set { range = value; }
        }

        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(Position, new Vector3(), range, DiffuseColor, SpecularColor);
        }
    }
}
