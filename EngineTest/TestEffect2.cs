using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.Utility;
using Dope.DDXX.ParticleSystems;

namespace EngineTest
{
    class TestEffect2 : BaseDemoEffect
    {
        private ModelNode node;
        //private MovingTrailNode movingTrail;

        public TestEffect2(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            ModelBuilder.CreateMaterial("Material");
            ModelBuilder.SetDiffuseTexture("Material", TextureFactory.CreateFromName("Noise256base1024"));
            ModelBuilder.SetDiffuseColor("Material", Color.BlanchedAlmond);
            ModelBuilder.SetEffect("Material", "Content\\effects\\CubeEffect");
            ModelDirector.CreateBox(3, 3, 3);
            IModel model = ModelDirector.Generate("Material");
            node = new ModelNode("Kuben", model, GraphicsDevice);
            Scene.AddNode(node);

            CreateStandardCamera(10);

            //movingTrail = new MovingTrailNode("Moving Trail", 1.0f, 10.0f);
            //movingTrail.Initialize(GraphicsFactory, 20);
            //movingTrail.Material.DiffuseTexture = TextureFactory.CreateFromName("Circle64");
            //Scene.AddNode(movingTrail);
        }

        public override void Step()
        {
            node.WorldState.Turn(Time.DeltaTime);
            node.WorldState.Tilt(Time.DeltaTime * -1.234f);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
