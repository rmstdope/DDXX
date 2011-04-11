using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.SceneGraph
{
    public abstract class LightNode : NodeBase
    {
        private Color diffuseColor;
        private Color specularColor;

        public LightNode(string name)
            : base(name)
        {
            diffuseColor = new Color(255, 255, 255, 255);
            specularColor = new Color(255, 255, 255, 255);
        }

        public Color DiffuseColor
        {
            get { return diffuseColor; }
            set { diffuseColor = value; }
        }

        public Color SpecularColor
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

    }
}
