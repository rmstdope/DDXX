using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Graphics;
using Dope.DDXX.Utility;

namespace EngineTest
{
    public class Tubes : BaseDemoEffect
    {
        private DummyNode torus;

        public Tubes(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            ModelBuilder.SetDiffuseTexture("Default", "Content\\textures\\BENEDETI2");
            torus = new DummyNode("Torus");
            Scene.AddNode(torus);

            float radius = 1;
            int rings = 11;
            double phi = 0;
            for (int i = 0; i < rings; i++)
            {
                phi += (Math.PI / (rings + 1));
                float y = (float)(radius * Math.Cos(phi));
                float innerRadius = (float)(-radius * Math.Sin(phi));
                CreateTorus(0.07f, innerRadius, y);
            }
            Scene.ActiveCamera.WorldState.MoveForward(6);
        }

        private void CreateTorus(float innerRadius, float outerRadius, float y)
        {
            ModelDirector.CreateTorus(innerRadius, outerRadius, 8, 32);
            ModelDirector.UvRemap(0, 0, 6, 1);
            IModel model = ModelDirector.Generate("Default");
            ModelNode node = new ModelNode("Model", model, GraphicsDevice);
            node.WorldState.MoveUp(y);
            torus.AddChild(node);
        }

        public override void Step()
        {
            torus.WorldState.Turn(Time.DeltaTime * 1.2f);
            torus.WorldState.Tilt(Time.DeltaTime * 0.5f);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
