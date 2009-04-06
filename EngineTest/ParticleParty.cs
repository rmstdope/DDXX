using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.TextureBuilder;
using Dope.DDXX.Utility;
using Microsoft.Xna.Framework.Graphics;
using Dope.DDXX.Graphics;
using Microsoft.Xna.Framework;

namespace EngineTest
{
    public class ParticleParty : BaseDemoEffect
    {
        private CameraNode camera;
        private List<NodeBase> systems;

        public ParticleParty(string name, float start, float end)
            : base(name, start, end)
        {
            systems = new List<NodeBase>();
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 50);

            TextureDirector director = new TextureDirector(TextureFactory);
            director.CreateCircle(0.2f, 0.35f, 0.5f, 0.5f, new Vector2(0.5f, 0.5f));

            SpiralSystemNode spiralSystem;
            ITexture2D texture = director.Generate("Circle256", 256, 256, 0, SurfaceFormat.Color);
            for (int i = 0; i < 1; i++)
            {
                spiralSystem = new SpiralSystemNode("ps" + i, 1.0f);
                spiralSystem.Initialize(GraphicsFactory, 10000);
                spiralSystem.Material.DiffuseTexture = texture;
                Scene.AddNode(spiralSystem);
                systems.Add(spiralSystem);
            }
            systems[0].WorldState.MoveLeft(10);
            //systems[1].WorldState.MoveRight(10);

            FallingSystemNode fallingSystem = new FallingSystemNode("falling", 1.0f);
            fallingSystem.Initialize(GraphicsFactory, 100);
            fallingSystem.Material.DiffuseTexture = texture;
            Scene.AddNode(fallingSystem);
            //systems.Add(fallingSystem);
        }

        public override void Step()
        {
            foreach (SpiralSystemNode spiralSystem in systems)
            {
                spiralSystem.WorldState.Turn(Time.DeltaTime * 0.4f);
                spiralSystem.WorldState.Roll(Time.DeltaTime * 0.1f);
            }
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
