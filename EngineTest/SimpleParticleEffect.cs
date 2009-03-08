using System;
using System.Collections.Generic;
using System.Text;
using Dope.DDXX.DemoFramework;
using Dope.DDXX.SceneGraph;
using Dope.DDXX.ParticleSystems;
using Dope.DDXX.TextureBuilder;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Dope.DDXX.Utility;

namespace EngineTest
{
    class SimpleParticleEffect : BaseDemoEffect
    {
        private CameraNode camera;
        //private FloaterSystemNode floaterSystem;
        private FallingCurtainSystemNode fallingCurtainParticleSystem;
        private FallingCurtainSystemNode fallingCurtainParticleSystem2;
        private FallingCurtainSystemNode fallingCurtainParticleSystem3;

        public SimpleParticleEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardCamera(out camera, 50);

            TextureDirector director = new TextureDirector(TextureFactory);
            director.CreateCircle(0.2f, 0.5f);

            //floaterSystem = new FloaterSystemNode("ps", 0.0f, 0.6f, 3);
            //floaterSystem.Initialize(GraphicsDevice, GraphicsFactory, 100);
            //floaterSystem.Material.DiffuseTexture = director.Generate(256, 256, 0, SurfaceFormat.Color);
            //Scene.AddNode(floaterSystem);

            fallingCurtainParticleSystem = new FallingCurtainSystemNode("Falling Curtain Particle System", 1.0f);
            fallingCurtainParticleSystem.Initialize(GraphicsFactory, 500);
            fallingCurtainParticleSystem.Material.DiffuseTexture = TextureFactory.CreateFromName("Circle256");
            Scene.AddNode(fallingCurtainParticleSystem);

            fallingCurtainParticleSystem2 = new FallingCurtainSystemNode("Falling Curtain Particle System", 1.3f);
            fallingCurtainParticleSystem2.Initialize(GraphicsFactory, 500);
            fallingCurtainParticleSystem2.Material.DiffuseTexture = TextureFactory.CreateFromName("Circle256");
            Vector3 pos;
            pos.X = -2.0f;
            pos.Y = 2.0f;
            pos.Z = 1.0f;
            fallingCurtainParticleSystem2.Position = pos;
            fallingCurtainParticleSystem2.WorldState.Tilt(Math.PI / 4.0);
            Scene.AddNode(fallingCurtainParticleSystem2);

            fallingCurtainParticleSystem3 = new FallingCurtainSystemNode("Falling Curtain Particle System", 0.8f);
            fallingCurtainParticleSystem3.Initialize(GraphicsFactory, 500);
            fallingCurtainParticleSystem3.Material.DiffuseTexture = TextureFactory.CreateFromName("Circle256");
            pos.X = 3.0f;
            pos.Y = -1.0f;
            pos.Z = -1.0f;
            fallingCurtainParticleSystem3.Position = pos;
            fallingCurtainParticleSystem3.WorldState.Roll(Math.PI / 4.0);
            Scene.AddNode(fallingCurtainParticleSystem3);
        }

        public override void Step()
        {
            fallingCurtainParticleSystem.WorldState.Turn(Time.DeltaTime * 0.4f);
            fallingCurtainParticleSystem.WorldState.Roll(Time.DeltaTime * 0.1f);
            Scene.Step();
        }

        public override void Render()
        {
            Scene.Render();
        }
    }
}
