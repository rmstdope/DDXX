using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.Physics;

namespace Dope.DDXX.MeshBuilder
{
    public class ClothPrimitive : PlanePrimitive
    {
        internal const float HORIZONTAL_STIFFNESS = 0.8f;
        internal const float VERTICAL_STIFFNESS = 0.8f;
        internal const float DIAGONAL_STIFFNESS = 0.1f;

        private List<int> pinned = new List<int>();

        public void PinParticle(int index)
        {
            pinned.Add(index);
        }

        public override void Generate(out Vertex[] vertices, out short[] indices, out IBody body)
        {
            IPhysicalParticle p1;
            IPhysicalParticle p2;
            IPhysicalParticle p3;
            IPhysicalParticle p4;
            int x;
            int y;

            base.Generate(out vertices, out indices, out body);

            body = new Body();
            for (int i = 0; i < vertices.Length; i++)
                body.AddParticle(new PhysicalParticle(vertices[i].Position, 1, 0.01f));
            for (y = 0; y < HeightSegments; y++)
            {
                for (x = 0; x < WidthSegments; x++)
                {
                    // p1--p2
                    // | \/     
                    // | /\      
                    // p3  p4
                    p1 = body.Particles[(y + 0) * (WidthSegments + 1) + x + 0];
                    p2 = body.Particles[(y + 0) * (WidthSegments + 1) + x + 1];
                    p3 = body.Particles[(y + 1) * (WidthSegments + 1) + x + 0];
                    p4 = body.Particles[(y + 1) * (WidthSegments + 1) + x + 1];
                    AddStickConstraint(body, p1, p2, HORIZONTAL_STIFFNESS);
                    AddStickConstraint(body, p1, p3, VERTICAL_STIFFNESS);
                    AddStickConstraint(body, p1, p4, DIAGONAL_STIFFNESS);
                    AddStickConstraint(body, p1, p4, DIAGONAL_STIFFNESS);
                }
                // p1
                // |
                // |
                // p3
                p1 = body.Particles[(y + 0) * (WidthSegments + 1) + x + 0];
                p3 = body.Particles[(y + 1) * (WidthSegments + 1) + x + 0];
                AddStickConstraint(body, p1, p3, VERTICAL_STIFFNESS);
            }
            for (x = 0; x < WidthSegments; x++)
            {
                // p1--p2
                p1 = body.Particles[(y + 0) * (WidthSegments + 1) + x + 0];
                p2 = body.Particles[(y + 0) * (WidthSegments + 1) + x + 1];
                AddStickConstraint(body, p1, p2, HORIZONTAL_STIFFNESS);
            }
            foreach (int index in pinned)
            {
                body.AddConstraint(new PositionConstraint(body.Particles[index],
                    body.Particles[index].Position));
            }
        }

        private void AddStickConstraint(IBody body, IPhysicalParticle p1, IPhysicalParticle p2, float stiffness)
        {
            body.AddConstraint(new StickConstraint(p1, p2, (p1.Position - p2.Position).Length(), stiffness));
        }
    }
}
