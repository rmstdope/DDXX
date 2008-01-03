using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public class DirectionalLightNode : LightNode
    {
        private Vector3 direction;

        public Vector3 Direction
        {
            get { return direction; }
            set { direction = value; direction.Normalize(); }
        }

        public DirectionalLightNode(string name)
            : base(name)
        {
        }

        protected override void SetLightStateNode(LightState state)
        {
            state.NewState(new Vector3(), Direction, Color);
        }
    }
}
