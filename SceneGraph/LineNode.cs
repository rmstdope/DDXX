using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Graphics;
using Microsoft.DirectX;
using System.Drawing;

namespace Dope.DDXX.SceneGraph
{
    public class LineNode : NodeBase
    {
        private ILine line;
        private Color color;
        private Vector3[] positions = new Vector3[2];

        public LineNode(string name, ILine line, Vector3 startPosition, Vector3 endPosition, Color color)
            : base(name)
        {
            this.line = line;
            this.color = color;
            positions[0] = startPosition;
            positions[1] = endPosition;
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
            Matrix transform = WorldMatrix * scene.ActiveCamera.ViewMatrix * scene.ActiveCamera.ProjectionMatrix;
            line.Begin();
            line.DrawTransform(positions, transform, color);
            line.End();
        }
    }
}
