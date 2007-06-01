using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.DirectX;

namespace Dope.DDXX.SceneGraph
{
    public class DirectionalLightNode : LightNode
    {
        private Vector3 direction;

        public Vector3 Direction
        {
            get { return direction; }
            set { value.Normalize();  direction = value; }
        }

        public DirectionalLightNode(string name)
            : base(name)
        {
            Direction = new Vector3(1, 1, 1);
        }

        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(new Vector3(), Direction, DiffuseColor, SpecularColor);
        }
    }
}
