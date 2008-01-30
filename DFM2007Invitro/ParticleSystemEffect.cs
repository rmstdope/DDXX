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

namespace DFM2007Invitro
{
    public class ParticleSystemEffect : BaseDemoEffect
    {
        private IScene scene;
        private CameraNode camera;
        //private FloaterSystemNode floaterSystem;
        private SpiralSystemNode spiralSystem;

        public ParticleSystemEffect(string name, float start, float end)
            : base(name, start, end)
        {
        }

        protected override void Initialize()
        {
            CreateStandardSceneAndCamera(out scene, out camera, 50);

            TextureDirector director = new TextureDirector(TextureFactory);
            director.CreateCircle(0.2f, 0.5f);

            //floaterSystem = new FloaterSystemNode("ps", 0.0f, 0.6f, 3);
            //floaterSystem.Initialize(GraphicsDevice, GraphicsFactory, 100);
            //floaterSystem.Material.DiffuseTexture = director.Generate(256, 256, 0, SurfaceFormat.Color);
            //scene.AddNode(floaterSystem);

            spiralSystem = new SpiralSystemNode("ps", 1.0f);
            spiralSystem.Initialize(GraphicsDevice, GraphicsFactory, 10000);
            spiralSystem.Material.DiffuseTexture = director.Generate(256, 256, 0, SurfaceFormat.Color);
            scene.AddNode(spiralSystem);
        }

        public override void Step()
        {
            spiralSystem.WorldState.Turn(Time.DeltaTime * 0.4f);
            spiralSystem.WorldState.Roll(Time.DeltaTime * 0.1f);
            scene.Step();
        }

        public override void Render()
        {
            scene.Render();
        }
    }
}
