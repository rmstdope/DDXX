using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Dope.DDXX.SceneGraph
{
    public abstract class LightNode : NodeBase
    {
        private Color color;

        public LightNode(string name)
            : base(name)
        {
            color = new Color(255, 255, 255, 255);
        }

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
        }

    }
}
