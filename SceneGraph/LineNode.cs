using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

namespace Dope.DDXX.SceneGraph
{
    public class LineNode : NodeBase
    {
        private ISpline<InterpolatedVector3> positionSpline;
        private UserPrimitive<VertexPositionColor> primitive;
        private float endTime;

        public LineNode(string name, IGraphicsFactory graphicsFactory, MaterialHandler material, ISpline<InterpolatedVector3> positionSpline, int segments)
            : base(name)
        {
            this.positionSpline = positionSpline;
            primitive = new UserPrimitive<VertexPositionColor>(VertexPositionColor.VertexDeclaration, material, PrimitiveType.LineStrip, segments);
            endTime = positionSpline.EndTime;
        }

        public float EndTime
        {
            set { endTime = value; }
        }

        protected override void StepNode()
        {
        }

        protected override void RenderNode(IScene scene)
        {
            primitive.Material.SetupRendering(new Matrix[] { WorldMatrix }, scene.ActiveCamera.ViewMatrix,
                scene.ActiveCamera.ProjectionMatrix, scene.AmbientColor);
            primitive.Begin();
            float step = (endTime - positionSpline.StartTime) / primitive.BufferSize;
            float time = positionSpline.StartTime;
            for (int i = 0; i < primitive.BufferSize; i++)
            {
                primitive.AddVertex(new VertexPositionColor(positionSpline.GetValue(time), primitive.Material.DiffuseColor));
                time += step;
            }
            primitive.End();
        }

    }
}
